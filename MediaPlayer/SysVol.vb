Public Class SysVol
    'Windows zugriff
    Private Declare Sub keybd_event Lib "user32" _
    (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    Const KEYEVENTF_KEYDOWN As Integer = 0
    Const KEYEVENTF_KEYUP As Integer = 2
    ' Lauter
    Public Shared Sub system_volume_down()
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeDown), 0, KEYEVENTF_KEYDOWN, 0)  ' Taste runter
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeDown), 0, KEYEVENTF_KEYUP, 0)    ' Taste rauf
    End Sub
    ' Leiser
    Public Shared Sub system_volume_up()
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeUp), 0, KEYEVENTF_KEYDOWN, 0)
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeUp), 0, KEYEVENTF_KEYUP, 0)
    End Sub
    ' Mute
    Public Shared Sub system_volume_mute()
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeMute), 0, KEYEVENTF_KEYDOWN, 0)
        Call keybd_event(CByte(System.Windows.Forms.Keys.VolumeMute), 0, KEYEVENTF_KEYUP, 0)
    End Sub
End Class
