<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateRoleAllocation.aspx.vb" Inherits="Administration_CreateRoleAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <asp:Label ID="Title_Label" runat="server" class="h3"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="panel btn-group">
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save New Role Allocation"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
     </div>
    <div class="col-lg-12">
        <asp:UpdatePanel ID="Main_Table" runat="server">
            <ContentTemplate>
                <table class="table table-responsive table-striped table-hover">
                    <tr>
                        <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                    </tr>
                    <tr ID="Name_Row" runat="server">
                        <td>User Full Name:</td>
                        <td>
                            <asp:DropDownList ID="Name_Dropdownlist" runat="server" CssClass="form-control" AppendDataBoundItems="true"></asp:DropDownList>
                            <asp:CustomValidator ID="Name_Dropdownlist_Validator" runat="server" OnServerValidate="Name_Dropdownlist_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="Company_Row" runat="server">
                        <td>Company:</td>
                        <td><asp:DropDownList ID="Company_DropDownList" runat="server" CssClass="form-control" AppendDataBoundItems="true"></asp:DropDownList>
                            <asp:CustomValidator ID="Company_DropDownList_Validator" runat="server" OnServerValidate="Company_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="Role_Row" runat="server">
                        <td>Role:</td>
                        <td><asp:DropDownList ID="Role_DropDownList" runat="server" CssClass="form-control" AppendDataBoundItems="true"></asp:DropDownList>
                            <asp:CustomValidator ID="Role_DropDownList_Validator" runat="server" OnServerValidate="Role_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
</asp:Content>

