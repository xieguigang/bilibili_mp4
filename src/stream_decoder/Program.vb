Imports System.IO
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection

<CLI>
Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/convert")>
    <Usage("/convert /src <folder_path> [/save <output.mp4>]")>
    Public Function Convert(args As CommandLine) As Integer
        Dim src As String = args("/src")
        Dim save As String = args("/save") Or $"{src}/output.mp4"
        Dim m4s As String() = src.ListFiles("*.m4s").ToArray
        Dim video As String = m4s.Where(Function(s) s.BaseName.EndsWith("30120.m4s")).First
        Dim audio As String = m4s.Where(Function(s) s.BaseName.EndsWith("30280.m4s")).First
        Dim video_mp4 As String = video.ChangeSuffix("mp4")
        Dim audio_aac As String = audio.ChangeSuffix("aac")

        Call stream_offset(video, video_mp4)
        Call stream_offset(audio, audio_aac)

        ' call ffmpeg commandline 
        Return ffmpeg.Combine(video_mp4, audio_aac, save_mp4:=save)
    End Function

    Private Sub stream_offset(filepath As String, save As String)
        Dim s As Stream = filepath.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
        Dim out As Stream = save.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)

        Call s.Seek(9, SeekOrigin.Begin)
        Call s.CopyTo(out)
        Call out.Flush()
        Call out.Close()
        Call s.Close()
    End Sub
End Module
