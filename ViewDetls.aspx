<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewDetls.aspx.vb" Inherits="ViewDetls" MaintainScrollPositionOnPostback="true"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html lang="en">

<head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <%--<meta content="width=device-width, initial-scale=1" name="viewport" />--%>
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
  <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
    <!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->
<!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
<!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->
     <%-- <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet"> --%>
     <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->


  <!-- Template Main CSS File -->
  <link href="assets/css/style.css" rel="stylesheet">
  

</head>

<body>
    
   
    <form id="form1" runat="server">
 
 <asp:ScriptManager runat="server" ID="scpt"></asp:ScriptManager>
        <div class="container">
  <div class="page-header">
    <h1>360 DEGREE FEEDBACK SURVEY</h1>      
  </div>
      <div class="row">
                <div class="col-md-4 col-sm-4">
                   Type
                 </div>
                <div class="col-md-4 col-sm-4">
                         <asp:Label runat="server" ID="lbltype" text="Pno( Pno/Eamil if respondent)" ></asp:Label>            
                  </div>
                </div>
            <br />
            <asp:UpdatePanel runat="server" ID="uppanel">
                <ContentTemplate>
            <div class="row content">
                                
                                <div class="col-md-4 col-sm-4">
                                    <asp:RadioButtonList runat="server" ID="rbbtton" AutoPostBack="true" OnSelectedIndexChanged="rbbtton_SelectedIndexChanged">
                                        <asp:ListItem Value="asse">Assesse</asp:ListItem>
                                        <asp:ListItem Value="res">Respondent</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-4 col-sm-4">
                                     <asp:TextBox runat="server" ID="txtrespno" CssClass="form-control" ></asp:TextBox>
                                    
                                </div>
                 <div class="col-md-4 col-sm-4">
                                     <asp:Button runat="server" ID="btnsearch" CssClass="btn-learn-more" Text="Search" OnClick="btnsearch_Click" />
                                    
                                </div>
                               
                                                                                       
            </div>
                      
</div>

         <div class="panel">
       
        <div class="panel panel-body">
            
              <asp:GridView ID="GvManager" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana" Width="95%" style="overflow:scroll"
                                                        EmptyDataText="No Record Found"  BackColor="#ffccff" BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                        <Columns>
                                                           
                                                            <asp:TemplateField HeaderText="Year" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server" Text='<%# Eval("SS_YEAR")%>'></asp:Label>
                                                                  
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Assesse Pno">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname" runat="server" Text='<%# Eval("ASSESSEPNO")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel" runat="server" Text='<%# Eval("CAT")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldesg" runat="server" Text='<%# Eval("IDS")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >                                                           
                                                            <asp:TemplateField HeaderText="Respondent Pno">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("RESPNO")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Respondent Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblemail" runat="server" Text='<%# Eval("RESNAME")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Designation" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltype" runat="server" Text='<%# Eval("DESG")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Department" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbld" runat="server" Text='<%# Eval("DEPT")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="EMail" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblem" runat="server" Text='<%# Eval("mail")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Delete" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblde" runat="server" Text='<%# Eval("Del")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Approver" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblap" runat="server" Text='<%# Eval("APPROVER")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Approved" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="iblapprd" runat="server" Text='<%# Eval("APPROVED")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Status" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstst" runat="server" Text='<%# Eval("STATUS")%>' CssClass="wrap-Text" ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             
                                                                                                                    
                                                        </Columns>
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                  
        </div>
    </div>
        </ContentTemplate>
            </asp:UpdatePanel>




 </form>
</body>

</html>