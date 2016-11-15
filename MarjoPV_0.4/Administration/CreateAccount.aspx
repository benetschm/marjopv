<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateAccount.aspx.vb" Inherits="Admininstration_CreateAccount" %>

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
                <asp:button ID="SaveNewAccount_Button" runat="server" class="btn btn-primary" Text="Save New Account"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
     </div>
     <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel id="Main_Table"  runat="server">
                <ContentTemplate>
                    <table class="table table-responsive table-striped table-hover">
                        <tr>
                            <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                        </tr>
                        <tr ID="Username_Row" runat="server">
                            <td>Username:</td>
                            <td>
                                <asp:Textbox ID="Username_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Username_Textbox_Validator" runat="server" OnServerValidate="Username_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="Password_Row" runat="server">
                            <td>Password:</td>
                            <td>
                                <asp:Textbox ID="Password_Textbox" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                                <asp:CustomValidator ID="Password_Textbox_Validator" runat="server" OnServerValidate="Password_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ConfirmPassword_Row" runat="server">
                            <td>Confirm Password:</td>
                            <td>
                                <asp:Textbox ID="ConfirmPassword_Textbox" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                                <asp:CustomValidator ID="ConfirmPassword_Textbox_Validator" runat="server" OnServerValidate="ConfirmPassword_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="Name_Row" runat="server">
                            <td>Full Name:</td>
                            <td>
                                <asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="Phone_Row" runat="server">
                            <td>Phone:</td>
                            <td>
                                <asp:Textbox ID="Phone_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Phone_Textbox_Validator" runat="server" OnServerValidate="Phone_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="Active_Row" runat="server">
                            <td>Active:</td>
                            <td>
                                <asp:DropDownList ID="Active_Dropdownlist" runat="server" CssClass="form-control">
                                    <asp:ListItem Selected="True">True</asp:ListItem>
                                    <asp:ListItem>False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanViewCompanies_Row" runat="server">
                            <td>Can View Companies:</td>
                            <td>
                                <asp:DropDownList ID="CanViewCompanies_Dropdownlist" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="Admin_Row" runat="server">
                            <td>Administrator:</td>
                            <td>
                                <asp:DropDownList ID="Admin_Dropdownlist" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

