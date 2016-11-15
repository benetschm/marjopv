<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Reports.aspx.vb" Inherits="Application_Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="col-lg-12">
        <h3><asp:Label ID="Title_Label" runat="server"></asp:Label></h3>
    </div>
    <div id="Content_Tabs" class="row" runat="server">
        <div class="col-lg-12">
            <div class="panel" id="Tabs" role="tabpanel" style="width: 200%">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-justified" role="tablist">
                    <li>
                        <a  href="#Tab1" aria-controls="Tab1" role="tab" data-toggle="tab">
                            Adverse Events List
                        </a>
                    </li>
                    <li>
                        <a href="#Tab2" aria-controls="Tab2" role="tab" data-toggle="tab">
                            Report 2
                        </a>
                    </li>
                    <li>
                        <a href="#Tab3" aria-controls="Tab3" role="tab" data-toggle="tab">
                            Report 3
                        </a>
                    </li>
                    <li>
                        <a href="#Tab4" aria-controls="Tab4" role="tab" data-toggle="tab">
                            Report 4
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content Tab_Border" style="width: auto" >
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab1">
                        <div class="panel Tab_Body_Panel">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="panel btn-group">
                                        <asp:Button ID="AEsList_DownloadToExcel_Button" runat="server" class="btn btn-primary" OnClick="AEsList_DownloadToExcel_Button_Click" Text="Download to Excel" />
                                    </div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-lg-12" style="width: auto">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Report1_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-responsive table-striped table-hover" style="width:200%">
                                                        <tr>
                                                            <th id="Company_Header" runat="server" visible="false">
                                                                <asp:Label runat="server" Text="Company"></asp:Label></th>
                                                            <th>ICSR ID</th>
                                                            <th>First Report Received</th>
                                                            <th>Patient Initials</th>
                                                            <th>Patient YOB</th>
                                                            <th>Patient Gender</th>
                                                            <th>ICSR Is Serious</th>
                                                            <th>ICSR Seriousness Criterion</th>
                                                            <th>Event</th>
                                                            <th>Event Start</th>
                                                            <th>Event Stop</th>
                                                            <th>Event Outcome</th>
                                                            <th>Dechallenge Result</th>
                                                            <th>Rechallenge Result</th>
                                                            <th>Suspected Drug</th>
                                                            <th>Total Daily Dose</th>
                                                            <th>Dose Type</th>
                                                            <th>Allocations per Day</th>
                                                            <th>Treatment Start</th>
                                                            <th>Treatment Stop</th>
                                                            <th>Action taken with Drug</th>
                                                            <th>ICSR Narrative</th>
                                                            <th>ICSR Company Comment</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td ID="Company_Label" runat="server" Visible ="False"><asp:Label runat="server" Text='<%# Eval("Company") %>' ></asp:Label></td>
                                                        <td><asp:Label ID="ICSR_ID_Label" runat="server" Text='<%# SqlIntDisplay(Eval("ICSR_ID")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="FirstReportReceived_Label" runat="server" Text='<%# SqlDateDisplay(Eval("FirstReportReceived")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="PatientInitials_Lable" runat="server" Text='<%# Eval("PatientInitials") %>'></asp:Label></td>
                                                        <td><asp:Label ID="PatientYearOfBirth_Label" runat="server" Text='<%# Eval("PatientYearOfBirth") %>'></asp:Label></td>
                                                        <td><asp:Label ID="PatientGender_Label" runat="server" Text='<%# Eval("PatientGender") %>'></asp:Label></td>
                                                        <td><asp:Label ID="IsSerious_Label" runat="server" Text='<%# Eval("IsSerious") %>'></asp:Label></td>
                                                        <td><asp:Label ID="SeriousnessCriterion_Label" runat="server" Text='<%# Eval("SeriousnessCriterion") %>'></asp:Label></td>
                                                        <td><asp:Label ID="AE_MedDRATerm_Label" runat="server" Text='<%# Eval("Event") %>'></asp:Label></td>
                                                        <td><asp:Label ID="AE_Start_Label" runat="server" Text='<%# SqlDateDisplay(Eval("EventStart")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="AE_Stop_Label" runat="server" Text='<%# SqlDateDisplay(Eval("EventStop")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="Outcome_Label" runat="server" Text='<%# Eval("EventOutcome") %>'></asp:Label></td>
                                                        <td><asp:Label ID="DechallengeResult_Label" runat="server" Text='<%# Eval("EventDechallengeResult") %>'></asp:Label></td>
                                                        <td><asp:Label ID="RechallengeResult_Label" runat="server" Text='<%# Eval("EventRechallengeResult") %>'></asp:Label></td>
                                                        <td><asp:Label ID="SuspectedDrug_Label" runat="server" Text='<%# Eval("SuspectedDrug") %>'></asp:Label></td>
                                                        <td><asp:Label ID="TotalDailyDose_Label" runat="server" Text='<%# SqlIntDisplay(Eval("TotalDailyDose")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="DoseType_Label" runat="server" Text='<%# Eval("DoseType") %>'></asp:Label></td>
                                                        <td><asp:Label ID="Allocations_Label" runat="server" Text='<%# SqlIntDisplay(Eval("AllocationsPerDay")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="TreatmentStart_Label" runat="server" Text='<%# SqlDateDisplay(Eval("TreatmentStart")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="TreatmentStop_Label" runat="server" Text='<%# SqlDateDisplay(Eval("TreatmentStop")) %>'></asp:Label></td>
                                                        <td><asp:Label ID="DrugAction_Label" runat="server" Text='<%# Eval("ActionTakenWithDrug") %>'></asp:Label></td>
                                                        <td><asp:Label ID="ICSRNarrative_Label" runat="server" Text='<%# Eval("Narrative") %>'></asp:Label></td>
                                                        <td><asp:Label ID="ICSRCompanyComment_Label" runat="server" Text='<%# Eval("CompanyComment") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab2">
                        <div class="panel Tab_Body_Panel">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="panel btn-group">
                                        <asp:Button ID="Report2_DownloadToExcel_Button" runat="server" class="btn btn-primary" OnClick="Report2_DownloadToExcel_Button_Click" Text="Download to Excel" />
                                    </div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Report2_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th>Title1</th>
                                                            <th>Title2</th>
                                                            <th>Title3</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        
                                                            
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
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
                                    <div class="panel btn-group">
                                        <asp:Button ID="Report3_DownloadToExcel_Button" runat="server" class="btn btn-primary" OnClick="Report3_DownloadToExcel_Button_Click" Text="Download to Excel" />
                                    </div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Report3_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th>Title1</th>
                                                            <th>Title2</th>
                                                            <th>Title3</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        
                                                            
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
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
                                    <div class="panel btn-group">
                                        <asp:Button ID="Report4_DownloadToExcel_Button" runat="server" class="btn btn-primary" OnClick="Report4_DownloadToExcel_Button_Click" Text="Download to Excel" />
                                    </div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Report4_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th>Title1</th>
                                                            <th>Title2</th>
                                                            <th>Title3</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        
                                                            
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>

