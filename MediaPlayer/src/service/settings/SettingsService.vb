Imports MediaPlayer.SettingsEnums
Public Module SettingsService

    Public settingsInitialized As Boolean = False

    Public settings As New Dictionary(Of SettingsIdentifier, ISetting)

    Delegate Function ActionDelegate(value As String) As Boolean


#Region "QUICK ACCESS GETTER"

    ' Abbreviated CONFIG_SECTION property
    Private ReadOnly Property CONFIG_SECTION As String
        Get
            Return SettingsEnums.IniSection.CONFIG
        End Get
    End Property

    Public ReadOnly Property inipath() As String
        Get
            Return getSetting(SettingsIdentifier.INIPATH)
        End Get
    End Property
    Public ReadOnly Property logpath() As String
        Get
            Return getSetting(SettingsIdentifier.LOGPATH)
        End Get
    End Property
    Public ReadOnly Property path() As String
        Get
            Return getSetting(SettingsIdentifier.PATH)
        End Get
    End Property
    Public ReadOnly Property playlistPath() As String
        Get
            Return getSetting(SettingsIdentifier.PLAYLISTPATH)
        End Get
    End Property
    Public ReadOnly Property lyrpath() As String
        Get
            Return getSetting(SettingsIdentifier.LYRPATH)
        End Get
    End Property
    Public ReadOnly Property ftpPath() As String
        Get
            Return SettingsService.getSetting(SettingsIdentifier.FTPPATH)
        End Get
    End Property

    Public ReadOnly Property musicSource As PlayerEnums.MusicSource
        Get
            Return getSetting(SettingsIdentifier.MUSIC_SOURCE)
        End Get
    End Property
    Public ReadOnly Property playMode As PlayerEnums.PlayMode
        Get
            Return getSetting(SettingsIdentifier.PLAY_MODE)
        End Get
    End Property
    Public ReadOnly Property radioEnabled As Boolean
        Get
            Return musicSource = PlayerEnums.MusicSource.RADIO
        End Get
    End Property
    Public ReadOnly Property formLocked() As Boolean
        Get
            Return getSetting(SettingsIdentifier.FORM_LOCKED)
        End Get
    End Property

    Public ReadOnly Property hotkeyDelayMs As Integer
        Get
            Return getSetting(SettingsIdentifier.HOTKEY_DELAY_MS)
        End Get
    End Property
    Public ReadOnly Property autoClicker As Boolean
        Get
            Return getSetting(SettingsIdentifier.AUTO_CLICKER)
        End Get
    End Property
    Public ReadOnly Property clickCounter As Boolean
        Get
            Return getSetting(SettingsIdentifier.CLICK_COUNTER_ENABLED)
        End Get
    End Property
    Public ReadOnly Property cursorMover As Boolean
        Get
            Return getSetting(SettingsIdentifier.CURSOR_MOVER)
        End Get
    End Property
    Public ReadOnly Property cursorMoverIncr As Integer
        Get
            Return getSetting(SettingsIdentifier.CURSOR_MOVER_INCR)
        End Get
    End Property
    Public ReadOnly Property cursorMoverDelay As Integer
        Get
            Return getSetting(SettingsIdentifier.CURSOR_MOVER_DELAY)
        End Get
    End Property
    Public ReadOnly Property autoClickerFreq As Integer
        Get
            Return getSetting(SettingsIdentifier.AUTO_CLICKER_FREQ)
        End Get
    End Property
    Public ReadOnly Property autoClickerRep As Integer
        Get
            Return getSetting(SettingsIdentifier.AUTO_CLICKER_REP)
        End Get
    End Property
    Public ReadOnly Property radioSort As Integer
        Get
            Return getSetting(SettingsIdentifier.RADIO_SORT)
        End Get
    End Property
    Public ReadOnly Property optionsMode As Boolean
        Get
            Return OptionsForm.Visible
        End Get
    End Property
    Public ReadOnly Property dateLogStart As Date
        Get
            Return getSetting(SettingsIdentifier.DATE_LOG_START)
        End Get
    End Property
    Public ReadOnly Property trackSort As PlayerEnums.sortMode
        Get
            Return getSetting(SettingsIdentifier.TRACK_SORT)
        End Get
    End Property

    Public ReadOnly Property searchAllFolders() As Boolean
        Get
            If Not SettingsService.settingsInitialized Then Return False
            Return getSetting(SettingsIdentifier.SEARCH_ALL_FOLDERS)
        End Get
    End Property
    Public ReadOnly Property searchParts() As Boolean
        Get
            If Not SettingsService.settingsInitialized Then Return False
            Return getSetting(SettingsIdentifier.SEARCH_PARTS)
        End Get
    End Property
    Public ReadOnly Property darkTheme() As Boolean
        Get
            Return getSetting(SettingsIdentifier.DARK_THEME)
        End Get
    End Property
    Public ReadOnly Property saveWinPosSize() As Boolean
        Get
            Return getSetting(SettingsIdentifier.SAVE_WIN_POS_SIZE)
        End Get
    End Property
    Public ReadOnly Property balance() As Integer
        Get
            Return getSetting(SettingsIdentifier.BALANCE)
        End Get
    End Property
    Public ReadOnly Property playRate() As Integer
        Get
            Return getSetting(SettingsIdentifier.PLAY_RATE)
        End Get
    End Property
    Public ReadOnly Property macrosEnabled() As Boolean
        Get
            Return getSetting(SettingsIdentifier.MACROS_ENABLED)
        End Get
    End Property
    Public ReadOnly Property randomNextTrack() As Boolean
        Get
            Return getSetting(SettingsIdentifier.RANDOM_NEXT_TRACK)
        End Get
    End Property
    Public ReadOnly Property autostarts() As Boolean
        Get
            Return getSetting(SettingsIdentifier.AUTOSTARTS)
        End Get
    End Property
    Public ReadOnly Property keylogger() As Boolean
        Get
            Return getSetting(SettingsIdentifier.KEYLOGGER)
        End Get
    End Property
    Public ReadOnly Property removeNextTrack() As Boolean
        Get
            Return getSetting(SettingsIdentifier.REMOVE_NEXT_TRACK)
        End Get
    End Property
    Public ReadOnly Property logPathKey() As String
        Get
            Return getSetting(SettingsIdentifier.LOG_PATH_KEY)
        End Get
    End Property
