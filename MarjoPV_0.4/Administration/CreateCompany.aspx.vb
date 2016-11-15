Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Administration_CreateCompany
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New Company"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveNewCompany_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadWrite(Name_Textbox)
                    TextBoxReadWrite(Street_Textbox)
                    TextBoxReadWrite(PostalCode_Textbox)
                    TextBoxReadWrite(City_Textbox)
                    TextBoxReadWrite(Country_Textbox)
                    TextBoxReadWrite(SortOrder_Textbox)
                    DropDownListEnabled(Active_DropDownList)
                Else
                    Title_Label.Text = Lockout_Text
                    SaveNewCompany_Button.Visible = False
                    Cancel_Button.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateNewCompany.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered company name is already taken and fail validation if so
        Dim Command As New SqlCommand("SELECT Name FROM Companies", Connection)
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Name As String = String.Empty
        While Reader.Read()
            Db_Name = Reader.GetString(0)
            If Db_Name = Name_Textbox.Text Then
                Name_Textbox.CssClass = CssClassFailure
                Name_Textbox.ToolTip = CompanyNameTakenValidationFailToolTip
                Connection.Close()
                args.IsValid = False
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if company name was entered and fail validation if not
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

    Protected Sub Street_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Street_Textbox.CssClass = CssClassSuccess
        Street_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub PostalCode_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        PostalCode_Textbox.CssClass = CssClassSuccess
        PostalCode_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub City_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        City_Textbox.CssClass = CssClassSuccess
        City_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub Country_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Country_Textbox.CssClass = CssClassSuccess
        Country_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if sort order was entered as numeric characters only and fail validation if not
        If SortOrder_Textbox.Text.Trim = String.Empty Then
            SortOrder_Textbox.CssClass = CssClassFailure
            SortOrder_Textbox.ToolTip = SortOrderValidationFailToolTip
            args.IsValid = False
        ElseIf Regex.IsMatch(SortOrder_Textbox.Text, "^\d{0,16}$") Then
            SortOrder_Textbox.CssClass = CssClassSuccess
            SortOrder_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            SortOrder_Textbox.CssClass = CssClassFailure
            SortOrder_Textbox.ToolTip = SortOrderValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Active_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Active_DropDownList.CssClass = CssClassSuccess
        Active_DropDownList.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SaveNewCompany_Button_Click(sender As Object, e As EventArgs) Handles SaveNewCompany_Button.Click
        If Page.IsValid = True Then
            Dim Command As New SqlCommand("INSERT INTO Companies(Name, Street = (CASE WHEN @Street = '' THEN NULL ELSE @Street END), PostalCode = (CASE WHEN @PostalCode = '' THEN NULL ELSE @PostalCode END), City = (CASE WHEN @City = '' THEN NULL ELSE @City END), Country = (CASE WHEN @Country = '' THEN NULL ELSE @Country END), SortOrder, Active) VALUES(@Name, @Street, @PostalCode, @City, @Country, @SortOrder, @Active)", Connection)
            Command.Parameters.AddWithValue("@Name", Name_Textbox.Text)
            Command.Parameters.AddWithValue("@Street", Street_Textbox.Text)
            Command.Parameters.AddWithValue("@PostalCode", PostalCode_Textbox.Text)
            Command.Parameters.AddWithValue("@City", City_Textbox.Text)
            Command.Parameters.AddWithValue("@Country", Country_Textbox.Text)
            Command.Parameters.AddWithValue("@SortOrder", CType(SortOrder_Textbox.Text, Integer))
            Command.Parameters.AddWithValue("@Active", Active_DropDownList.SelectedValue)
            Try
                Connection.Open()
                Command.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            AtSaveButtonClickButtonsFormat(Status_Label, SaveNewCompany_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(Street_Textbox)
            TextBoxReadOnly(PostalCode_Textbox)
            TextBoxReadOnly(City_Textbox)
            TextBoxReadOnly(Country_Textbox)
            TextBoxReadOnly(SortOrder_Textbox)
            DropDownListDisabled(Active_DropDownList)
        End If
    End Sub
End Class
