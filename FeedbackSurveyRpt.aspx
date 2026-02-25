<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FeedbackSurveyRpt.aspx.vb" Inherits="FeedbackSurveyRpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="scripts/jquerypromise.min.js"></script>
    <script src="scripts/html2canvas.js"></script>
    <script src="scripts/jsPDF.js"></script>
    <script src="scripts/pdfmake.min.js"></script>
    <script type="text/javascript">
        function screenshot(date) {
            html2canvas(document.getElementById('pnlContainer'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL('image/jpeg');
                    var docDefinition = {
                        pageSize: 'A4',

                        // by default we use portrait, you can change it to landscape if you wish
                        pageOrientation: 'portrait',
                        content: [{
                            image: data,
                            width: 1000,
                            height: 2200,
                        }
                        ]
                    };
                    pdfMake.createPdf(docDefinition).download("Feedback.pdf")
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="pnlContainer" style="background-color:white;">
            <table width = "100%" >
                <tr height ="20%">
     <td colspan = "2" style="text-align:left;"><asp:Image ID="imgLogo" ImageUrl="/Images/Logo.JPG" runat="server" alt="" height="100" width="200"/></td>
     </tr>
                <tr>
                    <td colspan = "2"><asp:Image ID="bgImg" runat="server" ImageUrl="/Images/Feedback360.JPG"  alt="Feedback360" height="800" width="1000" /></td>
                </tr>
                <tr>
                    <td style="text-align:left; font-size:22px; font-weight:bold;margin-left:30px;"><asp:Label ID="lblReceiptNm" runat="server" Text="P. K. MISHRA"></asp:Label>,
                    <asp:Label ID="lblDesignation" runat="server" Text="HR Manager"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <h2>Overall report for <asp:Label ID="lblReceiptNm1" runat="server" Text="P. K. Mishra"></asp:Label></h2><br /><br />
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                      
                         <asp:Label runat="server" ID="lblnor"></asp:Label> 
                      
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center;">
                         <asp:Label runat="server" ID="lblTableScore"></asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <h2>Qualitative comments for <asp:Label ID="lblReceiptNm3" runat="server" Text="P. K. Mishra"></asp:Label></h2><br /><br />
                        <h4><b>Strengths</b></h4>
                    </td>
                </tr>
                <tr>
                    <td><h4>Self</h4></td>
                        <td><asp:Label ID="lblQASelf" runat="server" Text=""/></td>
                </tr>
                <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td><h4>Other respondents</h4></td>
                        <td><asp:Label ID="lblQAOtrRespond" runat="server" Text=""/></td>
                    </tr>
                <tr>
                    <td colspan="2">
                        <h2>Qualitative comments for <asp:Label ID="lblReceiptNm4" runat="server" Text="P. K. Mishra"></asp:Label></h2><br /><br />
               
                    <h4><b>Opportunities for improvement</b></h4>
                    </td>
                </tr>
                <tr>
                        <td><h4>Self</h4></td>
                        <td><asp:Label ID="lblQBSelf" runat="server" Text=""/></td>
                    </tr>
                    <tr>
                        <td><h4>Other respondents</h4></td>
                        <td><asp:Label ID="lblQBOtrRespond" runat="server" Text=""/></td>
                    </tr>
            </table>
    </div>
    </form>
</body>
</html>
