<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="Activatescreen.aspx.vb" Inherits="Activate_Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" /> 
<meta http-equiv="Pragma" content="no-cache" /> 
<meta http-equiv="Expires" content="-1" />
<!DOCTYPE html>
<html lang="en">

   <head runat="server">

  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">


  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
     <%-- Start WI368  by Manoj Kumar on 30-05-2021--%>
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">   <%-- WI368 one line added --%>
     <%-- End  by Manoj Kumar on 30-05-2021--%>
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
     <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">--%>
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
     <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


  <!-- Template Main CSS File -->
  <link href="assets/css/styleIL3.css" rel="stylesheet">
    <style type="text/css">
        
        .black .ajax__calendar_container
{
width:190px;
background-color: #000000; border:solid 1px #666666;
-moz-border-radius-topleft: 8px; -webkit-border-top-left-radius: 8px; -khtml-border-top-left-radius: 8px; border-top-left-radius: 8px;
-moz-border-radius-topright: 8px; -webkit-border-top-right-radius: 8px; -khtml-border-top-right-radius: 8px; border-top-right-radius: 8px;
-moz-border-radius-bottomleft: 8px; -webkit-border-bottom-left-radius: 8px; -khtml-border-bottom-left-radius: 8px; border-bottom-left-radius: 8px;
-moz-border-radius-bottomright: 8px; -webkit-border-bottom-right-radius: 8px; -khtml-border-bottom-right-radius: 8px; border-bottom-right-radius: 8px;
}
.black .ajax__calendar_body
{
width:180px;
height:150px;
background-color: #000000; border: solid 1px #666666;
}
.black .ajax__calendar_header
{
background-color: #626262; margin-bottom: 8px;
-moz-border-radius-topleft: 4px; -webkit-border-top-left-radius: 4px; -khtml-border-top-left-radius: 4px; border-top-left-radius: 4px;
-moz-border-radius-topright: 4px; -webkit-border-top-right-radius: 4px; -khtml-border-top-right-radius: 4px; border-top-right-radius: 4px;
-moz-border-radius-bottomleft: 4px; -webkit-border-bottom-left-radius: 4px; -khtml-border-bottom-left-radius: 4px; border-bottom-left-radius: 4px;
-moz-border-radius-bottomright: 4px; -webkit-border-bottom-right-radius: 4px; -khtml-border-bottom-right-radius: 4px; border-bottom-right-radius: 4px;
}
.black .ajax__calendar_title
{
color: #ffffff; padding-top: 3px;
}
.black .ajax__calendar_next,
.black .ajax__calendar_prev
{
border:solid 4px #ffffff;
background-color: #ffffff;
-moz-border-radius-topleft: 18px; -webkit-border-top-left-radius: 18px; -khtml-border-top-left-radius: 18px; border-top-left-radius: 18px;
-moz-border-radius-topright: 18px; -webkit-border-top-right-radius: 18px; -khtml-border-top-right-radius: 18px; border-top-right-radius: 18px;
-moz-border-radius-bottomleft: 18px; -webkit-border-bottom-left-radius: 18px; -khtml-border-bottom-left-radius: 18px; border-bottom-left-radius: 18px;
-moz-border-radius-bottomright: 18px; -webkit-border-bottom-right-radius: 18px; -khtml-border-bottom-right-radius: 18px; border-bottom-right-radius: 18px;
}
.black .ajax__calendar_hover .ajax__calendar_next,
.black .ajax__calendar_hover .ajax__calendar_prev
{
border:solid 4px #328BC8;
background-color: #ffffff;
-moz-border-radius-topleft: 4px; -webkit-border-top-left-radius: 4px; -khtml-border-top-left-radius: 4px; border-top-left-radius: 4px;
-moz-border-radius-topright: 4px; -webkit-border-top-right-radius: 4px; -khtml-border-top-right-radius: 4px; border-top-right-radius: 4px;
-moz-border-radius-bottomleft: 4px; -webkit-border-bottom-left-radius: 4px; -khtml-border-bottom-left-radius: 4px; border-bottom-left-radius: 4px;
-moz-border-radius-bottomright: 4px; -webkit-border-bottom-right-radius: 4px; -khtml-border-bottom-right-radius: 4px; border-bottom-right-radius: 4px;
}
.black .ajax__calendar_dayname
{
text-align:center; margin-bottom: 4px; margin-top: 2px;
color:#ffffff;
background-color: #000000;
}
.black .ajax__calendar_day,
.black .ajax__calendar_month,
.black .ajax__calendar_year
{
margin:1px 1px 1px 1px;
text-align:center;
border:solid 1px #000000;
color:#ffffff;
background-color: #626262;
}
.black .ajax__calendar_hover .ajax__calendar_day,
.black .ajax__calendar_hover .ajax__calendar_month,
.black .ajax__calendar_hover .ajax__calendar_year
{
color: #ffffff; font-weight:bold; background-color: #328BC8;border:solid 1px #328BC8;
}
.black .ajax__calendar_active .ajax__calendar_day,
.black .ajax__calendar_active .ajax__calendar_month,
.black .ajax__calendar_active .ajax__calendar_year
{
color: #ffffff; font-weight:bold; background-color: #F7B64A;
}
.black .ajax__calendar_today .ajax__calendar_day
{
color: #ffffff; font-weight:bold; background-color: #F7B64A;
}
.black .ajax__calendar_other,
.black .ajax__calendar_hover .ajax__calendar_today
{
color: #ffffff;
font-weight:bold;
}
.black .ajax__calendar_days
{
background-color: #000000;
}



         .panel-heading span
{
    margin-top: -26px;
    /*margin-right: 10px;*/
}

.clickable {
    /*background: rgba(0, 0, 0, 0.15);*/
    display: inline-block;
    padding: 8px 12px;
    border-radius: 10px;
    cursor: pointer;
}

