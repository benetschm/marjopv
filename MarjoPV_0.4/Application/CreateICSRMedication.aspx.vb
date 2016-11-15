Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateICSRMedication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hidden field to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add ICSR Medication"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
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
                    'Populate MedicationPerICSRRoles_DropDownList
                    MedicationPerICSRRoles_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationPerICSRRoles)
                    MedicationPerICSRRoles_DropDownList.DataValueField = "ID"
                    MedicationPerICSRRoles_DropDownList.DataTextField = "Name"
                    MedicationPerICSRRoles_DropDownList.DataBind()
                    'Populate Medications_DropDownList with all medications associated with that company.
                    'Note: Use of corresponding function is not possible because the Company_ID which is needed is not passed to the function
                    Dim CompanyIDOfCurrentICSR As Integer = Nothing
                    Dim CompanyIDOfCurrentICSRReadCommand As New SqlCommand("SELECT Companies.ID FROM Companies JOIN ICSRs ON Companies.ID = ICSRs.Company_ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                    CompanyIDOfCurrentICSRReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
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
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else  'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateICSRMedication.aspx?ICSRID=" & CurrentICSR_ID)
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
        If SelectionValidator(MedicationPerICSRRoles_DropDownList) = True Then
            MedicationPerICSRRoles_DropDownList.CssClass = CssClassSuccess
            MedicationPerICSRRoles_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
            MedicationPerICSRRoles_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Medications_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(Medications_DropDownList) = True Then
            Medications_DropDownList.CssClass = CssClassSuccess
            Medications_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Medications_DropDownList.CssClass = CssClassFailure
            Medications_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub TotalDailyDose_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If IntegerOrEmptyValidator(TotalDailyDose_Textbox) = True Then
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
        If IntegerOrEmptyValidator(Allocations_Textbox) = True Then
            Allocations_Textbox.CssClass = CssClassSuccess
            Allocations_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Allocations_Textbox.CssClass = CssClassFailure
            Allocations_Textbox.ToolTip = NumberValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Start_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(Start_Textbox) = True Then
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
        If DateOrEmptyValidator(Stop_Textbox) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DrugActions_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        DrugActions_DropDownList.CssClass = CssClassSuccess
        DrugActions_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Write new entry to database
            Dim InsertCommand As New SqlCommand("INSERT INTO MedicationsPerICSR (ICSR_ID, Medication_ID, TotalDailyDose, Allocations, Start, Stop, DrugAction_ID, MedicationPerICSRRole_ID) VALUES(@CurrentICSR_ID, @Medication_ID, CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END, CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END, CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END, CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END)", Connection)
            InsertCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            InsertCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@TotalDailyDose", TryCType(TotalDailyDose_Textbox.Text.Trim, InputTypes.Number))
            InsertCommand.Parameters.AddWithValue("@Allocations", TryCType(Allocations_Textbox.Text.Trim, InputTypes.Number))
            InsertCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            InsertCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            InsertCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
            Try
                Connection.Open()
                InsertCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add change history entry with database updates
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim NewMedicationPerICSR_ID As Integer = Nothing
            Dim NewMedicationPerICSRRole_ID As Integer = Nothing
            Dim NewMedication_ID As Integer = Nothing
            Dim NewTotalDailyDose As Integer = Nothing
            Dim NewAllocations As Integer = Nothing
            Dim NewStart As DateTime = Date.MinValue
            Dim NewStop As DateTime = Date.MinValue
            Dim NewDrugAction_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewMedicationPerICSR_ID = NewReader.GetInt32(0)
                    NewMedicationPerICSRRole_ID = NewReader.GetInt32(1)
                    NewMedication_ID = NewReader.GetInt32(2)
                    NewTotalDailyDose = NewReader.GetInt32(3)
                    NewAllocations = NewReader.GetInt32(4)
                    NewStart = DateOrDateMinValue(NewReader.GetDateTime(5))
                    NewStop = DateOrDateMinValue(NewReader.GetDateTime(6))
                    NewDrugAction_ID = NewReader.GetInt32(7)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("ICSR Medication", NewMedicationPerICSR_ID)
            EntryString += HistoryEntryReferencedValue("ICSR Medication", NewMedicationPerICSR_ID, "Role", tables.MedicationPerICSRRoles, fields.Name, Nothing, NewMedicationPerICSRRole_ID)
            EntryString += HistoryEnrtyPlainValue("ICSR Medication", NewMedicationPerICSR_ID, "Total Daily Dose", Nothing, NewTotalDailyDose)
            EntryString += HistoryEnrtyPlainValue("ICSR Medication", NewMedicationPerICSR_ID, "Allocations Per Day", Nothing, NewAllocations)
            EntryString += HistoryEnrtyPlainValue("ICSR Medication", NewMedicationPerICSR_ID, "Start Date", Date.MinValue, NewStart)
            EntryString += HistoryEnrtyPlainValue("ICSR Medication", NewMedicationPerICSR_ID, "Stop Date", Date.MinValue, NewStop)
            EntryString += HistoryEntryReferencedValue("ICSR Medication", NewMedicationPerICSR_ID, "Action Taken With Drug", tables.DrugActions, fields.Name, Nothing, NewDrugAction_ID)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@CurrentICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
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
            'Format Page Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
            DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
            DropDownListDisabled(Medications_DropDownList)
            TextBoxReadOnly(TotalDailyDose_Textbox)
            TextBoxReadOnly(Allocations_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(DrugActions_DropDownList)
        End If
    End Sub

End Class
