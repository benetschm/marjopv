<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomFileUpload.aspx.vb" Inherits="Tests_CustomFileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
	    function triggerFileUpload()
	        {document.getElementById("FileUpload1").click();}	  
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server" OnUnload="Button3_Click" />
        <br />
	    <br />
	    <asp:Button ID="Button1" OnClientClick="triggerFileUpload()" runat="server" CssClass="btn btn-primary" Text="Select File"/>
	    <br />
	    <br />
        <asp:Label ID="FileName_Label" runat="server" Text="No File Specified"></asp:Label>
    </div>
    </form>
</body>
</html>
