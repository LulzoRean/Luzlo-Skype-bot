Imports System.Speech.Synthesis
Imports System.Windows.Threading

Module Funciones
    Public skype As Skype
    Dim conteo As Integer = 0
    Public dt As DispatcherTimer = New DispatcherTimer()
    Dim PingIP As New System.Net.NetworkInformation.Ping
    Dim RespuestaPing As System.Net.NetworkInformation.PingReply
    Public result As New ArrayList
    'Variables para las funciones
    Dim voz As New SpeechSynthesizer 'Sintetizador de voz de windows
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
        result.Add(" ~di {texto}")
    End Function
#End Region

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

#Region "vida"
    Public Function vida()
        result.Add("Sigo vivo " & niceemote())

        Dim num As Integer = conteo
        Dim hor As Integer
        Dim min As Integer
        Dim seg As Integer

        hor = Math.Floor(num / 3600)
        min = Math.Floor((num - hor * 3600) / 60)
        seg = num - (hor * 3600 + min * 60)
        result.Add(Trim(hor) + " Hora(s), " + Trim(min) + " Minuto(s), " + Trim(seg) + " Segundo(s)")

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

#Region "speak"
    Public Function ASpeak(ByVal say As String)
        If skype.ActiveCalls.Count > 0 Then
            Hablar(say)

            result.Add("Speak: " & say)
        Else
            result.Add("No hay llamada en curso, ¿a quien le voy a hablar?")
        End If
      
    End Function
#End Region

#Region "Buitreo"
    Public Function buitreo(ByVal victima As String)
        If victima = "" Then
            result.Add("¿A quien buitreo?")
            Exit Function
        End If
        Dim Saludo() As String = {"Hola", "¿Que cuentas?", "¿Como has estado?", "Jelou", _
                                 "Bonjour", "Buenas"}
        Dim adj() As String = {"hermosa", "preciosa", "bella", "radiante"}

        Dim frase() As String = {"¿estudias o trabajas?", "¿Como ha estado tu dia?", _
                                 "¿Te puedo decir cosas bellas?", "te he estado esperando todo el dia", _
                                 "Mi corazon late al verte conectada", "tengo que decirte algo en privado", _
                                 "¿como seria mi vida sin ti?", "mi corazon palpita como una patata frita"}

        result.Add(Saludo(CInt(Int((Saludo.Count * Rnd()) + 0))) & _
                          " " & victima & " " & adj(CInt(Int((adj.Count * Rnd()) + 0))) & _
                          ", " & frase(CInt(Int((frase.Count * Rnd()) + 0))) & " " & buitreemote)





    End Function
#End Region

#Region "Di"
    Public Function decir(ByVal texto As String)
        If texto = "" Then
            result.Add("¿Que quieres que diga?")
        Else
            result.Add(texto)
        End If

    End Function
#End Region

#Region "Luzlo"
    Public Function luzlo(ByVal texto As String)
        Dim luzloIA2() As String = {"Estaba pensando justo en lo mismo", _
                                    "Otro dia sera, no se cual, pero sera otro", _
                                    "Estor ebrio no se de que hablan xD", _
                                    "Sigo rascandome los huevos.... :D", _
                                   "El buitreo cosmico ha comenzado... "}

        Dim LuzloIA() As String = {"Que pajo?", "Ah?", "Pero dime!", "mmmm ?", _
                                   "Ni me mires, hablale a reisa", "Hola", "y ahora que?", _
                                   "si?"}

        If texto.Length > 10 Then
            Return luzloIA2(CInt(Int((luzloIA2.Count * Rnd()) + 0)))
        Else
            Return LuzloIA(CInt(Int((LuzloIA.Count * Rnd()) + 0)))
        End If

    End Function
#End Region


End Module
