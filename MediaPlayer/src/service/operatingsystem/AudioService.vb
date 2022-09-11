Imports CoreAudioApi

Public Class AudioService

    Private Declare Sub keybd_event Lib "user32" (bVk As Byte, bScan As Byte, dwFlags As Integer, dwExtraInfo As Integer)
    Const KEYEVENTF_KEYDOWN As Integer = 0
    Const KEYEVENTF_KEYUP As Integer = 2

    Public Shared Sub system_volume_down()
        Call keybd_event(Keys.VolumeDown, 0, KEYEVENTF_KEYDOWN, 0)
        Call keybd_event(Keys.VolumeDown, 0, KEYEVENTF_KEYUP, 0)
    End Sub

    Public Shared Sub system_volume_up()
        Call keybd_event(Keys.VolumeUp, 0, KEYEVENTF_KEYDOWN, 0)
        Call keybd_event(Keys.VolumeUp, 0, KEYEVENTF_KEYUP, 0)
    End Sub

    Public Shared Sub system_volume_mute()
        Call keybd_event(Keys.VolumeMute, 0, KEYEVENTF_KEYDOWN, 0)
        Call keybd_event(Keys.VolumeMute, 0, KEYEVENTF_KEYUP, 0)
    End Sub

    Public Shared Sub SetVolume(ByVal vol As Integer)
        If vol > 100 Then vol = 100
        If vol < 0 Then vol = 0
        Dim DevEnum As New MMDeviceEnumerator
        Dim device As MMDevice = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia)
        device.AudioEndpointVolume.MasterVolumeLevelScalar = vol / 100.0F
    End Sub


    Public Shared Sub setSoundDevice(ByVal dev As String)
        Try
            Shell(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Utils\SoundVolumeView\SoundVolumeView.exe /SwitchDefault " & dev & " 0")
            HotkeyService.startHotkeyDelay(250)
        Catch ex As Exception
            Form1.keyt.Stop()
            MsgBox(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Utils\SoundVolumeView\SoundVolumeView.exe not found." & vbNewLine & "Please install manually to that location.")
            HotkeyService.startHotkeyDelay()
        End Try
    End Sub
End Class
