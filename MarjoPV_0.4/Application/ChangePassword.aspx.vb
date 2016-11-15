Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalCode
Imports GlobalVariables

Partial Class Application_ChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                Title_Label.Text = "Change Password"
                AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                TextBoxReadWrite(Password_Textbox)
                Password_Textbox.ToolTip = PasswordInputToolTip
                TextBoxReadWrite(ConfirmPassword_Textbox)
                ConfirmPassword_Textbox.ToolTip = ConfirmPasswordInputToolTip
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ChangePassword.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Application/Account.aspx")
    End Sub

    Protected Sub Password_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Login_Status = True Then
            If PasswordValidator(Password_Textbox) = True Then
                Password_Textbox.CssClass = CssClassSuccess
                Password_Textbox.ToolTip = String.Empty
                args.IsValid = True
            Else
                Password_Textbox.CssClass = CssClassFailure
                Password_Textbox.ToolTip = PasswordValidationFailToolTip
                args.IsValid = False
            End If
        Else
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ChangePassword.aspx")
        End If
    End Sub

    Protected Sub ConfirmPassword_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Login_Status = True Then 'If user is logged in
            If PasswordValidator(ConfirmPassword_Textbox) = True Then 'If a password confirmation was entered
                If ConfirmPassword_Textbox.Text = Password_Textbox.Text Then 'If the password and password confirmation match
                    Password_Textbox.CssClass = CssClassSuccess
                    Password_Textbox.ToolTip = String.Empty
                Else 'If the password and password confirmation do not match
                    ConfirmPassword_Textbox.CssClass = CssClassFailure
                    ConfirmPassword_Textbox.ToolTip = ConfirmPasswordValidationFailToolTip
                    args.IsValid = False
                End If
            Else 'If no password confirnmation was entered
                ConfirmPassword_Textbox.CssClass = CssClassFailure
                ConfirmPassword_Textbox.ToolTip = PasswordValidationFailToolTip
                args.IsValid = False
            End If
        Else 'If user is not logged in
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ChangePassword.aspx")
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Password_Textbox)
            TextBoxReadOnly(ConfirmPassword_Textbox)
            Dim HashedPassword As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Password_Textbox.Text, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString())
            Dim UpdateCommand As New SqlCommand("UPDATE Users SET Password = @Password WHERE ID = @CurrentUser_ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Password", HashedPassword)
            UpdateCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
                Connection.Close()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

End Class
