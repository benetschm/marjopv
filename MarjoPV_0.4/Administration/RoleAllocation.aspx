<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="RoleAllocation.aspx.vb" Inherits="Administration_RoleAllocation" %>

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
                <asp:button ID="Delete_Button" runat="server" class="btn btn-primary" Text="Permanently Delete Role Allocation"></asp:button>
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save Role Allocation Updates"></asp:button>
                <asp:button ID="ConfirmDeletion_Button" runat="server" class="btn btn-primary" Text="Confirm Role Allocation Deletion"></asp:button>
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
                        <td><asp:DropDownList ID="Name_Dropdownlist" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                    <tr id="Company_Row" runat="server">
                        <td>Company:</td>
                        <td><asp:DropDownList ID="Company_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                    <tr id="Role_Row" runat="server">
                        <td>Role:</td>
                        <td><asp:DropDownList ID="Role_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
</asp:Content>

