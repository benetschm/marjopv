Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateMedication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Title_Label.Text = "Create New Medication"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.Medications, Nothing, tables.Medications, fields.Create) = True Then
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ConfirmMedicationInput_Button)
                    If LoggedIn_User_CanViewCompanies = True Then
                        Company_Row.Visible = True
                        DropDownListEnabled(Companies_DropDownList)
                        Companies_DropDownList.DataSource = CreateDropDownListDatatable(tables.Companies)
                        Companies_DropDownList.DataValueField = "ID"
                        Companies_DropDownList.DataTextField = "Name"
                        Companies_DropDownList.DataBind()
                    End If
                    If CanEdit(tables.Medications, Nothing, tables.Medications, fields.Name) = True Then
                        TextBoxReadWrite(Name_Textbox)
                    Else
                        TextBoxReadOnly(Name_Textbox)
                        Name_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.Medications, Nothing, tables.Medications, fields.MedicationType_ID) = True Then
                        DropDownListEnabled(MedicationTypes_DropDownList)
                        MedicationTypes_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationTypes)
                        MedicationTypes_DropDownList.DataValueField = "ID"
                        MedicationTypes_DropDownList.DataTextField = "Name"
                        MedicationTypes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(MedicationTypes_DropDownList)
                    End If
                    If CanEdit(tables.Medications, Nothing, tables.Medications, fields.AdministrationRoute_ID) = True Then
                        DropDownListEnabled(AdministrationRoutes_DropDownList)
                        AdministrationRoutes_DropDownList.DataSource = CreateDropDownListDatatable(tables.AdministrationRoutes)
                        AdministrationRoutes_DropDownList.DataValueField = "ID"
                        AdministrationRoutes_DropDownList.DataTextField = "Name"
                        AdministrationRoutes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(AdministrationRoutes_DropDownList)
                    End If
                    If CanEdit(tables.Medications, Nothing, tables.Medications, fields.DoseType_ID) = True Then
                        DropDownListEnabled(DoseTypes_DropDownList)
                        DoseTypes_DropDownList.DataSource = CreateDropDownListDatatable(tables.DoseTypes)
                        DoseTypes_DropDownList.DataValueField = "ID"
                        DoseTypes_DropDownList.DataTextField = "Name"
                        DoseTypes_DropDownList.DataBind()
                    Else
                        DropDownListDisabled(DoseTypes_DropDownList)
                    End If
                Else
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMedication.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Application/ICSRs.aspx")
    End Sub

    Protected Sub Companies_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(Companies_DropDownList) = True Then
            Companies_DropDownList.CssClass = CssClassSuccess
            Companies_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Companies_DropDownList.CssClass = CssClassFailure
            Companies_DropDownList.ToolTip = SelectCompanyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If TextValidator(Name_Textbox) = True Then
            'Check if entered medication name is unique and fail validation if not
            Dim NamesMatch As Boolean = False
            Dim NamesReadCommand As New SqlCommand("SELECT Name FROM Medications WHERE Company_ID = @SelecteddCompany_ID", Connection)
            NamesReadCommand.Parameters.AddWithValue("@SelecteddCompany_ID", Companies_DropDownList.SelectedValue)
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
                Name_Textbox.CssClass = CssClassSuccess
                Name_Textbox.ToolTip = String.Empty
                args.IsValid = True
            Else
                Name_Textbox.CssClass = CssClassFailure
                Name_Textbox.ToolTip = NameUniquenessValidationFailToolTip
                args.IsValid = False
            End If
        Else
            Name_Textbox.CssClass = CssClassFailure
            Name_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub MedicationTypes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(MedicationTypes_DropDownList) = True Then
            MedicationTypes_DropDownList.CssClass = CssClassSuccess
            MedicationTypes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedicationTypes_DropDownList.CssClass = CssClassFailure
            MedicationTypes_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub AdministrationRoutes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(AdministrationRoutes_DropDownList) = True Then
            AdministrationRoutes_DropDownList.CssClass = CssClassSuccess
            AdministrationRoutes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            AdministrationRoutes_DropDownList.CssClass = CssClassFailure
            AdministrationRoutes_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DoseTypes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(DoseTypes_DropDownList) = True Then
            DoseTypes_DropDownList.CssClass = CssClassSuccess
            DoseTypes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            DoseTypes_DropDownList.CssClass = CssClassFailure
            DoseTypes_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ConfirmMedicationInput_Button)
            Status_Label.CssClass = "form-control alert-danger"
            Status_Label.Text = "WARNING: Company cannot be altered after saving a New Medication. Please verify that your input is correct"
            DropDownListDisabled(Companies_DropDownList)
            TextBoxReadOnly(Name_Textbox)
            DropDownListDisabled(MedicationTypes_DropDownList)
            DropDownListDisabled(AdministrationRoutes_DropDownList)
            DropDownListDisabled(DoseTypes_DropDownList)
        End If
    End Sub

    Protected Sub ConfirmMedicationInput_Button_Click(sender As Object, e As EventArgs) Handles ConfirmMedicationInput_Button.Click
        If Page.IsValid = True Then
            'Write New Medication to database
            Dim InsertMedicationCommand As New SqlCommand("INSERT INTO Medications(Company_ID, MedicationType_ID, Name, AdministrationRoute_ID, DoseType_ID) VALUES(CASE WHEN @Company_ID = 0 THEN NULL ELSE @Company_ID END, CASE WHEN @MedicationType_ID = 0 THEN NULL ELSE @MedicationType_ID END, CASE WHEN @Name = '' THEN NULL ELSE @Name END, CASE WHEN @AdministrationRoute_ID = 0 THEN NULL ELSE @AdministrationRoute_ID END, CASE WHEN @DoseType_ID = 0 THEN NULL ELSE @DoseType_ID END)", Connection)
            InsertMedicationCommand.Parameters.AddWithValue("@Company_ID", Companies_DropDownList.SelectedValue)
            InsertMedicationCommand.Parameters.AddWithValue("@MedicationType_ID", MedicationTypes_DropDownList.SelectedValue)
            InsertMedicationCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text.Trim)
            InsertMedicationCommand.Parameters.AddWithValue("@AdministrationRoute_ID", AdministrationRoutes_DropDownList.SelectedValue)
            InsertMedicationCommand.Parameters.AddWithValue("@DoseType_ID", DoseTypes_DropDownList.SelectedValue)
            Try
                Connection.Open()
                InsertMedicationCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add history entry with database updates
            Dim NewMedicationReadCommand As New SqlCommand("SELECT TOP 1 ID, CASE WHEN Name IS NULL THEN '' ELSE Name END AS Name, CASE WHEN MedicationType_ID IS NULL Then 0 ELSE MedicationType_ID END AS MedicationTpe_ID, CASE WHEN AdministrationRoute_ID IS NULL THEN 0 ELSE AdministrationRoute_ID END AS AdministrationRoute_ID, CASE WHEN DoseType_ID IS NULL THEN 0 ELSE DoseType_ID END AS DoseType_ID FROM Medications ORDER BY ID DESC", Connection)
            Dim Medication_ID As Integer = Nothing
            Dim Name As String = String.Empty
            Dim MedicationType_ID As Integer = Nothing
            Dim AdministrationRoute_ID As Integer = Nothing
            Dim DoseType_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewMedicationReader As SqlDataReader = NewMedicationReadCommand.ExecuteReader()
                While NewMedicationReader.Read()
                    Medication_ID = NewMedicationReader.GetInt32(0)
                    Name = NewMedicationReader.GetString(1)
                    MedicationType_ID = NewMedicationReader.GetInt32(2)
                    AdministrationRoute_ID = NewMedicationReader.GetInt32(3)
                    DoseType_ID = NewMedicationReader.GetInt32(4)
                End While
                Connection.Close()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Medication", Medication_ID)
            EntryString += HistoryEnrtyPlainValue("Medication", Medication_ID, "Generic Name", String.Empty, Name)
            EntryString += HistoryEntryReferencedValue("Medication", Medication_ID, "Medication Type", tables.MedicationTypes, fields.Name, Nothing, MedicationType_ID)
            EntryString += HistoryEntryReferencedValue("Medication", Medication_ID, "Administration Route", tables.AdministrationRoutes, fields.Name, Nothing, AdministrationRoute_ID)
            EntryString += HistoryEntryReferencedValue("Medication", Medication_ID, "Dose Type", tables.DoseTypes, fields.Name, Nothing, DoseType_ID)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories(Medication_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Medication_ID", Medication_ID)
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
            SaveUpdates_Button.Visible = False
            ConfirmMedicationInput_Button.Visible = False
            Cancel_Button.Visible = False
            Status_Label.Visible = True
            Status_Label.CssClass = "form-control alert-success"
            Status_Label.Text = "Changes saved"
            DropDownListDisabled(Companies_DropDownList)
            TextBoxReadOnly(Name_Textbox)
            DropDownListDisabled(MedicationTypes_DropDownList)
            DropDownListDisabled(AdministrationRoutes_DropDownList)
            DropDownListDisabled(DoseTypes_DropDownList)
        End If
    End Sub

End Class
