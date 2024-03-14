Imports System.IO
Imports System.IO.Compression
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.Pipeline

Module ffmpeg

    ReadOnly ffmpeg As String = $"{App.HOME}/ffmpeg.exe"

    Sub New()
        Using ms As New MemoryStream(My.Resources.ffmpeg)
            Dim zip As New ZipArchive(ms, ZipArchiveMode.Read)
            Dim release As Stream = ffmpeg.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
            Dim app_stream = zip.GetEntry("ffmpeg.exe").Open

            Call app_stream.CopyTo(release)
            Call release.Flush()
            Call release.Close()
            Call app_stream.Close()
            Call zip.Dispose()
        End Using
    End Sub

    Public Function Combine(video As String, audio As String, save_mp4 As String) As Integer
        Dim code As Integer = 0
        ' ffmpeg -i video.mp4 -i audio.aac -c:v copy -strict experimental save.mp4
        Dim print As String = PipelineProcess.Call(ffmpeg, $"-i {video.CLIPath} -i {audio.CLIPath} -c:v copy -strict experimental {save_mp4.CLIPath}", exitCode:=code)
        Dim logfile As String = save_mp4.ChangeSuffix("log")

        Call print.SaveTo(logfile)

        Return code
    End Function
End Module
