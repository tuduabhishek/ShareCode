<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ErrorPage.aspx.vb" Inherits="ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  
<script type ="text/javascript">  
   
</script>  
  
    <title></title>  
</head>  
  
    
<body>
   
    <form id="form1" runat="server">
    <div style="margin-top:80px;">
    <center>
        <div style="font-size: 20px;color:red;">
        <asp:Label ID="lblMsg" runat="server" Text="" />
            </div>
        <%--You are not authorised to use this website...!--%>
    </center>
    </div>
    </form>
</body>
</html>
