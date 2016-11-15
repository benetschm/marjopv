Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSRMedication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRMedicationID_HiddenField.Value = Request.QueryString("ICSRMedicationID")
            Dim CurrentICSRMedication_ID As Integer = ICSRMedicationID_HiddenField.Value
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.MedicationsPerICSR, fields.ICSR_ID, CurrentICSRMedication_ID)
            ICSRID_HiddenField.Value = CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit ICSR Medication " & CurrentICSRMedication_ID & " Details"
            If Login_Status = True Then
                'Populate Title_Label
                If Delete = False Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit ICSR Medication " & CurrentICSRMedication_ID
                ElseIf Delete = True Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete ICSR Medication " & CurrentICSRMedication_ID
                End If
                'Check if user has edit rights and lock out if not
                If Delete = False Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Edit) = True Then
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.MedicationPerICSRRole_ID) = True Then
                            DropDownListEnabled(MedicationPerICSRRoles_DropDownList)
                        Else
                            DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                            MedicationPerICSRRoles_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Medication_ID) = True Then
                            DropDownListEnabled(Medications_DropDownList)
                        Else
                            DropDownListDisabled(Medications_DropDownList)
                            Medications_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.TotalDailyDose) = True Then
                            TextBoxReadWrite(TotalDailyDose_Textbox)
                            TotalDailyDose_Textbox.ToolTip = NumberInputToolTip
                        Else
                            TextBoxReadOnly(TotalDailyDose_Textbox)
                            TotalDailyDose_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Allocations) = True Then
                            TextBoxReadWrite(Allocations_Textbox)
                            Allocations_Textbox.ToolTip = NumberInputToolTip
                        Else
                            TextBoxReadOnly(Allocations_Textbox)
                            Allocations_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Start) = True Then
                            TextBoxReadWrite(Start_Textbox)
                            Start_Textbox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(Start_Textbox)
                            Start_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Stop) = True Then
                            TextBoxReadWrite(Stop_Textbox)
                            Stop_Textbox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(Stop_Textbox)
                            Stop_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.DrugAction_ID) = True Then
                            DropDownListEnabled(DrugActions_DropDownList)
                        Else
                            DropDownListDisabled(DrugActions_DropDownList)
                        End If
                    Else
                        'Lock out user
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                        DropDownListDisabled(Medications_DropDownList)
                        TextBoxReadOnly(TotalDailyDose_Textbox)
                        TextBoxReadOnly(Allocations_Textbox)
                        TextBoxReadOnly(Start_Textbox)
                        TextBoxReadOnly(Stop_Textbox)
                        DropDownListDisabled(DrugActions_DropDownList)
                        'Check if there is a dataset in 'Relations' which is dependent on the current dataset
                        If RelationDependency(CurrentICSRMedication_ID) = True Then
                            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                            Status_Label.Text = DependencyFoundMessage
                            Status_Label.CssClass = "form-control alert-danger"
                        End If
                    Else
                        'Lock out user
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate MedicationPerICSRRoles_DropDownList
                MedicationPerICSRRoles_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationPerICSRRoles)
                MedicationPerICSRRoles_DropDownList.DataValueField = "ID"
                MedicationPerICSRRoles_DropDownList.DataTextField = "Name"
                MedicationPerICSRRoles_DropDownList.DataBind()
                'Populate Medications_DropDownList with all medications associated with that company.
                'Note: Use of corresponding function is not possible because the Company_ID which is needed is not passed to the function
                Dim CompanyIDOfCurrentICSR As Integer = Nothing
                Dim CompanyIDOfCurrentICSRReadCommand As New SqlCommand("SELECT Companies.ID FROM Companies JOIN ICSRs ON Companies.ID = ICSRs.Company_ID JOIN MedicationsPerICSR ON ICSRs.ID = MedicationsPerICSR.ICSR_ID WHERE MedicationsPerICSR.ID = @CurrentICSRMedication_ID", Connection)
                CompanyIDOfCurrentICSRReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
                Try
                    Connection.Open()
                    Dim CompanyIDOfCurrentICSRReader As SqlDataReader = CompanyIDOfCurrentICSRReadCommand.ExecuteReader()
                    While CompanyIDOfCurrentICSRReader.Read()
                        CompanyIDOfCurrentICSR = CompanyIDOfCurrentICSRReader.GetInt32(0)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                Dim DropDownListReadCommand As New SqlCommand("SELECT ID, Name, SortOrder FROM Medications WHERE Company_ID = @CompanyIDOfCurrentICSR", Connection)
                DropDownListReadCommand.Parameters.AddWithValue("@CompanyIDOfCurrentICSR", CompanyIDOfCurrentICSR)
                Dim DropDownListDataTable As New DataTable()
                DropDownListDataTable.Columns.AddRange(New DataColumn(2) {
                                                     New DataColumn("ID", Type.GetType("System.Int32")),
                                                     New DataColumn("Name", Type.GetType("System.String")),
                                                     New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                     })
                DropDownListDataTable.Rows.Add(0, "Select", 0)
                Dim ID As Integer = Nothing
                Dim Name As String = String.Empty
                Dim SortOrder As Integer = Nothing
                Try
                    Connection.Open()
                    Dim DropDownListReader As SqlDataReader = DropDownListReadCommand.ExecuteReader()
                    While DropDownListReader.Read()
                        ID = DropDownListReader.GetInt32(0)
                        Name = DropDownListReader.GetString(1)
                        SortOrder = DropDownListReader.GetInt32(2)
                        DropDownListDataTable.Rows.Add(ID, Name, SortOrder)
                    End While
                Catch ex As Exception
                    DropDownListDataTable.Rows.Add(-1, DatabaseConnectionErrorString, 0)
                Finally
                    Connection.Close()
                End Try
                DropDownListDataTable.DefaultView.Sort = "SortOrder"
                Medications_DropDownList.DataSource = DropDownListDataTable
                Medications_DropDownList.DataValueField = "ID"
                Medications_DropDownList.DataTextField = "Name"
                Medications_DropDownList.DataBind()
                'Populate DrugActions_DropDownList
                DrugActions_DropDownList.DataSource = CreateDropDownListDatatable(tables.DrugActions)
                DrugActions_DropDownList.DataValueField = "ID"
                DrugActions_DropDownList.DataTextField = "Name"
                DrugActions_DropDownList.DataBind()
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        MedicationPerICSRRoles_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(0)
                        AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(0)
                        Medications_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(1)
                        AtEditPageLoad_Medication_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                        TotalDailyDose_Textbox.Text = AtEditPageLoadReader.GetInt32(2)
                        AtEditPageLoad_TotalDailyDose_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                        Allocations_Textbox.Text = AtEditPageLoadReader.GetInt32(3)
                        AtEditPageLoad_Allocations_HiddenField.Value = AtEditPageLoadReader.GetInt32(3)
                        Start_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(4))
                        AtEditPageLoad_Start_HiddenField.Value = AtEditPageLoadReader.GetDateTime(4)
                        Stop_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(5))
                        AtEditPageLoad_Stop_HiddenField.Value = AtEditPageLoadReader.GetDateTime(5)
                        DrugActions_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(6)
                        AtEditPageLoad_DrugAction_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(6)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Specify dose type in TotalDailyDose_Textbox label
                Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = String.Empty
                Dim DoseTypeReadCommand As New SqlCommand("SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = @SelectedMedication_ID", Connection)
                DoseTypeReadCommand.Parameters.AddWithValue("@SelectedMedication_ID", Medications_DropDownList.SelectedValue)
                Try
                    Connection.Open()
                    Dim DoseTypeReader As SqlDataReader = DoseTypeReadCommand.ExecuteReader()
                    While DoseTypeReader.Read()
                        Medications_DropDownList_SelectedValue_DoseType_Name = DoseTypeReader.GetString(0)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                If Medications_DropDownList.SelectedValue = 0 Then
                    TotalDailyDose_Label.Text = "Total Daily Dose:"
                Else
                    TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
                End If
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?ICSRMedicationID=" & CurrentICSRMedication_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?ICSRMedicationID=" & CurrentICSRMedication_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub Medications_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = String.Empty
        Dim DoseTypeReadCommand As New SqlCommand("SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = @SelectedMedication_ID", Connection)
        DoseTypeReadCommand.Parameters.AddWithValue("@SelectedMedication_ID", Medications_DropDownList.SelectedValue)
        Try
            Connection.Open()
            Dim DoseTypeReader As SqlDataReader = DoseTypeReadCommand.ExecuteReader()
            While DoseTypeReader.Read()
                Medications_DropDownList_SelectedValue_DoseType_Name = DoseTypeReader.GetString(0)
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        If Medications_DropDownList.SelectedValue = 0 Then
            TotalDailyDose_Label.Text = "Total Daily Dose:"
        Else
            TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedicationPerICSRRoles_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CurrentICSRMedication_ID As Integer = ICSRMedicationID_HiddenField.Value
        If MedicationPerICSRRoles_DropDownList.SelectedValue = 2 And RelationDependency(CurrentICSRMedication_ID) = True Then 'If the selected ICSR medication role is 'Concomitant Medication' and there is at least one dataset in 'Relations' which is dependent on the current ICSR Medication
            MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
            MedicationPerICSRRoles_DropDownList.ToolTip = DependencyFoundMessage
            args.IsValid = False
        ElseIf SelectionValidator(MedicationPerICSRRoles_DropDownList) = False Then 'If no ICSR Medication Role was selected
            MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
            MedicationPerICSRRoles_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        Else
            MedicationPerICSRRoles_DropDownList.CssClass = CssClassSuccess
            MedicationPerICSRRoles_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Medications_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Medications_DropDownList.SelectedValue <> 0 Then
            Medications_DropDownList.CssClass = "form-control alert-success"
            Medications_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Medications_DropDownList.CssClass = "form-control alert-danger"
            Medications_DropDownList.ToolTip = "Please ensure you are selecting a valid entry"
            args.IsValid = True
        End If
    End Sub

    Protected Sub TotalDailyDose_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(TotalDailyDose_Textbox.Text, InputTypes.Number) = True Then
            TotalDailyDose_Textbox.CssClass = CssClassSuccess
            TotalDailyDose_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf TotalDailyDose_Textbox.Text = String.Empty Then
            TotalDailyDose_Textbox.CssClass = CssClassSuccess
            TotalDailyDose_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            TotalDailyDose_Textbox.CssClass = CssClassFailure
            TotalDailyDose_Textbox.ToolTip = NumberValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Allocations_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Allocations_Textbox.Text, InputTypes.Number) = True Then
            Allocations_Textbox.CssClass = CssClassSuccess
            Allocations_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Allocations_Textbox.Text = String.Empty Then
            Allocations_Textbox.CssClass = CssClassSuccess
            Allocations_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Allocations_Textbox.CssClass = "form-control alert-danger"
            Allocations_Textbox.ToolTip = NumberValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Start_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Start_Textbox.Text, InputTypes.Date) = True Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Start_Textbox.Text = String.Empty Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Stop_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Stop_Textbox.Text, InputTypes.Date) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Stop_Textbox.Text = String.Empty Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub StartStop_Textbox_Consistency_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrDateMinValue(Stop_Textbox.Text) >= DateOrDateMinValue(Start_Textbox.Text) Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DrugActions_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        DrugActions_DropDownList.CssClass = CssClassSuccess
        DrugActions_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSRMedication_ID As Integer = ICSRMedicationID_HiddenField.Value
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_MedicationPerICSRRole_ID As Integer = AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value
            Dim AtEditPageLoad_Medication_ID As Integer = AtEditPageLoad_Medication_ID_HiddenField.Value
            Dim AtEditPageLoad_TotalDailyDose As Integer = AtEditPageLoad_TotalDailyDose_HiddenField.Value
            Dim AtEditPageLoad_Allocations As Integer = AtEditPageLoad_Allocations_HiddenField.Value
            Dim AtEditPageLoad_Start As DateTime = TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_Stop As DateTime = TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_DrugAction_ID As Integer = AtEditPageLoad_DrugAction_ID_HiddenField.Value
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
            Dim AtSaveButtonClick_MedicationPerICSRRole_ID As Integer = Nothing
            Dim AtSaveButtonClick_Medication_ID As Integer = Nothing
            Dim AtSaveButtonClick_TotalDailyDose As Integer = Nothing
            Dim AtSaveButtonClick_Allocations As Integer = Nothing
            Dim AtSaveButtonClick_Start As DateTime = Date.MinValue
            Dim AtSaveButtonClick_Stop As DateTime = Date.MinValue
            Dim AtSaveButtonClick_DrugAction_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_MedicationPerICSRRole_ID = AtSaveButtonClickReader.GetInt32(0)
                    AtSaveButtonClick_Medication_ID = AtSaveButtonClickReader.GetInt32(1)
                    AtSaveButtonClick_TotalDailyDose = AtSaveButtonClickReader.GetInt32(2)
                    AtSaveButtonClick_Allocations = AtSaveButtonClickReader.GetInt32(3)
                    AtSaveButtonClick_Start = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(4))
                    AtSaveButtonClick_Stop = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(5))
                    AtSaveButtonClick_DrugAction_ID = AtSaveButtonClickReader.GetInt32(6)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_MedicationPerICSRRole_ID, AtSaveButtonClick_MedicationPerICSRRole_ID, "Role")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Medication_ID, AtSaveButtonClick_Medication_ID, "Medication")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_TotalDailyDose, AtSaveButtonClick_TotalDailyDose, "Total DAily Dose")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Allocations, AtSaveButtonClick_Allocations, "Allocations")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Start, AtSaveButtonClick_Start, "Start Date")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Stop, AtSaveButtonClick_Stop, "Stop Date")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_DrugAction_ID, AtSaveButtonClick_DrugAction_ID, "Action Taken With Drug")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                DropDownListDisabled(Medications_DropDownList)
                TextBoxReadOnly(TotalDailyDose_Textbox)
                TextBoxReadOnly(Allocations_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
                DropDownListDisabled(DrugActions_DropDownList)
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE MedicationsPerICSR SET MedicationPerICSRRole_ID = (CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END), Medication_ID = (CASE WHEN @Medication_ID = 0 THEN NULL ELSE @Medication_ID END), TotalDailyDose = (CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END), Allocations = (CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END), Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END), DrugAction_ID = (CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END) WHERE ID = @CurrentICSRMedication_ID"
                UpdateCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@TotalDailyDose", TotalDailyDose_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Allocations", Allocations_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
            End If
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Read updated values from database and store in variables
            Dim Updated_MedicationPerICSRRole_ID As Integer = Nothing
            Dim Updated_Medication_ID As Integer = Nothing
            Dim Updated_TotalDailyDose As Integer = Nothing
            Dim Updated_Allocations As Integer = Nothing
            Dim Updated_Start As DateTime = Date.MinValue
            Dim Updated_Stop As DateTime = Date.MinValue
            Dim Updated_DrugAction_ID As Integer = Nothing
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_MedicationPerICSRRole_ID = UpdatedReader.GetInt32(0)
                        Updated_Medication_ID = UpdatedReader.GetInt32(1)
                        Updated_TotalDailyDose = UpdatedReader.GetInt32(2)
                        Updated_Allocations = UpdatedReader.GetInt32(3)
                        Updated_Start = DateOrDateMinValue(UpdatedReader.GetDateTime(4))
                        Updated_Stop = DateOrDateMinValue(UpdatedReader.GetDateTime(5))
                        Updated_DrugAction_ID = UpdatedReader.GetInt32(6)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If sender Is ConfirmDeletion_Button Then
                EntryString += DeleteReportIntro("ICSR Medication", CurrentICSRMedication_ID)
            End If
            If Updated_MedicationPerICSRRole_ID <> AtSaveButtonClick_MedicationPerICSRRole_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR Medication", CurrentICSRMedication_ID, "Role", tables.MedicationPerICSRRoles, fields.Name, AtSaveButtonClick_MedicationPerICSRRole_ID, Updated_MedicationPerICSRRole_ID)
            End If
            If Updated_Medication_ID <> AtSaveButtonClick_Medication_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR Medication", CurrentICSRMedication_ID, "Medication", tables.Medications, fields.Name, AtSaveButtonClick_Medication_ID, Updated_Medication_ID)
            End If
            If Updated_TotalDailyDose <> AtSaveButtonClick_TotalDailyDose Then
                EntryString += HistoryEnrtyPlainValue("ICSR Medication", CurrentICSRMedication_ID, "Total Daily Dose", AtSaveButtonClick_TotalDailyDose, Updated_TotalDailyDose)
            End If
            If Updated_Allocations <> AtSaveButtonClick_Allocations Then
                EntryString += HistoryEnrtyPlainValue("ICSR Medication", CurrentICSRMedication_ID, "Allocations per Day", AtSaveButtonClick_Allocations, Updated_Allocations)
            End If
            If Updated_Start <> AtSaveButtonClick_Start Then
                EntryString += HistoryEnrtyPlainValue("ICSR Medication", CurrentICSRMedication_ID, "Start Date", AtSaveButtonClick_Start, Updated_Start)
            End If
            If Updated_Stop <> AtSaveButtonClick_Stop Then
                EntryString += HistoryEnrtyPlainValue("ICSR Medication", CurrentICSRMedication_ID, "Stop Date", AtSaveButtonClick_Stop, Updated_Stop)
            End If
            If Updated_DrugAction_ID <> AtSaveButtonClick_DrugAction_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR Medication", CurrentICSRMedication_ID, "Action Taken With Drug", tables.DrugActions, fields.Name, AtSaveButtonClick_DrugAction_ID, Updated_DrugAction_ID)
            End If
            EntryString += HistoryDatabasebUpdateOutro
            'Generate History Entry if any data was changed in the database
            If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
                Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories (ICSR_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
                InsertHistoryEntryCommand.Parameters.AddWithValue("@Medication_ID", CurrentICSR_ID)
                InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
                InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
                InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
                Try
                    Connection.Open()
                    InsertHistoryEntryCommand.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Format Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            If sender Is SaveUpdates_Button Then
                DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                DropDownListDisabled(Medications_DropDownList)
                TextBoxReadOnly(TotalDailyDose_Textbox)
                TextBoxReadOnly(Allocations_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
                DropDownListDisabled(DrugActions_DropDownList)
            ElseIf sender Is ConfirmDeletion_Button Then
                MedicationRole_Row.Visible = False
                MedicationName_Row.Visible = False
                TotalDailyDose_Row.Visible = False
                Allocations_Row.Visible = False
                Start_Row.Visible = False
                Stop_Row.Visible = False
                DrugAction_Row.Visible = False
            End If
        End If
    End Sub

End Class
