Imports System.Text
Imports uPLibrary.Networking.M2Mqtt
Imports uPLibrary.Networking.M2Mqtt.Messages

Public Class Form1
    Dim client As MqttClient
    Dim clientId As String
    Private Delegate Sub SetTextCallback(text As String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        client = New MqttClient("127.0.0.1")
        AddHandler client.MqttMsgPublishReceived, AddressOf client_MqttMsgPublishReceived
        clientId = Guid.NewGuid().ToString()
        client.Connect(clientId)

    End Sub

    Private Sub client_MqttMsgPublishReceived(sender As Object, e As MqttMsgPublishEventArgs)
        Dim ReceivedMessage As String = Encoding.UTF8.GetString(e.Message)
        SetText(ReceivedMessage)
    End Sub

    Private Sub SetText(text As String)
        If Me.TextBox4.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {text})
        Else
            Me.TextBox4.Text = text
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" Then
            Dim Topic As String = TextBox1.Text
            client.Subscribe(New String() {Topic}, New Byte() {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE})
        Else
            MsgBox("Please enter a topic")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox2.Text <> "" Then
            Dim Topic As String = TextBox2.Text
            client.Publish(Topic, Encoding.UTF8.GetBytes(TextBox3.Text), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, False)
        Else
            MsgBox("Please enter a topic")
        End If
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        client.Disconnect()
    End Sub
End Class
