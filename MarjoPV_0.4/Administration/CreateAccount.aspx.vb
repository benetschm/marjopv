Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Admininstration_CreateAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    AtEditPageLoadButtonsFormat(Status_Label, SaveNewAccount_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadWrite(Username_Textbox)
                    TextBoxReadWrite(Password_Textbox)
                    TextBoxReadWrite(ConfirmPassword_Textbox)
                    TextBoxReadWrite(Name_Textbox)
                    TextBoxReadWrite(Phone_Textbox)
                    DropDownListEnabled(Active_Dropdownlist)
                    DropDownListEnabled(CanViewCompanies_Dropdownlist)
                    DropDownListEnabled(Admin_Dropdownlist)
                Else
                    Title_Label.Text = Lockout_Text
                    SaveNewAccount_Button.Visible = False
                    Cancel_Button.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateAccount.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Username_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered username is already in database and fail validation if yes
        Dim Command As New SqlCommand("SELECT Username FROM Users", Connection)
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Username As String = String.Empty
        While Reader.Read()
            Db_Username = Reader.GetString(0)
            If Db_Username = Username_Textbox.Text Then
                Username_Textbox.CssClass = CssClassFailure
                Username_Textbox.ToolTip = UserNameTakenValidationFailToolTip
                args.IsValid = False
                Connection.Close()
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if username was entered as valid eMail address and fail validation if not
        If Username_Textbox.Text = String.Empty Then
            Username_Textbox.CssClass = CssClassFailure
            Username_Textbox.ToolTip = UsernameValidationFailToolTip
            args.IsValid = False
        ElseIf Username_Textbox.Text Like "*[@]*?.[a-z]*" Then
            Username_Textbox.CssClass = CssClassSuccess
            Username_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Username_Textbox.CssClass = CssClassFailure
            Username_Textbox.ToolTip = UsernameValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Password_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if password was entered and fail validation if not
        If Password_Textbox.Text = String.Empty Then
            Password_Textbox.CssClass = CssClassFailure
            Password_Textbox.ToolTip = PasswordValidationFailToolTip
            args.IsValid = False
        Else
            Password_Textbox.CssClass = CssClassSuccess
            Password_Textbox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub ConfirmPassword_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if password confirmation was entered and fail validation if not
        If ConfirmPassword_Textbox.Text = String.Empty Then
            ConfirmPassword_Textbox.CssClass = CssClassFailure
            ConfirmPassword_Textbox.ToolTip = ConfirmPasswordValidationFailToolTip
            args.IsValid = False
            'Check if password confirmation matches password and fail validation if not
        ElseIf ConfirmPassword_Textbox.Text <> Password_Textbox.Text Then
            Password_Textbox.CssClass = CssClassFailure
            ConfirmPassword_Textbox.CssClass = CssClassFailure
            Password_Textbox.ToolTip = ConfirmPasswordValidationFailToolTip
            ConfirmPassword_Textbox.ToolTip = ConfirmPasswordValidationFailToolTip
            args.IsValid = False
        Else
            ConfirmPassword_Textbox.CssClass = CssClassSuccess
            ConfirmPassword_Textbox.ToolTip = String.Empty
            args.IsValid = True
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
            Phone_Textbox.CssClass = "form-control alert-success"
            Phone_Textbox.ToolTip = ""
            args.IsValid = True
        ElseIf Regex.IsMatch(Phone_Textbox.Text, "^[0-9-+]+$") Then
            Phone_Textbox.CssClass = "form-control alert-success"
            Phone_Textbox.ToolTip = ""
            args.IsValid = True
        Else
            Phone_Textbox.CssClass = "form-control alert-danger"
            Phone_Textbox.ToolTip = "Please make sure you are entering a valid phone number"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveNewAccount_Button_Click(sender As Object, e As EventArgs) Handles SaveNewAccount_Button.Click
        If Page.IsValid = True Then
            Dim Command As New SqlCommand("INSERT INTO Users(Username, Password, Name, Phone, Active, CanViewCompanies, Admin) VALUES(@Username, @Password, @Name, @Phone, @Active, @CanViewCompanies, @Admin)", Connection)
            Command.Parameters.AddWithValue("@Username", Username_Textbox.Text)
            Command.Parameters.AddWithValue("@Password", FormsAuthentication.HashPasswordForStoringInConfigFile(Password_Textbox.Text, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString()))
            Command.Parameters.AddWithValue("@Name", Name_Textbox.Text)
            Command.Parameters.AddWithValue("@Phone", Phone_Textbox.Text)
            Command.Parameters.AddWithValue("@Active", Active_Dropdownlist.SelectedValue)
            Command.Parameters.AddWithValue("@CanViewCompanies", CanViewCompanies_Dropdownlist.SelectedValue)
            Command.Parameters.AddWithValue("@Admin", Admin_Dropdownlist.SelectedValue)
            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
            Response.Redirect("~/Administration/Administration.aspx")
        End If
    End Sub
End Class
