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

    End Function
End Module
