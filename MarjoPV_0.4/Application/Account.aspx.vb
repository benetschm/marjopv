Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalCode
Imports GlobalVariables

Partial Class Application_Account
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                Title_Label.Text = "Edit Account Details"
                AtEditPageLoadButtonsFormat(Status_Label, SaveAccountUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                ChangePassword_Button.Visible = True
                TextBoxReadWrite(Username_Textbox)
                Username_Textbox.ToolTip = EmailInputToolTip
                TextBoxReadWrite(Name_Textbox)
                TextBoxReadWrite(Phone_Textbox)
                Phone_Textbox.ToolTip = PhoneInputToolTip
                Dim UserReadCommand As New SqlCommand("SELECT Username, CASE WHEN Name IS NULL THEN '' ELSE Name END AS Name, CASE WHEN PHONE IS NULL THEN '' ELSE Phone END AS Phone FROM Users WHERE ID = @Current_User_ID", Connection)
                UserReadCommand.Parameters.AddWithValue("@Current_User_ID", LoggedIn_User_ID)
                Try
                    Connection.Open()
                    Dim UserReader As SqlDataReader = UserReadCommand.ExecuteReader()
                    While UserReader.Read()
                        Username_Textbox.Text = UserReader.GetString(0)
                        Name_Textbox.Text = UserReader.GetString(1)
                        Phone_Textbox.Text = UserReader.GetString(2)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/Account.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/")
    End Sub

    Protected Sub ChangePassword_Button_Click(sender As Object, e As EventArgs) Handles ChangePassword_Button.Click
        Response.Redirect("~/Application/ChangePassword.aspx")
    End Sub

    Protected Sub Username_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Username_Textbox.Text.Trim = String.Empty Then 'If username was not entered
            Username_Textbox.CssClass = CssClassFailure
            Username_Textbox.ToolTip = UsernameValidationFailToolTip
            args.IsValid = False
        ElseIf Username_Textbox.Text.Trim Like "*[@]*?.[a-z]*" Then 'If username was entered in the right format 
            'Check if entered username is already taken and fail validation if not
            Dim UsernamesReadCommand As New SqlCommand("SELECT Username FROM Users WHERE ID <> @Current_User_ID", Connection)
            UsernamesReadCommand.Parameters.AddWithValue("@Current_User_ID", LoggedIn_User_ID)
            Try
                Connection.Open()
                Dim UsernamesReadReader As SqlDataReader = UsernamesReadCommand.ExecuteReader()
                While UsernamesReadReader.Read()
                    If UsernamesReadReader.GetString(0) = Username_Textbox.Text Then
                        Username_Textbox.CssClass = CssClassFailure
                        Username_Textbox.ToolTip = UserNameTakenValidationFailToolTip
                        args.IsValid = False
                        Exit Sub
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            ' Pass validation if entered username has the correct format and is not already taken
            Username_Textbox.CssClass = CssClassSuccess
            Username_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else 'If username was entered in the wrong format
            Username_Textbox.CssClass = CssClassFailure
            Username_Textbox.ToolTip = UsernameValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if user full name was entered and fail validation if not
        If Name_Textbox.Text = String.Empty Then
            Name_Textbox.CssClass = CssClassFailure
            Name_Textbox.ToolTip = NameValidationFailToolTip
            args.IsValid = False
        Else
            Name_Textbox.CssClass = CssClassSuccess
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Phone_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Phone_Textbox.Text = String.Empty Then
            Phone_Textbox.CssClass = CssClassSuccess
            Phone_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Regex.IsMatch(Phone_Textbox.Text, "^[0-9-+]+$") Then
            Phone_Textbox.CssClass = CssClassSuccess
            Phone_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Phone_Textbox.CssClass = CssClassFailure
            Phone_Textbox.ToolTip = PhoneValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveAccountUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveAccountUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveAccountUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            ChangePassword_Button.Visible = False
            TextBoxReadOnly(Username_Textbox)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(Phone_Textbox)
            Dim UpdateCommand As New SqlCommand("UPDATE Users SET Username = @Username, Name = (CASE WHEN @Name = '' THEN NULL ELSE @Name END), Phone = (CASE WHEN @Phone = '' THEN NULL ELSE @Phone END) WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Username", Username_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Phone", Phone_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@ID", LoggedIn_User_ID)
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
