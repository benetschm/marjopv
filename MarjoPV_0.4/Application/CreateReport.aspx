<%@ Page Title="Add Report" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateReport.aspx.vb" Inherits="Application_CreateReport" %>

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
            <div id="ButtonGroup_Div" class="panel btn-group" runat="server">
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save New Report"></asp:button>
                <asp:button ID="PopulateReporterFromLastReport_Button" runat="server" class="btn btn-primary" Text="Populate Reporter From Last Report"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
                <asp:button ID="ReturnToICSROverview_Button" runat="server" class="btn btn-primary" Text="ICSR Overview"></asp:button>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel id="Main_Table" runat="server">
                <ContentTemplate>
                    <table class="table table-responsive table-striped table-hover">
                        <tr>
                            <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                        </tr>
                        <tr ID="ReportType_Row" runat="server">
                            <td>Report Type:</td>
                            <td>
                                <asp:DropDownList ID="ReportType_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="ReportType_DropDownList_Validator" runat="server" OnServerValidate="ReportType_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReportReceived_Row" runat="server">
                            <td>Report Received:</td>
                            <td>
                                <asp:TextBox ID="ReportReceived_TextBox" runat="server" CssClass="form-control" ></asp:TextBox>
                                <asp:CustomValidator ID="ReportReceived_Validator" runat="server" OnServerValidate="ReportReceived_TextBox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReportDue_Row" runat="server">
                            <td>Report Due:</td>
                            <td>
                                <asp:TextBox ID="ReportDue_TextBox" runat="server" CssClass="form-control" ></asp:TextBox>
                                <asp:CustomValidator ID="ReportDue_TextBox_Validator" runat="server" OnServerValidate="ReportDue_TextBox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReportComplexity_Row" runat="server">
                            <td>Report Complexity:</td>
                            <td>
                                <asp:DropDownList ID="ReportComplexity_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="ReportComplexity_DropDownList_Validator" runat="server" OnServerValidate="ReportComplexity_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReportSource_Row" runat="server">
                            <td>Report Source:</td>
                            <td>
                                <asp:DropDownList ID="ReportSource_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="ReportSource_Validator" runat="server" OnServerValidate="ReportSource_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReporterName_Row" runat="server">
                            <td>Reporter Name:</td>
                            <td>
                                <asp:TextBox ID="ReporterName_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CustomValidator ID="ReporterName_Textbox_Validator" runat="server" OnServerValidate="ReporterName_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReporterAddress_Row" runat="server">
                            <td>Reporter Address:</td>
                            <td>
                                <asp:TextBox ID="ReporterAddress_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CustomValidator ID="ReporterAddress_Textbox_Validator" runat="server" OnServerValidate="ReporterAddress_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReporterPhone_Row" runat="server">
                            <td>Reporter Phone:</td>
                            <td>
                                <asp:TextBox ID="ReporterPhone_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CustomValidator ID="ReporterPhone_Textbox_Validator" runat="server" OnServerValidate="ReporterPhone_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReporterFax_Row" runat="server">
                            <td>Reporter Fax:</td>
                            <td>
                                <asp:TextBox ID="ReporterFax_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CustomValidator ID="ReporterFax_Textbox_Validator" runat="server" OnServerValidate="ReporterFax_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ReporterEmail_Row" runat="server">
                            <td>Reporter Email:</td>
                            <td>
                                <asp:TextBox ID="ReporterEmail_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CustomValidator ID="ReporterEmail_Textbox_Validator" runat="server" OnServerValidate="ReporterEmail_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ExpeditedReportingRequired_Row" runat="server">
                            <td>Expedited Reporting Required:</td>
                            <td>
                                <asp:DropDownList ID="ExpeditedReportingRequired_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="ExpeditedReportingRequired_DropDownList_Validator" runat="server" OnServerValidate="ExpeditedReportingConsistency_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ExpeditedReportingDone_Row" runat="server">
                            <td>Expedited Reporting Done:</td>
                            <td>
                                <asp:DropDownList ID="ExpeditedReportingDone_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="ExpeditedReportingDone_DropDownList_Validator" runat="server" OnServerValidate="ExpeditedReportingConsistency_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="ExpeditedReportingDate_Row" runat="server">
                            <td>Expedited Reporting Date:</td>
                            <td>
                                <asp:Textbox ID="ExpeditedReportingDate_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="ExpeditedReportingDate_Textbox_Validator" runat="server" OnServerValidate="ExpeditedReportingDate_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

