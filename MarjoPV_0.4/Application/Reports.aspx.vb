Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_Reports
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                'Populate AEs List according to user rights to view companies
                Dim AEsListCommand As New SqlCommand("SELECT CASE WHEN Companies.Name IS NULL THEN '' ELSE Companies.Name END AS Company, CASE WHEN ICSRs.ID IS NULL THEN 0 ELSE ICSRs.ID END AS ICSR_ID, CASE WHEN FirstReportsTable.FirstReportReceived IS NULL THEN 0 ELSE FirstReportsTable.FirstReportReceived END AS FirstReportReceived, CASE WHEN ICSRs.Patient_Initials IS NULL THEN '' ELSE ICSRs.Patient_Initials END AS PatientInitials, CASE WHEN YearsOfBirth.Name IS NULL THEN '' ELSE YearsOfBirth.Name END AS PatientYearOfBirth, CASE WHEN Genders.Name IS NULL THEN '' ELSE Genders.Name END AS PatientGender, CASE WHEN IsSerious.Name IS NULL THEN '' ELSE IsSerious.Name END AS IsSerious, CASE WHEN SeriousnessCriteria.Name IS NULL THEN '' ELSE SeriousnessCriteria.Name END AS SeriousnessCriterion, CASE WHEN AEs.MedDRATerm IS NULL THEN '' ELSE AEs.MedDRATerm END AS Event, CASE WHEN AEs.Start IS NULL THEN 0 ELSE AEs.Start END AS EventStart, CASE WHEN AEs.Stop IS NULL THEN 0 ELSE AEs.Stop END AS EventStop, CASE WHEN Outcomes.Name IS NULL THEN '' ELSE Outcomes.Name END AS EventOutcome, CASE WHEN DechallengeResults.Name IS NULL THEN '' ELSE DechallengeResults.Name END AS EventDechallengeResult, CASE WHEN RechallengeResults.Name IS NULL THEN '' ELSE RechallengeResults.Name END AS EventRechallengeResult, CASE WHEN Medications.Name IS NULL THEN '' ELSE Medications.NAME END AS SuspectedDrug, CASE WHEN MedicationsPerICSR.TotalDailyDose IS NULL THEN 0 ELSE MedicationsPerICSR.TotalDailyDose END AS TotalDailyDose, CASE WHEN DoseTypes.Name IS NULL THEN '' ELSE DoseTypes.Name END AS DoseType, CASE WHEN MedicationsPerICSR.Allocations IS NULL THEN 0 ELSE MedicationsPerICSR.Allocations END AS AllocationsPerDay, CASE WHEN MedicationsPerICSR.Start IS NULL THEN 0 ELSE MedicationsPerICSR.Start END AS TreatmentStart, CASE WHEN MedicationsPerICSR.Stop IS NULL THEN 0 ELSE MedicationsPerICSR.Stop END AS TreatmentStop, CASE WHEN DrugActions.Name IS NULL THEN '' ELSE DrugActions.Name END AS ActionTakenWithDrug, CASE WHEN ICSRs.Narrative IS NULL THEN '' ELSE ICSRs.Narrative END AS Narrative, CASE WHEN ICSRs.CompanyComment IS NULL THEN '' ELSE ICSRs.CompanyComment END AS CompanyComment FROM ICSRs INNER JOIN Companies ON Companies.ID = ICSRs.Company_ID INNER JOIN (SELECT ICSR_ID, MIN(Received) AS FirstReportReceived FROM Reports GROUP BY ICSR_ID)FirstReportsTable ON ICSRs.ID = FirstReportsTable.ICSR_ID INNER JOIN AEs ON ICSRs.ID = AEs.ICSR_ID INNER JOIN Relations ON AEs.ID = Relations.AE_ID INNER JOIN MedicationsPerICSR ON MedicationsPerICSR.ID = Relations.MedicationperICSR_ID LEFT JOIN Medications ON Medications.ID = MedicationsPerICSR.Medication_ID LEFT JOIN YearsOfBirth ON YearsOfBirth.ID = ICSRs.Patient_YearOfBirth_ID LEFT JOIN Genders ON Genders.ID = ICSRs.Patient_Gender_ID LEFT JOIN IsSerious ON IsSerious.ID = ICSRs.IsSerious_ID LEFT JOIN SeriousnessCriteria ON SeriousnessCriteria.ID = ICSRs.SeriousnessCriterion_ID LEFT JOIN Outcomes ON Outcomes.ID = AEs.Outcome_ID LEFT JOIN DechallengeResults ON DechallengeResults.ID = AEs.DechallengeResult_ID LEFT JOIN RechallengeResults ON RechallengeResults.ID = AEs.RechallengeResult_ID LEFT JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID LEFT JOIN DrugActions ON DrugActions.ID = MedicationsPerICSR.DrugAction_ID INNER JOIN RoleAllocations ON Companies.ID = RoleAllocations.Company_ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @CurrentUser_ID", Connection)
                AEsListCommand.Parameters.AddWithValue("@Active", 1)
                AEsListCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
                Try
                    Connection.Open()
                    Report1_Repeater.DataSource = AEsListCommand.ExecuteReader()
                    Report1_Repeater.DataBind()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            If LoggedIn_User_CanViewCompanies = True Then
                Dim Report1_Repeater_HeaderTemplate As Control = Report1_Repeater.Controls(0).Controls(0)
                Dim Company_Header As Control = TryCast(Report1_Repeater_HeaderTemplate.FindControl("Company_Header"), Control)
                Company_Header.Visible = True
                For Each item In Report1_Repeater.Items
                    item.findcontrol("Company_Label").visible = True
                Next
            End If
        End If
    End Sub

    Protected Sub AEsList_DownloadToExcel_Button_Click(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Dim filename As String = "MarjoPV_Adverse_Events_List_" & Now.ToShortDateString & "_" & Now.ToShortTimeString & ".xls"
        Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Report1_Repeater.RenderControl(hw)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub Report2_DownloadToExcel_Button_Click(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Dim filename As String = "MarjoPV_Report2_" & Now.ToShortDateString & "_" & Now.ToShortTimeString & ".xls"
        Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Report1_Repeater.RenderControl(hw)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub Report3_DownloadToExcel_Button_Click(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Dim filename As String = "MarjoPV_Report3_" & Now.ToShortDateString & "_" & Now.ToShortTimeString & ".xls"
        Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Report1_Repeater.RenderControl(hw)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub Report4_DownloadToExcel_Button_Click(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Dim filename As String = "MarjoPV_Report4_" & Now.ToShortDateString & "_" & Now.ToShortTimeString & ".xls"
        Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Report1_Repeater.RenderControl(hw)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

End Class
