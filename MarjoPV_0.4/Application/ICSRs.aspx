<%@ Page Title="ICSRs" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="ICSRs.aspx.vb" Inherits="Application_ICSRs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <asp:Label ID="Title_Label" runat="server" CssClass="h3"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="panel btn-group">
                <asp:Button ID="CreateICSR_Button" runat="server" Text="Create ICSR" CssClass="btn btn-sm btn-primary" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table class="table table-responsive table-striped table-hover">
                        <tr>
                            <th>ID</th>
                            <th><asp:Label ID="ICSRCompany_Label" runat="server" Text="Company" Visible="false"></asp:Label></th>
                            <th>Patient Initials</th>
                            <th>ICSR Status</th>
                            <th>Assignee</th>
                            <th>Is Serious</th>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="ICSR_ID_Filter_Textbox" runat="server" CssClass="form-control" OnTextChanged="Filter_ICSRs_List" AutoPostBack="true"></asp:TextBox></td>
                            <td><asp:DropDownList ID="Companies_Filter_DropDownList_ICSRs" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_ICSRs_List" AutoPostBack="true" Visible="false"></asp:DropDownList></td>
                            <td><asp:Textbox ID="PatientInitials_Filter_Textbox" runat="server" CssClass="form-control" OntextChanged="Filter_ICSRs_List" AutoPostBack="true"></asp:Textbox></td>
                            <td><asp:DropDownList ID="ICSRStatuses_Filter_DropDownList" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_ICSRs_List" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList ID="Assignees_Filter_DropDownList" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_ICSRs_List" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList ID="IsSerious_Filter_DropDownList" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_ICSRs_List" AutoPostBack="true"></asp:DropDownList></td>
                        </tr>
                        <asp:Repeater ID="ICSRsList_Repeater" runat="server"><ItemTemplate>
                        <tr>                
                            <td>
                                <asp:hyperlink ID="ID" runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Company" runat="server" Text='<%# Eval("Company_Name") %>' Visible="false" NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Patient" runat="server" Text='<%# Eval("Patient_Initials") %>' NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Status" runat="server" Text='<%# Eval("ICSRStatus_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Assignee" runat="server" Text='<%# Eval("Assignee_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="IsSerious" runat="server" Text='<%# Eval("IsSerious_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/ICSROverview.aspx?ICSRID={0}") %>'></asp:hyperlink>
                            </td>
                        </tr>
                        </ItemTemplate></asp:Repeater>
                    </table>
                    <hr />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

