Public Class Player

    Private Shared formHandle As Form1

    Private Shared ReadOnly Property wmp() As AxWMPLib.AxWindowsMediaPlayer
        Get
            Return formHandle.wmp
        End Get
    End Property

    Public Shared Sub initPlayer(formHandle As Form1)
        Player.formHandle = formHandle
    End Sub

    Public Shared Function getCurrentMedia() As WMPLib.IWMPMedia
        Return wmp.currentMedia
    End Function
    Public Shared Sub setCurrentMedia(value As WMPLib.IWMPMedia)
        wmp.currentMedia = value
    End Sub

    Public Shared Function getCurrentPosition() As Double
        Return wmp.Ctlcontrols.currentPosition
    End Function
    Public Shared Sub setCurrentPosition(value As Double)
        wmp.Ctlcontrols.currentPosition = value
    End Sub
    Public Shared Sub increaseCurrentPosition(value As Double)
        setCurrentPosition(getCurrentPosition() + value)
    End Sub
    Public Shared Sub decreaseCurrentPosition(value As Double)
        setCurrentPosition(getCurrentPosition() - value)
    End Sub
    Public Shared Function getVolume() As Integer
        Return wmp.settings.volume
    End Function
    Public Shared Sub setVolume(value As Integer)
        wmp.settings.volume = value
    End Sub
    Public Shared Sub increaseVolume(value As Integer)
        setVolume(getVolume() + value)
    End Sub
    Public Shared Sub decreaseVolume(value As Integer)
        setVolume(getVolume() - value)
    End Sub
    Public Shared Function getPlayRate() As Double
        Return wmp.settings.rate
    End Function
    Public Shared Sub setPlayRate(value As Double)
        wmp.settings.rate = value
    End Sub

    Public Shared Function getBalance() As Integer
        Return wmp.settings.balance
    End Function
    Public Shared Sub setBalance(value As Integer)
        wmp.settings.balance = value
    End Sub
    Public Shared Function isMute() As Boolean
        Return wmp.settings.mute
    End Function
    Public Shared Sub setMute(value As Boolean)
        wmp.settings.mute = value
    End Sub
    Public Shared Sub mute()
        setMute(True)
    End Sub
    Public Shared Sub unmute()
        setMute(False)
    End Sub

    Public Shared Function getUrl() As String
        Return wmp.URL
    End Function
    Public Shared Function isUrlEmpty() As Boolean
        Return wmp.URL = ""
    End Function
    Public Shared Sub setUrl(value As String)
        wmp.URL = value
    End Sub
    Public Shared Sub resetUrl()
        setUrl("")
    End Sub

    Public Shared Function newMedia(url As String) As WMPLib.IWMPMedia
        Return wmp.newMedia(url)
    End Function

    Public Shared Function getPlayState() As WMPLib.WMPPlayState
        Return wmp.playState
    End Function

    Public Shared Sub showPropertyPages()
        wmp.ShowPropertyPages()
    End Sub
    Public Shared Sub pause()
        wmp.Ctlcontrols.pause()
    End Sub

    Public Shared Sub play()
        wmp.Ctlcontrols.play()
    End Sub
End Class
