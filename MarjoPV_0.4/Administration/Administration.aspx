<%@ Page Title="Administration" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Administration.aspx.vb" Inherits="Admininstration_Administration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12" style="text-align: center">
            <asp:Label ID="Title_Label" runat="server" CssClass="h3" Text ="Administration Main Page"></asp:Label>
        </div>
    </div>
    <div id="Content_Tabs" class="row" runat="server">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
        <div class="col-lg-12">
            <div class="panel" id="Tabs" role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-justified" role="tablist">
                    <li>
                        <a href="#Tab1" aria-controls="Tab1" role="tab" data-toggle="tab" class="active">
                            Users, Roles & Companies
                        </a>
                    </li>
                    <li>
                        <a href="#Tab2" aria-controls="Tab2" role="tab" data-toggle="tab">
                            Statuses & Status Rights
                        </a>
                    </li>
                    <li>
                        <a href="#Tab3" aria-controls="Tab3" role="tab" data-toggle="tab">
                            Report Attributes
                        </a>
                    </li>
                    <li>
                        <a href="#Tab4" aria-controls="Tab4" role="tab" data-toggle="tab">
                            Medication & Treatment
                        </a>
                    </li>
                    <li>
                        <a href="#Tab5" aria-controls="Tab5" role="tab" data-toggle="tab">
                            Event Attributes
                        </a>
                    </li>
                    <li>
                        <a href="#Tab6" aria-controls="Tab6" role="tab" data-toggle="tab">
                            Basic Reference Tables
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content Tab_Border">
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab1">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="UserList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="5" style="text-align: center; vertical-align: middle">User Accounts</th>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateAccount_Button" CommandName="CreateAccount" runat="server" Text="Create User Account" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Username</th>
                                                    <th>Full Name</th>
                                                    <th>Phone</th>
                                                    <th>Active</th>
                                                    <th>Can View Companies</th>
                                                    <th>Administrator</th>
                                                    <th>Last Login</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="UserName_Hyperlink" runat="server" Text='<%# Eval("Username") %>' NavigateUrl='<%# eval("ID", "~/Administration/Account.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "Name_Label" runat="server" Text='<%# Eval("Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Phone_Label" runat="server" Text='<%# Eval("Phone") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    <td><asp:Label ID="CanViewCompanies_Label" runat="server" Text='<%# Eval("CanViewCompanies") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Admin_Label" runat="server" Text='<%# Eval("Admin") %>'></asp:Label></td>
                                                    <td><asp:Label ID="LastLogin_Label" runat="server" Text='<%# Eval("LastLogin") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="CompaniesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="4" style="text-align: center; vertical-align: middle">Companies</th>
                                                    <th colspan="3" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateCompany_Button" CommandName="CreateCompany" runat="server" Text="Create Company" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Street</th>
                                                    <th>Postal Code</th>
                                                    <th>City</th>
                                                    <th>Country</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/Company.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID="Street_Label" runat="server" Text='<%# Eval("Street") %>'></asp:Label></td>
                                                    <td><asp:Label ID="PostalCode_Label" runat="server" Text='<%# Eval("PostalCode") %>'></asp:Label></td>
                                                    <td><asp:Label ID="City_Label" runat="server" Text='<%# Eval("City") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Country_Label" runat="server" Text='<%# Eval("Country") %>'></asp:Label></td>
                                                    <td><asp:Label ID="SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="RoleAllocationsList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="3" style="text-align: center; vertical-align: middle">Role Allocations</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateRoleAllocation_Button" CommandName="CreateRoleAllocation" runat="server" Text="Create Role Allocation" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>User Full Name</th>
                                                    <th>Company</th>
                                                    <th>Role</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("RoleAllocation_ID") %>' NavigateUrl='<%# eval("RoleAllocation_ID", "~/Administration/RoleAllocation.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "User_Label" runat="server" Text='<%# Eval("Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Company_Label" runat="server" Text='<%# Eval("Company_Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="Role_Label" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body" id="Tab2">
                        <div class="panel Tab_Body_Panel">
                            <div class="row">
                                <div class="col-lg-12">
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ICSRStatusesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="3" style="text-align: center; vertical-align: middle">
                                                        ICSR Statuses
                                                    </th>
                                                    <th colspan="3" style="text-align: center"">
                                                        <asp:Button id="CreateICSRStatus_Button" CommandName="CreateICSRStatus" runat="server" Text="Create ICSR Status" CssClass="btn btn-sm btn-default" />
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Is Status New</th>
                                                    <th>Is Status Closed</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ICSRStatus.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "IsStatusNew_Label" runat="server" Text='<%# Eval("IsStatusNew") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "IsStatusClosed_Label" runat="server" Text='<%# Eval("IsStatusClosed") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ICSRStatusUpdateRightsList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="3" style="text-align: center; vertical-align: middle">ICSR Status Update Rights</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateICSRStatusUpdateRight_Button" CommandName="CreateICSRStatusUpdateRight" runat="server" Text="Create ICSR Status Update Right" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Role</th>
                                                    <th>Can Update ICSR Status From</th>
                                                    <th>Can Update ICSR Status To</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# eval("ID", "~/Administration/ICSRStatusUpdateRight.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "Role_Label" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="CanUpdateFromICSRStatus_Label" runat="server" Text='<%# Eval("CanUpdateFromICSRStatus_Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="CanUpdateToICSRStatus_Label" runat="server" Text='<%# Eval("CanUpdateToICSRStatus_Name") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ReportStatusesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th  colspan="3" style="text-align: center">
                                                        Report Statuses
                                                    </th>
                                                    <th colspan="2" style="text-align: center">
                                                        <asp:Button id="CreateReportStatus_Button" CommandName="CreateReportStatus" runat="server" Text="Create Report Status" CssClass="btn btn-sm btn-default" />
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Is Status New</th>
                                                    <th>Is Status Closed</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ReportStatus.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "IsStatusNew_Label" runat="server" Text='<%# Eval("IsStatusNew") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "IsStatusClosed_Label" runat="server" Text='<%# Eval("IsStatusClosed") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ReportStatusUpdateRightsList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="3" style="text-align: center; vertical-align: middle">Report Status Update Rights</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateReportStatusUpdateRight_Button" CommandName="CreateReportStatusUpdateRight" runat="server" Text="Create Report Status Update Right" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Role</th>
                                                    <th>Can Update Report Status From</th>
                                                    <th>Can Update Report Status To</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# eval("ID", "~/Administration/ReportStatusUpdateRight.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "Role_Label" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="CanUpdateFromReportStatus_Label" runat="server" Text='<%# Eval("CanUpdateFromReportStatus_Name") %>'></asp:Label></td>
                                                    <td><asp:Label ID="CanUpdateToReportStatus_Label" runat="server" Text='<%# Eval("CanUpdateToReportStatus_Name") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body" id="Tab3">
                        <div class="panel Tab_Body_Panel">
                            <div class="row">
                                <div class="col-lg-12">
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ReportComplexitiesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Report Complexities</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateReportComplexity_Button" CommandName="CreateReportComplexity" runat="server" Text="Create Report Complexity" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ReportComplexity.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ReportSourcesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Report Sources</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateReportSource_Button" CommandName="CreateReportSource" runat="server" Text="Create Report Source" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ReportSource.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ReportTypesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Report Types</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateReportType_Button" CommandName="CreateReportType" runat="server" Text="Create Report Type" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ReportType.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body" id="Tab4">
                        <div class="panel Tab_Body_Panel">
                            <div class="row">
                                <div class="col-lg-12">
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="MedicationTypesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Medication Types</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateMedicationType_Button" CommandName="CreateMedicationType" runat="server" Text="Create Medication Type" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/MedicationType.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="UnitsList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Units</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateUnit_Button" CommandName="CreateUnit" runat="server" Text="Create Unit" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/Unit.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="AdministrationRoutesList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Administration Routes</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateAdministrationRoute_Button" CommandName="CreateAdministrationRoute" runat="server" Text="Create Administration Route" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/AdministrationRoute.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="DrugActionsList_Repeater" runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th colspan="2" style="text-align: center; vertical-align: middle">Drug Actions</th>
                                                    <th colspan="1" style="text-align: center; vertical-align: middle">
                                                        <asp:Button id="CreateDrugAction_Button" CommandName="CreateDrugAction" runat="server" Text="Create Drug Action" CssClass="btn btn-sm btn-default"/>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Sort Order</th>
                                                    <th>Active</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/DrugAction.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                    <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                    <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body" id="Tab5">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="SeriousnessCriteriaList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Seriousness Criteria</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateSeriousnessCriterion_Button" CommandName="CreateSeriousnessCriterion" runat="server" Text="Create Seriousness Criterion" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/SeriousnessCriterion.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater> 
                                    </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="RelatednessCriteriaManufacturerList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Relatedness Criteria Manufacturer</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateRelatednessCriterionManufacturer_Button" CommandName="CreateRelatednessCriterionManufacturer" runat="server" Text="Create Relatedness Criterion Manfuacturer" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/RelatednessCriterionManufacturer.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                     </table>
                                     <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="RelatednessCriteriaReporterList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Relatedness Criteria Reporter</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateRelatednessCriterionReporter_Button" CommandName="CreateRelatednessCriterionReporter" runat="server" Text="Create Relatedness Criterion Reporter" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/RelatednessCriterionReporter.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                     </table>
                                     <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="ExpectednessCriteriaList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Expectedness Criteria</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateExpectednessCriterion_Button" CommandName="CreateExpectednessCriterion" runat="server" Text="Create Expectedness Criterion" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/ExpectednessCriterion.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater> 
                                    </table>
                                     <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="OutcomesList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Outcomes</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateOutcome_Button" CommandName="CreateOutcome" runat="server" Text="Create Outcome" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/Outcome.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                     </table>
                                     <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="DechallengeResultsList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Dechallenge Results</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateDechallengeResult_Button" CommandName="CreateDechallengeResult" runat="server" Text="Create Dechallenge Result" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/DechallengeResult.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                     </table>
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="RechallengeResultsList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Rechallenge Results</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateRechallengeResult_Button" CommandName="CreateRechallengeResult" runat="server" Text="Create Rechallenge Result" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/RechallengeResult.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body" id="Tab6">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <table class="table table-responsive table-striped table-hover">
                                        <asp:Repeater ID="GendersList_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th colspan="2" style="text-align: center; vertical-align: middle">Genders</th>
                                                        <th colspan="1" style="text-align: center; vertical-align: middle">
                                                            <asp:Button id="CreateGender_Button" CommandName="CreateGender" runat="server" Text="Create Gender" CssClass="btn btn-sm btn-default"/>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Sort Order</th>
                                                        <th>Active</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Administration/Gender.aspx?ID={0}") %>'></asp:hyperlink></td>
                                                        <td><asp:Label ID = "SortOrder_Label" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label></td>
                                                        <td><asp:Label ID = "Active_Label" runat="server" Text='<%# Eval("Active") %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

