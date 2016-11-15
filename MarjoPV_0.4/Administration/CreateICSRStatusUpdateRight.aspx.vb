Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_CreateICSRStatusUpdateRight
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New ICSR Status Update Right"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
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
                Else
                    Title_Label.Text = Lockout_Text
                    Buttons.Visible = False
                    Main_Table.Visible = False
                End If
            End If
        End If
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

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SaveNewICSRStatusUpdateRight_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Title_Label.Text = "Create New ICSR Status Update Right"
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            DropDownListDisabled(Role_Dropdownlist)
            DropDownListDisabled(UpdateFromStatus_DropDownList)
            DropDownListDisabled(UpdateToStatus_DropDownList)
            Dim Command As New SqlCommand("INSERT INTO CanUpdateICSRStatus(Role_ID, CanUpdateFromICSRStatus_ID, CanUpdateToICSRStatus_ID) VALUES(@Role_ID, @CanUpdateFromICSRStatus_ID, @CanUpdateToICSRStatus_ID)", Connection)
            Command.Parameters.AddWithValue("@Role_ID", Role_Dropdownlist.SelectedValue)
            Command.Parameters.AddWithValue("@CanUpdateFromICSRStatus_ID", UpdateFromStatus_DropDownList.SelectedValue)
            Command.Parameters.AddWithValue("@CanUpdateToICSRStatus_ID", UpdateToStatus_DropDownList.SelectedValue)
            Try
                Connection.Open()
                Command.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
        End If
    End Sub
End Class
