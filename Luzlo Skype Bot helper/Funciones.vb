Imports System.Speech.Synthesis
Imports System.Speech.Recognition
Imports System.Speech.Recognition.SrgsGrammar
Imports System.Windows.Threading
Imports NCalc
Imports AIMLbot
Imports System.Collections.Generic
Imports System.Threading
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.IO
Imports System.Web

Module Funciones
    Public recognizer As SpeechRecognizer


    Public mybot As New Bot
    Public myuser As New User("Luzlo", mybot)
    Public skype As Skype



    Dim conteo As Integer = 0
    Public dt As DispatcherTimer = New DispatcherTimer()
    Dim PingIP As New System.Net.NetworkInformation.Ping
    Dim RespuestaPing As System.Net.NetworkInformation.PingReply
    Public result As New ArrayList
    'Variables para las funciones
    Dim voz As New SpeechSynthesizer

    'Sintetizador de voz de windows
    'Emoticones "Buenos" para respuestas random
    Dim EmoticonVar1() As String = {"(poolparty)", ":3", ";)", _
                                    "(heidy)", ":-)", ":P", ":)", "(sun)"}
    'Emoticones "Malos" para respuestas random
    Dim EmoticonVar2() As String = {"(puke)", "(facepalm)", ":S", _
                                    "(:|", "(envy)", ":(", "|-("}

    Dim EmoticonBuitre() As String = {"(mm)", ":*", ":$", "(heart)", _
                                      "(inlove)", "(hug)"}

  
    Public Sub Hablar(ByVal decir As String)
        voz.SpeakAsync(decir)
    End Sub
    Public Function buitreemote()
        Return EmoticonBuitre(CInt(Int((EmoticonBuitre.Count * Rnd()) + 0)))
    End Function
    Public Function bademote()
        Return EmoticonVar2(CInt(Int((EmoticonVar2.Count * Rnd()) + 0)))
    End Function
    Public Function niceemote()
        Return EmoticonVar1(CInt(Int((EmoticonVar1.Count * Rnd()) + 0)))
    End Function
    Sub contador()
        conteo = conteo + 1
    End Sub

    '---------------FUNCIONES POR COMANDOS -------------------------
#Region "List"
    Public Function list()
        If skype.ActiveChats.Count > 0 Then
            Hablar("Aqui esta la lista de comandos.")
        End If
        result.Add("Lista de comandos: ")
        result.Add(" ~thalassa")
        result.Add(" ~aegwynn")
        result.Add(" ~vulcania")
        result.Add(" ~totalplayers")
        result.Add(" ~ping {ip/url}")
        result.Add(" ~list")
        result.Add(" ~vida")
        result.Add(" ~call {usuario skype}")
        result.Add(" ~speak {texto a pronunciar}")
        result.Add(" ~buitrear {victima}")
        result.Add(" ~insultar {victima}")
        result.Add(" ~chiste")
        result.Add(" ~adv")
        result.Add(" ~tqd")
        result.Add(" ~di {texto}")
        result.Add(" ~buscarimg {argumento de busqueda}")
        result.Add(" ~play {-list 'Muestra lista de sonidos' / sonido}")
    End Function
#End Region

    'Ping Finalizado
#Region "Ping"
    Public Function ping(ByVal argumento As String)

        Try
            Hablar("Haciendo ping")
            RespuestaPing = PingIP.Send(argumento)
            If RespuestaPing.RoundtripTime = 0 Then
                result.Add("Error de ping " & bademote())
            Else
                result.Add("Pong " & niceemote())
                result.Add("Ping a: " & argumento & " = " & RespuestaPing.RoundtripTime & " Ms")
            End If

        Catch ex As Exception
            result.Add(ex.Message & " " & bademote())
        End Try
    End Function
#End Region

    'Vida Finalizado
#Region "vida"
    Public Function vida()
        result.Add("Estoy vivo " & niceemote())

        Dim num As Integer = conteo
        Dim hor As Integer
        Dim min As Integer
        Dim seg As Integer

        hor = Math.Floor(num / 3600)
        min = Math.Floor((num - hor * 3600) / 60)
        seg = num - (hor * 3600 + min * 60)
        result.Add("Online: " & Trim(hor) + " Hora(s), " + Trim(min) + " Minuto(s), " + Trim(seg) + " Segundo(s)")

    End Function

#End Region

#Region "Call"
    Public Function sCall(ByVal handler As String)
        If skype.ActiveCalls.Count > 0 Then
            result.Add("Actualmente ya hay una llamada")
        Else
            result.Add("Llamando...")
            Dim a As [Call] = skype.PlaceCall(handler)

            System.Threading.Thread.Sleep(2500)
            If skype.ActiveChats.Count > 0 Then
                Hablar("Hola, ya estoy en linea.")
                result.Add("En linea")
            End If
        End If
    End Function
