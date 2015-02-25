Imports System.Threading
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.IO

Module Rean
    Public tokenhash As String
    Dim Json As String
    Public Delegate Sub delegado()
<<<<<<< HEAD
    Dim loginuser As String = "killerphantom2"
    Dim loginpw As String = "617de0bb3277d0f94866314cd85537aa"
=======
    Dim loginuser As String = ""
    Dim loginpw As String = ""
>>>>>>> origin/master
    Public ApiLogin As String = "http://api.wowrean.es/1.0/LoginData.php"
    Public ApiRealms As String = "http://api.wowrean.es/1.0/RealmStatus.php"

    Private rootWindow As MainWindow = TryCast(Application.Current.MainWindow, MainWindow)

    ''Propiedades Reinos''
    Public realmstatus As New ArrayList
    Public realmponline As New ArrayList
    Public realmuptime As New ArrayList
    '-------------------------'

#Region "API Rean Login"
    Public Function ApiLoginFunction()
        Dim request As WebRequest = WebRequest.Create(ApiLogin)
        request.Method = "POST"
        Dim postData As String = "username=" & loginuser & "&password=" & loginpw
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = byteArray.Length
        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        Try

            Dim response As WebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Return responseFromServer

        Catch ex As Exception
            Return "00"
        End Try

        'Dim d As New delegado(AddressOf Jsonlogin)
        'Dispatcher.Invoke(d)

    End Function
    Public Function LoginRean()
        Dim JsonPropiedades = New With {Key .success = "", Key .error_message = "", Key .user = "", _
                                       Key .hash = ""}
        Try
            Dim JsonData = JsonConvert.DeserializeAnonymousType(ApiLoginFunction(), JsonPropiedades)
            If JsonData.success = 1 Then
                tokenhash = JsonData.hash
                loginuser = JsonData.user
                rootWindow.log("Conectado : " & loginuser)

            Else
                rootWindow.log("Error")
            End If
        Catch ex As Exception
            rootWindow.log("Error: " & ex.Message)
        End Try

    End Function
#End Region

#Region "API Rean Reinos"
    Public Function apirealmfunction()

        Dim request As WebRequest = WebRequest.Create(ApiRealms)
        request.Method = "POST"
        Dim postData As String = "username=" & Loginuser & "&hash=" & tokenhash
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = byteArray.Length

        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()

        Try
            Dim response As WebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Return responseFromServer
        Catch ex As Exception
            Return "00"
        End Try
    End Function

    Public Function ReinosInfo()
        realmstatus.Clear()
        realmponline.Clear()
        realmuptime.Clear()
        Dim JsonPropiedades = New With {Key .Name = "", Key .Exp = "", Key .Build = "", _
                                               Key .ArenaPoints = "", Key .WeeklyQuest = "", _
                                               Key .Status = "", _
                                               Key .POnline = "", Key .Uptime = ""}

        Try
            Dim PostJson As JArray = JsonConvert.DeserializeObject(apirealmfunction())


            For i As Integer = 0 To 2
                Dim JsonItem As String = PostJson(i).ToString
                Dim JsonArray = JsonConvert.DeserializeAnonymousType(JsonItem, JsonPropiedades)
                realmstatus.Add(JsonArray.Status)
                realmponline.Add(JsonArray.POnline)
                realmuptime.Add(JsonArray.Uptime)
            Next
            Return "OK"
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function
#End Region
End Module
