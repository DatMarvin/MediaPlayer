
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Reflection
Imports System.Runtime.InteropServices


Public Class Tcp

    Private listener As TcpListener
    Public isListenerActive As Boolean
    Public port As Integer
    Public connections As List(Of ClientConnection)
    Public ReadOnly Property isEstablished() As Boolean
        Get
            Return connections IsNot Nothing AndAlso connections.Count > 0
        End Get
    End Property



    Public Sub New()
        connections = New List(Of ClientConnection)
    End Sub


    Public Function startListener(ByVal port As Integer) As Boolean
        If port = 0 Then Return False
        Try
            If listener IsNot Nothing Then
                listener.Stop()
            End If

            listener = New TcpListener(IPAddress.Any, port)
            listener.Start()
            isListenerActive = True
            Me.port = port
            Return True
        Catch ex As SocketException
            Return False
        End Try
    End Function

    Public Function stopListener() As Boolean
        Try
            listener.Stop()
            isListenerActive = False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Structure ListenResult
        Public resultCode As Integer
        Public client As TcpClient
    End Structure

    Private Function getListenResult(resultCode As Integer, client As TcpClient) As ListenResult
        Return New ListenResult() With {.client = client, .resultCode = resultCode}
    End Function


    Public Async Function listen(Optional ByVal acceptExternalIP As Boolean = True) As Tasks.Task(Of ListenResult)
        Try
            If Not isListenerActive Then Return getListenResult(3, Nothing)

            Dim newClient As TcpClient = Await listener.AcceptTcpClientAsync

            If Not acceptExternalIP AndAlso isExternalIP(getIp(newClient)) Then
                Return getListenResult(1, newClient)
            End If

            Return establishConnection(newClient)
        Catch ex As Exception
            Return getListenResult(0, Nothing)
        End Try
    End Function

    Public Function establishConnection(newClient As TcpClient) As ListenResult
        Dim newConnection As New ClientConnection(Me)
        connections.Add(newConnection)

        newConnection.client = newClient

        newConnection.send("ack")

        Dim newThread As New Thread(AddressOf newConnection.intercept)
        newThread.IsBackground = True

        newConnection.thread = newThread
        newConnection.startThread()

        Return getListenResult(2, newClient)
    End Function


    Function getRandomConnectionOrder() As List(Of ClientConnection)
        Dim res As New List(Of ClientConnection)
        Dim order = getRandomOrder(connections.Count)
        For Each value In order
            res.Add(connections(value))
        Next
        Return res
    End Function

    Function getRandomOrder(len As Integer) As List(Of Integer)
        Dim res As New List(Of Integer)
        Dim rnd As New Random()
        Do Until res.Count = len
            Dim val As Integer = rnd.Next(0, len)
            If Not res.Contains(val) Then
                res.Add(val)
            End If
        Loop
        Return res
    End Function


    Public Sub sendAll(ByVal msg As String)
        connections.ForEach(
            Sub(c)
                c.send(msg)
            End Sub)
    End Sub

    Public Sub send(ip As String, ByVal msg As String)
        Dim connection As ClientConnection = getConnection(ip)
        connection.send(msg)
    End Sub


    Public Function send(ByVal ip As String, ByVal port As Integer, ByVal msg As String) As Boolean
        Try
            Dim tempClient = New TcpClient(ip, port)
            Dim writer As New StreamWriter(tempClient.GetStream())
            writer.Write(msg)
            writer.Flush()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function stopAllConnections() As Boolean
        Dim success As Boolean = True
        For i = connections.Count - 1 To 0 Step -1
            If Not connections(i).closeConnection() Then
                success = False
            End If
        Next
        Return success
    End Function

    Public Function stopConnection(ip As String) As Boolean
        Dim connection As ClientConnection = getConnection(ip)
        If connection IsNot Nothing Then
            Return connection.closeConnection()
        End If
        Return False
    End Function


    Private Function getConnection(ip As String) As ClientConnection
        For i = 0 To connections.Count - 1
            If getIp(connections(i).client) = ip Then
                Return connections(i)
            End If
        Next
        Return Nothing
    End Function

    Public Function getIp(ByVal client As TcpClient) As String
        Dim sa As Net.SocketAddress = client.Client.RemoteEndPoint.Serialize
        Dim ip As String = ""
        For i = 4 To 6
            ip &= CStr(CInt(sa.Item(i))) & "."
        Next
        ip &= CStr(CInt(sa.Item(7)))
        Return ip
    End Function

    Public Function getAllIps() As List(Of String)
        Dim res As New List(Of String)
        If isEstablished Then
            For Each connection In connections
                res.Add(getIp(connection.client))
            Next
        End If
        Return res
    End Function

    Function getIPAddress(ByVal hostName As String) As String
        Try
            Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(hostName)

            For i = iphe.AddressList.Count - 1 To 0 Step -1
                If iphe.AddressList(i).AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                    Return iphe.AddressList(i).ToString()
                End If
            Next
        Catch ex As Exception
        End Try
        Return ""
    End Function

    Function getExternalIP() As String
        Try
            Dim wc As New WebClient
            Dim res As String = wc.DownloadString("http://checkip.dyndns.org/")
            res = res.Substring(res.IndexOf(":") + 2)
            Return res.Substring(0, res.IndexOf("<"))
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Function isExternalIP(ByVal checkIP As String) As Boolean
        Try
            If checkIP = "127.0.0.1" Then Return False
            Dim firstUnequal As Integer = -1
            Dim loc As String = getIPAddress(Dns.GetHostName)
            For i = 0 To loc.Length - 1
                If checkIP.Length < i - 1 OrElse loc(i) <> checkIP(i) Then
                    Exit For
                ElseIf loc(i) = "." Then
                    firstUnequal += 1
                End If
            Next
            Return firstUnequal < 1
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Class ClientConnection

        Private ReadOnly remoteTcp As Tcp

        Public client As TcpClient
        Public thread As Thread

        Private message As String = ""
        Private ReadOnly _sync As New Object()

        Public ReadOnly Property hasMessage() As Boolean
            Get
                Return tcpMsg IsNot Nothing
            End Get
        End Property

        Public Property tcpMsg As String
            Get
                SyncLock _sync
                    Return message
                End SyncLock
            End Get
            Set(ByVal value As String)
                SyncLock _sync
                    message = value
                End SyncLock
            End Set
        End Property

        Public Sub New(remoteTcp As Tcp)
            Me.remoteTcp = remoteTcp
        End Sub

        Public Sub intercept()
            Do
                If client IsNot Nothing Then
                    Dim s As String = ""
                    Try
                        Dim reader As New StreamReader(client.GetStream)
                        While (reader.Peek > -1)
                            s += Convert.ToChar(reader.Read()).ToString
                        End While
                    Catch ex As IOException
                        If Err().Number = 57 Then
                            'connection forcibly closed by remote host
                        End If
                        tcpMsg = Nothing
                        Return
                    End Try
                    If s = "" Then
                        tcpMsg = Nothing 'Distinction to "" message
                        Return
                    Else
                        tcpMsg = s
                    End If
                End If
            Loop
        End Sub

        Public Sub send(ByVal msg As String)
            If client IsNot Nothing Then
                If client.Connected Then
                    Try
                        Dim writer As New StreamWriter(client.GetStream())
                        writer.AutoFlush = True
                        writer.Write(msg)
                        writer.Flush()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
            End If
        End Sub

        Public Function closeConnection() As Integer
            Dim res As Integer = 0
            If thread IsNot Nothing Then
                Try
                    thread.Abort()
                Catch ex As Exception
                    res = 1
                End Try
            End If
            If client IsNot Nothing Then
                Try
                    client.Close()
                Catch ex As Exception
                    res = 2
                End Try
            End If
            tcpMsg = ""
            remoteTcp.connections.Remove(Me)
            Return res
        End Function

        Public Sub startThread()
            If thread IsNot Nothing AndAlso client IsNot Nothing Then
                thread.Start()
            End If
        End Sub
    End Class
End Class