#End Region

    'Speak Finalizado
#Region "speak"
    Public Function ASpeak(ByVal say As String)
        If skype.ActiveCalls.Count > 0 Then
            Hablar(say)
            result.Add("")
        Else
            result.Add("No hay llamada en curso, ¿a quien le voy a hablar?")
        End If
    End Function
#End Region

#Region "Buitreo"
    Public Function buitreo(ByVal victima As String)
        If victima = "" Or victima = " " Then
            result.Add("¿A quien buitreo?")
            Exit Function
        End If
        Dim Saludo() As String = {"", "Hola", "¿Que cuentas?", "¿Como has estado?", "Jelou", _
                                 "Bonjour", "Buenas", ""}
        Dim adj() As String = {"", "hermosa", "preciosa", "bella", "radiante", "dulce", ""}

        Dim frase() As String = {"¿estudias o trabajas?", "¿Como ha estado tu dia?", _
                                 "¿Te puedo decir cosas bellas?", "te he estado esperando todo el dia", _
                                 "Mi corazon late al verte conectada", "tengo que decirte algo en privado", _
                                 "¿como seria mi vida sin ti?", "mi corazon palpita como una patata frita", _
                                 "Me gustaria verte, mandame una foto", "hoy estas mas hermosa que nunca", _
                                 "Tengo algo que mostrarte", "Hace rato no me dices cosas bellas", _
                                 "ven te invito a salir y nos tomamos una cerveza.", "grrrrrrrrr", _
                                 "y si nos vamos tomados de las manos?", "(mm) (mm) (mm)", _
                                 "¿crees en el amor a primera vista o tengo que volver a pasar delante de ti?"}

        result.Add(Saludo(CInt(Int((Saludo.Count * Rnd()) + 0))) & _
                          " " & victima & " " & adj(CInt(Int((adj.Count * Rnd()) + 0))) & _
                          ", " & frase(CInt(Int((frase.Count * Rnd()) + 0))) & " " & buitreemote())




    End Function
#End Region

    'Di finalizado
#Region "Di"
    Public Function decir(ByVal texto As String)
        If texto = "" Then
            result.Add("¿Que quieres que diga?")
        Else
            result.Add(texto)
        End If

    End Function
#End Region

    'Luzlo finalizado
#Region "Luzlo"
    Public Function luzlo(ByVal texto As String)
        Dim r As Request = New Request(texto, myuser, mybot)
        Dim resu As Result = mybot.Chat(r)
        Return resu.Output

        'Dim luzloIA2() As String = {"Estaba pensando justo en lo mismo", _
        '                            "Otro dia sera, no se cual, pero sera otro", _
        '                            "Estor ebrio no se de que hablan xD", _
        '                            "Sigo rascandome los huevos.... " & niceemote(), _
        '                           "El buitreo cosmico ha comenzado... ", _
        '                            "Estoy aburrido, algo para hacer?" & niceemote(), _
        '                            "Ni idea", _
        '                            "Creo que no me gusta eso", _
        '                            "Creo que esta funcion la programaron mal " & bademote(), _
        '                            "La cerveza ya esta fria! " & niceemote(), _
        '                            "Ya vuelvo, voy a ponerme los pantalones"}

        'Dim LuzloIA() As String = {"Que pajo?", "Ah?", "Pero dime!", "mmmm ?", _
        '                           "Ni me mires, hablale a reisa", "Hola", "y ahora que?", _
        '                           "si?"}

        'If texto.Length > 10 Then
        '    Return luzloIA2(CInt(Int((luzloIA2.Count * Rnd()) + 0)))
        'Else
        '    Return LuzloIA(CInt(Int((LuzloIA.Count * Rnd()) + 0)))
        'End If

    End Function
#End Region

#Region "play"
    Public Function playsonido(ByVal argumento As String)
        If skype.ActiveCalls.Count > 0 Then
            If argumento = "-list" Then
                result.Add("No hay archivos de sonido indexados")
            Else
                result.Add("No se encuentra sonido, funcion sin implementar")
            End If
        Else
            result.Add("No hay llamada en curso, ¿Quien va a escuchar?")
        End If
    End Function
#End Region

    'Math finalizado
#Region "Math"
    Public Function IgualMath(ByVal expresion As String)
        If expresion = "" Then
            result.Add("Que expresion debo evaluar?")
        Else
            Try
                Dim a As Expression = New Expression(expresion)
                Return (": " & a.Evaluate().ToString)
            Catch ex As Exception
                Return ("Error: " & ex.Message)
            End Try
        End If
    End Function
#End Region

