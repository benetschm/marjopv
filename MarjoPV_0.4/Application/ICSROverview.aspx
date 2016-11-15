<%@ Page Title="ICSR Overview" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="ICSROverview.aspx.vb" Inherits="Application_ICSROverview" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="col-lg-12" >
        <h3><asp:Label ID="Title_Label" runat="server"></asp:Label></h3>
    </div>
    <div id="Content_Tabs" class="row" runat="server" >
        <div class="col-lg-12" >
            <div class="panel" id="Tabs" role="tabpanel" max-width: 100%">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-justified" role="tablist" style="width: 100%">
                    <li>
                        <a  href="#Tab1" aria-controls="Tab1" role="tab" data-toggle="tab">
                            Basic Information
                        </a>
                    </li>
                    <li>
                        <a href="#Tab3" aria-controls="Tab3" role="tab" data-toggle="tab">
                            Reports
                        </a>
                    </li>
                    <li>
                        <a href="#Tab4" aria-controls="Tab4" role="tab" data-toggle="tab">
                            Suspected Drugs
                        </a>
                    </li>
                    <li>
                        <a href="#Tab5" aria-controls="Tab5" role="tab" data-toggle="tab">
                            Events
                        </a>
                    </li>
                    <li>
                        <a href="#Tab6" aria-controls="Tab6" role="tab" data-toggle="tab">
                            Relations
                        </a>
                    </li>
                    <li>
                        <a href="#Tab7" aria-controls="Tab7" role="tab" data-toggle="tab">
                            Coccomitant Medication
                        </a>
                    </li>
                    <li>
                        <a href="#Tab8" aria-controls="Tab8" role="tab" data-toggle="tab">
                            Medical History
                        </a>
                    </li>
                    <li>
                        <a href="#Tab9" aria-controls="Tab9" role="tab" data-toggle="tab">
                            Audit Trail
                        </a>
                    </li>
                    <li>
                        <a href="#Tab10" aria-controls="Tab10" role="tab" data-toggle="tab">
                            Attached Files
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content Tab_Border">
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab1">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="panel btn-group">
                                                        <asp:button ID="EditICSRBasicInformation_Button" runat="server" class="btn btn-primary" Text="Edit Basic Information"></asp:button>
                                                    </div>
                                                </div>
                                            </div>
                                            <table class="table table-responsive table-striped table-hover" runat="server">
                                                <tr id="Company_Row" runat="server"  Visible="false">
                                                    <td>Company:</td>
                                                    <td><asp:Textbox ID="CompanyName_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; font-weight: bold">
                                                        <asp:Label ID="ProcessingSectionTitle_Label" runat="server" Text="Processing" style="text-align: center"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>ICSR Status:</td>
                                                    <td><asp:Textbox ID="ICSRStatus_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Assignee:</td>
                                                    <td><asp:Textbox ID="Assignee_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; font-weight: bold">
                                                        <asp:Label ID="PatientSectionTitle_Label" runat="server" Text="Patient Details" style="text-align: center"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>Patient Initials:</td>
                                                    <td><asp:Textbox ID="PatientInitials_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Patient Year of Birth:</td>
                                                    <td><asp:Textbox ID="PatientYearOfBirth_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Patient Gender:</td>
                                                    <td><asp:Textbox ID="PatientGender_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; font-weight: bold">
                                                        <asp:Label ID="SeriousnessSectionTitle_Label" runat="server" Text="Seriousness" style="text-align: center"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>Is serious:</td>
                                                    <td><asp:Textbox ID="IsSerious_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Seriousness Criterion:</td>
                                                    <td><asp:Textbox ID="SeriousnessCriterion_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; font-weight: bold">
                                                        <asp:Label ID="NarrativeCompanyCommentSectionTitle_Label" runat="server" Text="Narrative & Company Comment" style="text-align: center"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>Narrative:</td>
                                                    <td><asp:Textbox ID="Narrative_Textbox" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Company Comment:</td>
                                                    <td><asp:Textbox ID="CompanyComment_Textbox" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:Textbox></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab3">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Reports_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateReport_Button" runat="server" class="btn btn-primary" OnClick="CreateReport_Button_Click" Text="Add Report"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr id="ReportHeader_Row" runat="server" visible="true">
                                                            <th id="ReportTitle" style="text-align: center; vertical-align: middle"><asp:Label ID="ReportTitle_Label" runat="server" Text='<%# "Report ID " + SqlIntDisplay(Eval("Report_ID")) %>'></asp:Label></th>
                                                            <th id="ReportButtons_Header" runat="server"  style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditReport_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditReport_Button_Click" CommandArgument='<%# Eval("Report_ID") %>'/>
                                                                    <asp:button ID="DeleteReport_Button" runat="server" class="btn btn-sm btn-primary" Text="Delete" OnClick="DeleteReport_Button_Click" CommandArgument='<%# Eval("Report_ID") %>'></asp:button>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Report Type:</td>
                                                            <td><asp:TextBox ID="ReportType_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ReportType_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Report Status:</td>
                                                            <td><asp:TextBox ID="ReportStatus_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ReportStatus_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Received Date:</td>
                                                            <td><asp:TextBox ID="ReportReceived_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Report_Received")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Report Due:</td>
                                                            <td><asp:TextBox ID="ReportDue_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Report_Due")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr id="ReportComplexity_Row" runat="server" visible="true">
                                                            <td>Report Complexity:</td>
                                                            <td><asp:TextBox ID="ReportComplexity_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ReportComplexity_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Report Source:</td>
                                                            <td><asp:TextBox ID="ReportSource_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ReportSource_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Reporter Name and Address:</td>
                                                            <td><asp:TextBox ID="ReporterNameAddress_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("Reporter_Name") + ", " + Eval("Reporter_Address") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Reporter Phone:</td>
                                                            <td><asp:TextBox ID="ReporterPhone_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("Reporter_Phone") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Reporter Fax:</td>
                                                            <td><asp:TextBox ID="ReporterFax_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("Reporter_Fax") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Reporter Email:</td>
                                                            <td><asp:TextBox ID="ReporterEmail_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("Reporter_Mail") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Expedited Reporting Required:</td>
                                                            <td><asp:TextBox ID="ExpeditedReportingRequired_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ExpeditedReportingRequired_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Expedited Reporting Done:</td>
                                                            <td><asp:TextBox ID="ExpeditedReportingDone_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ExpeditedReportingDone_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Expedited Reporting Date:</td>
                                                            <td><asp:TextBox ID="ExpeditedReportingDate_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("ExpeditedReportingDate")) %>' ></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab4">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="SuspectedDrugs_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateICSRMedication_Button" runat="server" class="btn btn-primary" OnClick="CreateICSRMedication_Button_Click" Text="Add Medication to ICSR"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr id="SuspectedDrugHeader_Row" runat="server" visible="true">
                                                            <th id="SuspectedDrugTitle" style="text-align: center; vertical-align: middle"><asp:Label ID="SuspectedDrugTitle_Label" runat="server" Text='<%# "Suspected Drug ID " + SqlIntDisplay(Eval("ICSRMedication_ID")) %>'></asp:Label></th>
                                                            <th id="ICSRMedication_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditICSRMedication_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditICSRMedication_Button_Click" CommandArgument='<%# Eval("ICSRMedication_ID") %>'/>
                                                                    <asp:Button id="DeleteICSRMedication_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteICSRMedication_Button_Click" CommandArgument='<%# Eval("ICSRMedication_ID") %>'/>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Suspected Drug Name:</td>
                                                            <td><asp:Textbox ID="ICSRMedication_Name_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("ICSRMedication_Name") %>'></asp:Textbox></td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="TotalDailyDose_Textbox_Label" runat="server" Text='<%# "Total Daily Dose (in " + Eval("DoseType_Name") + ")"%>'/></td>
                                                            <td><asp:Textbox ID="TotalDailyDose_Textbox" runat="server" CssClass="form-control" Text='<%# SqlIntDisplay(Eval("TotalDailyDose")) %>'></asp:Textbox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Allocations per Day:</td>
                                                            <td><asp:TextBox ID="Allocations_TextBox" runat="server" CssClass="form-control" Text='<%# SqlIntDisplay(Eval("Allocations")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Start Date:</td>
                                                            <td><asp:TextBox ID="Start_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Start")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Stop Date:</td>
                                                            <td><asp:TextBox ID="Stop_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Stop")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Action taken with Drug:</td>
                                                            <td><asp:TextBox ID="DrugAction_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("DrugAction_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab5">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="AEs_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateAE_Button" runat="server" OnClick="CreateAE_Button_Click" class="btn btn-primary" Text="Add Event"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th ID="AETitle" runat="server" style="text-align: center"><asp:Label ID="MedDRATerm_Label" runat="server" Text='<%# "Event ID " + SqlIntDisplay(Eval("AE_ID")) %>'></asp:Label></th>
                                                            <th id="AE_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditAE_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditAE_Button_Click" CommandArgument='<%# Eval("AE_ID") %>'/>
                                                                    <asp:Button id="DeleteAE_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteAE_Button_Click" CommandArgument='<%# Eval("AE_ID") %>'/>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>MedDRA LLT:</td>
                                                            <td><asp:TextBox ID="MedDRATerm_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MedDRATerm") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Start Date:</td>
                                                            <td><asp:TextBox ID="Start_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Start")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Stop Date:</td>
                                                            <td><asp:TextBox ID="Stop_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Stop")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Outcome:</td>
                                                            <td><asp:TextBox ID="Outcome_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("Outcome_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Dechallenge Result:</td>
                                                            <td><asp:TextBox ID="DechallengeResult_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("DechallengeResult_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Rechallenge Result:</td>
                                                            <td><asp:TextBox ID="RechallengeResult_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("RechallengeResult_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab6">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Relations_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateRelation_Button" runat="server" OnClick="CreateRelation_Button_Click" class="btn btn-primary" Text="Add Relation"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th ID="RelationTitle" runat="server" style="text-align: center"><asp:Label ID="Relation_Label" runat="server" Text='<%# "Relation ID " + SqlIntDisplay(Eval("Relation_ID")) %>'></asp:Label></th>
                                                            <th id="Relation_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditRelation_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditRelation_Button_Click" CommandArgument='<%# Eval("Relation_ID") %>'/>
                                                                    <asp:Button id="DeleteRelation_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteRelation_Button_Click" CommandArgument='<%# Eval("Relation_ID") %>'/>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Suspected Drug:</td>
                                                            <td><asp:TextBox ID="SuspectedDrug_TextBox" runat="server" CssClass="form-control" Text='<%# CType(Eval("Medication_Name"), String) & " (ICSR Medication ID " & SqlIntDisplay(Eval("MedicationPerICSR_ID")) & ")" %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Event:</td>
                                                            <td><asp:TextBox ID="AE_TextBox" runat="server" CssClass="form-control" Text='<%# CType(Eval("AE_MedDRATerm"), String) & " (Event ID " & SqlIntDisplay(Eval("AE_ID")) & ")" %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Relatedness as per Reporter:</td>
                                                            <td><asp:TextBox ID="RelatednessCriterionReporter_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("RelatednessCriterionReporter_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Relatedness as per Manufacturer:</td>
                                                            <td><asp:TextBox ID="RelatednessCriterionManufacturer_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("RelatednessCriterionManufacturer_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Expectedness:</td>
                                                            <td><asp:TextBox ID="ExpectendessCriterion_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("ExpectendessCriterion_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab7">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="ConMed_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateICSRMedication_Button" runat="server" class="btn btn-primary" OnClick="CreateICSRMedication_Button_Click" Text="Add Medication to ICSR"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr id="ConMedHeader_Row" runat="server" visible="true">
                                                            <th id="ConMedTitle" style="text-align: center; vertical-align: middle"><asp:Label ID="ConMedTitle_Label" runat="server" Text='<%# "Concomitant Medication ID " + SqlIntDisplay(Eval("ICSRMedication_ID")) %>'></asp:Label></th>
                                                            <th id="ICSRMedication_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditICSRMedication_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditICSRMedication_Button_Click" CommandArgument='<%# Eval("ICSRMedication_ID") %>'/>
                                                                    <asp:Button id="DeleteICSRMedication_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteICSRMedication_Button_Click" CommandArgument='<%# Eval("ICSRMedication_ID") %>'/>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Concomitant Medication Name:</td>
                                                            <td><asp:Textbox ID="ConMedName_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("Medication_Name") %>'></asp:Textbox></td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="TotalDailyDose_Textbox_Label" runat="server" Text='<%# "Total Daily Dose (in " + Eval("DoseType_Name") + ")"%>'/></td>
                                                            <td><asp:Textbox ID="TotalDailyDose_Textbox" runat="server" CssClass="form-control" Text='<%# SqlIntDisplay(Eval("TotalDailyDose")) %>'></asp:Textbox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Allocations per Day:</td>
                                                            <td><asp:TextBox ID="Allocations_TextBox" runat="server" CssClass="form-control" Text='<%# SqlIntDisplay(Eval("Allocations")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Start Date:</td>
                                                            <td><asp:TextBox ID="Start_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Start")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Stop Date:</td>
                                                            <td><asp:TextBox ID="Stop_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Stop")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Action taken with Drug:</td>
                                                            <td><asp:TextBox ID="DrugAction_Name_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("DrugAction_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab8">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="MedicalHistory_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateMedicalHistory_Button" runat="server" OnClick="CreateMedicalHistory_Button_Click" class="btn btn-primary" Text="Add Medical History Entry"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th ID="MedicalHistoryTitle" runat="server" style="text-align: center"><asp:Label ID="MedicalHistory_Label" runat="server" Text='<%# "Medical History Entry " + CTYPE(Eval("MedicalHistory_ID"), String) %>'></asp:Label></th>
                                                            <th id="MedicalHistory_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditMedicalHistory_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditMedicalHistory_Button_Click" CommandArgument='<%# Eval("MedicalHistory_ID") %>'/>
                                                                    <asp:Button id="DeleteMedicalHistory_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteMedicalHistory_Button_Click" CommandArgument='<%# Eval("MedicalHistory_ID") %>'/>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>MedDRA LLT:</td>
                                                            <td><asp:TextBox ID="MedDRATerm_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MedDRATerm") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Start Date:</td>
                                                            <td><asp:TextBox ID="Start_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Start")) %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Stop Date:</td>
                                                            <td><asp:TextBox ID="Stop_TextBox" runat="server" CssClass="form-control" Text='<%# SqlDateDisplay(Eval("Stop")) %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab9">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="CaseHistoryRepeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateICSRHistoryEntryButton" runat="server" OnClick="CreateICSRHistoryEntry_Button_Click" class="btn btn-primary" Text="Add Case History Entry"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th style="text-align: center"><asp:Label ID="CaseHistoryEntryTitle_Label" runat="server" Text='<%# "Update made by " + Eval("User_Name") + " on " + Eval("Timepoint", "{0:dd-MMM-yyyy}") + " at " + Eval("Timepoint", "{0:HH:mm}") + " (UTC " + Eval("Timepoint", "{0:zzz}") + ")" %>'></asp:Label></th>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Literal ID="Entry_TextBox" runat="server" Text='<%# Eval("Entry") %>'></asp:Literal></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab10">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Repeater ID="AttachedFiles_Repeater" runat="server">
                                        <HeaderTemplate>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="panel btn-group">
                                                        <asp:button ID="AttachFile_Button" runat="server" class="btn btn-primary" Text="Attach File" OnClick="AttachFile_Button_Click"></asp:button>
                                                    </div>
                                                </div>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table class="table table-responsive table-striped table-hover">
                                                <tr id="AttachedFileHeader_Row" runat="server" visible="true">
                                                    <th id="AttachedFileTitle" style="text-align: center; vertical-align: middle">
                                                        <asp:Label ID="AttachedFileTitle_Label" runat="server" Text='<%# "Attached File ID " + SqlIntDisplay(Eval("AttachedFile_ID")) %>'></asp:Label>
                                                    </th>
                                                    <th id="AttachedFile_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                        <div class="btn-group">
                                                            <asp:Button ID="DownloadAttachedFile_Button" runat="server" Text="Download" CssClass="btn btn-sm btn-primary" OnClick="DownloadAttachedFile_Button_Click" CommandArgument='<%# " ~/AttachedFiles/" + Convert.ToString(Eval("Company_ID")) + "/" + [Enum].GetName(GetType(tables), tables.ICSRs) + "/" + Convert.ToString(Eval("Association_ID")) + "/" + Convert.ToString(Eval("GUID")) + Convert.ToString(Eval("Extension")) %>' ></asp:Button>
                                                            <asp:Button id="DeleteAttachedFile_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteAttachedFile_Button_Click" />
                                                            <asp:Button id="ConfirmDeleteAttachedFile_Button" runat="server" Text="Confirm Deletion" CssClass="btn btn-sm btn-primary btn-danger" OnClick="ConfirmDeleteAttachedFile_Button_Click" CommandArgument='<%# " ~/Attached_Files/" + Convert.ToString(Eval("Company_ID")) + "/" + Convert.ToString(Eval("Association_ID")) + "/" + Convert.ToString(Eval("GUID")) + Convert.ToString(Eval("Extension")) %>' Visible="false"/>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>File Name:</td>
                                                    <td><asp:Textbox ID="FileName_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("Name") %>' ></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Date Added:</td>
                                                    <td><asp:Textbox ID="DateAdded_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("Added", "{0:dd-MMM-yyyy}") %>' ></asp:Textbox></td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

