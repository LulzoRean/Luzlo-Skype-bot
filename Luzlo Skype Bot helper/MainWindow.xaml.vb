Imports System.Speech.Recognition
Imports System.Speech.Synthesis
Imports SKYPE4COMLib
Imports System
Imports System.Threading
Imports NCalc
Imports AIMLbot
Class MainWindow
    Dim TriggerComando As String = "-"
    Dim TriggerSpeech As String = "luzlo"

    Public Sub log(ByVal a As String)
        LogList.Items.Add(a)
        LbRegistro.Content = LogList.Items.Count
    End Sub

    'Iniciando la aplicacion, funcion de inicio
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Hablar("Iniciando el bot")
        log("Iniciando el bot")
        'BOT

        mybot.loadSettings()
        mybot.isAcceptingUserInput = False
        mybot.loadAIMLFromFiles()
        mybot.isAcceptingUserInput = True


        'Inicio de proceso API Skype
        AddHandler dt.Tick, AddressOf contador
        dt.Interval = (New TimeSpan(0, 0, 1))
        dt.Start()

        Try

            skype = New Skype
            skype.Attach(7, False)
            AddHandler skype.MessageStatus, AddressOf skype_stat 'Proceso de lectura de chat (attach)

        Catch ex As Exception
            log("--ERROR! --" & ex.Message)
            Hablar(ex.Message)
            ' errorclose()
        End Try

        'Inicio de Api Exitoso en skype
        log("Skype attach : OK")
        Hablar("Bot iniciado y adherido al proceso")
        log("Iniciando seccion en WowRean")
        Dim d As New delegado(AddressOf LoginRean)
        Me.Dispatcher.Invoke(d)
        Me.WindowState = Windows.WindowState.Minimized
    End Sub

    'Funcion de lectura de mensajes del chat
    Public Sub skype_stat(ByVal msg As ChatMessage, ByVal status As TChatMessageStatus)


        If status = TChatMessageStatus.cmsRead Then 'Comprobacion antiredundancia de lectura
            Return
        End If

        ' log(msg.Body & " Despues -  " & msg.Status)
        'Inicio de funcion con Trigger "-"
        If (msg.Body.IndexOf(TriggerComando) = 0 And status = TChatMessageStatus.cmsReceived) Or (msg.Body.IndexOf(TriggerComando) = 0 And status = TChatMessageStatus.cmsSending) Then 'And status = TChatMessageStatus.cmsReceived
            Dim comando As String = msg.Body.Replace(TriggerComando, "").ToLower
            Try
                skype.ResetCache()
                Comandos(comando, msg.Sender.Handle)
                For i As Integer = 0 To result.Count - 1
                    If result(i).ToString = "" Then
                    Else
                        msg.Chat.SendMessage(result(i).ToString)
                        Thread.Sleep(200)
                    End If
                    'skype.SendMessage(msg.Sender.Handle, result(i).ToString)
                Next
                log("Mensaje enviado - " & result(0))
            Catch ex As Exception
                log("--ERROR! --" & ex.Message)
                Hablar("ERROR!! " & ex.Message)
            End Try
            Exit Sub
        End If

        Dim entrante As String = msg.Body.ToLower

        'Inicio de funcion con "Luzlo"
        If (entrante.Contains(TriggerSpeech) And status = TChatMessageStatus.cmsReceived) Or (entrante.Contains(TriggerSpeech) And status = TChatMessageStatus.cmsSending) Then 'And status = TChatMessageStatus.cmsReceived
            If entrante.Contains("http") Or entrante.Contains("Http") Then
                Exit Sub
            End If
            If entrante.Contains("buitreseñal") Then
                msg.Chat.SendMessage("http://k31.kn3.net/taringa/1/2/3/2/9/6/48/_serialkiller_/92C.gif")
                log("Mensaje enviado - Buitreseñal")
                Exit Sub
            End If
            If entrante.Contains("cuando") And entrante.Contains("?") Then
                If entrante.Contains("gnomo") Then
                    msg.Chat.SendMessage("Nunca")
                    Exit Sub
                Else
                    msg.Chat.SendMessage("Mañana")
                    If skype.ActiveCalls.Count > 0 Then
                        Hablar("mañana")
                    End If
                    log("Mensaje enviado - Mañana")
                    Exit Sub
                End If
               
            End If
            Dim comando As String = msg.Body.Replace(TriggerComando, "").ToLower
            Try
                skype.ResetCache()
                Dim respuesta As String = luzlo(comando)
                If skype.ActiveCalls.Count > 0 Then
                    Hablar(respuesta)
                End If
                msg.Chat.SendMessage(respuesta)
                log("Mensaje enviado - " & comando & ": " & respuesta)
            Catch ex As Exception
                log("--ERROR! --" & ex.Message)
                Hablar("ERROR!! " & ex.Message)
            End Try
            Exit Sub
        End If

        'Funcion de calculadora
        If (msg.Body.IndexOf("=") = 0 And status = TChatMessageStatus.cmsReceived) Or (msg.Body.IndexOf("=") = 0 And status = TChatMessageStatus.cmsSending) Then 'And status = TChatMessageStatus.cmsReceived
            Dim comando As String = msg.Body.Replace("=", "")
            Try
                skype.ResetCache()
                Dim respuesta As String = IgualMath(comando)
                msg.Chat.SendMessage(respuesta)
                log("Mensaje enviado - " & comando & ": " & respuesta)
            Catch ex As Exception
                log("--ERROR! --" & ex.Message)
                Hablar("ERROR!! " & ex.Message)
            End Try
            Exit Sub
        End If


    End Sub


    Public Function Comandos(ByVal comando As String, ByVal handler As String)
        result.Clear() 'Reinicia las respuestas

        'organizar strings
        Dim cdm As String = ""
        Dim argumento As String = ""
        Try
            argumento = comando.Substring(comando.IndexOf(" ") + 1)
            cdm = comando.Substring(0, comando.IndexOf(" "))
        Catch ex As Exception
            cdm = comando
        End Try

        If argumento = cdm Then
            argumento = ""
        End If

        'Lista de funciones y comandos iniciados por trigger "-"
        Select Case cdm
            Case Is = "list"
                list()
                Exit Select
            Case Is = "ping"
                ping(argumento)
                Exit Select
            Case Is = "vida"
                vida()
                Exit Select
            Case Is = "call"
                sCall(handler)
                Exit Select
            Case Is = "speak"
                ASpeak(argumento)
                Exit Select
            Case Is = "buitrear"
                buitreo(argumento)
                Exit Select
            Case Is = "di"
                decir(argumento)
                Exit Select
            Case Is = "thalassa"
                reinos(0)
                Exit Select
            Case Is = "aegwynn"
                reinos(1)
                Exit Select
            Case Is = "vulcania"
                reinos(2)
                Exit Select
            Case Is = "totalplayers"
                TotalP()
                Exit Select
            Case Is = "play"
                playsonido(argumento)
                Exit Select
            Case Is = "buscarimg"
                buscarimg(argumento)
                Exit Select
            Case Is = "insultar"
                insultar(argumento)
                Exit Select
            Case Is = "chiste"
                chiste()
                Exit Select
            Case Is = "adv"
                adv()
                Exit Select
            Case Is = "tqd"
                tqd()
                Exit Select
            Case Else
                'Cuando no encuentra el comando en el select case
                result.Add("No reconozco ese comando, -list para mostrar los comandos.")
                log("-- Comando no encontrado (" & cdm & " " & argumento & ")")
                Exit Select
        End Select
        Return result
    End Function

    Private Sub InputText_KeyUp(sender As Object, e As KeyEventArgs) Handles InputText.KeyUp

    End Sub
End Class
