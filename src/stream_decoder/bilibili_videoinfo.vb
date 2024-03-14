Public Class bilibili_videoinfo

    Public Property groupId As String
    Public Property bvid As String
    Public Property title As String

    Public Function DefaultFileName() As String
        Return $"{bvid} - {title.NormalizePathString(False,)}"
    End Function

End Class
