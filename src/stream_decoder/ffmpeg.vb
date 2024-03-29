﻿Imports System.IO
Imports System.IO.Compression
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine

Module ffmpeg

    ReadOnly ffmpeg As String = $"{App.HOME}/ffmpeg.exe"

    Sub New()
        If ffmpeg.FileLength < 100 * 1024 * 1024 Then
            Call release_ffmpeg()
        End If
    End Sub

    Private Sub release_ffmpeg()
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
        ' ffmpeg -i video.mp4 -i audio.aac -c:v copy -strict experimental save.mp4
        Dim args As String = $"-i {video.CLIPath} -i {audio.CLIPath} -c:v copy -strict experimental {save_mp4.CLIPath}"
        Dim run As New ProcessStartInfo With {.FileName = ffmpeg, .Arguments = args, .CreateNoWindow = True, .RedirectStandardInput = True}
        Dim call_ffmpeg As Process = Process.Start(run)

        Call call_ffmpeg.StandardInput.WriteLine("y")
        Call call_ffmpeg.WaitForExit()

        Return call_ffmpeg.ExitCode
    End Function
End Module
