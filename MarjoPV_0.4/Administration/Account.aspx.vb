Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Administration_Account
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Selected User Account Details"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveAccountUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadWrite(Username_Textbox)
                    TextBoxReadWrite(Name_Textbox)
                    TextBoxReadWrite(Phone_Textbox)
                    DropDownListEnabled(Active_Dropdownlist)
                    DropDownListEnabled(CanViewCompanies_Dropdownlist)
                    DropDownListEnabled(Admin_DropDownList)
                    Dim Command As New SqlCommand("SELECT Username, Name, CASE WHEN Phone IS NULL THEN '' ELSE Phone END AS Phone, Active, CanViewCompanies, Admin FROM Users WHERE ID = @Current_User_ID", Connection)
                    Command.Parameters.AddWithValue("@Current_User_ID", Request.QueryString("ID"))
                    Dim Username As String = String.Empty
                    Dim Name As String = String.Empty
                    Dim Phone As String = String.Empty
                    Dim Active As Boolean = False
                    Dim CanViewCompanies As Boolean = False
                    Dim Admin As Boolean = False
                    Try
                        Connection.Open()
                        Dim Reader As SqlDataReader = Command.ExecuteReader()
                        While Reader.Read()
                            Username = Reader.GetString(0)
                            Name = Reader.GetString(1)
                            Phone = Reader.GetString(2)
                            Active = Reader.GetBoolean(3)
                            CanViewCompanies = Reader.GetBoolean(4)
                            Admin = Reader.GetBoolean(5)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                    Username_Textbox.Text = Username
                    Name_Textbox.Text = Name
                    Phone_Textbox.Text = Phone
                    Active_Dropdownlist.SelectedValue = Active
                    CanViewCompanies_Dropdownlist.SelectedValue = CanViewCompanies
                    Admin_DropDownList.SelectedValue = Admin
                Else
                    Title_Label.Text = Lockout_Text
                    SaveAccountUpdates_Button.Visible = False
                    Cancel_Button.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/Account.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Username_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered username is already taken and fail validation if not
        Dim Command As New SqlCommand("Select Username FROM Users WHERE ID <> @Current_User_ID", Connection)
        Command.Parameters.AddWithValue("@Current_User_ID", Request.QueryString("ID"))
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Username As String = String.Empty
        While Reader.Read()
            Db_Username = Reader.GetString(0)
            If Db_Username = Username_Textbox.Text Then
                Username_Textbox.CssClass = CssClassFailure
                Username_Textbox.ToolTip = UserNameTakenValidationFailToolTip
                Connection.Close()
                args.IsValid = False
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if username was entered and fail validation if not
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

    Protected Sub Active_Dropdownlist_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Active_Dropdownlist.CssClass = CssClassSuccess
        Active_Dropdownlist.ToolTip = String.Empty
    End Sub

    Protected Sub CanViewCompanies_Dropdownlist_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        CanViewCompanies_Dropdownlist.CssClass = CssClassSuccess
        CanViewCompanies_Dropdownlist.ToolTip = String.Empty
    End Sub

    Protected Sub Admin_Dropdownlist_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Admin_DropDownList.CssClass = CssClassSuccess
        Admin_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub SaveAccountUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveAccountUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveAccountUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Username_Textbox)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(Phone_Textbox)
            DropDownListDisabled(Active_Dropdownlist)
            DropDownListDisabled(CanViewCompanies_Dropdownlist)
            DropDownListDisabled(Admin_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE Users SET Username = @Username, Name = @Name, Phone = (CASE WHEN @Phone = '' THEN NULL ELSE @Phone END), Active = @Active, CanViewCompanies = @CanViewCompanies, Admin = @Admin WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Username", Username_Textbox.Text)
            UpdateCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text)
            UpdateCommand.Parameters.AddWithValue("@Phone", Phone_Textbox.Text)
            UpdateCommand.Parameters.AddWithValue("@Active", Active_Dropdownlist.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CanViewCompanies", CanViewCompanies_Dropdownlist.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@Admin", Admin_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

End Class
