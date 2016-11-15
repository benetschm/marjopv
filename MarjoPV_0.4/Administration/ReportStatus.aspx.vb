Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalCode
Imports GlobalVariables

Partial Class Administration_ReportStatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            AtEditPageLoad_ReportStatusID_HiddenField.Value = Request.QueryString("ID")
            Dim CurrentReportStatus_ID As Integer = AtEditPageLoad_ReportStatusID_HiddenField.Value
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Report Status"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadOnly(Name_Textbox)
                    Name_Textbox.ToolTip = "Report status names cannot be changed. Instead, deactivate the current report status and create a new report status with the desired name."
                    DropDownListEnabled(IsStatusNew_DropDownList)
                    DropDownListEnabled(IsStatusClosed_DropDownList)
                    TextBoxReadWrite(SortOrder_Textbox)
                    DropDownListEnabled(Active_DropDownList)
                    Dim ReadCommand As New SqlCommand("SELECT Name, IsStatusNew, IsStatusClosed, SortOrder, Active FROM ReportStatuses WHERE ID = @ID", Connection)
                    ReadCommand.Parameters.AddWithValue("@ID", CurrentReportStatus_ID)
                    Try
                        Connection.Open()
                        Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
                        While Reader.Read()
                            Name_Textbox.Text = Reader.GetString(0)
                            IsStatusNew_DropDownList.SelectedValue = Reader.GetBoolean(1)
                            IsStatusClosed_DropDownList.SelectedValue = Reader.GetBoolean(2)
                            SortOrder_Textbox.Text = Reader.GetInt32(3)
                            Active_DropDownList.SelectedValue = Reader.GetBoolean(4)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    Buttons.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/ReportStatus.aspx?ID=" & CurrentReportStatus_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub IsStatusNew_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(IsStatusNew_DropDownList) = True Then
            IsStatusNew_DropDownList.CssClass = CssClassSuccess
            IsStatusNew_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            IsStatusNew_DropDownList.CssClass = CssClassFailure
            IsStatusNew_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub IsStatusClosed_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(IsStatusClosed_DropDownList) = True Then
            IsStatusClosed_DropDownList.CssClass = CssClassSuccess
            IsStatusClosed_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            IsStatusClosed_DropDownList.CssClass = CssClassFailure
            IsStatusClosed_DropDownList.ToolTip = SelectorValidationFailToolTip
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
        If NoValidator(Active_DropDownList) = True Then
            Active_DropDownList.CssClass = CssClassSuccess
            Active_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Active_DropDownList.CssClass = CssClassFailure
            Active_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentReportStatus_ID As Integer = AtEditPageLoad_ReportStatusID_HiddenField.Value
            Title_Label.Text = "Edit Report Status"
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            DropDownListDisabled(IsStatusNew_DropDownList)
            DropDownListDisabled(IsStatusClosed_DropDownList)
            TextBoxReadOnly(SortOrder_Textbox)
            DropDownListDisabled(Active_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE ReportStatuses SET IsStatusNew = @IsStatusNew, IsStatusClosed = @IsStatusClosed, SortOrder = @SortOrder, Active = @Active WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@IsStatusNew", CType(IsStatusNew_DropDownList.SelectedValue, Boolean))
            UpdateCommand.Parameters.AddWithValue("@IsStatusClosed", CType(IsStatusClosed_DropDownList.SelectedValue, Boolean))
            UpdateCommand.Parameters.AddWithValue("@SortOrder", CType(SortOrder_Textbox.Text, Integer))
            UpdateCommand.Parameters.AddWithValue("@Active", Active_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ID", CurrentReportStatus_ID)
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

End Class
