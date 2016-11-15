<%@ Page Title="Administration" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="AdministrationRoute.aspx.vb" Inherits="Administration_AdministrationRoute" %>

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
                <asp:button ID="EditAdministrationRoute_Button" runat="server" class="btn btn-primary" Text="Edit Administration Route"></asp:button>
                <asp:button ID="DeleteAdministrationRoute_Button" runat="server" class="btn btn-primary" Text="Permanently Delete Administration Route"></asp:button>
                <asp:button ID="SaveAdministrationRouteUpdate_Button" runat="server" class="btn btn-primary" Text="Save Administration Route Updates"></asp:button>
                <asp:button ID="ConfirmAdministrationRouteDeletion_Button" runat="server" class="btn btn-primary" Text="Confirm Administration Route Deletion"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
     </div>
    <div class="col-lg-12">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="table table-responsive table-striped table-hover">
                    <tr>
                        <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                    </tr>
                    <tr ID="Name_Row" runat="server">
                        <td>Name:</td>
                        <td>
                            <asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                            <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="SortOrder_Row" runat="server">
                        <td>Sort Order:</td>
                        <td>
                            <asp:Textbox ID="SortOrder_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                            <asp:CustomValidator ID="SortOrder_Textbox_Validator" runat="server" OnServerValidate="SortOrder_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="Active_Row" runat="server">
                        <td>Active:</td>
                        <td>
                            <asp:DropDownList ID="Active_DropDownList" runat="server" CssClass="form-control">
                                <asp:ListItem>True</asp:ListItem>
                                <asp:ListItem>False</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="Active_DropDownList_Validator" runat="server" OnServerValidate="Active_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

