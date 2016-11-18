Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditMedicationBasicInformation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            Title_Label.Text = "Medication " & CurrentMedication_ID & ": Edit Basic Information"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.Create) = True Then
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
                    If LoggedIn_User_CanViewCompanies = True Then
                        Company_Row.Visible = True
                        TextBoxReadOnly(Company_Textbox)
                    End If
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.Name) = True Then
                        TextBoxReadWrite(Name_Textbox)
                        Name_Textbox.ToolTip = "Please enter a generic medication name"
                    Else
                        TextBoxReadOnly(Name_Textbox)
                        Name_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.MedicationType_ID) = True Then
                        DropDownListEnabled(MedicationTypes_DropDownList)
                        MedicationTypes_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationTypes)
                        MedicationTypes_DropDownList.DataValueField = "ID"
                        MedicationTypes_DropDownList.DataTextField = "Name"
                        MedicationTypes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(MedicationTypes_DropDownList)
                    End If
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.AdministrationRoute_ID) = True Then
                        DropDownListEnabled(AdministrationRoutes_DropDownList)
                        AdministrationRoutes_DropDownList.DataSource = CreateDropDownListDatatable(tables.AdministrationRoutes)
                        AdministrationRoutes_DropDownList.DataValueField = "ID"
                        AdministrationRoutes_DropDownList.DataTextField = "Name"
                        AdministrationRoutes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(AdministrationRoutes_DropDownList)
                    End If
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.DoseType_ID) = True Then
                        DropDownListEnabled(DoseTypes_DropDownList)
                        DoseTypes_DropDownList.DataSource = CreateDropDownListDatatable(tables.DoseTypes)
                        DoseTypes_DropDownList.DataValueField = "ID"
                        DoseTypes_DropDownList.DataTextField = "Name"
                        DoseTypes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(DoseTypes_DropDownList)
                    End If
                    'Populate Controls
                    Dim MedicationReadCommand As New SqlCommand("SELECT Companies.Name As Company_Name, CASE WHEN Medications.Name IS NULL THEN '' ELSE Medications.Name END AS Medication_Name, CASE WHEN MedicationType_ID IS NULL Then 0 ELSE MedicationType_ID END AS MedicationTpe_ID, CASE WHEN AdministrationRoute_ID IS NULL THEN 0 ELSE AdministrationRoute_ID END AS AdministrationRoute_ID, CASE WHEN DoseType_ID IS NULL THEN 0 ELSE DoseType_ID END AS DoseType_ID FROM Medications LEFT JOIN Companies ON Medications.Company_ID = Companies.ID WHERE Medications.ID = @CurrentMedication_ID", Connection)
                    MedicationReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
                    Try
                        Connection.Open()
                        Dim MedicationReader As SqlDataReader = MedicationReadCommand.ExecuteReader()
                        While MedicationReader.Read()
                            Company_Textbox.Text = MedicationReader.GetString(0)
                            Name_Textbox.Text = MedicationReader.GetString(1)
                            AtEditPageLoad_Name_HiddenField.Value = MedicationReader.GetString(1)
                            MedicationTypes_DropDownList.SelectedValue = MedicationReader.GetInt32(2)
                            AtEditPageLoad_MedicationType_HiddenField.Value = MedicationReader.GetInt32(2)
                            AdministrationRoutes_DropDownList.SelectedValue = MedicationReader.GetInt32(3)
                            AtEditPageLoad_AdministrationRoute_HiddenField.Value = MedicationReader.GetInt32(3)
                            DoseTypes_DropDownList.SelectedValue = MedicationReader.GetInt32(4)
                            AtEditPageLoad_DoseType_HiddenField.Value = MedicationReader.GetInt32(4)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicationBasicInformation.aspx?MedicationID=" & CurrentMedication_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Dim CurrentMedication_ID As Integer = CInt(MedicationID_HiddenField.Value)
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub ReturnToMedicationOverview_Button_Click() Handles ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Name_Textbox.Text.Trim <> String.Empty Then
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Determine Company_ID of current Medication
            Dim CurrentMedicationReadCommand As New SqlCommand("SELECT Company_ID FROM Medications WHERE ID = @CurrentMedication_ID", Connection)
            CurrentMedicationReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Dim CurrentMedicationCompany_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim CurrentMedicationReader As SqlDataReader = CurrentMedicationReadCommand.ExecuteReader()
                While CurrentMedicationReader.Read()
                    CurrentMedicationCompany_ID = CurrentMedicationReader.GetInt32(0)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim NamesMatch As Boolean = False
            Dim NamesReadCommand As New SqlCommand("SELECT Name FROM Medications WHERE Company_ID = @CurrentMedicationCompany_ID AND ID <> @CurrentMedication_ID", Connection)
            NamesReadCommand.Parameters.AddWithValue("@CurrentMedicationCompany_ID", CurrentMedicationCompany_ID)
            NamesReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Try
                Connection.Open()
                Dim NamesReader As SqlDataReader = NamesReadCommand.ExecuteReader()
                While NamesReader.Read()
                    If Name_Textbox.Text.Trim = NamesReader.GetString(0) Then
                        NamesMatch = True
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            If NamesMatch = False Then
                Name_Textbox.CssClass = "form-control alert-success"
                Name_Textbox.ToolTip = String.Empty
                args.IsValid = True
            Else
                Name_Textbox.CssClass = "form-control alert-danger"
                Name_Textbox.ToolTip = "The generic name you have chosen is already in the database. Please select a different name"
                args.IsValid = False
            End If
        Else
            Name_Textbox.CssClass = "form-control alert-danger"
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = False
        End If
    End Sub

    Protected Sub MedicationTypes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If MedicationTypes_DropDownList.SelectedValue <> 0 Then
            MedicationTypes_DropDownList.CssClass = "form-control alert-success"
            MedicationTypes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedicationTypes_DropDownList.CssClass = "form-control alert-danger"
            MedicationTypes_DropDownList.ToolTip = "Please ensure you are selecting a valid medication type"
            args.IsValid = False
        End If
    End Sub

    Protected Sub AdministrationRoutes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If AdministrationRoutes_DropDownList.SelectedValue <> 0 Then
            AdministrationRoutes_DropDownList.CssClass = "form-control alert-success"
            AdministrationRoutes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            AdministrationRoutes_DropDownList.CssClass = "form-control alert-danger"
            AdministrationRoutes_DropDownList.ToolTip = "Please ensure you are selecting a valid administration route"
            args.IsValid = False
        End If
    End Sub

    Protected Sub DoseTypes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DoseTypes_DropDownList.SelectedValue <> 0 Then
            DoseTypes_DropDownList.CssClass = "form-control alert-success"
            DoseTypes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            DoseTypes_DropDownList.CssClass = "form-control alert-danger"
            DoseTypes_DropDownList.ToolTip = "Please ensure you are selecting a valid dose type"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Retrieve values as present in database at edit page load from hidden fields and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_Name As String = AtEditPageLoad_Name_HiddenField.Value
            Dim AtEditPageLoad_MedicationType_ID As Integer = AtEditPageLoad_MedicationType_HiddenField.Value
            Dim AtEditPageLoad_AdministrationRoute_ID As Integer = AtEditPageLoad_AdministrationRoute_HiddenField.Value
            Dim AtEditPageLoad_DoseType_ID As Integer = AtEditPageLoad_DoseType_HiddenField.Value
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT CASE WHEN Name IS NULL THEN '' ELSE Name END AS Name, CASE WHEN MedicationType_ID IS NULL THEN 0 ELSE MedicationType_ID END AS MedicationType_ID, CASE WHEN AdministrationRoute_ID IS NULL THEN 0 ELSE AdministrationRoute_ID END AS AdministrationRoute_ID, CASE WHEN DoseType_ID IS NULL THEN 0 ELSE DoseType_ID END AS DoseType_ID FROM Medications WHERE ID = @CurrentMedication_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Dim AtSaveButtonClick_Name As String = String.Empty
            Dim AtSaveButtonClick_MedicationType_ID As Integer = Nothing
            Dim AtSaveButtonClick_AdministrationRoute_ID As Integer = Nothing
            Dim AtSaveButtonClick_DoseType_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_Name = AtSaveButtonClickReader.GetString(0)
                    AtSaveButtonClick_MedicationType_ID = AtSaveButtonClickReader.GetInt32(1)
                    AtSaveButtonClick_AdministrationRoute_ID = AtSaveButtonClickReader.GetInt32(2)
                    AtSaveButtonClick_DoseType_ID = AtSaveButtonClickReader.GetInt32(3)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            'Dim DiscrepancyString As String = String.Empty
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Name, AtSaveButtonClick_Name, "Generic Name")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_MedicationType_ID, AtSaveButtonClick_MedicationType_ID, "Medication Type")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_AdministrationRoute_ID, AtSaveButtonClick_AdministrationRoute_ID, "Administration Route")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_DoseType_ID, AtSaveButtonClick_DoseType_ID, "Dose Type")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            'If DiscrepancyString <> String.Empty Then
            '    AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
            '    Status_Label.Style.Add("text-align", "left")
            '    Status_Label.Style.Add("height", "auto")
            '    Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
            '    Status_Label.CssClass = "form-control alert-danger"
            '    If LoggedIn_User_CanViewCompanies = True Then
            '        Company_Row.Visible = True
            '    End If
            '    TextBoxReadOnly(Company_Textbox)
            '    DropDownListDisabled(MedicationTypes_DropDownList)
            '    DropDownListDisabled(AdministrationRoutes_DropDownList)
            '    DropDownListDisabled(DoseTypes_DropDownList)
            '    Exit Sub
            'End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand("UPDATE Medications SET Name = (CASE WHEN @Name = '' THEN NULL ELSE @Name END), MedicationType_ID = (CASE WHEN @MedicationType_ID = 0 THEN NULL ELSE @MedicationType_ID END), AdministrationRoute_ID = (CASE WHEN @AdministrationRoute_ID = 0 THEN NULL ELSE @AdministrationRoute_ID END), DoseType_ID = (CASE WHEN @DoseType_ID = 0 THEN NULL ELSE @DoseType_ID END) WHERE ID = @CurrentMedication_ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@MedicationType_ID", MedicationTypes_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@AdministrationRoute_ID", AdministrationRoutes_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            UpdateCommand.Parameters.AddWithValue("@DoseType_ID", DoseTypes_DropDownList.SelectedValue)
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
            Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN Name IS NULL THEN '' ELSE Name END AS Name, CASE WHEN MedicationType_ID IS NULL THEN 0 ELSE MedicationType_ID END AS MedicationType_ID, CASE WHEN AdministrationRoute_ID IS NULL THEN 0 ELSE AdministrationRoute_ID END AS AdministrationRoute_ID, CASE WHEN DoseType_ID IS NULL THEN 0 ELSE DoseType_ID END AS DoseType_ID FROM Medications WHERE ID = @CurrentMedication_ID", Connection)
            UpdatedReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Dim Updated_Name As String = String.Empty
            Dim Updated_MedicationType_ID As Integer = Nothing
            Dim Updated_AdministrationRoute_ID As Integer = Nothing
            Dim Updated_DoseType_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                While UpdatedReader.Read()
                    Updated_Name = UpdatedReader.GetString(0)
                    Updated_MedicationType_ID = UpdatedReader.GetInt32(1)
                    Updated_AdministrationRoute_ID = UpdatedReader.GetInt32(2)
                    Updated_DoseType_ID = UpdatedReader.GetInt32(3)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Compare old and new variables to generate EntryString for Change History Entry
            'Dim EntryString As String = String.Empty
            'EntryString = HistoryDatabasebUpdateIntro
            'If Updated_Name <> AtSaveButtonClick_Name Then
            '    EntryString += HistoryEntryPlainValue("Medication", CurrentMedication_ID, "Generic Name", AtSaveButtonClick_Name, Updated_Name)
            'End If
            'If Updated_MedicationType_ID <> AtSaveButtonClick_MedicationType_ID Then
            '    EntryString += HistoryEntryReferencedValue("Medication", CurrentMedication_ID, "Type", tables.MedicationTypes, fields.Name, AtSaveButtonClick_MedicationType_ID, Updated_MedicationType_ID)
            'End If
            'If Updated_AdministrationRoute_ID <> AtSaveButtonClick_AdministrationRoute_ID Then
            '    EntryString += HistoryEntryReferencedValue("Medication", CurrentMedication_ID, "Administration Route", tables.AdministrationRoutes, fields.Name, AtSaveButtonClick_AdministrationRoute_ID, Updated_AdministrationRoute_ID)
            'End If
            'If Updated_DoseType_ID <> AtSaveButtonClick_DoseType_ID Then
            '    EntryString += HistoryEntryReferencedValue("Medication", CurrentMedication_ID, "Dose Type", tables.DoseTypes, fields.Name, AtSaveButtonClick_DoseType_ID, Updated_DoseType_ID)
            'End If
            'EntryString += HistoryDatabasebUpdateOutro
            ''Generate History Entry if any data was changed in the database
            'If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
            '    Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories (Medication_ID, User_ID, Timepoint, Entry) VALUES (@CurrentMedication_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            '    Try
            '        Connection.Open()
            '        InsertHistoryEntryCommand.ExecuteNonQuery()
            '    Catch ex As Exception
            '        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            '        Exit Sub
            '    Finally
            '        Connection.Close()
            '    End Try
            'End If
            'Format Controls
            If LoggedIn_User_CanViewCompanies = True Then
                Company_Row.Visible = True
            End If
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
            TextBoxReadOnly(Company_Textbox)
            TextBoxReadOnly(Name_Textbox)
            DropDownListDisabled(MedicationTypes_DropDownList)
            DropDownListDisabled(AdministrationRoutes_DropDownList)
            DropDownListDisabled(DoseTypes_DropDownList)
        End If
    End Sub

End Class
