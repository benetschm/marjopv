Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            Title_Label.Text = "Edit ICSR " & CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Format Controls according to user edit rights
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Edit) = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Assignee_ID) = True Then
                        Assignees_DropDownList.Enabled = True
                        Assignees_DropDownList.ToolTip = "Please select an assignee"
                    Else
                        Assignees_DropDownList.Enabled = False
                        Assignees_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.IsSerious) = True Then
                        IsSerious_DropDownList.Enabled = True
                        IsSerious_DropDownList.ToolTip = "Please select 'True' or 'False'"
                    Else
                        IsSerious_DropDownList.Enabled = False
                        IsSerious_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.SeriousnessCriterion_ID) = True Then
                        SeriousnessCriteria_DropDownList.Enabled = True
                        SeriousnessCriteria_DropDownList.ToolTip = "Please select a seriousness criterion if the ICSR is serious"
                    Else
                        SeriousnessCriteria_DropDownList.Enabled = False
                        SeriousnessCriteria_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Narrative) = True Then
                        Narrative_Textbox.ReadOnly = False
                        Narrative_Textbox.ToolTip = "Please enter a narrative text"
                    Else
                        Narrative_Textbox.ReadOnly = True
                        Narrative_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.CompanyComment) = True Then
                        CompanyComment_Textbox.ReadOnly = False
                        CompanyComment_Textbox.ToolTip = "Please enter a company comment text"
                    Else
                        CompanyComment_Textbox.ReadOnly = True
                        CompanyComment_Textbox.ToolTip = CannotEditControlText
                    End If
                    'Populate Assignees_DropDownList with users who have any role for the company assoiated with the current ICSR
                    Dim AssigneesDropDownListReadCommand As New SqlCommand("SELECT DISTINCT Users.ID AS Assignee_ID, Users.Name As Assignee_Name FROM RoleAllocations INNER JOIN Users ON RoleAllocations.User_ID = Users.ID INNER JOIN ICSRs ON RoleAllocations.Company_ID = ICSRs.Company_ID WHERE ICSRs.ID = @CurrentICSR_ID AND Users.Active = @Active ORDER BY Users.ID ", Connection)
                    AssigneesDropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    AssigneesDropDownListReadCommand.Parameters.AddWithValue("@Active", 1)
                    Dim AssigneesDropDownList_DataTable As New DataTable()
                    AssigneesDropDownList_DataTable.Columns.AddRange(New DataColumn(1) {
                                                                     New DataColumn("Assignee_ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("Assignee_Name", Type.GetType("System.String"))
                                                                     })
                    AssigneesDropDownList_DataTable.Rows.Add(-1, "None") 'Add row 'None' so that users can specify that there should be no assignee
                    Try
                        Connection.Open()
                        Dim AssigneesDropDownListReader As SqlDataReader = AssigneesDropDownListReadCommand.ExecuteReader()
                        Dim DropDownList_Assignee_ID As Integer = Nothing
                        Dim DropDownList_Assignee_Name As String = String.Empty
                        While AssigneesDropDownListReader.Read()
                            DropDownList_Assignee_ID = AssigneesDropDownListReader.GetInt32(0)
                            DropDownList_Assignee_Name = AssigneesDropDownListReader.GetString(1)
                            AssigneesDropDownList_DataTable.Rows.Add(DropDownList_Assignee_ID, DropDownList_Assignee_Name)
                        End While
                        Connection.Close()
                    Catch ex As Exception
                        Connection.Close()
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    End Try
                    AssigneesDropDownList_DataTable.DefaultView.Sort = "Assignee_ID"
                    Assignees_DropDownList.DataSource = AssigneesDropDownList_DataTable
                    Assignees_DropDownList.DataValueField = "Assignee_ID"
                    Assignees_DropDownList.DataTextField = "Assignee_Name"
                    Assignees_DropDownList.DataBind()
                    'Store the current ICSR's Company_ID and ICSRstatus_ID as variables to use in populating the ICSRStatus_DropDownList
                    Dim CurrentICSRCompanyStatusReadCommand As New SqlCommand("SELECT ICSRs.Company_ID, ICSRs.ICSRStatus_ID, ICSRStatuses.Name, ICSRStatuses.SortOrder FROM ICSRs INNER JOIN ICSRStatuses ON ICSRs.ICSRStatus_ID = ICSRStatuses.ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                    CurrentICSRCompanyStatusReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    Dim CurrentICSRCompany_ID As Integer = Nothing
                    Dim CurrentICSRStatus_ID As Integer = Nothing
                    Dim CurrentICSRStatus_Name As String = String.Empty
                    Dim CurrentICSRStatus_SortOrder As Integer = Nothing
                    Try
                        Connection.Open()
                        Dim CurrentICSRCompanyStatusReader As SqlDataReader = CurrentICSRCompanyStatusReadCommand.ExecuteReader()
                        While CurrentICSRCompanyStatusReader.Read()
                            CurrentICSRCompany_ID = CurrentICSRCompanyStatusReader.GetInt32(0)
                            CurrentICSRStatus_ID = CurrentICSRCompanyStatusReader.GetInt32(1)
                            CurrentICSRStatus_Name = CurrentICSRCompanyStatusReader.GetString(2)
                            CurrentICSRStatus_SortOrder = CurrentICSRCompanyStatusReader.GetInt32(3)
                        End While
                        Connection.Close()
                    Catch ex As Exception
                        Connection.Close()
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    End Try
                    'Populate ICSRStatus_DropDownList with the current ICSRstatus and all statuses the current user can update the current ICSR to
                    Dim ICSRStatusDropDownListReadCommand As New SqlCommand("SELECT DISTINCT CanUpdateICSRStatus.CanUpdateToICSRStatus_ID AS CanUpdateToICSRStatus_ID, ICSRStatuses.Name AS CanUpdateToICSRStatus_Name, ICSRStatuses.SortOrder AS CanUpdateToICSRStatus_SortOrder FROM ICSRStatuses INNER JOIN CanUpdateICSRStatus ON ICSRStatuses.ID = CanUpdateICSRStatus.CanUpdateToICSRStatus_ID INNER JOIN RoleAllocations ON CanUpdateICSRStatus.Role_ID = RoleAllocations.Role_ID INNER JOIN ICSRs ON RoleAllocations.Company_ID = ICSRs.Company_ID INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ICSRStatuses.Active = @Active AND Companies.Active = @Active AND CanUpdateICSRStatus.CanUpdateFromICSRStatus_ID = @CurrentICSR_StatusID AND RoleAllocations.User_ID = @CurrentUser_ID AND RoleAllocations.Company_ID = @CurrentICSR_CompanyID AND ICSRs.ID = @CurrentICSR_ID", Connection)
                    ICSRStatusDropDownListReadCommand.Parameters.AddWithValue("@Active", 1)
                    ICSRStatusDropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_StatusID", CurrentICSRStatus_ID)
                    ICSRStatusDropDownListReadCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
                    ICSRStatusDropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_CompanyID", CurrentICSRCompany_ID)
                    ICSRStatusDropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    Dim ICSRStatusDropDownList_DataTable As New DataTable()
                    ICSRStatusDropDownList_DataTable.Columns.AddRange(New DataColumn(2) {
                                                                     New DataColumn("ICSRStatus_ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("ICSRStatus_Name", Type.GetType("System.String")),
                                                                     New DataColumn("ICSRStatus_SortOrder", Type.GetType("System.Int32"))
                                                                     })
                    ICSRStatusDropDownList_DataTable.Rows.Add(CurrentICSRStatus_ID, CurrentICSRStatus_Name, CurrentICSRStatus_SortOrder) 'Add row with current ICSRStatus so that users can specify that the status should remain unchanged
                    Try
                        Connection.Open()
                        Dim ICSRStatusDropDownListReader As SqlDataReader = ICSRStatusDropDownListReadCommand.ExecuteReader()
                        Dim DropDownList_ICSRStatus_ID As Integer = Nothing
                        Dim DropDownList_ICSRStatus_Name As String = String.Empty
                        Dim DropDownList_ICSRStatus_SortOrder As Integer = Nothing
                        While ICSRStatusDropDownListReader.Read()
                            DropDownList_ICSRStatus_ID = ICSRStatusDropDownListReader.GetInt32(0)
                            DropDownList_ICSRStatus_Name = ICSRStatusDropDownListReader.GetString(1)
                            DropDownList_ICSRStatus_SortOrder = ICSRStatusDropDownListReader.GetInt32(2)
                            ICSRStatusDropDownList_DataTable.Rows.Add(DropDownList_ICSRStatus_ID, DropDownList_ICSRStatus_Name, DropDownList_ICSRStatus_SortOrder)
                        End While
                        ICSRStatusDropDownList_DataTable.DefaultView.Sort = "ICSRStatus_SortOrder"
                        ICSRStatuses_DropDownList.DataSource = ICSRStatusDropDownList_DataTable
                        ICSRStatuses_DropDownList.DataValueField = "ICSRStatus_ID"
                        ICSRStatuses_DropDownList.DataTextField = "ICSRStatus_Name"
                        ICSRStatuses_DropDownList.DataBind()
                        Connection.Close()
                    Catch ex As Exception
                        Connection.Close()
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    End Try
                    'Populate IsSerious_DropDownList
                    Dim IsSeriousDropDownlist_DataTable As New DataTable()
                    IsSeriousDropDownlist_DataTable.Columns.AddRange(New DataColumn(2) {
                                                                     New DataColumn("IsSerious_Value", Type.GetType("System.Boolean")),
                                                                     New DataColumn("IsSerious_Name", Type.GetType("System.String")),
                                                                     New DataColumn("IsSerious_SortOrder", Type.GetType("System.Int32"))
                                                                     })
                    IsSeriousDropDownlist_DataTable.Rows.Add(True, "True", 1)
                    IsSeriousDropDownlist_DataTable.Rows.Add(False, "False", 2)
                    IsSerious_DropDownList.DataSource = IsSeriousDropDownlist_DataTable
                    IsSerious_DropDownList.DataValueField = "IsSerious_Value"
                    IsSerious_DropDownList.DataTextField = "IsSerious_Name"
                    IsSerious_DropDownList.DataBind()
                    'Populate Seriousness Criterion DropDownList
                    Dim SeriousnessCriteriaReadCommand As New SqlCommand("SELECT ID, Name, SortOrder FROM SeriousnessCriteria WHERE Active = @Active", Connection)
                    SeriousnessCriteriaReadCommand.Parameters.AddWithValue("@ACtive", 1)
                    Dim SeriousnessCriteria_DataTable As New DataTable()
                    SeriousnessCriteria_DataTable.Columns.AddRange(New DataColumn(2) {
                                                                   New DataColumn("SeriousnessCriterion_ID", Type.GetType("System.Int32")),
                                                                   New DataColumn("SeriousnessCriterion_Name", Type.GetType("System.String")),
                                                                   New DataColumn("SeriousnessCriterion_SortOrder", Type.GetType("System.Int32"))
                                                                   })
                    SeriousnessCriteria_DataTable.Rows.Add(-1, "None", 0) 'Add row 'None' so that users can specify that there is no applicable seriousness criterion
                    Dim DropDownList_SeriousnessCriterion_ID As Integer = Nothing
                    Dim DropDownList_SeriousnessCriterion_Name As String = String.Empty
                    Dim DropDownList_SeriousnessCriterion_SortOrder As Integer = Nothing
                    Try
                        Connection.Open()
                        Dim SeriousnessCriteriaReader As SqlDataReader = SeriousnessCriteriaReadCommand.ExecuteReader()
                        While SeriousnessCriteriaReader.Read()
                            DropDownList_SeriousnessCriterion_ID = SeriousnessCriteriaReader.GetInt32(0)
                            DropDownList_SeriousnessCriterion_Name = SeriousnessCriteriaReader.GetString(1)
                            DropDownList_SeriousnessCriterion_SortOrder = SeriousnessCriteriaReader.GetInt32(2)
                            SeriousnessCriteria_DataTable.Rows.Add(DropDownList_SeriousnessCriterion_ID, DropDownList_SeriousnessCriterion_Name, DropDownList_SeriousnessCriterion_SortOrder)
                        End While
                        Connection.Close()
                    Catch ex As Exception
                        Connection.Close()
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    End Try
                    SeriousnessCriteria_DataTable.DefaultView.Sort = "SeriousnessCriterion_SortOrder"
                    SeriousnessCriteria_DropDownList.DataSource = SeriousnessCriteria_DataTable
                    SeriousnessCriteria_DropDownList.DataValueField = "SeriousnessCriterion_ID"
                    SeriousnessCriteria_DropDownList.DataTextField = "SeriousnessCriterion_Name"
                    SeriousnessCriteria_DropDownList.DataBind()
                    'Populate ICSR controls
                    Dim AtEditPageLoadICSRReadCommand As New SqlCommand("SELECT Companies.Name AS Company_Name, ICSRs.Assignee_ID, ICSRs.ICSRStatus_ID, ICSRs.IsSerious, ICSRs.SeriousnessCriterion_ID, ICSRs.Narrative, ICSRs.CompanyComment FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                    AtEditPageLoadICSRReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    Try
                        Connection.Open()
                        Dim AtEditPageLoadICSRReader As SqlDataReader = AtEditPageLoadICSRReadCommand.ExecuteReader()
                        While AtEditPageLoadICSRReader.Read()
                            CompanyName_Textbox.Text = AtEditPageLoadICSRReader.GetString(0)
                            Assignees_DropDownList.SelectedValue = AtEditPageLoadICSRReader.GetInt32(1)
                            AtEditPageLoad_AssigneeID_HiddenField.Value = AtEditPageLoadICSRReader.GetInt32(1)
                            ICSRStatuses_DropDownList.SelectedValue = AtEditPageLoadICSRReader.GetInt32(2)
                            AtEditPageLoad_ICSRStatusID_HiddenField.Value = AtEditPageLoadICSRReader.GetInt32(2)
                            IsSerious_DropDownList.SelectedValue = AtEditPageLoadICSRReader.GetBoolean(3)
                            AtEditPageLoad_IsSerious_HiddenField.Value = AtEditPageLoadICSRReader.GetBoolean(3)
                            SeriousnessCriteria_DropDownList.SelectedValue = AtEditPageLoadICSRReader.GetInt32(4)
                            AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value = AtEditPageLoadICSRReader.GetInt32(4)
                            Narrative_Textbox.Text = AtEditPageLoadICSRReader.GetString(5)
                            AtEditPageLoad_Narrative_HiddenField.Value = AtEditPageLoadICSRReader.GetString(5)
                            CompanyComment_Textbox.Text = AtEditPageLoadICSRReader.GetString(6)
                            AtEditPageLoad_CompanyComment_HiddenField.Value = AtEditPageLoadICSRReader.GetString(6)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else 'Lock out user if he/she does not have edit rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ICSR.aspx?ICSRID=" & CurrentICSR_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub ReturnToICSROverview_Button_Click() Handles ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub Assignees_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Assignees_DropDownList) = True Then
            Assignees_DropDownList.CssClass = CssClassSuccess
            Assignees_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Assignees_DropDownList.CssClass = CssClassFailure
            Assignees_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ICSRStatuses_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ICSRStatuses_DropDownList) = True Then
            ICSRStatuses_DropDownList.CssClass = CssClassSuccess
            ICSRStatuses_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ICSRStatuses_DropDownList.CssClass = CssClassFailure
            ICSRStatuses_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub IsSerious_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(IsSerious_DropDownList) = True Then
            IsSerious_DropDownList.CssClass = CssClassSuccess
            IsSerious_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            IsSerious_DropDownList.CssClass = CssClassFailure
            IsSerious_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SeriousnessCriteria_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If IsSerious_DropDownList.SelectedValue = False And SeriousnessCriteria_DropDownList.SelectedValue = -1 Then 'If the ICSR is not serious and no seriousness criterion was selected
            SeriousnessCriteria_DropDownList.CssClass = CssClassSuccess
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        ElseIf IsSerious_DropDownList.SelectedValue = True And SeriousnessCriteria_DropDownList.SelectedValue <> -1 Then 'If the ICSR is serious and a seriousness criterion was selected
            SeriousnessCriteria_DropDownList.CssClass = CssClassSuccess
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else 'If the ICSR is not serious and a seriousness criterion was selected or if the ICSR is serious and no seriousness criterion was selected 
            IsSerious_DropDownList.CssClass = CssClassFailure
            IsSerious_DropDownList.ToolTip = SeriousnessConsistencyValidationFailToolTip
            SeriousnessCriteria_DropDownList.CssClass = "form-control alert-danger"
            SeriousnessCriteria_DropDownList.ToolTip = SeriousnessConsistencyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Narrative_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Narrative_Textbox) = True Then
            Narrative_Textbox.CssClass = CssClassSuccess
            Narrative_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Narrative_Textbox.CssClass = CssClassFailure
            Narrative_Textbox.ToolTip = String.Empty
            args.IsValid = False
        End If
    End Sub

    Protected Sub CompanyComment_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(CompanyComment_Textbox) = True Then
            CompanyComment_Textbox.CssClass = CssClassSuccess
            CompanyComment_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            CompanyComment_Textbox.CssClass = CssClassFailure
            CompanyComment_Textbox.ToolTip = String.Empty
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Retrieve ICSR values as present in database at edit page load
            Dim AtEditPageLoad_Assignee_ID As Integer = AtEditPageLoad_AssigneeID_HiddenField.Value
            Dim AtEditPageLoad_ICSRStatusID As Integer = AtEditPageLoad_ICSRStatusID_HiddenField.Value
            Dim AtEditPageLoad_IsSerious As Boolean = AtEditPageLoad_IsSerious_HiddenField.Value
            Dim AtEditPageLoad_SeriousnessCriterion_ID As Integer = AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value
            Dim AtEditPageLoad_Narrative As String = AtEditPageLoad_Narrative_HiddenField.Value
            Dim AtEditPageLoad_CompanyComment As String = AtEditPageLoad_CompanyComment_HiddenField.Value
            'Store ICSR values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickICSRReadCommand As New SqlCommand("SELECT Assignee_ID, ICSRStatus_ID, IsSerious, SeriousnessCriterion_ID, Narrative, CompanyComment FROM ICSRs WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
            AtSaveButtonClickICSRReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim AtSaveButtonClick_Assignee_ID As Integer = Nothing
            Dim AtSaveButtonClick_ICSRStatus_ID As Integer = Nothing
            Dim AtSaveButtonClick_IsSerious As Boolean = False
            Dim AtSaveButtonClick_SeriousnessCriterion_ID As Integer = Nothing
            Dim AtSaveButtonClick_Narrative As String = String.Empty
            Dim AtSaveButtonClick_CompanyComment As String = String.Empty
            Try
                Connection.Open()
                Dim AtSaveButtonClickICSRReader As SqlDataReader = AtSaveButtonClickICSRReadCommand.ExecuteReader()
                While AtSaveButtonClickICSRReader.Read()
                    Try 'If assignee was specified in database when save button was clicked
                        AtSaveButtonClick_Assignee_ID = AtSaveButtonClickICSRReader.GetInt32(0)
                    Catch ex As Exception 'If no assignee was specified in database when save button was clicked
                        AtSaveButtonClick_Assignee_ID = Nothing
                    End Try
                    AtSaveButtonClick_ICSRStatus_ID = AtSaveButtonClickICSRReader.GetInt32(1)
                    AtSaveButtonClick_IsSerious = AtSaveButtonClickICSRReader.GetBoolean(2)
                    Try 'If seriousness criterion was specified in database when save button was clicked
                        AtSaveButtonClick_SeriousnessCriterion_ID = AtSaveButtonClickICSRReader.GetInt32(3)
                    Catch ex As Exception 'If no seriousness criterion was specified in database when save button was clicked
                        AtSaveButtonClick_SeriousnessCriterion_ID = Nothing
                    End Try
                    Try 'If narrative was present in database when save button was clicked
                        AtSaveButtonClick_Narrative = AtSaveButtonClickICSRReader.GetString(4)
                    Catch ex As Exception 'If no narrative was present in database when save button was clicked
                        AtSaveButtonClick_Narrative = String.Empty
                    End Try
                    Try 'If company comment was present in database when save button was clicked
                        AtSaveButtonClick_CompanyComment = AtSaveButtonClickICSRReader.GetString(5)
                    Catch ex As Exception 'If no company comment was present in database when save button was clicked
                        AtSaveButtonClick_CompanyComment = String.Empty
                    End Try
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyFound As Boolean = False
            Dim DiscrepancyString As String = String.Empty
            If AtEditPageLoad_Assignee_ID <> AtSaveButtonClick_Assignee_ID Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>Assignee was changed</li>"
            End If
            If AtEditPageLoad_ICSRStatusID <> AtSaveButtonClick_ICSRStatus_ID Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>ICSR Status was changed</li>"
            End If
            If AtEditPageLoad_IsSerious <> AtSaveButtonClick_IsSerious Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>Is Serious was changed</li>"
            End If
            If AtEditPageLoad_SeriousnessCriterion_ID <> AtSaveButtonClick_SeriousnessCriterion_ID Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>Seriousness Criterion was changed</li>"
            End If
            If AtEditPageLoad_Narrative <> AtSaveButtonClick_Narrative Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>Narrative was changed</li>"
            End If
            If AtEditPageLoad_CompanyComment <> AtSaveButtonClick_CompanyComment Then
                DiscrepancyFound = True
                DiscrepancyString += "<li>Company Comment was changed</li>"
            End If
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyFound = True Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                If LoggedIn_User_CanViewCompanies = True Then
                    Company_Row.Visible = True
                End If
                CompanyName_Textbox.ReadOnly = True
                Assignee_Row.Visible = True
                Assignees_DropDownList.Enabled = False
                Assignees_DropDownList.ToolTip = String.Empty
                Assignees_DropDownList.CssClass = "form-control"
                ICSRStatus_Row.Visible = True
                ICSRStatuses_DropDownList.Enabled = False
                ICSRStatuses_DropDownList.ToolTip = String.Empty
                ICSRStatuses_DropDownList.CssClass = "form-control"
                IsSerious_Row.Visible = True
                IsSerious_DropDownList.Enabled = False
                IsSerious_DropDownList.ToolTip = String.Empty
                IsSerious_DropDownList.CssClass = "form-control"
                SeriousnessCriterion_Row.Visible = True
                SeriousnessCriteria_DropDownList.Enabled = False
                SeriousnessCriteria_DropDownList.ToolTip = String.Empty
                SeriousnessCriteria_DropDownList.CssClass = "form-control"
                Narrative_Row.Visible = True
                Narrative_Textbox.ReadOnly = True
                Narrative_Textbox.ToolTip = String.Empty
                Narrative_Textbox.CssClass = "form-control"
                CompanyComment_Row.Visible = True
                CompanyComment_Textbox.ReadOnly = True
                CompanyComment_Textbox.ToolTip = String.Empty
                CompanyComment_Textbox.CssClass = "form-control"
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write ICSR Updates to database
            Dim ICSRUpdateCommand As New SqlCommand("Update ICSRs SET Assignee_ID = @Assignee_ID, ICSRStatus_ID = @ICSRStatus_ID, IsSerious = @IsSerious, SeriousnessCriterion_ID = @SeriousnessCriterion_ID, Narrative = @Narrative, CompanyComment = @CompanyComment WHERE ID = @CurrentICSR_ID", Connection)
            ICSRUpdateCommand.Parameters.Add("@Assignee_ID", SqlDbType.Int, 32)
            If Assignees_DropDownList.SelectedValue = -1 Then 'If user selected 'None' in Assignees_DropDownList
                ICSRUpdateCommand.Parameters("@Assignee_ID").Value = DBNull.Value
            Else
                ICSRUpdateCommand.Parameters("@Assignee_ID").Value = Assignees_DropDownList.SelectedValue
            End If
            ICSRUpdateCommand.Parameters.Add("@ICSRStatus_ID", SqlDbType.Int, 32)
            ICSRUpdateCommand.Parameters("@ICSRStatus_ID").Value = ICSRStatuses_DropDownList.SelectedValue
            ICSRUpdateCommand.Parameters.Add("@IsSerious", SqlDbType.Bit, 1)
            ICSRUpdateCommand.Parameters("@IsSerious").Value = IsSerious_DropDownList.SelectedValue
            ICSRUpdateCommand.Parameters.Add("@SeriousnessCriterion_ID", SqlDbType.Int, 32)
            If SeriousnessCriteria_DropDownList.SelectedValue = -1 Then 'If user selected 'None' in SeriousnessCriteria_DropDownList
                ICSRUpdateCommand.Parameters("@SeriousnessCriterion_ID").Value = DBNull.Value
            Else
                ICSRUpdateCommand.Parameters("@SeriousnessCriterion_ID").Value = SeriousnessCriteria_DropDownList.SelectedValue
            End If
            ICSRUpdateCommand.Parameters.Add("@Narrative", SqlDbType.NVarChar, -1)
            ICSRUpdateCommand.Parameters("@Narrative").Value = Narrative_Textbox.Text
            ICSRUpdateCommand.Parameters.Add("@CompanyComment", SqlDbType.NVarChar, -1)
            ICSRUpdateCommand.Parameters("@CompanyComment").Value = CompanyComment_Textbox.Text
            ICSRUpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Try
                Connection.Open()
                ICSRUpdateCommand.ExecuteNonQuery()
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Read new ICSR values from database and store in variables
            Dim NewICSRReadCommand As New SqlCommand("SELECT Assignee_ID, ICSRStatus_ID, IsSerious, SeriousnessCriterion_ID, Narrative, CompanyComment FROM ICSRs WHERE ID = @CurrentICSR_ID", Connection)
            NewICSRReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim New_Assignee_ID As Integer = Nothing
            Dim New_ICSRStatus_ID As Integer = Nothing
            Dim New_IsSerious As Boolean = False
            Dim New_SeriousnessCriterion_ID As Integer = Nothing
            Dim New_Narrative As String = String.Empty
            Dim New_CompanyComment As String = String.Empty
            Try
                Connection.Open()
                Dim NewICSRReader As SqlDataReader = NewICSRReadCommand.ExecuteReader()
                While NewICSRReader.Read()
                    Try
                        New_Assignee_ID = NewICSRReader.GetInt32(0)
                    Catch ex As Exception
                        New_Assignee_ID = Nothing
                    End Try
                    New_ICSRStatus_ID = NewICSRReader.GetInt32(1)
                    New_IsSerious = NewICSRReader.GetBoolean(2)
                    Try
                        New_SeriousnessCriterion_ID = NewICSRReader.GetInt32(3)
                    Catch ex As Exception
                        New_SeriousnessCriterion_ID = Nothing
                    End Try
                    Try
                        New_Narrative = NewICSRReader.GetString(4)
                    Catch ex As Exception
                        New_Narrative = String.Empty
                    End Try
                    Try
                        New_CompanyComment = NewICSRReader.GetString(5)
                    Catch ex As Exception
                        New_CompanyComment = String.Empty
                    End Try
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Compare old and new ICSR variables to generate EntryString for Case History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If New_Assignee_ID <> AtSaveButtonClick_Assignee_ID Then
                Dim Old_Assignee_Name As String = String.Empty
                Dim OldAssigneeReadCommand As New SqlCommand("SELECT Name FROM Users WHERE ID = @Old_Assignee_ID", Connection)
                OldAssigneeReadCommand.Parameters.AddWithValue("@Old_Assignee_ID", AtSaveButtonClick_Assignee_ID)
                Try
                    Connection.Open()
                    Dim OldAssigneeReader As SqlDataReader = OldAssigneeReadCommand.ExecuteReader()
                    While OldAssigneeReader.Read()
                        Old_Assignee_Name = OldAssigneeReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                If Old_Assignee_Name = String.Empty Then 'If no assignee was specified in the database before the update
                    Old_Assignee_Name = "None"
                End If
                Dim New_Assignee_Name As String = String.Empty
                Dim NewAssigneeReadCommand As New SqlCommand("SELECT Name FROM Users WHERE ID = @New_Assignee_ID", Connection)
                NewAssigneeReadCommand.Parameters.AddWithValue("@New_Assignee_ID", New_Assignee_ID)
                Try
                    Connection.Open()
                    Dim NewAssigneeReader As SqlDataReader = NewAssigneeReadCommand.ExecuteReader()
                    While NewAssigneeReader.Read()
                        New_Assignee_Name = NewAssigneeReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                If New_Assignee_Name = String.Empty Then 'If no assignee was specified in the database after the update
                    New_Assignee_Name = "None"
                End If
                EntryString += "<b>Assignee</b> changed from '" & Old_Assignee_Name & "' to '" & New_Assignee_Name & "'</br>"
            End If
            If New_ICSRStatus_ID <> AtSaveButtonClick_ICSRStatus_ID Then
                Dim Old_ICSRStatus_Name As String = String.Empty
                Dim OldICSRStatusReadCommand As New SqlCommand("SELECT Name FROM ICSRStatuses WHERE ID = @Old_ICSRStatus_ID", Connection)
                OldICSRStatusReadCommand.Parameters.AddWithValue("@Old_ICSRStatus_ID", AtSaveButtonClick_ICSRStatus_ID)
                Try
                    Connection.Open()
                    Dim OldICSRStatusReader As SqlDataReader = OldICSRStatusReadCommand.ExecuteReader()
                    While OldICSRStatusReader.Read()
                        Old_ICSRStatus_Name = OldICSRStatusReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                Dim New_ICSRStatus_Name As String = String.Empty
                Dim NewICSRStatusReadCommand As New SqlCommand("SELECT Name FROM ICSRStatuses WHERE ID = @New_ICSRStatus_ID", Connection)
                NewICSRStatusReadCommand.Parameters.AddWithValue("@New_ICSRStatus_ID", New_ICSRStatus_ID)
                Try
                    Connection.Open()
                    Dim NewICSRStatusReader As SqlDataReader = NewICSRStatusReadCommand.ExecuteReader()
                    While NewICSRStatusReader.Read()
                        New_ICSRStatus_Name = NewICSRStatusReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                EntryString += "<b>ICSR Status</b> changed from '" & Old_ICSRStatus_Name & "' to '" & New_ICSRStatus_Name & "'</br>"
            End If
            If New_IsSerious <> AtSaveButtonClick_IsSerious Then
                EntryString += "<b>Is Serious</b> changed from '" & AtSaveButtonClick_IsSerious & "' to '" & New_IsSerious & "'</br>"
            End If
            If New_SeriousnessCriterion_ID <> AtSaveButtonClick_SeriousnessCriterion_ID Then
                Dim Old_SeriousnessCriterion_Name As String = String.Empty
                Dim OldSeriousnessCriterionNameReadCommand As New SqlCommand("SELECT Name FROM SeriousnessCriteria WHERE ID = @Old_SeriousnessCriterion_ID", Connection)
                OldSeriousnessCriterionNameReadCommand.Parameters.AddWithValue("@Old_SeriousnessCriterion_ID", AtSaveButtonClick_SeriousnessCriterion_ID)
                Try
                    Connection.Open()
                    Dim OldSeriousnessCriterionNameReader As SqlDataReader = OldSeriousnessCriterionNameReadCommand.ExecuteReader()
                    While OldSeriousnessCriterionNameReader.Read()
                        Old_SeriousnessCriterion_Name = OldSeriousnessCriterionNameReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                If Old_SeriousnessCriterion_Name = String.Empty Then 'If no seriousness criterion was specified in the database before the update
                    Old_SeriousnessCriterion_Name = "None"
                End If
                Dim New_SeriousnessCriterion_Name As String = String.Empty
                Dim NewSeriousnessCriterionNameReadCommand As New SqlCommand("SELECT Name FROM SeriousnessCriteria WHERE ID = @New_SeriousnessCriterion_ID", Connection)
                NewSeriousnessCriterionNameReadCommand.Parameters.AddWithValue("@New_SeriousnessCriterion_ID", New_SeriousnessCriterion_ID)
                Try
                    Connection.Open()
                    Dim NewSeriousnessCriterionNameReader As SqlDataReader = NewSeriousnessCriterionNameReadCommand.ExecuteReader()
                    While NewSeriousnessCriterionNameReader.Read()
                        New_SeriousnessCriterion_Name = NewSeriousnessCriterionNameReader.GetString(0)
                    End While
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                If New_SeriousnessCriterion_Name = String.Empty Then 'If no seriousness criterion was specified in the database after the update
                    New_SeriousnessCriterion_Name = "None"
                End If
                EntryString += "<b>Seriousness Criterion</b> changed from '" & Old_SeriousnessCriterion_Name & "' to '" & New_SeriousnessCriterion_Name & "'</br>"
            End If
            If New_Narrative <> AtSaveButtonClick_Narrative Then
                If AtSaveButtonClick_Narrative = String.Empty Then 'If no narrative was specified in the database before the update
                    AtSaveButtonClick_Narrative = "None"
                End If
                If New_Narrative = String.Empty Then 'If no narrative was specified in the database after the update
                    New_Narrative = "None"
                End If
                EntryString += "<b>Narrative</b> changed from '" & AtSaveButtonClick_Narrative & "' to '" & New_Narrative & "'</br>"
            End If
            If New_CompanyComment <> AtSaveButtonClick_CompanyComment Then
                If AtSaveButtonClick_CompanyComment = String.Empty Then 'If no company comment was specified in the database before the update
                    AtSaveButtonClick_CompanyComment = "None"
                End If
                If New_CompanyComment = String.Empty Then 'If no company comment was specified in the database after the update
                    New_CompanyComment = "None"
                End If
                EntryString += "<b>Company Comment</b> changed from '" & AtSaveButtonClick_CompanyComment & "' to '" & New_CompanyComment & "'</br>"
            End If
            EntryString += HistoryDatabasebUpdateOutro
            'Generate History Entry if any data was changed in the database
            If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
                Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
                InsertHistoryEntryCommand.Parameters.Add("@ICSR_ID", SqlDbType.Int, 32)
                InsertHistoryEntryCommand.Parameters("@ICSR_ID").Value = CurrentICSR_ID
                InsertHistoryEntryCommand.Parameters.Add("@User_ID", SqlDbType.Int, 32)
                InsertHistoryEntryCommand.Parameters("@User_ID").Value = LoggedIn_User_ID
                InsertHistoryEntryCommand.Parameters.Add("@Timepoint", SqlDbType.DateTime, 8)
                InsertHistoryEntryCommand.Parameters("@Timepoint").Value = Now()
                InsertHistoryEntryCommand.Parameters.Add("@Entry", SqlDbType.NVarChar, -1)
                InsertHistoryEntryCommand.Parameters("@Entry").Value = EntryString
                Try
                    Connection.Open()
                    InsertHistoryEntryCommand.ExecuteNonQuery()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
            End If
            'Format Controls
            Title_Label.Text = "Edit ICSR"
            SaveUpdates_Button.Visible = False
            Cancel_Button.Visible = False
            ReturnToICSROverview_Button.Visible = True
            Status_Label.Text = "Changes saved"
            Status_Label.CssClass = "form-control alert-success"
            If LoggedIn_User_CanViewCompanies = True Then
                Company_Row.Visible = True
            End If
            CompanyName_Textbox.ReadOnly = True
            Assignee_Row.Visible = True
            Assignees_DropDownList.Enabled = False
            Assignees_DropDownList.ToolTip = String.Empty
            Assignees_DropDownList.CssClass = "form-control"
            ICSRStatus_Row.Visible = True
            ICSRStatuses_DropDownList.Enabled = False
            ICSRStatuses_DropDownList.ToolTip = String.Empty
            ICSRStatuses_DropDownList.CssClass = "form-control"
            IsSerious_Row.Visible = True
            IsSerious_DropDownList.Enabled = False
            IsSerious_DropDownList.ToolTip = String.Empty
            IsSerious_DropDownList.CssClass = "form-control"
            SeriousnessCriterion_Row.Visible = True
            SeriousnessCriteria_DropDownList.Enabled = False
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            SeriousnessCriteria_DropDownList.CssClass = "form-control"
            Narrative_Row.Visible = True
            Narrative_Textbox.ReadOnly = True
            Narrative_Textbox.ToolTip = String.Empty
            Narrative_Textbox.CssClass = "form-control"
            CompanyComment_Row.Visible = True
            CompanyComment_Textbox.ReadOnly = True
            CompanyComment_Textbox.ToolTip = String.Empty
            CompanyComment_Textbox.CssClass = "form-control"
        End If
    End Sub
End Class
