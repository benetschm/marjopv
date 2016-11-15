Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_ICSRStatusUpdateRight
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit ICSR Status Update Right"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
                    DropDownListEnabled(Role_Dropdownlist)
                    DropDownListEnabled(UpdateFromStatus_DropDownList)
                    DropDownListEnabled(UpdateToStatus_DropDownList)
                    'Populate Role_DropDownList
                    Role_Dropdownlist.DataSource = CreateDropDownListDatatable(tables.Roles)
                    Role_Dropdownlist.DataValueField = "ID"
                    Role_Dropdownlist.DataTextField = "Name"
                    Role_Dropdownlist.DataBind()
                    'Populate UpdateFromStatus_DropDownList
                    UpdateFromStatus_DropDownList.DataSource = CreateDropDownListDatatable(tables.ICSRStatuses)
                    UpdateFromStatus_DropDownList.DataValueField = "ID"
                    UpdateFromStatus_DropDownList.DataTextField = "Name"
                    UpdateFromStatus_DropDownList.DataBind()
                    'Populate UpdateToStatus_DropDownList
                    UpdateToStatus_DropDownList.DataSource = CreateDropDownListDatatable(tables.ICSRStatuses)
                    UpdateToStatus_DropDownList.DataValueField = "ID"
                    UpdateToStatus_DropDownList.DataTextField = "Name"
                    UpdateToStatus_DropDownList.DataBind()
                    Dim UpdateICSRStatusUpdateRightReadCommand As New SqlCommand("SELECT Role_ID, CanUpdateFromICSRStatus_ID, CanUpdateToICSRStatus_ID FROM CanUpdateICSRStatus WHERE ID = @CurrentICSRStatusUpdateRight_ID", Connection)
                    UpdateICSRStatusUpdateRightReadCommand.Parameters.AddWithValue("@CurrentICSRStatusUpdateRight_ID", Request.QueryString("ID"))
                    Try
                        Connection.Open()
                        Dim UpdateICSRStatusUpdateRightReader As SqlDataReader = UpdateICSRStatusUpdateRightReadCommand.ExecuteReader()
                        While UpdateICSRStatusUpdateRightReader.Read()
                            Role_Dropdownlist.SelectedValue = UpdateICSRStatusUpdateRightReader.GetInt32(0)
                            UpdateFromStatus_DropDownList.SelectedValue = UpdateICSRStatusUpdateRightReader.GetInt32(1)
                            UpdateToStatus_DropDownList.SelectedValue = UpdateICSRStatusUpdateRightReader.GetInt32(2)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    Buttons.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/ICSRStatusUpdateRight.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Delete_Button_Click(sender As Object, e As EventArgs) Handles Delete_Button.Click
        Title_Label.Text = "Permanently delete ICSR Status Update Right"
        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        DropDownListDisabled(Role_Dropdownlist)
        DropDownListDisabled(UpdateFromStatus_DropDownList)
        DropDownListDisabled(UpdateToStatus_DropDownList)
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
            DropDownListDisabled(Role_Dropdownlist)
            DropDownListDisabled(UpdateFromStatus_DropDownList)
            DropDownListDisabled(UpdateToStatus_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE CanUpdateICSRStatus SET Role_ID = @Role_ID, CanUpdateFromICSRStatus_ID = @CanUpdateFromICSRStatus_ID, CanUpdateToICSRStatus_ID = @CanUpdateToICSRStatus_ID WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Role_ID", Role_Dropdownlist.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CanUpdateFromICSRStatus_ID", UpdateFromStatus_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CanUpdateToICSRStatus_ID", UpdateToStatus_DropDownList.SelectedValue)
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

    Protected Sub ConfirmDeletion_Button_Click(sender As Object, e As EventArgs) Handles ConfirmDeletion_Button.Click
        AtConfirmDeletionButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        Role_Row.Visible = False
        UpdateFromStatus_Row.Visible = False
        UpdateToStatus_Row.Visible = False
        Dim DeleteCommand As New SqlCommand("DELETE FROM CanUpdateICSRStatus WHERE ID = @ID", Connection)
        DeleteCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Try
            Connection.Open()
            DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
    End Sub
End Class