#hero .btn-get-started
	{
	font-size: 15px !important;
	}

.table-responsive1 {
    display: block;
    width: 100%;
    -webkit-overflow-scrolling: touch;
}
.divWaiting {
            position: fixed;
            background-color: #FAFAFA;
            z-index: 2147483647 !important;
            filter: alpha(opacity=80);
            opacity: 0.8;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            color: Black;
        }

#ifMobile1 {
    /*background-image: url(/images/arts/IMG_1447m.png)*/
    width:300px;
    height:200px;
}

@media all and (max-width: 499px) {
    #ifMobile1 {
        /*background-image: url(/images/arts/IMG_1447.png)*/
    }
}

.button {
  background-color:cornflowerblue;
  border: none;
  color: white;
  padding: 8px 10px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
}

     .chk input[type="radio"]
            {
               margin-left: 210px;
               margin-right: 50px;
               margin-top:5px;
                transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
        align-items:center;
            
}
    </style>
   
<script type="text/javascript">
    function showGenericMessageModal(type, message) {
        swal('', message, type);
    }
</script>
</head>

    <body>
        <form id="form1" runat="server">

            <cc1:ToolkitScriptManager ID="toolScriptManageer1" runat="server"></cc1:ToolkitScriptManager> 
    <div class="panel panel-primary">
        <div class="panel panel-heading">
            <h5>Activate - Deactivate Screen</h5>
        </div>
        <div class="panel panel-body">
             <div class="row" >                                      
                                                                                           
                       <div class="col-lg-3">
                     Assesse pno
                                                     
                   </div>
                          <div class="col-lg-3">
                    Respondent Pno/name
                                                     
                   </div>
                          <div class="col-lg-3">
                     Start Date
                                                     
                   </div>
                          <div class="col-lg-3">
                     End Date
                                                     
                   </div>
                                                

                                                <div class="col-sm-4 form-group">
                                                 </div>

                                              
               </div>

                     <div class="row" >                                      
                                                                                           
                       <div class="col-lg-3">
                     <asp:TextBox runat="server" ID="txtassesspno"  CssClass="form-control" placeholder="Assesee P.No"   />
                                                     
                   </div>
                          <div class="col-lg-3">
                    
                             <asp:DropDownList runat ="server" ID="ddlactivate" CssClass="form-control">
                        <asp:ListItem Value="" Text="Action"></asp:ListItem>
                        <asp:ListItem Value="A" Text="Activate"></asp:ListItem>
                        <asp:ListItem Value="D" Text="De-Activate"></asp:ListItem>
                    </asp:DropDownList>                         
                   </div>
                          <div class="col-lg-3">
                     <asp:TextBox runat="server" ID="Txtstdt"  CssClass="form-control" placeholder="Start date"   />

                            <cc1:CalendarExtender runat="server" ID="calstds" TargetControlID="Txtstdt"  Format="dd/MM/yyyy" Animated="true" CssClass="black" ></cc1:CalendarExtender>                        
                   </div>
                          <div class="col-lg-3">
                     <asp:TextBox runat="server" ID="txtendDt"  CssClass="form-control" placeholder="End date"   />
                              <cc1:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="txtendDt"  Format="dd/MM/yyyy" Animated="true" CssClass="black" ></cc1:CalendarExtender>                        
                                                     
                   </div>                     
               </div>
            <br />

                <div class="row" >                                      
                                                                                           
                       <div class="col-lg-12">
                            <asp:CheckBoxList runat="server" ID ="chkres" RepeatColumns="4" RepeatDirection="Vertical" ></asp:CheckBoxList>
                    
                        
                   </div>                     
               </div>

              <div class="row" >                                      
                                                                                           
                       <div class="col-lg-3">
                            <asp:TextBox runat="server" ID="txtrespnoemail"  TextMode="MultiLine" Rows="3" Visible="false" CssClass="form-control" placeholder="Respondent Pno/mail"   />
                   
                                                     
                   </div>
                          <div class="col-lg-3">

                               <asp:Button runat="server" ID="btnresp" Text="Find Respondent" CssClass=" button" OnClick="btnresp_Click" />  
                     
                                      <asp:Button runat="server" ID="btnactivate" Text="Activate" CssClass=" button" Visible="false" OnClick="btnactivate_Click" />
                              
                               <asp:Button runat="server" ID="btndact" Text="De-Activate" CssClass=" button" Visible="false" OnClick="btndact_Click" />               
                   </div>
                          <div class="col-lg-3">
                     
                                                     
                   </div>
                          <div class="col-lg-3">
                     
                                                     
                   </div>
                                                

                                                <div class="col-sm-4 form-group">
                                                 </div>

                                              
               </div>



             
            
        </div>
    </div>

        <!-- Modal -->
  <div class="modal fade" id="divSuccess" role="dialog">
    <div class=" modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal"></button>
          <h4 class="modal-title">Message</h4>
        </div>
        <div class="modal-body">
           <p>Successfully updated !</p>
        </div>
        <div class="modal-footer">
           <asp:Button ID="btn_Exit" CssClass="btn btn-primary" runat="server" Text="OK" CausesValidation="false"  />
        </div>
      </div>
      
    </div>
  </div>

     <!-- Modal -->
  <div class="modal fade" id="divError" role="dialog">
    <div class=" modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal"></button>
          <h4 class="modal-title">Message</h4>
        </div>
        <div class="modal-body">
           <p>Something went wrong, Please try again !</p>
        </div>
        <div class="modal-footer">
           <asp:Button ID="btn_Ok" CssClass="btn btn-primary" runat="server" Text="OK" CausesValidation="false" />
        </div>
      </div>
      
    </div>
  </div>
            </form>
        </body>
</html>