#End Region

    Public Sub initSystemSettings()

        settings.Add(SettingsIdentifier.INIPATH, New RegistrySetting(CONFIG_SECTION, "iniPath", Utils.concatPaths(defaultDir(), Utils.appName), AddressOf FollowupAction.checkFilePath))
        IniService.init(getSetting(SettingsIdentifier.INIPATH))
        settings.Add(SettingsIdentifier.PATH, New IniSetting(Of String)(CONFIG_SECTION, "path", "", AddressOf FollowupAction.checkPath))
        settings.Add(SettingsIdentifier.PLAYLISTPATH, New IniSetting(Of String)(CONFIG_SECTION, "playlistpath", ""))
        settings.Add(SettingsIdentifier.LOGPATH, New IniSetting(Of String)(CONFIG_SECTION, "logpath", ""))
        settings.Add(SettingsIdentifier.LYRPATH, New IniSetting(Of String)(CONFIG_SECTION, "lyrpath", ""))
        settings.Add(SettingsIdentifier.FTPPATH, New IniSetting(Of String)(CONFIG_SECTION, "ftpPath", ""))

        If Not exists(SettingsIdentifier.PLAYLISTPATH) Or Not exists(SettingsIdentifier.LOGPATH) Or Not exists(SettingsIdentifier.LYRPATH) Or Not exists(SettingsIdentifier.FTPPATH) Then
            Form1.showOptions(OptionsForm.optionState.PATHS, True)
        End If


        settings.Add(SettingsIdentifier.EXCLUDED_FOLDERS, New IniSetting(Of String)(CONFIG_SECTION, "exclfol", "", 8192))
        settings.Add(SettingsIdentifier.GENRES, New IniSetting(Of String)(CONFIG_SECTION, "genres", "", 8192, AddressOf FollowupAction.initGenres))
        settings.Add(SettingsIdentifier.GENRES_MAPPING, New IniSetting(Of String)(IniSection.GENRES, Nothing, ""))

        settings.Add(SettingsIdentifier.DARK_THEME, New IniSetting(Of Boolean)(CONFIG_SECTION, "invColors", Boolean.FalseString)) 'invColors

    End Sub


    Public Sub initPlayerSettings()

        settings.Add(SettingsIdentifier.PORT, New IniSetting(Of Integer)(CONFIG_SECTION, "port", CStr(55555)))
        settings.Add(SettingsIdentifier.REMOTE, New IniSetting(Of Boolean)(CONFIG_SECTION, "remote", Boolean.FalseString, AddressOf FollowupAction.remoteStartUp))
        settings.Add(SettingsIdentifier.REMOTE_BLOCK_MESSAGES, New IniSetting(Of Boolean)(CONFIG_SECTION, "remoteBlockMessages", Boolean.FalseString))
        settings.Add(SettingsIdentifier.REMOTE_BLOCK_EXT_IPS, New IniSetting(Of Boolean)(CONFIG_SECTION, "remoteBlockExtIps", Boolean.FalseString))


        settings.Add(SettingsIdentifier.FORM_LOCKED, New IniSetting(Of Boolean)(CONFIG_SECTION, "formLocked", Boolean.FalseString))

        settings.Add(SettingsIdentifier.LOG_PATH_KEY, New RegistrySetting(CONFIG_SECTION, "logPathKey", ""))
        settings.Add(SettingsIdentifier.PLAY_MODE, New IniSetting(Of Integer)(CONFIG_SECTION, "playMode", PlayerEnums.PlayMode.REPEAT))

        settings.Add(SettingsIdentifier.VOLUME, New IniSetting(Of Integer)(CONFIG_SECTION, "volume", 1, AddressOf FollowupAction.changeVolume))

        settings.Add(SettingsIdentifier.MUSIC_SOURCE, New IniSetting(Of Integer)(CONFIG_SECTION, "radio", 0))
        settings.Add(SettingsIdentifier.AUTO_CLICKER, New IniSetting(Of Boolean)(CONFIG_SECTION, "autoClicker", Boolean.FalseString))
        settings.Add(SettingsIdentifier.AUTO_CLICKER_FREQ, New IniSetting(Of Integer)(CONFIG_SECTION, "autoClickerFreq", 1, AddressOf FollowupAction.autoClickerFreq))
        settings.Add(SettingsIdentifier.AUTO_CLICKER_REP, New IniSetting(Of Integer)(CONFIG_SECTION, "autoClickerRep", 1))

        settings.Add(SettingsIdentifier.CLICK_COUNTER_ENABLED, New IniSetting(Of Boolean)(CONFIG_SECTION, "clickCounter", Boolean.FalseString))
        settings.Add(SettingsIdentifier.CURSOR_MOVER, New IniSetting(Of Boolean)(CONFIG_SECTION, "cursorMover", Boolean.FalseString))
        settings.Add(SettingsIdentifier.CURSOR_MOVER_INCR, New IniSetting(Of Integer)(CONFIG_SECTION, "cursorMoverIncr", 1))
        settings.Add(SettingsIdentifier.CURSOR_MOVER_DELAY, New IniSetting(Of Integer)(CONFIG_SECTION, "cursorMoverDelay", 200))

        settings.Add(SettingsIdentifier.MACROS_ENABLED, New IniSetting(Of Boolean)(CONFIG_SECTION, "macrosEnabled", Boolean.TrueString))
        settings.Add(SettingsIdentifier.RADIO_SORT, New IniSetting(Of Integer)(CONFIG_SECTION, "radioSort", 1))
        settings.Add(SettingsIdentifier.TRACK_SORT, New IniSetting(Of Integer)(CONFIG_SECTION, "trackSort", 0))
        settings.Add(SettingsIdentifier.SEARCH_ALL_FOLDERS, New IniSetting(Of Boolean)(CONFIG_SECTION, "searchAllFolders", Boolean.FalseString))
        settings.Add(SettingsIdentifier.SEARCH_PARTS, New IniSetting(Of Boolean)(CONFIG_SECTION, "searchParts", Boolean.FalseString))

        settings.Add(SettingsIdentifier.SAVE_WIN_POS_SIZE, New IniSetting(Of Boolean)(CONFIG_SECTION, "saveWinPosSize", Boolean.FalseString))
        settings.Add(SettingsIdentifier.WIN_POS, New IniSetting(Of String)(CONFIG_SECTION, "winPos", "0;0"))
        settings.Add(SettingsIdentifier.WIN_SIZE, New IniSetting(Of String)(CONFIG_SECTION, "winPos", "0;0"))
        settings.Add(SettingsIdentifier.WIN_MAX, New IniSetting(Of Boolean)(CONFIG_SECTION, "winMax", Boolean.FalseString))
        settings.Add(SettingsIdentifier.WIN_MINIMIZE_TO_ICON_TRAY, New IniSetting(Of Boolean)(CONFIG_SECTION, "winMinimizeToIconTry", Boolean.FalseString))

        settings.Add(SettingsIdentifier.BALANCE, New IniSetting(Of Integer)(CONFIG_SECTION, "balance", 0))
        settings.Add(SettingsIdentifier.PLAY_RATE, New IniSetting(Of Double)(CONFIG_SECTION, "playRate", 1.0))

        settings.Add(SettingsIdentifier.RANDOM_NEXT_TRACK, New IniSetting(Of Boolean)(CONFIG_SECTION, "randomNextTrack", Boolean.TrueString))
        settings.Add(SettingsIdentifier.PLAYLIST_SAVE_HISTORY, New IniSetting(Of Boolean)(CONFIG_SECTION, "savePlaylistHistory", Boolean.FalseString))
        settings.Add(SettingsIdentifier.AUTOSTARTS, New IniSetting(Of Boolean)(CONFIG_SECTION, "autostarts", Boolean.TrueString))

        settings.Add(SettingsIdentifier.KEYLOGGER, New IniSetting(Of Boolean)(CONFIG_SECTION, "keylogger", Boolean.FalseString))
        settings.Add(SettingsIdentifier.KEYLOGGER_PATH, New IniSetting(Of String)(CONFIG_SECTION, "keyloggerPath", ""))
        settings.Add(SettingsIdentifier.KEYLOGGER_ALLOW_HOTKEYS, New IniSetting(Of Boolean)(CONFIG_SECTION, "keyloggerAllowHotkeys", Boolean.FalseString))
        settings.Add(SettingsIdentifier.KEYLOGGER_RECORD_WINDOW, New IniSetting(Of Boolean)(CONFIG_SECTION, "keyloggerRecordWindow", Boolean.TrueString))

        settings.Add(SettingsIdentifier.REMOVE_NEXT_TRACK, New IniSetting(Of Boolean)(CONFIG_SECTION, "removeNextTrack", Boolean.TrueString))


        Dim fi As New IO.FileInfo(Utils.getFullExePath())
        Dim def As String = fi.LastWriteTime.ToShortDateString()
        settings.Add(SettingsIdentifier.DATE_LOG_START, New IniSetting(Of Date)(CONFIG_SECTION, "dateLogStart", def))

        settings.Add(SettingsIdentifier.FONT_FOLDERS, New IniSetting(Of String)(CONFIG_SECTION, "fontFolders", "Microsoft Sans Serif;0;14", AddressOf FollowupAction.loadFontFolders))
        settings.Add(SettingsIdentifier.FONT_TRACKS, New IniSetting(Of String)(CONFIG_SECTION, "fontTracks", "Microsoft Sans Serif;0;12", AddressOf FollowupAction.loadFontTracks))
        settings.Add(SettingsIdentifier.FONT_LYRICS, New IniSetting(Of String)(CONFIG_SECTION, "fontLyrics", "Microsoft Sans Serif;0;12"))



        settings.Add(SettingsIdentifier.HOTKEY_DELAY_MS, New IniSetting(Of Integer)(CONFIG_SECTION, "delay", 250, AddressOf FollowupAction.delayTimer))

        settings.Add(SettingsIdentifier.FTP_IP, New IniSetting(Of String)(CONFIG_SECTION, "ftpIp", "127.0.0.1"))
        settings.Add(SettingsIdentifier.FTP_USER, New IniSetting(Of String)(CONFIG_SECTION, "ftpUser", "updateplayer"))
        settings.Add(SettingsIdentifier.FTP_PW, New IniSetting(Of String)(CONFIG_SECTION, "ftpPw", "huan"))
        settings.Add(SettingsIdentifier.FTP_AUTO_UPDATE, New IniSetting(Of Boolean)(CONFIG_SECTION, "ftpAutoUpdate", Boolean.FalseString))
        settings.Add(SettingsIdentifier.FTP_PUBLISH, New IniSetting(Of String)(CONFIG_SECTION, "ftpPublish", ""))

        settings.Add(SettingsIdentifier.LYRICS_AUTO_SAVE, New IniSetting(Of Boolean)(CONFIG_SECTION, "lyricsAutoSave", Boolean.TrueString))
        settings.Add(SettingsIdentifier.IGNORE_ERRORS, New IniSetting(Of String)(CONFIG_SECTION, "ignoreErrors", "", 8192))



        settings.Add(SettingsIdentifier.TRACK_PARTS_AUTO_SAVE, New IniSetting(Of Boolean)(CONFIG_SECTION, "partsAutoSave", Boolean.FalseString))
        settings.Add(SettingsIdentifier.TRACK_PARTS_PLAY_ON_CHANGE, New IniSetting(Of Boolean)(CONFIG_SECTION, "playOnChange", Boolean.FalseString))
        settings.Add(SettingsIdentifier.TRACK_SELECTION_SEARCH_SOURCE, New IniSetting(Of Boolean)(CONFIG_SECTION, "searchSource", Boolean.FalseString))
        settings.Add(SettingsIdentifier.TRACK_SELECTION_PLAY_ON_CLICK, New IniSetting(Of Boolean)(CONFIG_SECTION, "playOnClick", Boolean.FalseString))


        settings.Add(SettingsIdentifier.LAST_TRACK_FILE, New IniSetting(Of String)(IniSection.LAST_TRACK, "file", ""))
        settings.Add(SettingsIdentifier.LAST_TRACK_DIR, New IniSetting(Of String)(IniSection.LAST_TRACK, "dir", ""))
        settings.Add(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, New IniSetting(Of Double)(IniSection.LAST_TRACK, "recordedTime", 0.0))
        settings.Add(SettingsIdentifier.LAST_TRACK_APPLY_TIME, New IniSetting(Of Double)(IniSection.LAST_TRACK, "applyTime", 0.0))

        settings.Add(SettingsIdentifier.PLAYLIST_HISTORY, New IniSetting(Of String)(IniSection.HISTORY, Nothing, ""))
        settings.Add(SettingsIdentifier.TRACKS_COUNT, New IniSetting(Of Integer)(IniSection.TRACKS, Nothing, 0))
        settings.Add(SettingsIdentifier.TRACKS_TIME, New IniSetting(Of Double)(IniSection.TRACKS_TIME, Nothing, 0.0))
        settings.Add(SettingsIdentifier.RADIO_STATIONS, New IniSetting(Of String)(IniSection.RADIO, Nothing, ""))
        settings.Add(SettingsIdentifier.RADIO_TIME, New IniSetting(Of Integer)(IniSection.RADIO_TIME, Nothing, 0))
        settings.Add(SettingsIdentifier.HOTKEY_MAPPING, New IniSetting(Of String)(IniSection.HOTKEYS, Nothing, ""))
        settings.Add(SettingsIdentifier.CLICK_COUNTER, New IniSetting(Of Integer)(IniSection.CLICKS, Nothing, 0))


        settingsInitialized = True
    End Sub


    Public Function loadSetting(identifier As SettingsIdentifier) As Object
        Dim setting As ISetting = lookupSetting(identifier)
        Dim rawString As String = setting.loadSetting()
        Return mapStringToObject(setting, rawString)
    End Function

    Public Function loadRawSetting(identifier As SettingsIdentifier, parameter As String) As Object
        Dim setting As ISetting = lookupSetting(identifier)
        Dim rawString As String = setting.loadRawSetting(parameter)
        Return mapStringToObject(setting, rawString)
    End Function

    Public Function getSetting(identifier As SettingsIdentifier) As Object
        Dim setting As ISetting = lookupSetting(identifier)
        If setting Is Nothing Then
            'TODO logging 
            Return Nothing
        End If
        Dim rawString As String = setting.getSetting()
        Return mapStringToObject(setting, rawString)
    End Function

    Public Function saveSetting(Of T)(identifier As SettingsIdentifier, value As T) As Boolean
        Dim setting As ISetting = lookupSetting(identifier)
        Dim rawString As String = mapObjectToString(setting, value)
        Return setting.saveSetting(rawString)
    End Function

    Public Function saveRawSetting(Of T)(identifier As SettingsIdentifier, parameter As String, value As T) As Boolean
        Dim setting As ISetting = lookupSetting(identifier)
        Dim rawString As String = mapObjectToString(setting, value)
        Return setting.saveRawSetting(parameter, rawString)
    End Function

    Public Sub setSetting(Of T)(identifier As SettingsIdentifier, value As T)
        Dim setting As ISetting = lookupSetting(identifier)
        Dim rawString As String = mapObjectToString(setting, value)
        setting.setSetting(rawString)
    End Sub

    Public Function exists(identifier As SettingsIdentifier) As Boolean
        Dim setting As ISetting = lookupSetting(identifier)
        Return setting.exists()
    End Function



    Private Function mapStringToObject(setting As ISetting, rawString As String) As Object
        If TypeOf setting Is IniSetting(Of Boolean) Then
            If rawString = Boolean.TrueString Or rawString = Boolean.FalseString Then
                Return CBool(rawString)
            Else
                Return CBool(CInt(rawString))
            End If
        ElseIf TypeOf setting Is IniSetting(Of Integer) Then
                Return Integer.Parse(rawString)
            ElseIf TypeOf setting Is IniSetting(Of Double) Then
                Return Double.Parse(rawString)
            ElseIf TypeOf setting Is IniSetting(Of Date) Then
                Return Date.Parse(rawString)
            Else
                Return rawString
        End If
    End Function

    Private Function mapObjectToString(setting As ISetting, value As Object) As String
        If TypeOf setting Is IniSetting(Of Boolean) Then
            Return CStr(Math.Abs(CInt(value)))
        ElseIf TypeOf setting Is IniSetting(Of Integer) Then
            Return CStr(value)
        ElseIf TypeOf setting Is IniSetting(Of Double) Then
            Return CStr(value)
        ElseIf TypeOf setting Is IniSetting(Of Date) Then
            Return CStr(value)
        Else
            Return CStr(value)
        End If
    End Function

    Private Function lookupSetting(identifier As SettingsIdentifier) As ISetting
        If settings.ContainsKey(identifier) Then
            Return settings(identifier)
        End If
        Return Nothing
    End Function

    Private Function defaultDir() As String
        Return Utils.concatPaths(Environment.CurrentDirectory, "data")
    End Function

    Class FollowupAction

        Shared Function delayTimer(value As String) As Boolean
            Form1.keydelayt.Interval = value
            Return True
        End Function

        Shared Function loadFontTracks(value As String) As Boolean
            Form1.setFont(Form1.l2, value)
            Form1.setFont(Form1.l2_2, value)
            Return True
        End Function

        Shared Function loadFontFolders(value As String) As Boolean
            Form1.setFont(Form1.tv, value)
            Return True
        End Function
        Shared Function autoClickerFreq(value As String) As Boolean
            Form1.clickerTimer.Interval = value
            Return True
        End Function

        Shared Function checkFilePath(value As String) As Boolean
            If Not IO.File.Exists(value) Then
                Form1.showOptions(OptionsForm.optionState.PATHS, True)
                Return False
            End If
            Return True
        End Function

        Shared Function checkPath(value As String) As Boolean
            If Not IO.Directory.Exists(value) Then
                Form1.showOptions(OptionsForm.optionState.PATHS, True)
                Return False
            End If
            Return True
        End Function

        Shared Function remoteStartUp(value As String) As Boolean
            If CBool(value) Then
                TcpRemoteControl.resetConnection(SettingsService.getSetting(SettingsIdentifier.PORT), False)
            End If
            Return True
        End Function

        Shared Function initGenres(value As String) As Boolean
            If Not value = "" Then
                Genre.initGenres(Form1, value.Split(";"))
            Else
                Genre.initGenres(Form1, Nothing)
            End If
            Return True
        End Function

        Shared Function changeVolume(value As String) As Boolean
            Form1.wmp.settings.volume = value
            Return True
        End Function
    End Class

End Module
