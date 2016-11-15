<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="ICSRStatusUpdateRight.aspx.vb" Inherits="Administration_ICSRStatusUpdateRight" %>

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
            <div id="Buttons" class="panel btn-group" runat="server">
                <asp:button ID="Delete_Button" runat="server" class="btn btn-primary" Text="Permanently Delete ICSR Status Update Right"></asp:button>
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save ICSR Status Update Right Updates"></asp:button>
                <asp:button ID="ConfirmDeletion_Button" runat="server" class="btn btn-primary" Text="Confirm ICSR Status Update Right Deletion"></asp:button>
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
                    <tr ID="Role_Row" runat="server">
                        <td>Role:</td>
                        <td><asp:DropDownList ID="Role_Dropdownlist" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                    <tr id="UpdateFromStatus_Row" runat="server">
                        <td>Update from Status:</td>
                        <td><asp:DropDownList ID="UpdateFromStatus_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                    <tr id="UpdateToStatus_Row" runat="server">
                        <td>Update to Status:</td>
                        <td><asp:DropDownList ID="UpdateToStatus_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
</asp:Content>

