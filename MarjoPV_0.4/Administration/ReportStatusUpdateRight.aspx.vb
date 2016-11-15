Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_ReportStatusUpdateRight
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Report Status Update Right"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
                    DropDownListEnabled(Role_Dropdownlist)
                    DropDownListEnabled(UpdateFromStatus_DropDownList)
                    DropDownListEnabled(UpdateToStatus_DropDownList)
                    Role_Dropdownlist.DataSource = CreateDropDownListDatatable(tables.Roles)
                    Role_Dropdownlist.DataValueField = "ID"
                    Role_Dropdownlist.DataTextField = "Name"
                    Role_Dropdownlist.DataBind()
                    UpdateFromStatus_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportStatuses)
                    UpdateFromStatus_DropDownList.DataValueField = "ID"
                    UpdateFromStatus_DropDownList.DataTextField = "Name"
                    UpdateFromStatus_DropDownList.DataBind()
                    UpdateToStatus_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportStatuses)
                    UpdateToStatus_DropDownList.DataValueField = "ID"
                    UpdateToStatus_DropDownList.DataTextField = "Name"
                    UpdateToStatus_DropDownList.DataBind()
                    Dim CanUpdateReportStatusReadCommand As New SqlCommand("SELECT Role_ID, CanUpdateFromReportStatus_ID, CanUpdateToReportStatus_ID FROM CanUpdateReportStatus WHERE CanUpdateReportStatus.ID = @CanUpdateReportStatus_ID", Connection)
                    CanUpdateReportStatusReadCommand.Parameters.AddWithValue("@CanUpdateReportStatus_ID", Request.QueryString("ID"))
                    Try
                        Connection.Open()
                        Dim CanUpdateReportStatusReader As SqlDataReader = CanUpdateReportStatusReadCommand.ExecuteReader()
                        While CanUpdateReportStatusReader.Read()
                            Role_Dropdownlist.SelectedValue = CanUpdateReportStatusReader.GetInt32(0)
                            UpdateFromStatus_DropDownList.SelectedValue = CanUpdateReportStatusReader.GetInt32(1)
                            UpdateToStatus_DropDownList.SelectedValue = CanUpdateReportStatusReader.GetInt32(2)
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
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/ReportStatusUpdateRight.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Delete_Button_Click(sender As Object, e As EventArgs) Handles Delete_Button.Click
        Title_Label.Text = "Permanently delete Report Status Update Right"
        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        DropDownListDisabled(Role_Dropdownlist)
        DropDownListDisabled(UpdateFromStatus_DropDownList)
        DropDownListDisabled(UpdateToStatus_DropDownList)
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Role_Dropdownlist_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(Role_Dropdownlist) = True Then
            Role_Dropdownlist.CssClass = CssClassSuccess
            Role_Dropdownlist.ToolTip = String.Empty
            args.IsValid = True
        Else
            Role_Dropdownlist.CssClass = CssClassFailure
            Role_Dropdownlist.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub UpdateFromStatus_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(UpdateFromStatus_DropDownList) = True Then
            UpdateFromStatus_DropDownList.CssClass = CssClassSuccess
            UpdateFromStatus_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            UpdateFromStatus_DropDownList.CssClass = CssClassFailure
            UpdateFromStatus_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub UpdateToStatus_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(UpdateToStatus_DropDownList) = True Then
            UpdateToStatus_DropDownList.CssClass = CssClassSuccess
            UpdateToStatus_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            UpdateToStatus_DropDownList.CssClass = CssClassFailure
            UpdateToStatus_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Title_Label.Text = "Edit Report Status Update Right"
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
            DropDownListDisabled(Role_Dropdownlist)
            DropDownListDisabled(UpdateFromStatus_DropDownList)
            DropDownListDisabled(UpdateToStatus_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE CanUpdateReportStatus SET Role_ID = @Role_ID, CanUpdateFromReportStatus_ID = @CanUpdateFromReportStatus_ID, CanUpdateToReportStatus_ID = @CanUpdateToReportStatus_ID WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Role_ID", Role_Dropdownlist.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CanUpdateFromReportStatus_ID", UpdateFromStatus_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CanUpdateToReportStatus_ID", UpdateToStatus_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
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

    Protected Sub ConfirmDeletion_Button_Click(sender As Object, e As EventArgs) Handles ConfirmDeletion_Button.Click
        Title_Label.Text = "Permanently delete Report Status Update Right"
        AtConfirmDeletionButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        Dim DeleteCommand As New SqlCommand("DELETE FROM CanUpdateReportStatus WHERE ID = @ID", Connection)
        DeleteCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Try
            Connection.Open()
            DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
        Finally
            Connection.Close()
        End Try
    End Sub

End Class
