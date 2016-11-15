<%@ Page Title="Medications" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Medications.aspx.vb" Inherits="Application_Medications" %>

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
                <asp:Button ID="CreateMedication_Button" runat="server" Text="Create Medication" CssClass="btn btn-sm btn-primary" />
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
                            <th><asp:Label ID="MedicationCompany_Label" runat="server" Text="Company" Visible="false"></asp:Label></th>
                            <th>Generic Name</th>
                            <th>Medication Type</th>
                            <th>Administration Route</th>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="Medication_ID_Filter_TextBox" runat="server" CssClass="form-control" OnTextChanged="Filter_Medications_List" AutoPostBack="true"></asp:TextBox></td>
                            <td><asp:DropDownList ID="Companies_Filter_DropDownList_Medications" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_Medications_List" AutoPostBack="true" Visible="false"></asp:DropDownList></td>
                            <td><asp:TextBox ID="Name_Filter_TextBox" runat="server" CssClass="form-control" OnTextChanged="Filter_Medications_List" AutoPostBack="true"></asp:TextBox></td>
                            <td><asp:DropDownList ID="Medication_Types_Filter_DropDownList" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_Medications_List" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList ID="AdministrationRoute_Filter_DropDownList" runat="server" CssClass="form-control" OnSelectedIndexChanged="Filter_Medications_List" AutoPostBack="true"></asp:DropDownList></td>
                        </tr>
                        <asp:Repeater ID="MedicationsList_Repeater" runat="server"><ItemTemplate>
                        <tr>                
                            <td>
                                <asp:hyperlink ID="ID_Hyperlink" runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# eval("ID", "~/Application/MedicationOverview.aspx?MedicationID={0}") %>' ></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Company_Hyperlink" runat="server" Text='<%# Eval("Company_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/MedicationOverview.aspx?MedicationID={0}") %>' Visible="false"></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="Name_Hyperlink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# eval("ID", "~/Application/MedicationOverview.aspx?MedicationID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="MedicationType_Hyperlink" runat="server" Text='<%# Eval("MedicationType_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/MedicationOverview.aspx?MedicationID={0}") %>'></asp:hyperlink>
                            </td>
                            <td>
                                <asp:hyperlink ID="AdministrationRoute_Hyperlink" runat="server" Text='<%# Eval("AdministrationRoute_Name") %>' NavigateUrl='<%# eval("ID", "~/Application/MedicationOverview.aspx?MedicationID={0}") %>'></asp:hyperlink>
                            </td>
                        </tr>
                        </ItemTemplate></asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

