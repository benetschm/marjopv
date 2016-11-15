Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalCode
Imports GlobalVariables

Partial Class Administration_CreateReportComplexity
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New Report Complexity"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadWrite(Name_Textbox)
                    TextBoxReadWrite(SortOrder_Textbox)
                    DropDownListEnabled(Active_DropDownList)
                Else
                    Title_Label.Text = Lockout_Text
                    Buttons.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateReportComplexity.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If TextValidator(Name_Textbox) = True Then 'If valid text was entered
            'Check if entered status name is already taken and fail validation if not
            Dim MatchFound As Boolean = False
            Dim Command As New SqlCommand("SELECT Name FROM ReportComplexities", Connection)
            Dim Reader As SqlDataReader
            Try
                Connection.Open()
                Reader = Command.ExecuteReader()
                While Reader.Read()
                    If Name_Textbox.Text.Trim = Reader.GetString(0) Then
                        MatchFound = True
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Finally
                Connection.Close()
            End Try
            If MatchFound = False Then
                Name_Textbox.CssClass = CssClassSuccess
                Name_Textbox.ToolTip = String.Empty
                args.IsValid = True
            Else
                Name_Textbox.CssClass = CssClassFailure
                Name_Textbox.ToolTip = NameUniquenessValidationFailToolTip
                args.IsValid = False
            End If
        Else 'if no valid text was entered
            Name_Textbox.CssClass = CssClassFailure
            Name_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if sort order was entered as numeric characters only and fail validation if not
        If IntegerValidator(SortOrder_Textbox) = True Then
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
        Active_DropDownList.CssClass = "form-control alert-success"
        Active_DropDownList.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(SortOrder_Textbox)
            DropDownListDisabled(Active_DropDownList)
            Dim Command As New SqlCommand("INSERT INTO ReportComplexities(Name, SortOrder, Active) VALUES(@Name, @SortOrder, @Active)", Connection)
            Command.Parameters.AddWithValue("@Name", Name_Textbox.Text)
            Command.Parameters.AddWithValue("@SortOrder", CType(SortOrder_Textbox.Text, Integer))
            Command.Parameters.AddWithValue("@Active", Active_DropDownList.SelectedValue)
            Try
                Connection.Open()
                Command.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

End Class