#Region "REINOS -Thalassa , -Aegwynn , -Vulcania"
    Public Function reinos(ByVal Reino As Integer)
        Dim Control As String = ReinosInfo()
        Dim ThalaTime As DateTime = CDate(Date.UtcNow.AddHours(-5)) 'Thalassa UTC -5
        Dim AeTime As DateTime = CDate(Date.UtcNow.AddHours(-6)) 'Aewgynn UTC -6
        Dim VulcaTime As DateTime = CDate(Date.UtcNow.AddHours(-6)) 'Vulcania UTC -6
        If Control = "OK" Then
            Dim estado As String
            If realmstatus(Reino) = 1 Then
                estado = "Online"
            Else
                estado = "Offline"
                result.Add("Estado: " & estado)
                Exit Function
            End If

            Select Case Reino
                Case 0 'Thalassa ------------------------------------------------------
                    result.Add("Estado: " & estado & " - " & ThalaTime.ToString("HH:mm") & " ST")
                    result.Add(uptime(realmuptime(0)))
                    RespuestaPing = PingIP.Send("192.99.147.87")
                    result.Add("Jugadores Online: " & realmponline(Reino) & ", (" & RespuestaPing.RoundtripTime & "Ms)")
                Case 1 'Aegwynn ---------------------------------------------------------
                    result.Add("Estado: " & estado & " - " & AeTime.ToString("HH:mm") & " ST")
                    result.Add(uptime(realmuptime(1)))
                    RespuestaPing = PingIP.Send("37.187.132.56")
                    result.Add("Jugadores Online: " & realmponline(Reino) & ", (" & RespuestaPing.RoundtripTime & "Ms)")
                Case 2 'Vulcania ----------------------------------------------------------
                    result.Add("Estado: " & estado & " - " & VulcaTime.ToString("HH:mm") & " ST")
                    result.Add(uptime(realmuptime(2)))
                    RespuestaPing = PingIP.Send("192.99.200.93")
                    result.Add("Jugadores Online: " & realmponline(Reino) & ", (" & RespuestaPing.RoundtripTime & "Ms)")

            End Select
        Else
            result.Add("Error al obtener datos del reino")
        End If
    End Function
#End Region

#Region "Total Players"
    Public Function TotalP()
        Dim Control As String = ReinosInfo()
        Dim thalap As Integer = realmponline(0)
        Dim aep As Integer = realmponline(1)
        Dim vulcap As Integer = realmponline(2)

        If Control = "OK" Then
            result.Add("Jugadores Online totales: " & thalap + aep + vulcap)
        Else
            result.Add("Error al obtener datos de los reinos")
        End If
    End Function
#End Region

#Region "IMGFind"
    Public Function buscarimg(ByVal argumento As String)
        argumento = argumento.Replace(" ", "%20")

1:      Dim json As String
        'Conexion API Google:
        Dim webClient As New System.Net.WebClient

        Try
            Dim result As String = webClient.DownloadString("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=8&imgsz=xlarge&as_filetype=jpg&q=" & argumento)
            json = result
        Catch ex As Exception
            result.Add("ErrorIMG : " & ex.Message)
        End Try

        Dim JsonPropiedades = New With {Key .results = "results"}
        Dim JsonPropiedades2 = New With {Key .unescapedUrl = "unescapedUrl"}

        Dim JsonConverter = JsonConvert.DeserializeObject(json)
        Dim a As String = JsonConverter.ToString

        Try

            a = a.Substring(a.IndexOf("["), a.IndexOf("]"))
            a = a.Substring(0, a.IndexOf("]")) & "]"

        Catch ex As Exception
            'GoTo 1
            result.Add("ErrorIMG : " & ex.Message)
        End Try

        Try
            Dim JSonA As JArray = JsonConvert.DeserializeObject(a)
            Dim JsonItem As String = JSonA(CInt(Int((JSonA.Count * Rnd()) + 0))).ToString

            Dim JsonReturn = JsonConvert.DeserializeAnonymousType(JsonItem, JsonPropiedades2)
            result.Add(JsonReturn.unescapedUrl.ToString)
        Catch ex As Exception
            result.Add("ErrorIMG : " & ex.Message)
        End Try

    End Function
#End Region

#Region "Chiste"
    Public Function chiste()
        Dim webClient As New System.Net.WebClient
        Try
1:          Dim resultado As String = webClient.DownloadString("http://www.chistes.com/ChisteAlAzar.asp?n=4")
          
            resultado = resultado.Substring(resultado.IndexOf("<div class=""chiste"">"))
            resultado = resultado.Substring(0, resultado.IndexOf("</div>"))
            resultado = resultado.Replace("<div class=""chiste"">", "")

            If resultado.Length > 400 Then
                GoTo 1
            End If
            result.Add(resultado.Replace("<BR>", "").Replace("\r", ""))
        Catch ex As Exception
            MsgBox(ex.Message)
            result.Add("No pude conectarme a mi servidor de chistes ...")
        End Try

    End Function
