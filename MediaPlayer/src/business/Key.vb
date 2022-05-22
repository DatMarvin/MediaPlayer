Public Class Key

    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal nVirtKey As Integer) As Short

    Public Shared ReadOnly Property dll As Utils
        Get
            Return Form1.dll
        End Get
    End Property

    Public Shared keyList As New List(Of Key)
    Public keyCombiSet As New List(Of KeyCombi)
    Public name As keyName

    Public Class KeyCombi
        Public key As Keys
        Public modf As modifier = modifier.None

        Public Sub New(key As Keys, modf As modifier)
            Me.key = key
            Me.modf = modf
        End Sub

        Public Overrides Function ToString() As String
            If modf = modifier.None Then
                Return key.ToString()
            Else
                Return modf.ToString() & " + " & key.ToString()
            End If
        End Function
    End Class

    Public Sub New(ByVal name As keyName, combi As KeyCombi)
        Me.name = name
        If Not combi.key = Keys.None Then keyCombiSet.Add(combi)
    End Sub
    Public Sub New(ByVal nameInt As Integer, combi As KeyCombi)
        Me.name = nameInt
        If Not combi.key = Keys.None Then keyCombiSet.Add(combi)
    End Sub

    Public Sub New(ByVal name As keyName, combis() As KeyCombi)
        Me.name = name
        keyCombiSet.AddRange(combis.ToList)
    End Sub
    Public Sub New(ByVal nameInt As Integer, combis() As KeyCombi)
        Me.name = nameInt
        keyCombiSet.AddRange(combis.ToList)
    End Sub

    Public Sub New(ByVal name As keyName, ByVal key As Keys, Optional ByVal modf As modifier = modifier.None)
        Me.name = name
        If Not key = Keys.None Then keyCombiSet.Add(New KeyCombi(key, modf))
    End Sub
    Public Sub New(ByVal name As keyName, ByVal keys() As Keys, Optional ByVal modf As modifier = modifier.None)
        Me.name = name
        For i = 0 To keys.Length - 1
            keyCombiSet.Add(New KeyCombi(keys(i), modf))
        Next
    End Sub



    Public Shared ReadOnly Property ctrlKey As Boolean
        Get
            Return My.Computer.Keyboard.CtrlKeyDown And Not My.Computer.Keyboard.AltKeyDown
        End Get
    End Property

    Public Shared ReadOnly Property altKey As Boolean
        Get
            Return Not My.Computer.Keyboard.CtrlKeyDown And My.Computer.Keyboard.AltKeyDown
        End Get
    End Property
    Public Shared ReadOnly Property altGrKey As Boolean
        Get
            Return My.Computer.Keyboard.CtrlKeyDown And My.Computer.Keyboard.AltKeyDown
        End Get
    End Property
    Public Shared ReadOnly Property shiftKey As Boolean
        Get
            Return My.Computer.Keyboard.ShiftKeyDown
        End Get
    End Property
    Public Shared ReadOnly Property anyModKey As Boolean
        Get
            Return ctrlKey Or altGrKey Or shiftKey
        End Get
    End Property

    Enum modifier As Integer
        None
        Ctrl
        AltGr
        Shift
    End Enum

    Public Enum keyName
        Play_Pause      '1
        Next_Track      '2
        Previous_Track  '3
        Volume_Mute     '4
        Volume_Min      '5
        Volume_Half     '6
        Volume_Max      '7
        Volume_Down     '8
        Volume_Up       '9
        Fast_Forward    '10
        Slow_Forward    '11
        Fast_Rewind     '12
        Slow_Rewind     '13
        Repeat_Mode     '14
        Random_Mode     '15
        Source_Local    '16
        Source_Radio    '17
        Tree_Up         '18
        Tree_Down       '19
        Hotkey_Toggle   '20
        Track_ToQueue   '21
        Track_PlayNext  '22
        Track_Remove    '23
        Track_Delete    '24
        Track_Loop      '25
        Search          '26
        Next_Part       '27
        Previous_Part   '28
        Count_Sub       '29
        Count_Add       '30
        Restore_Window  '31
        Clicker_On      '32
        Clicker_Off     '33
        Cursor_Up       '34
        Cursor_Right    '35
        Cursor_Down     '36
        Cursor_Left     '37
        Macro_1         '38
        Macro_2         '39
        Macro_3         '40
        Macro_4         '41 
        Macro_5         '42
        Macro_6         '43
    End Enum


    Public Shared Sub initKeys()
        keyList.Clear()
        keyList.AddRange({New Key(0, {Keys.NumPad0, Keys.XButton2}),
                            New Key(1, Keys.NumPad6),
                            New Key(2, Keys.NumPad4),
                            New Key(3, Keys.NumPad1, modifier.Ctrl),
                            New Key(4, Keys.NumPad1),
                            New Key(5, Keys.NumPad9, modifier.Ctrl),
                            New Key(6, Keys.NumPad9),
                            New Key(7, Keys.NumPad2),
                            New Key(8, Keys.NumPad8),
                            New Key(9, Keys.NumPad3),
                            New Key(10, Keys.NumPad3, modifier.Ctrl),
                            New Key(11, Keys.NumPad7),
                            New Key(12, Keys.NumPad7, modifier.Ctrl),
                            New Key(13, Keys.Multiply),
                            New Key(14, Keys.Subtract),
                            New Key(15, Keys.NumPad5),
                            New Key(16, Keys.NumPad5, modifier.Ctrl),
                            New Key(17, Keys.NumPad8, modifier.Ctrl),
                            New Key(18, Keys.NumPad2, modifier.Ctrl),
                            New Key(19, Keys.Scroll),
                            New Key(20, Keys.Add),
                            New Key(21, Keys.Add, modifier.Ctrl),
                            New Key(22, Keys.Delete),
                            New Key(23, Keys.Delete, modifier.Shift),
                            New Key(24, Keys.Divide),
                            New Key(25, Keys.Decimal),
                            New Key(26, Keys.NumPad6, modifier.Ctrl),
                            New Key(27, Keys.NumPad4, modifier.Ctrl),
                            New Key(28, Keys.Multiply, modifier.Ctrl),
                            New Key(29, Keys.Subtract, modifier.Ctrl),
                            New Key(30, Keys.ShiftKey, modifier.AltGr),
                            New Key(31, Keys.F6, modifier.AltGr),
                            New Key(32, {Keys.F6, Keys.F7}),
                            New Key(33, Keys.Up, modifier.AltGr),
                            New Key(34, Keys.Right, modifier.AltGr),
                            New Key(35, Keys.Down, modifier.AltGr),
                            New Key(36, Keys.Left, modifier.AltGr),
                            New Key(37, Keys.F9, modifier.AltGr),'macro1
                            New Key(38, Keys.F10, modifier.AltGr),'macro2
                            New Key(39, Keys.PrintScreen),'macro3
                            New Key(40, Keys.F8),'macro4
                            New Key(41, Keys.SelectMedia),'macro5
                            New Key(42, Keys.F11, modifier.AltGr)}) 'macro6
        For k = 0 To keyList.Count - 1
            Dim commString As String = dll.iniReadValue("Hotkeys", keyList(k).ToString(), "", Form1.inipath)
            If commString <> "" Then
                If isValidCombination(commString) Then
                    Dim keyCombis As List(Of KeyCombi) = getKeyCombisFromString(commString)
                    'Dim exist As Key = getKeyByKeyCombiset(keyCombis.ToArray)
                    'If exist IsNot Nothing Then
                    '    dll.iniDeleteKey("Config", exist.ToString, Form1.inipath)
                    'End If
                    keyList(k) = New Key(keyList(k).name, keyCombis.ToArray)
                End If
            End If
        Next

    End Sub

    Public Shared Function isValidCombination(ByVal s As String) As Boolean
        s = s.Replace(" ", "")
        Dim combis() As String = s.Split(";")
        If combis.Length >= 1 AndAlso combis(0) <> "" Then
            If Array.TrueForAll(combis, Function(x) isCombi(x)) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function getModifierFromString(ByVal s As String) As modifier
        Dim parts() As String = s.Replace(" ", "").Split("+")
        If parts.Length = 1 Then Return modifier.None
        Return DirectCast([Enum].Parse(GetType(modifier), parts(0)), modifier)
    End Function

    Public Shared Function getKeyCombisFromString(ByVal s As String) As List(Of KeyCombi)
        Dim res As New List(Of KeyCombi)
        Dim combis() As String = s.Replace(" ", "").Split(";")
        For Each combi As String In combis
            Dim splitString() As String = combi.Split("+")
            Dim keyPart As String = splitString(IIf(splitString.Length > 1, 1, 0))
            Dim modf As modifier = modifier.None
            If splitString.Length > 1 Then modf = DirectCast([Enum].Parse(GetType(modifier), splitString(0)), modifier)
            res.Add(New KeyCombi(DirectCast([Enum].Parse(GetType(Keys), keyPart), Keys), modf))
        Next
        Return res
    End Function


    Public Shared Function keyCombinationExists(ByVal key As Keys, Optional ByVal modi As modifier = modifier.None) As Key
        For i = 0 To keyList.Count - 1
            For Each k As KeyCombi In keyList.Item(i).keyCombiSet
                If k.key = key Then
                    If k.modf = modi Then
                        Return keyList(i)
                    End If
                End If
            Next
        Next
        Return Nothing
    End Function



    Public Shared Function isCombi(ByVal s As String) As Boolean
        If Not s.Contains("+") Then Return isCombi(s, "")
        Return isCombi(s.Split("+")(1), s.Split("+")(0))
    End Function
    Public Shared Function isCombi(ByVal keyString As String, modfString As String) As Boolean
        For i = 1 To 255
            Dim k As Keys = i
            If k.ToString = keyString Then
                If modfString = "" Then Return True
                For j = 1 To 3
                    Dim m As modifier = j
                    If m.ToString = modfString Then Return True
                Next
            End If
        Next
        Return False
    End Function

    Public Shared Function isKey(ByVal s As String) As Boolean
        For i = 1 To 255
            Dim m As Keys = i
            If m.ToString = s Then Return True
        Next
        Return False
    End Function
    Public Shared Function isModifier(ByVal s As String) As Boolean
        For i = 1 To 3
            Dim m As modifier = i
            If m.ToString = s Then Return True
        Next
        Return False
    End Function

    Public Function pressed() As Boolean
        For Each k As KeyCombi In keyCombiSet
            If Not GetAsyncKeyState(k.key) = 0 Then
                If Not Form1.keydelayt.Enabled Then
                    Select Case k.modf
                        Case modifier.Ctrl
                            If ctrlKey Then Return True
                        Case modifier.AltGr
                            If altGrKey Then Return True
                        Case modifier.Shift
                            If shiftKey Then Return True
                        Case modifier.None
                            If Not ctrlKey And Not altGrKey And Not shiftKey Then Return True
                    End Select
                End If
            End If
        Next
        Return False
    End Function

    Public Shared Function getKey(ByVal name As String) As Key
        For Each k As Key In keyList
            If k.ToString() = name Then
                Return k
            End If
        Next
        Return Nothing
    End Function

    Public Sub execute()
        Form1.keyExecute(Me)
    End Sub
    Public Overrides Function ToString() As String
        Return name.ToString()
    End Function

End Class
