<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="PasswordRecovery.aspx.vb" Inherits="Application_PasswordRecovery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-sm-6">
            <div id="Login_Table" class="table" style="border: 1px solid black; padding: 15px; margin: 30px">
                <asp:Label ID="Login_Title" runat="server" class="h3" Text="Enter username to receive a new password"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        </br>
                        <table>
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <asp:Label ID="Status_Label" runat="server" cssclass="form-control alert-warning" text="Password recovery not ompleted"/>
                                </td>
                            </tr>
                            <tr>
                                <td>Username:</td>
                                <td>
                                    <asp:Textbox ID="Username_TextBox" runat="server" CssClass="form-control" style="margin-top: 15px; margin-bottom: 15px"></asp:Textbox>
                                    <asp:CustomValidator ID="Username_TextBox_Validator" runat="server" OnServerValidate="Username_TextBox_Validator_ServerValidate" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:button ID="RecoverPassword_Button" runat="server" class="btn btn-primary" Text="Receive new Password"></asp:button>
                                    <asp:button ID="Login_Button" runat="server" class="btn btn-primary" Text="Log In" Visible="False"></asp:button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