#End Region

#Region "Insultar"
    Public Function insultar(ByVal victima As String)
        If victima = "" Or victima = " " Then
            result.Add("¿A quien insulto?")
            Exit Function
        End If
        If victima.Contains("luzlo") Then
            Dim luz() As String = {"¿me crees pendejo?", "Acaso crees que soy como vos?", _
                                   "Andate a dormir mejor", "Eres puto sabelo!"}
            result.Add(luz(CInt(Int((luz.Count * Rnd()) + 0))))
            Exit Function
        End If
        If victima.Contains("lulzo") Then
            result.Add("Lulzo eres el rey, el mejor, mi amor incondicional .. gracias por todo creador")
            Exit Function
        End If
        Dim Saludo() As String = {"", "mmmmmm", "pues.., ", "eeeeem", "", _
                                 "", "", ""}
        Dim adj() As String = {"", "te digo que", "pero si", "", "", "", ""}

        Dim frase() As String = {"Eres una persona tan fea que el día que naciste te lanzaron al techo y dijeron ""Si vuela es un murciélago"".", _
                                 "Eres una persona tan fea que tu madre te dijo que sólo te quería como amigo.", _
                                 "Tienes el culo tan gordo que le han puesto su propio Código Postal.", _
                                 "Tienes el culo tan gordo que en el colegio te sentabas al lado de TODOS los niños.", _
                                 "Eres tan tonto que si vas a un concurso de tontos te echan por dopaje.", _
                                 "Eres tan tonto que si fueras a unas Olimpiadas de tontos te darían dos medallas, por si pierdes la primera.", _
                                 "Te huele tan mal el aliento que se lo echaste a una cebolla y ésta acabó llorando.", _
                                 "Eras un bebé tan feo que, en lugar de darte el pecho, te dieron la espalda.", _
                                 "Bebes tanto alcohol que como te acerquen una cerilla vas a estar tres días ardiendo.", _
                                 "¿Sabes cómo se deja a alguien corto de ideas en suspense? Luego te lo cuento.", _
                                 "El día que los tontos vuelen te ataré un cordel al tobillo para no perderte de vista."}

        result.Add(Saludo(CInt(Int((Saludo.Count * Rnd()) + 0))) & _
                          " " & victima & " " & adj(CInt(Int((adj.Count * Rnd()) + 0))) & _
                          ", " & frase(CInt(Int((frase.Count * Rnd()) + 0))) & " " & bademote())




    End Function
#End Region

#Region "ADV"
    Public Function adv()
        Dim webClient As New System.Net.WebClient
        Try
            Dim resultado As String = webClient.DownloadString("http://www.ascodevida.com/aleatorio")
            resultado = resultado.Substring(resultado.IndexOf("<p class=""story_content"">"))
            resultado = resultado.Substring(0, resultado.IndexOf("</p>"))
            resultado = resultado.Replace("<p class=""story_content"">", "")
            resultado = resultado.Substring(resultado.IndexOf(">") + 1)

            Dim a As String = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(resultado))

            result.Add(a.Replace("<BR>", "").Replace("</a>", ""))

        Catch ex As Exception

            result.Add("No pude conectarme a ADV")
        End Try


    End Function
#End Region
#Region "TQD"
    Public Function tqd()
        Dim webClient As New System.Net.WebClient
        Try
            Dim resultado As String = webClient.DownloadString("http://www.teniaquedecirlo.com/aleatorio")
            resultado = resultado.Substring(resultado.IndexOf("<p class=""story_content"">"))
            resultado = resultado.Substring(0, resultado.IndexOf("</p>"))
            resultado = resultado.Replace("<p class=""story_content"">", "")
            resultado = resultado.Substring(resultado.IndexOf(">") + 1)

            Dim a As String = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(resultado))

            result.Add(a.Replace("<BR>", "").Replace("</a>", ""))

        Catch ex As Exception

            result.Add("No pude conectarme a TQD")
        End Try


    End Function
#End Region


    Public Function uptime(ByVal segundos As Integer)

        Dim num As Integer = segundos
        Dim hor As Integer = Math.Floor(num / 3600)
        Dim min As Integer = Math.Floor((num - hor * 3600) / 60)
        Dim seg As Integer = num - (hor * 3600 + min * 60)

        Return ("Online: " & Trim(hor) + " Hora(s), " + Trim(min) + " Minuto(s), " + Trim(seg) + " Segundo(s)")

    End Function
End Module
