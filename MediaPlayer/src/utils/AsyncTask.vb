
Imports System.Threading
Imports WMPLib
Imports System.Threading.Tasks

Module AsyncTask
    Public ReadOnly Property getFormHandle() As Form
        Get
            Return formHandle
        End Get
    End Property

    Public cts As CancellationTokenSource
    Dim token As CancellationToken

    Public progressDialogHandle As ProgressDialog
    Dim formHandle As Form
    Public progressIndicator As Progress(Of Integer)
    Dim dll As New Utils

    Public Async Function getFolderStatsList(folders As List(Of Folder), dateLogStart As Date) As Tasks.Task(Of List(Of Integer()))
        Dim subsList As New List(Of Integer())
        Return Await Task(Of Integer()).Run(Function()
                                                For k = 0 To folders.Count - 1
                                                    Dim use As List(Of Track) = folders(k).tracks
                                                    If use.Count > 0 Then
                                                        For i = 0 To use.Count - 1
                                                            use(i).updatePopularity(True)
                                                        Next
                                                        Dim tot As Integer = 0
                                                        Dim len As Integer = 0
                                                        Dim pop As Integer = 0
                                                        Dim c As Integer = 0
                                                        Dim diff As Integer = Now.Subtract(dateLogStart).TotalDays
                                                        Dim age As Integer = 0
                                                        For i = 0 To use.Count - 1
                                                            age += IIf(use(i).added = Nothing, Now.Subtract(New Date(2011, 4, 6)).TotalDays - 1, Now.Subtract(use(i).added).TotalDays)
                                                            len += use(i).length
                                                            c += use(i).count
                                                            pop += use(i).popularity
                                                            tot += use(i).count * use(i).length
                                                        Next
                                                        subsList.Add({use.Count, diff, tot, c, len, pop, age})
                                                        If progressDialogHandle IsNot Nothing Then progressDialogHandle.updateProgress(progressIndicator, k + 1, folders.Count)
                                                    Else
                                                        subsList.Add({0, 0, 0, 0, 0, 0, 0})
                                                    End If
                                                Next

                                                Return subsList
                                            End Function)

    End Function


    'Public Async Function getTotaltime() As Tasks.Task(Of Integer)
    '    Dim allLen As Integer = 0
    '    Dim allMedia As IWMPPlaylist = Form1.wmp.mediaCollection.getAll


    '    Return Await Task(Of Integer).Run(Function()

    '                                          For i = 0 To allMedia.count - 1
    '                                              Try
    '                                                  Dim med As IWMPMedia = allMedia.Item(i)
    '                                                  If med.sourceURL.StartsWith(Folder.top.fullPath) Then
    '                                                      allLen += med.duration
    '                                                  End If
    '                                                  progressDialogHandle.updateProgress(progressIndicator, i + 1, allMedia.count)
    '                                              Catch ex As OperationCanceledException
    '                                                  Return 0
    '                                              Catch exGen As Exception
    '                                                  Return 0
    '                                              End Try

    '                                          Next
    '                                          Return allLen
    '                                      End Function)
    'End Function

    Public Async Function getStatsList(trackList As List(Of Track)) As Tasks.Task(Of List(Of String()))
        Dim subsList As New List(Of String())


        Return Await Task(Of List(Of String)).Run(Function()
                                                      For i = 0 To trackList.Count - 1
                                                          Try
                                                              trackList(i).updateStats()
                                                              Dim subCollection() As String = New String() {trackList(i).count,
                                                                                                 dll.SecondsTodhmsString(trackList(i).count * trackList(i).length),
                                                                                                 dll.SecondsTohmsString(trackList(i).popularity),
                                                                                                 dll.SecondsTohmsString(trackList(i).length),
                                                                                                 trackList(i).dateString,
                                                                                                 trackList(i).partsCount,
                                                                                                 trackList(i).genre.name}
                                                              subsList.Add(subCollection)
                                                              If progressDialogHandle IsNot Nothing Then progressDialogHandle.updateProgress(progressIndicator, i + 1, trackList.Count)
                                                          Catch ex As OperationCanceledException
                                                              ex = ex
                                                          Catch exGen As Exception
                                                              exGen = exGen
                                                          End Try

                                                      Next
                                                      Return subsList
                                                  End Function)
    End Function


    'folder stats call overlay
    Async Function executeTask(handle As Form, t As Task(Of List(Of Integer()))) As Task(Of List(Of Integer()))
        cts = New CancellationTokenSource()
        token = cts.Token

        formHandle = handle
        handle.Enabled = False
        progressDialogHandle = New ProgressDialog
        progressDialogHandle.TopMost = True
        progressDialogHandle.Show()

        progressIndicator = New Progress(Of Integer)(AddressOf progressDialogHandle.progressBarUpdate)


        Dim res As List(Of Integer()) = Await Task(Of List(Of Integer())).Run(Function() t)
        cancelTask()

        Return res
    End Function

    'totaltime call
    'Async Function executeTask(handle As Form, t As Task(Of Integer)) As Task(Of Integer)
    '    cts = New CancellationTokenSource()
    '    token = cts.Token

    '    formHandle = handle
    '    handle.Enabled = False
    '    progressDialogHandle = New ProgressDialog
    '    progressDialogHandle.TopMost = True
    '    progressDialogHandle.Show()

    '    progressIndicator = New Progress(Of Integer)(AddressOf progressDialogHandle.progressBarUpdate)


    '    Dim res As Integer = Await Task(Of Integer).Run(Function() t)
    '    cancelTask()

    '    Return res
    'End Function

    'track stats call
    Async Function executeTask(handle As Form1, t As Task(Of List(Of String()))) As Task(Of List(Of String()))
        cts = New CancellationTokenSource()
        token = cts.Token

        formHandle = handle
        handle.Enabled = False
        progressDialogHandle = New ProgressDialog
        progressDialogHandle.TopMost = True
        progressDialogHandle.Show()

        progressIndicator = New Progress(Of Integer)(AddressOf progressDialogHandle.progressBarUpdate)


        Dim res As List(Of String()) = Await Task(Of List(Of String())).Run(Function() t)
        cancelTask()

        Return res
    End Function

    'track stats call overlay
    Async Function executeTask(handle As StatsForm, t As Task(Of List(Of String()))) As Task(Of List(Of String()))
        cts = New CancellationTokenSource()
        token = cts.Token

        formHandle = handle
        handle.Enabled = False
        progressDialogHandle = New ProgressDialog
        progressDialogHandle.TopMost = True
        progressDialogHandle.Show()

        progressIndicator = New Progress(Of Integer)(AddressOf progressDialogHandle.progressBarUpdate)


        Dim res As List(Of String()) = Await Task(Of List(Of String())).Run(Function() t)
        cancelTask()

        Return res
    End Function

    'folder stats call
    Async Function executeTask(handle As Form1, t As Task(Of List(Of Folder))) As Task(Of List(Of Folder))
        cts = New CancellationTokenSource()
        token = cts.Token

        formHandle = handle
        handle.Enabled = False
        progressDialogHandle = New ProgressDialog
        progressDialogHandle.TopMost = True
        progressDialogHandle.Show()

        progressIndicator = New Progress(Of Integer)(AddressOf progressDialogHandle.progressBarUpdate)

        Dim res As List(Of Folder) = Await Task(Of List(Of Folder)).Run(Function() t)
        cancelTask()

        Return res
    End Function


    Sub cancelTask()
        Try
            AsyncTask.cts.Cancel()
            AsyncTask.cts.Dispose()
            Try
                progressDialogHandle.Close()
            Catch ex As Exception
            End Try
            AsyncTask.progressDialogHandle = Nothing
            cts = Nothing
        Catch ex As Exception

        Finally
            formHandle.Enabled = True
        End Try

    End Sub
End Module
