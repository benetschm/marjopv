<%@ Page Title="Login" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Account_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-sm-6">
            <div id="Login_Table" class="table" style="border: 1px solid black; padding: 15px; margin: 30px">
                <asp:Label ID="Login_Title" runat="server" class="h3" Text="Enter user credentials to log in to Marjo PV"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>Username:</td>
                                <td>
                                    <asp:Textbox ID="Username_TextBox" runat="server" CssClass="form-control" style="margin-top: 15px; margin-bottom: 15px"></asp:Textbox>
                                    <asp:CustomValidator ID="Username_TextBox_Validator" runat="server" OnServerValidate="Username_TextBox_Validator_ServerValidate" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Password:</td>
                                <td>
                                    <asp:Textbox ID="Password_TextBox" runat="server" CssClass="form-control" TextMode="Password" style="margin-bottom: 15px"></asp:Textbox>
                                    <asp:CustomValidator ID="Password_TextBox_Validator" runat="server" OnServerValidate="Password_TextBox_Validator_ServerValidate" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:button ID="LogIn" runat="server" class="btn btn-primary" Text="Log in to MarjoPV"></asp:button></td>
                                <td style="text-align: right"><asp:Hyperlink ID="PasswordRecovery_Hyperlink" runat="server" Text ="Forgot your password?" NavigateUrl="~/Application/PasswordRecovery.aspx"/></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

