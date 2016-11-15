<%@ Page Title="Change Password" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="Application_ChangePassword" %>

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
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save New Password"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table class="table table-responsive table-striped table-hover">
                        <tr>
                            <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                        </tr>
                        <tr ID="Password_Row" runat="server">
                            <td>New Password:</td>
                            <td>
                                <asp:Textbox ID="Password_Textbox" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                                <asp:CustomValidator ID="Password_Textbox_Validator" runat="server" OnServerValidate="Password_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ConfirmPassword_Row" runat="server">
                            <td>Confirm New Password:</td>
                            <td>
                                <asp:Textbox ID="ConfirmPassword_Textbox" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                                <asp:CustomValidator ID="ConfirmPassword_Textbox_Validator" runat="server" OnServerValidate="ConfirmPassword_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

