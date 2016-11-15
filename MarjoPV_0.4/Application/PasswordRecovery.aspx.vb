Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports GlobalVariables

Partial Class Application_PasswordRecovery
    Inherits System.Web.UI.Page

    Protected Sub Username_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if username was entered and fail validation if not
        If Username_TextBox.Text = String.Empty Then
            Username_TextBox.CssClass = "form-control alert-danger"
            Username_TextBox.ToolTip = "Please make sure you are entering a valid username"
            args.IsValid = False
        Else
            Dim Command As New SqlCommand("SELECT Username FROM Users WHERE ACtive = @Active", Connection)
            Command.Parameters.AddWithValue("@Active", 1)
            Dim Reader As SqlDataReader
            Dim Db_Username As String = String.Empty
            Connection.Open()
            Reader = Command.ExecuteReader()
            While Reader.Read()
                Try
                    Db_Username = Reader.GetString(0)
                Catch ex As Exception
                    Db_Username = String.Empty
                End Try
                If Db_Username = Username_TextBox.Text Then
                    Connection.Close()
                    Username_TextBox.CssClass = "form-control alert-success"
                    Username_TextBox.ToolTip = String.Empty
                    args.IsValid = True
                    Exit Sub
                End If
            End While
            Connection.Close()
            Username_TextBox.CssClass = "form-control alert-danger"
            Username_TextBox.ToolTip = "Username was not found in the database. Please ensure you are entering a valid username"
            args.IsValid = False
        End If
    End Sub

    Protected Sub RecoverPassword_Button_Click(sender As Object, e As EventArgs) Handles RecoverPassword_Button.Click
        If Page.IsValid = True Then
            Dim ReadCommand As New SqlCommand("SELECT [Name] FROM [Users] WHERE [Username] = @Entered_Username", Connection)
            ReadCommand.Parameters.AddWithValue("@Entered_Username", Username_TextBox.Text)
            Dim Reader As SqlDataReader
            Dim Db_Name As String = String.Empty
            Connection.Open()
            Reader = ReadCommand.ExecuteReader()
            Reader.Read()
            Try
                Db_Name = Reader.GetString(0)
            Catch ex As Exception
                Db_Name = String.Empty
            End Try
            Connection.Close()
            'Generate Random_Password as random alphanumeric string with length = 12 characters
            Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            Dim r As New Random
            Dim sb As New StringBuilder
            For i As Integer = 1 To 12
                Dim idx As Integer = r.Next(0, 35)
                sb.Append(s.Substring(idx, 1))
            Next
            Dim Random_Password As String = sb.ToString
            'Store Random_Password into the account dataset specified by the user 
            Dim UpdateCommand As New SqlCommand("UPDATE Users SET Password = @Password WHERE Username = @Username", Connection)
            UpdateCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 200, "Password")
            UpdateCommand.Parameters("@Password").Value = FormsAuthentication.HashPasswordForStoringInConfigFile(Random_Password, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString())
            UpdateCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 200, "Username")
            UpdateCommand.Parameters("@Username").Value = Username_TextBox.Text
            Connection.Open()
            UpdateCommand.ExecuteNonQuery()
            Connection.Close()
            'Send mailmessage with new password to user
            Dim PasswordRecoveryMessage As MailMessage = New MailMessage()
            PasswordRecoveryMessage.IsBodyHtml = True
            PasswordRecoveryMessage.Subject = "New Password from MarjoPV"
            PasswordRecoveryMessage.Body = "Hi " & Db_Name & ",</br>"
            PasswordRecoveryMessage.Body += "</br>"
            PasswordRecoveryMessage.Body += "Please find the new password which you have requested to receive below:</br>"
            PasswordRecoveryMessage.Body += "</br>"
            PasswordRecoveryMessage.Body += "Password: " & Random_Password & "</br>"
            PasswordRecoveryMessage.Body += "</br>"
            PasswordRecoveryMessage.Body += "To ensure your account authorization is safe, please log in to Marjo PV and change your password.</br>"
            PasswordRecoveryMessage.Body += "</br>"
            PasswordRecoveryMessage.Body += "Thanks & Regards, The Marjo PV Administration Team</br>"
            PasswordRecoveryMessage.From = New MailAddress("system@marjopv.com", "Marjo PV")
            PasswordRecoveryMessage.To.Add(New MailAddress(Username_TextBox.Text))
            Dim mySmtpClient As SmtpClient = New SmtpClient()
            Try
                mySmtpClient.Send(PasswordRecoveryMessage)
                TextBoxReadOnly(Username_TextBox)
                RecoverPassword_Button.Visible = False
                Login_Button.Visible = True
                Status_Label.Text = "A new password has been sent to your email address"
                Status_Label.CssClass = CssClassSuccess
            Catch ex As Exception
                Username_TextBox.CssClass = "form-control"
                Status_Label.Text = "Passowrd recovery failed. Please contact the administration team"
                Status_Label.CssClass = CssClassFailure
            End Try
        End If
    End Sub

    Protected Sub Login_Button_Click(sender As Object, e As EventArgs) Handles Login_Button.Click
        Response.Redirect("~/Login.aspx")
    End Sub
End Class
