Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_ICSROverview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            If Login_Status = True Then
                Title_Label.Text = "ICSR " & CurrentICSR_ID & " Overview"
                'Populate Basic Information Tab Controls
                Dim BasicInformationReadCommand As New SqlCommand("SELECT Companies.Name AS Company_Name, CASE WHEN ICSRStatuses.Name IS NULL THEN '' ELSE ICSRStatuses.Name END AS ICSRStatus_Name, CASE WHEN Users.Name IS NULL THEN '' ELSE Users.Name END AS Assignee_Name, CASE WHEN Patient_Initials IS NULL THEN '' ELSE Patient_Initials END AS Patient_Initials, CASE WHEN YearsOfBirth.Name IS NULL THEN '' ELSE YearsOfBirth.Name END AS Patient_YearOfBirth, CASE WHEN GENDers.Name IS NULL THEN '' ELSE Genders.Name END AS Patient_Gender, CASE WHEN IsSerious.Name IS NULL THEN '' ELSE IsSerious.Name END AS IsSerious_Name, CASE WHEN SeriousnessCriteria.Name IS NULL THEN '' ELSE SeriousnessCriteria.Name END AS SeriousnessCriterion_Name, CASE WHEN Narrative IS NULL THEN '' ELSE Narrative END AS Narrative, CASE WHEN CompanyComment IS NULL THEN '' ELSE CompanyComment END as CompanyComment FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID LEFT JOIN Users ON ICSRs.Assignee_ID = Users.ID INNER JOIN ICSRStatuses ON ICSRs.ICSRStatus_ID = ICSRStatuses.ID LEFT JOIN YearsOfBirth ON ICSRs.Patient_YearOfBirth_ID = YearsOfBirth.ID LEFT JOIN Genders ON ICSRs.Patient_Gender_ID = Genders.ID LEFT JOIN IsSerious ON ICSRs.IsSerious_ID = IsSerious.ID LEFT JOIN SeriousnessCriteria ON ICSRs.SeriousnessCriterion_ID = SeriousnessCriteria.ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                BasicInformationReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    Dim BasicInformationReader As SqlDataReader = BasicInformationReadCommand.ExecuteReader()
                    While BasicInformationReader.Read()
                        CompanyName_Textbox.Text = BasicInformationReader.GetString(0)
                        ICSRStatus_Textbox.Text = BasicInformationReader.GetString(1)
                        Assignee_Textbox.Text = BasicInformationReader.GetString(2)
                        PatientInitials_Textbox.Text = BasicInformationReader.GetString(3)
                        PatientYearOfBirth_Textbox.Text = BasicInformationReader.GetString(4)
                        PatientGender_Textbox.Text = BasicInformationReader.GetString(5)
                        IsSerious_Textbox.Text = BasicInformationReader.GetString(6)
                        SeriousnessCriterion_Textbox.Text = BasicInformationReader.GetString(7)
                        Narrative_Textbox.Text = BasicInformationReader.GetString(8)
                        CompanyComment_Textbox.Text = BasicInformationReader.GetString(9)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Format Basic Information Tab Controls
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Edit) = True Then 'Enable Controls if LoggedIn_User has corresponding rights
                    EditICSRBasicInformation_Button.Visible = True
                Else
                    EditICSRBasicInformation_Button.Visible = False
                End If
                If LoggedIn_User_CanViewCompanies = True Then 'Enable Control if LoggedIn_User has corresponding rights
                    Company_Row.Visible = True
                End If
                CompanyName_Textbox.ReadOnly = True
                Assignee_Textbox.ReadOnly = True
                ICSRStatus_Textbox.ReadOnly = True
                PatientInitials_Textbox.ReadOnly = True
                PatientYearOfBirth_Textbox.ReadOnly = True
                PatientGender_Textbox.ReadOnly = True
                IsSerious_Textbox.ReadOnly = True
                SeriousnessCriterion_Textbox.ReadOnly = True
                Narrative_Textbox.ReadOnly = True
                CompanyComment_Textbox.ReadOnly = True
                'Populate Report Tab Controls
                Dim ReportsReadCommand As New SqlCommand("SELECT Reports.ID AS Report_ID, CASE WHEN ReportComplexities.Name IS NULL THEN '' ELSE ReportComplexities.Name END AS ReportComplexity_Name, CASE WHEN ReportTypes.Name IS NULL THEN '' ELSE ReportTypes.Name END AS ReportType_Name, CASE WHEN ReportSources.Name IS NULL THEN '' ELSE ReportSources.Name END AS ReportSource_Name, CASE WHEN ReportStatuses.Name IS NULL THEN '' ELSE ReportStatuses.Name END AS ReportStatus_Name, CASE WHEN Reports.Received IS NULL THEN '' ELSE Reports.Received END AS Report_Received, CASE WHEN Reports.Due IS NULL THEN '' ELSE Reports.Due END AS Report_Due, CASE WHEN Reports.ReporterName IS NULL THEN '' ELSE Reports.ReporterName END AS Reporter_Name, CASE WHEN Reports.ReporterAddress IS NULL THEN '' ELSE Reports.ReporterAddress END AS Reporter_Address, CASE WHEN Reports.ReporterPhone IS NULL THEN '' ELSE Reports.ReporterPhone END AS Reporter_Phone, CASE WHEN Reports.ReporterFax IS NULL THEN '' ELSE Reports.ReporterFax END AS Reporter_Fax, CASE WHEN Reports.ReporterEmail IS NULL THEN '' ELSE Reports.ReporterEmail END AS Reporter_Mail, CASE WHEN ExpeditedReportingRequired.Name IS NULL THEN '' ELSE ExpeditedReportingRequired.Name END AS ExpeditedReportingRequired_Name, CASE WHEN ExpeditedReportingDone.Name IS NULL THEN '' ELSE ExpeditedReportingDone.Name END AS ExpeditedReportingDone_Name, CASE WHEN Reports.ExpeditedReportingDate IS NULL THEN '' ELSE Reports.ExpeditedReportingDate END AS ExpeditedReportingDate FROM Reports LEFT JOIN ReportComplexities ON Reports.ReportComplexity_ID = ReportComplexities.ID LEFT JOIN ReportTypes ON Reports.ReportType_ID = ReportTypes.ID LEFT JOIN ReportSources ON Reports.ReportSource_ID = ReportSources.ID LEFT JOIN ReportStatuses ON Reports.ReportStatus_ID = ReportStatuses.ID LEFT JOIN ExpeditedReportingRequired ON Reports.ExpeditedReportingRequired_ID = ExpeditedReportingRequired.ID LEFT JOIN ExpeditedReportingDone ON Reports.ExpeditedReportingDone_ID = ExpeditedReportingDone.ID INNER JOIN ICSRs ON Reports.ICSR_ID = ICSRs.ID WHERE ICSRs.ID = @CurrentICSR_ID ORDER BY Reports.ID DESC", Connection)
                ReportsReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    Reports_Repeater.DataSource = ReportsReadCommand.ExecuteReader()
                    Reports_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format Reports Tab Controls
                Dim Reports_Repeater_HeaderTemplate As Control = Reports_Repeater.Controls(0).Controls(0)
                Dim CreateReport_Button As Button = TryCast(Reports_Repeater_HeaderTemplate.FindControl("CreateReport_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Create) = True Then
                    CreateReport_Button.Visible = True
                Else
                    CreateReport_Button.Visible = False
                End If
                Dim CurrentUserCanEditReportsinCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Edit) = True Then 'Enable Control if LoggedIn_User has corresponding edit rights
                    CurrentUserCanEditReportsinCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteReportsinCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Delete) = True Then 'Enable Control if LoggedIn_User has corresponding edit rights
                    CurrentUserCanDeleteReportsinCurrentICSR = True
                End If
                For Each item In Reports_Repeater.Items
                    If CurrentUserCanEditReportsinCurrentICSR = True Or CurrentUserCanDeleteReportsinCurrentICSR = True Then 'Enable and format Control if LoggedIn_User has corresponding edit rights
                        item.findcontrol("ReportTitle").colspan = "1"
                        item.findcontrol("ReportButtons_Header").visible = True
                        If CurrentUserCanEditReportsinCurrentICSR = True Then
                            item.findcontrol("EditReport_Button").Visible = True
                        Else
                            item.findcontrol("EditReport_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteReportsinCurrentICSR = True Then
                            item.findcontrol("DeleteReport_Button").Visible = True
                        Else
                            item.findcontrol("DeleteReport_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("ReportTitle").colspan = "2"
                        item.findcontrol("ReportButtons_Header").Visible = False
                    End If
                    item.findcontrol("ReportType_Textbox").ReadOnly = True
                    item.findcontrol("ReportStatus_TextBox").ReadOnly = True
                    item.findcontrol("ReportReceived_TextBox").ReadOnly = True
                    item.findcontrol("ReportDue_TextBox").ReadOnly = True
                    item.findcontrol("ReportComplexity_TextBox").ReadOnly = True
                    item.findcontrol("ReportSource_TextBox").ReadOnly = True
                    item.findcontrol("ReporterNameAddress_TextBox").ReadOnly = True
                    item.findcontrol("ReporterPhone_TextBox").ReadOnly = True
                    item.findcontrol("ReporterFax_TextBox").ReadOnly = True
                    item.findcontrol("ReporterEmail_TextBox").ReadOnly = True
                    item.findcontrol("ExpeditedReportingRequired_TextBox").ReadOnly = True
                    item.findcontrol("ExpeditedReportingDone_TextBox").ReadOnly = True
                    item.findcontrol("ExpeditedReportingDate_TextBox").ReadOnly = True
                Next
                'Populate Suspected Drugs Tab Controls
                Dim SuspectedDrugsReadCommand As New SqlCommand("SELECT MedicationsPerICSR.ID AS ICSRMedication_ID, CASE WHEN Medications.Name IS NULL THEN '' ELSE Medications.Name END AS ICSRMedication_Name, CASE WHEN MedicationsPerICSR.TotalDailyDose IS NULL THEN 0 ELSE MedicationsPerICSR.TotalDailyDose END AS TotalDailyDose, CASE WHEN MedicationsPerICSR.Allocations IS NULL THEN 0 ELSE MedicationsPerICSR.Allocations END AS Allocations, CASE WHEN DoseTypes.Name IS NULL THEN '' ELSE DoseTypes.Name END AS DoseType_Name, CASE WHEN MedicationsPerICSR.Start IS NULL THEN '' ELSE MedicationsPerICSR.Start END AS Start, CASE WHEN MedicationsPerICSR.Stop IS NULL THEN '' ELSE MedicationsPerICSR.Stop END AS Stop, CASE WHEN DrugActions.Name IS NULL THEN '' ELSE DrugActions.Name END AS DrugAction_Name FROM MedicationsPerICSR LEFT JOIN Medications ON Medications.ID = MedicationsPerICSR.Medication_ID LEFT JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID LEFT JOIN DrugActions ON DrugActions.ID = MedicationsPerICSR.DrugAction_ID INNER JOIN ICSRs ON MedicationsPerICSR.ICSR_ID = ICSRs.ID WHERE MedicationsPerICSR.MedicationPerICSRRole_ID = @SuspectedDrug AND ICSRs.ID = @CurrentICSR_ID", Connection)
                SuspectedDrugsReadCommand.Parameters.AddWithValue("@SuspectedDrug", 1) 'selects entry corresponding to Suspected Drug from MedicationPerICSR_Roles
                SuspectedDrugsReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    SuspectedDrugs_Repeater.DataSource = SuspectedDrugsReadCommand.ExecuteReader()
                    SuspectedDrugs_Repeater.DataBind()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Format Suspected Drugs Tab Controls
                Dim SuspectedDrugs_Repeater_HeaderTemplate As Control = SuspectedDrugs_Repeater.Controls(0).Controls(0)
                Dim CreateSuspectedDrug_Button As Button = TryCast(SuspectedDrugs_Repeater_HeaderTemplate.FindControl("CreateICSRMedication_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Create) = True Then
                    CreateSuspectedDrug_Button.Visible = True
                Else
                    CreateSuspectedDrug_Button.Visible = False
                End If
                'Enable Controls if LoggedIn_User has corresponding edit rights
                Dim CurrentUserCanEditSuspectedDrugsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Edit) = True Then
                    CurrentUserCanEditSuspectedDrugsInCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteSuspectedDrugsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Delete) = True Then
                    CurrentUserCanDeleteSuspectedDrugsInCurrentICSR = True
                End If
                For Each item In SuspectedDrugs_Repeater.Items
                    If CurrentUserCanEditSuspectedDrugsInCurrentICSR = True Or CurrentUserCanDeleteSuspectedDrugsInCurrentICSR Then
                        item.findcontrol("SuspectedDrugTitle").colspan = "1"
                        item.findcontrol("ICSRMedication_Buttons_Header").visible = True
                        If CurrentUserCanEditSuspectedDrugsInCurrentICSR = True Then
                            item.findcontrol("EditICSRMedication_Button").Visible = True
                        Else
                            item.findcontrol("EditICSRMedication_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteSuspectedDrugsInCurrentICSR = True Then
                            item.findcontrol("DeleteICSRMedication_Button").Visible = True
                        Else
                            item.findcontrol("DeleteICSRMedication_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("SuspectedDrugTitle").colspan = "2"
                        item.findcontrol("ICSRMedication_Buttons_Header").Visible = False
                        item.findcontrol("EditICSRMedication_Button").Visible = False
                    End If
                    item.findcontrol("ICSRMedication_Name_Textbox").ReadOnly = True
                    item.findcontrol("TotalDailyDose_TextBox").ReadOnly = True
                    item.findcontrol("Allocations_TextBox").ReadOnly = True
                    item.findcontrol("Start_TextBox").ReadOnly = True
                    item.findcontrol("Stop_TextBox").ReadOnly = True
                    item.findcontrol("DrugAction_Name_TextBox").ReadOnly = True
                Next
                'Populate Events Tab Controls
                Dim AEsReadCommand As New SqlCommand("SELECT AEs.ID AS AE_ID, AEs.MedDRATerm, CASE WHEN AEs.Start IS NULL THEN '' ELSE AEs.Start END AS Start, CASE WHEN AEs.Stop IS NULL THEN '' ELSE AEs.Stop END AS Stop, CASE WHEN Outcomes.Name IS NULL THEN '' ELSE Outcomes.Name END AS Outcome_Name, CASE WHEN DechallengeResults.Name IS NULL THEN '' ELSE DechallengeResults.Name END AS DechallengeResult_Name, CASE WHEN RechallengeResults.Name IS NULL THEN '' ELSE RechallengeResults.Name END AS RechallengeResult_Name FROM AEs LEFT JOIN Outcomes ON AEs.Outcome_ID = Outcomes.ID LEFT JOIN DechallengeResults ON AEs.DechallengeResult_ID = DechallengeResults.ID LEFT JOIN RechallengeResults ON AEs.RechallengeResult_ID = RechallengeResults.ID WHERE AEs.ICSR_ID = @CurrentICSR_ID", Connection)
                AEsReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    AEs_Repeater.DataSource = AEsReadCommand.ExecuteReader()
                    AEs_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format Events Tab Controls
                Dim AEs_Repeater_HeaderTemplate As Control = AEs_Repeater.Controls(0).Controls(0)
                Dim CreateAE_Button As Button = TryCast(AEs_Repeater_HeaderTemplate.FindControl("CreateAE_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Create) = True Then
                    CreateAE_Button.Visible = True
                Else
                    CreateAE_Button.Visible = False
                End If
                Dim CurrentUserCanEditAEsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Edit) = True Then 'Enable Control if LoggedIn_User has corresponding edit rights
                    CurrentUserCanEditAEsInCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteAEsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Delete) = True Then
                    CurrentUserCanDeleteAEsInCurrentICSR = True
                End If
                For Each item In AEs_Repeater.Items
                    If CurrentUserCanEditAEsInCurrentICSR = True Or CurrentUserCanDeleteAEsInCurrentICSR = True Then 'Enable and format control if LoggedIn_User has corresponding edit rights
                        item.findcontrol("AETitle").colspan = "1"
                        item.findcontrol("AE_Buttons_Header").visible = True
                        If CurrentUserCanEditAEsInCurrentICSR = True Then
                            item.findcontrol("EditAE_Button").Visible = True
                        Else
                            item.findcontrol("EditAE_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteAEsInCurrentICSR = True Then
                            item.findcontrol("DeleteAE_Button").Visible = True
                        Else
                            item.findcontrol("DeleteAE_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("AETitle").colspan = "2"
                        item.findcontrol("AE_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("MedDRATerm_TextBox").ReadOnly = True
                    item.findcontrol("Start_TextBox").ReadOnly = True
                    item.findcontrol("Stop_TextBox").ReadOnly = True
                    item.findcontrol("Outcome_TextBox").ReadOnly = True
                    item.findcontrol("DechallengeResult_Name_TextBox").ReadOnly = True
                    item.findcontrol("RechallengeResult_Name_TextBox").ReadOnly = True
                    item.findcontrol("Start_TextBox").ReadOnly = True
                    item.findcontrol("Stop_TextBox").ReadOnly = True
                Next
                'Populate Relations Tab Controls
                Dim RelationsReadCommand As New SqlCommand("SELECT Relations.ID AS Relation_ID, CASE WHEN Relations.MedicationPerICSR_ID IS NULL THEN 0 ELSE Relations.MedicationPerICSR_ID END AS MedicationPerICSR_ID, CASE WHEN Medications.Name IS NULL THEN '' ELSE  Medications.Name END AS Medication_Name, CASE WHEN Relations.AE_ID IS NULL THEN 0 ELSE Relations.AE_ID END AS AE_ID, CASE WHEN AEs.MedDRATerm IS NULL THEN '' ELSE AEs.MedDRATerm END AS AE_MedDRATerm, CASE WHEN RelatednessCriteriaReporter.Name IS NULL THEN '' ELSE RelatednessCriteriaReporter.Name END AS RelatednessCriterionReporter_Name, CASE WHEN RelatednessCriteriaManufacturer.Name IS NULL THEN '' ELSE RelatednessCriteriaManufacturer.Name END AS RelatednessCriterionManufacturer_Name, CASE WHEN ExpectednessCriteria.Name IS NULL THEN '' ELSE ExpectednessCriteria.Name END AS ExpectendessCriterion_Name FROM Relations LEFT JOIN MedicationsPerICSR ON Relations.MedicationperICSR_ID = MedicationsPerICSR.ID LEFT JOIN Medications ON MedicationsperICSR.Medication_ID = Medications.ID LEFT JOIN AEs ON Relations.AE_ID = AEs.ID LEFT JOIN RelatednessCriteriaReporter ON Relations.RelatednessCriterionReporter_ID = RelatednessCriteriaReporter.ID LEFT JOIN RelatednessCriteriaManufacturer ON Relations.RelatednessCriterionManufacturer_ID = RelatednessCriteriaManufacturer.ID LEFT JOIN ExpectednessCriteria ON Relations.ExpectednessCriterion_ID = ExpectednessCriteria.ID INNER JOIN ICSRs AS AE_ICSRs ON AEs.ICSR_ID = AE_ICSRs.ID INNER JOIN ICSRs AS MedPerICSR_ICSRs ON MedicationsPerICSR.ICSR_ID = MedPerICSR_ICSRs.ID WHERE AE_ICSRs.ID = @CurrentICSR_ID", Connection)
                RelationsReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    Relations_Repeater.DataSource = RelationsReadCommand.ExecuteReader()
                    Relations_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format Relations Tab Controls
                Dim Relations_Repeater_HeaderTemplate As Control = Relations_Repeater.Controls(0).Controls(0)
                Dim CreateRelation_Button As Button = TryCast(Relations_Repeater_HeaderTemplate.FindControl("CreateRelation_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Create) = True Then
                    CreateRelation_Button.Visible = True
                Else
                    CreateRelation_Button.Visible = False
                End If
                Dim CurrentUserCanEditRelationsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Edit) = True Then
                    CurrentUserCanEditRelationsInCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteRelationsInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Delete) = True Then
                    CurrentUserCanDeleteRelationsInCurrentICSR = True
                End If
                For Each item In Relations_Repeater.Items
                    If CurrentUserCanEditRelationsInCurrentICSR = True Or CurrentUserCanDeleteRelationsInCurrentICSR = True Then
                        item.findcontrol("RelationTitle").colspan = "1"
                        item.findcontrol("Relation_Buttons_Header").visible = True
                        If CurrentUserCanEditRelationsInCurrentICSR = True Then
                            item.findcontrol("EditRelation_Button").Visible = True
                        Else
                            item.findcontrol("EditRelation_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteRelationsInCurrentICSR = True Then
                            item.findcontrol("DeleteRelation_Button").Visible = True
                        Else
                            item.findcontrol("DeleteRelation_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("RelationTitle").colspan = "2"
                        item.findcontrol("Relation_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("SuspectedDrug_TextBox").ReadOnly = True
                    item.findcontrol("AE_TextBox").ReadOnly = True
                    item.findcontrol("RelatednessCriterionReporter_Name_TextBox").ReadOnly = True
                    item.findcontrol("RelatednessCriterionManufacturer_Name_TextBox").ReadOnly = True
                    item.findcontrol("ExpectendessCriterion_Name_TextBox").ReadOnly = True
                Next
                'Populate ConMed Tab Controls
                Dim ConMedReadCommand As New SqlCommand("SELECT MedicationsPerICSR.ID AS ICSRMedication_ID, CASE WHEN Medications.Name IS NULL THEN '' ELSE Medications.Name END AS Medication_Name, CASE WHEN MedicationsPerICSR.TotalDailyDose IS NULL THEN 0 ELSE MedicationsPerICSR.TotalDailyDose END AS TotalDailyDose, CASE WHEN MedicationsPerICSR.Allocations IS NULL THEN 0 ELSE MedicationsPerICSR.Allocations END AS Allocations, CASE WHEN DoseTypes.Name IS NULL THEN '' ELSE DoseTypes.Name END AS DoseType_Name, CASE WHEN MedicationsPerICSR.Start IS NULL THEN '' ELSE MedicationsPerICSR.Start END AS Start, CASE WHEN MedicationsPerICSR.Stop IS NULL THEN '' ELSE MedicationsPerICSR.Stop END AS Stop, CASE WHEN DrugActions.Name IS NULL THEN '' ELSE DrugActions.Name END AS DrugAction_Name FROM MedicationsPerICSR LEFT JOIN Medications ON Medications.ID = MedicationsPerICSR.Medication_ID LEFT JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID LEFT JOIN DrugActions ON DrugActions.ID = MedicationsPerICSR.DrugAction_ID INNER JOIN ICSRs ON MedicationsPerICSR.ICSR_ID = ICSRs.ID WHERE MedicationsPerICSR.MedicationPerICSRRole_ID = @ConMed AND ICSRs.ID = @CurrentICSR_ID", Connection)
                ConMedReadCommand.Parameters.AddWithValue("@ConMed", 2) 'selects entry corresponding to Medication History from MedicationPerICSR_Roles
                ConMedReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    ConMed_Repeater.DataSource = ConMedReadCommand.ExecuteReader()
                    ConMed_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format ConMed Tab Controls
                Dim ConMed_Repeater_HeaderTemplate As Control = ConMed_Repeater.Controls(0).Controls(0)
                Dim CreateConMed_Button As Button = TryCast(ConMed_Repeater_HeaderTemplate.FindControl("CreateICSRMedication_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Create) = True Then
                    CreateConMed_Button.Visible = True
                Else
                    CreateConMed_Button.Visible = False
                End If
                Dim CurrentUserCanEditConMedInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Edit) = True Then
                    CurrentUserCanEditConMedInCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteConMedInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Delete) = True Then
                    CurrentUserCanDeleteConMedInCurrentICSR = True
                End If
                For Each item In ConMed_Repeater.Items
                    If CurrentUserCanEditConMedInCurrentICSR = True Or CurrentUserCanDeleteConMedInCurrentICSR = True Then
                        item.findcontrol("ConMedTitle").colspan = "1"
                        item.findcontrol("ICSRMedication_Buttons_Header").visible = True
                        If CurrentUserCanEditConMedInCurrentICSR = True Then
                            item.findcontrol("EditICSRMedication_Button").Visible = True
                        Else
                            item.findcontrol("EditICSRMedication_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteConMedInCurrentICSR = True Then
                            item.findcontrol("DeleteICSRMedication_Button").Visible = True
                        Else
                            item.findcontrol("DeleteICSRMedication_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("ConMedTitle").colspan = "2"
                        item.findcontrol("ICSRMedication_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("ConMedName_Textbox").ReadOnly = True
                    item.findcontrol("TotalDailyDose_TextBox").ReadOnly = True
                    item.findcontrol("Allocations_TextBox").ReadOnly = True
                    item.findcontrol("Start_TextBox").ReadOnly = True
                    item.findcontrol("Stop_TextBox").ReadOnly = True
                    item.findcontrol("DrugAction_Name_TextBox").ReadOnly = True
                Next
                'Populate Medical History Tab Controls
                Dim MedicalHistoryReadCommand As New SqlCommand("SELECT MedicalHistories.ID AS MedicalHistory_ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop FROM MedicalHistories JOIN ICSRs ON ICSRs.ID = MedicalHistories.ICSR_ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                MedicalHistoryReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                Try
                    Connection.Open()
                    MedicalHistory_Repeater.DataSource = MedicalHistoryReadCommand.ExecuteReader()
                    MedicalHistory_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format MedicalHistory Tab Controls
                Dim MedicalHistory_Repeater_HeaderTemplate As Control = MedicalHistory_Repeater.Controls(0).Controls(0)
                Dim CreateMedicalHistory_Button As Button = TryCast(MedicalHistory_Repeater_HeaderTemplate.FindControl("CreateMedicalHistory_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Create) = True Then
                    CreateMedicalHistory_Button.Visible = True
                Else
                    CreateMedicalHistory_Button.Visible = False
                End If
                Dim CurrentUserCanEditMedicalHistoryInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Edit) = True Then
                    CurrentUserCanEditMedicalHistoryInCurrentICSR = True
                End If
                Dim CurrentUserCanDeleteMedicalHistoryInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Delete) = True Then
                    CurrentUserCanDeleteMedicalHistoryInCurrentICSR = True
                End If
                For Each item In MedicalHistory_Repeater.Items
                    If CurrentUserCanEditMedicalHistoryInCurrentICSR = True Or CurrentUserCanDeleteMedicalHistoryInCurrentICSR = True Then
                        item.findcontrol("MedicalHistoryTitle").colspan = "1"
                        item.findcontrol("MedicalHistory_Buttons_Header").visible = True
                        If CurrentUserCanEditMedicalHistoryInCurrentICSR = True Then
                            item.findcontrol("EditMedicalHistory_Button").Visible = True
                        Else
                            item.findcontrol("EditMedicalHistory_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteMedicalHistoryInCurrentICSR = True Then
                            item.findcontrol("DeleteMedicalHistory_Button").Visible = True
                        Else
                            item.findcontrol("DeleteMedicalHistory_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("MedicalHistoryTitle").colspan = "2"
                        item.findcontrol("MedicalHistory_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("MedDRATerm_TextBox").ReadOnly = True
                    item.findcontrol("Start_Textbox").ReadOnly = True
                    item.findcontrol("Stop_Textbox").ReadOnly = True
                Next
                'Populate Case History Tab Controls
                CaseHistoryRepeater.DataSource = History(tables.ICSRHistories, fields.ICSR_ID, CurrentICSR_ID)
                CaseHistoryRepeater.DataBind()
                'Populate Attached Files List
                AttachedFiles_Repeater.DataSource = AttachedFiles(tables.ICSRs, CurrentICSR_ID)
                AttachedFiles_Repeater.DataBind()
                'Format Attached Files Tab Controls
                Dim AttachedFiles_Repeater_HeaderTemplate As Control = AttachedFiles_Repeater.Controls(0).Controls(0)
                Dim AttachFile_Button As Button = TryCast(AttachedFiles_Repeater_HeaderTemplate.FindControl("AttachFile_Button"), Button)
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRsAttachedFiles, fields.Create) = True Then 'Enable Controls if LoggedIn_User has corresponding rights
                    AttachFile_Button.Visible = True
                Else
                    AttachFile_Button.Visible = False
                End If
                Dim CurrentUserCanDeleteAttachedFilesInCurrentICSR As Boolean = False
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRsAttachedFiles, fields.Delete) = True Then
                    CurrentUserCanDeleteAttachedFilesInCurrentICSR = True
                End If
                For Each item In AttachedFiles_Repeater.Items
                    If CurrentUserCanDeleteAttachedFilesInCurrentICSR = True Then
                        item.findcontrol("DeleteAttachedFile_Button").Visible = True
                    Else
                        item.findcontrol("DeleteAttachedFile_Button").Visible = False
                    End If
                    item.findcontrol("Filename_Textbox").ReadOnly = True
                    item.findcontrol("DateAdded_Textbox").ReadOnly = True
                Next
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
            End If
        End If
    End Sub

    Protected Sub EditICRBasicInformation_Button_Click(sender As Object, e As EventArgs) Handles EditICSRBasicInformation_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/EditICSRBasicInformation.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub CreateReport_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim HeaderTemplate As Control = Reports_Repeater.Controls(0).Controls(0)
        Dim CreateReport_Button As Button = TryCast(HeaderTemplate.FindControl("CreateReport_Button"), Button)
        Response.Redirect("~/Application/CreateReport.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub EditReport_Button_Click(sender As Object, e As EventArgs)
        Dim EditReport_Button As Button = CType(sender, Button)
        Dim Report_ID As String = EditReport_Button.CommandArgument
        Response.Redirect("~/Application/EditReport.aspx?ReportID=" & Report_ID)
    End Sub

    Protected Sub DeleteReport_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteReport_Button As Button = CType(sender, Button)
        Dim Report_ID As String = DeleteReport_Button.CommandArgument
        Response.Redirect("~/Application/EditReport.aspx?ReportID=" & Report_ID & "&Delete=True")
    End Sub

    Protected Sub CreateICSRMedication_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim CreateICSRMedication_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateICSRMedication.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub EditICSRMedication_Button_Click(sender As Object, e As EventArgs)
        Dim EditICSRMedication_Button As Button = CType(sender, Button)
        Dim ICSRMedication_ID As String = EditICSRMedication_Button.CommandArgument
        Response.Redirect("~/Application/EditICSRMedication.aspx?ICSRMedicationID=" & ICSRMedication_ID)
    End Sub

    Protected Sub DeleteICSRMedication_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteICSRMedication_Button As Button = CType(sender, Button)
        Dim ICSRMedication_ID As String = DeleteICSRMedication_Button.CommandArgument
        Response.Redirect("~/Application/EditICSRMedication.aspx?ICSRMedicationID=" & ICSRMedication_ID & "&Delete=True")
    End Sub

    Protected Sub CreateAE_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim CreateAE_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateAE.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub EditAE_Button_Click(sender As Object, e As EventArgs)
        Dim EditAE_Button As Button = CType(sender, Button)
        Dim AE_ID As String = EditAE_Button.CommandArgument
        Response.Redirect("~/Application/EditAE.aspx?AEID=" & AE_ID)
    End Sub

    Protected Sub DeleteAE_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteAE_Button As Button = CType(sender, Button)
        Dim AE_ID As String = DeleteAE_Button.CommandArgument
        Response.Redirect("~/Application/EditAE.aspx?AEID=" & AE_ID & "&Delete=True")
    End Sub

    Protected Sub CreateRelation_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim CreateRelation_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateRelation.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub EditRelation_Button_Click(sender As Object, e As EventArgs)
        Dim EditRelation_Button As Button = CType(sender, Button)
        Dim Relation_ID As String = EditRelation_Button.CommandArgument
        Response.Redirect("~/Application/EditRelation.aspx?RelationID=" & Relation_ID)
    End Sub

    Protected Sub DeleteRelation_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteRelation_Button As Button = CType(sender, Button)
        Dim Relation_ID As String = DeleteRelation_Button.CommandArgument
        Response.Redirect("~/Application/EditRelation.aspx?RelationID=" & Relation_ID & "&Delete=True")
    End Sub

    Protected Sub CreateMedicalHistory_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim CreateMedicalHistory_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateMedicalHistory.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub EditMedicalHistory_Button_Click(sender As Object, e As EventArgs)
        Dim EditMedicalHistory_Button As Button = CType(sender, Button)
        Dim MedicalHistory_ID As String = EditMedicalHistory_Button.CommandArgument
        Response.Redirect("~/Application/EditMedicalHistory.aspx?MedicalHistoryID=" & MedicalHistory_ID)
    End Sub

    Protected Sub DeleteMedicalHistory_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteMedicalHistory_Button As Button = CType(sender, Button)
        Dim MedicalHistory_ID As String = DeleteMedicalHistory_Button.CommandArgument
        Response.Redirect("~/Application/EditMedicalHistory.aspx?MedicalHistoryID=" & MedicalHistory_ID & "&Delete=True")
    End Sub

    Protected Sub CreateICSRHistoryEntry_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim CreateHistoryEntry_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateICSRHistoryEntry.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub AttachFile_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim AttachFile_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/AttachFile.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub DownloadAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim VirtualPath As String = sender.commandargument
        Dim DownloadAttachedFile_Button As Button = CType(sender, Button)
        Dim AttachedFileItem As RepeaterItem = CType(DownloadAttachedFile_Button.NamingContainer, RepeaterItem)
        Dim FileName_Textbox As TextBox = AttachedFileItem.FindControl("FileName_Textbox")
        Dim FileName As String = FileName_Textbox.Text
        Dim path As String = Server.MapPath(VirtualPath) 'get file object as FileInfo
        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        Try
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment; filename=""" & FileName & """") '-- double quotes at end ensure that whitespaces in FileName are recognized correctly 
            Response.AddHeader("Content-Length", file.Length.ToString())
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(file.FullName)
        Catch ex As Exception
            Response.Redirect("~/Errors/OtherError.aspx")
        Finally
            Response.End()
        End Try
    End Sub

    Protected Sub DeleteAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteAttachedFile_Button As Button = CType(sender, Button)
        Dim ClickedItem As RepeaterItem = CType(DeleteAttachedFile_Button.NamingContainer, RepeaterItem)
        ClickedItem.FindControl("DeleteAttachedFile_Button").Visible = False
        ClickedItem.FindControl("ConfirmDeleteAttachedFile_Button").Visible = True
    End Sub

    Protected Sub ConfirmDeleteAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim VirtualPath As String = sender.CommandArgument
        Dim path As String = Server.MapPath(VirtualPath)
        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        Dim GUID As String = System.IO.Path.GetFileNameWithoutExtension(file.Name)
        'Update 'Removed' in dataset of deleted file in AttachedFiles to NOW()
        Dim FileDeletionUpdateCommand As New SqlCommand("UPDATE AttachedFiles SET Removed = @Removed WHERE GUID = @GUID", Connection)
        FileDeletionUpdateCommand.Parameters.AddWithValue("@Removed", Now())
        FileDeletionUpdateCommand.Parameters.AddWithValue("@GUID", GUID)
        Try
            Connection.Open()
            FileDeletionUpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        'Add change history entry with database updates
        Dim RemovedReadCommand As New SqlCommand("SELECT ID, Removed FROM AttachedFiles WHERE GUID = @GUID", Connection)
        RemovedReadCommand.Parameters.AddWithValue("@GUID", GUID)
        Dim RemovedAttachedFile_ID As Integer = Nothing
        Dim RemovedAttachedFile_Removed As DateTime = Date.MinValue
        Try
            Connection.Open()
            Dim RemovedReader As SqlDataReader = RemovedReadCommand.ExecuteReader()
            While RemovedReader.Read()
                RemovedAttachedFile_ID = RemovedReader.GetInt32(0)
                RemovedAttachedFile_Removed = DateOrDateMinValue(RemovedReader.GetDateTime(1))
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        Dim EntryString As String = String.Empty
        EntryString = HistoryDatabasebUpdateIntro
        EntryString += DeleteReportIntro("Attached File", RemovedAttachedFile_ID)
        EntryString += HistoryEnrtyPlainValue("Attached File", RemovedAttachedFile_ID, "Removed", Date.MinValue, RemovedAttachedFile_Removed)
        EntryString += HistoryDatabasebUpdateOutro
        Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
        InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
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
        'Refresh CaseHistoryRepeater
        CaseHistoryRepeater.DataSource = History(tables.ICSRHistories, fields.ICSR_ID, CurrentICSR_ID)
        CaseHistoryRepeater.DataBind()
        'Refresh AttachedFiles_Repeater Contents
        AttachedFiles_Repeater.DataSource = AttachedFiles(tables.ICSRs, CurrentICSR_ID)
        AttachedFiles_Repeater.DataBind()
    End Sub
End Class
