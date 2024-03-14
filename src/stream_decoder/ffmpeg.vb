Module ffmpeg

    Public Function Combine(video As String, audio As String, save_mp4 As String) As Integer
        Dim ffmpeg As String = $"{App.HOME}/"

        ' ffmpeg -i video.mp4 -i audio.aac -c:v copy -strict experimental save.mp4
    End Function
End Module
