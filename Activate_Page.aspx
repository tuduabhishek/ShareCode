<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Activate_Page.aspx.vb" Inherits="Activate_Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#" + '<%=datepicker1.ClientID%>').datetimepicker({
                format: 'MM/DD/YYYY',
                autoclose: true,
                showMeridian: true,
                startView: 2,
                minView: 2,
                maxView: 2,
                autoclose: true,
                todayBtn: true,
                pickerPosition: "bottom-left",
                pickTime: false

            });
        });


         $(document).ready(function() {
            $("#" + '<%=datepicker2.ClientID%>').datetimepicker({
                format: 'MM/DD/YYYY',
                autoclose: true,
                showMeridian: true,
                startView: 2,
                minView: 2,
                maxView: 2,
                autoclose: true,
                todayBtn: true,
                pickerPosition: "bottom-left",
                pickTime: false

            });
         });

        function showMsgSuccess() {
            $('#divSuccess').modal('show');
        }

        function showMsgError() {
            $('#divError').modal('show');
        }
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


    <div class="panel panel-primary">
        <div class="panel panel-heading">
            <h5>Activate - Deactivate Screen</h5>
        </div>
        <div class="panel panel-body">
            <div class="row">
                <div class="col-md-2 col-sm-2">
                   Screen Name
                 </div>
                <div class="col-md-4 col-sm-4">
                   <asp:DropDownList ID="ddlPageNm" runat="server" tabindex="1" CssClass="form-control" AutoPostBack="true">                                      
                   </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPageNm" runat="server" ErrorMessage="*required field" ControlToValidate="ddlPageNm" Font-Size="XX-Small"  CssClass="label label-danger" ForeColor="White" InitialValue=""></asp:RequiredFieldValidator>
                  </div>
                </div>
            <br />
            <div class="row">
                                
                                <div class="col-md-3 col-sm-3">
                                    <b>Start Date</b>
                                </div>
                                <div class="col-md-3 col-sm-3">
                                    <b>End Date</b>
                                </div>
                               <div class="col-md-3 col-sm-3">
                                    <b>Status</b>
                                </div>
                                <div class="col-md-3 col-sm-3">
                                    <b>Action</b>
                                </div>
                                                                                       
            </div>
           <div class="row">
                                                                                                             

                                 <div class="col-md-3 col-sm-3" tabindex="3" >
                                 <div class="input-group date" id="datepicker1" runat="server">
                                     <asp:TextBox ID="txtStartDt" runat="server" CssClass="form-control" MaxLength="11" placeholder="DD/MM/YYYY"></asp:TextBox>                                                
                                      <span class="input-group-addon" style="padding-left: 9px; padding-right: 9px;">
                                      <span class="glyphicon glyphicon-calendar" style="cursor:pointer;"></span></span>    
                                       <asp:RequiredFieldValidator ID="rfvStartDt" runat="server" ErrorMessage="*required field" ControlToValidate="txtStartDt" Font-Size="XX-Small"  CssClass="label label-danger" ForeColor="White" ></asp:RequiredFieldValidator>                           
                                </div>
                                  </div>
                                 
                                <div class="col-md-3 col-sm-3" tabindex="4" >
                                <div class="input-group date" id="datepicker2" runat="server">
                                         <asp:TextBox ID="txtEndDt" runat="server" CssClass="form-control" MaxLength="11" placeholder="DD/MM/YYYY"></asp:TextBox>                                                
                                          <span class="input-group-addon" style="padding-left: 9px; padding-right: 9px;">
                                          <span class="glyphicon glyphicon-calendar" style="cursor:pointer;"></span></span>     
                                    <asp:RequiredFieldValidator ID="rfvEndDt" runat="server" ErrorMessage="*required field" ControlToValidate="txtEndDt" Font-Size="XX-Small"  CssClass="label label-danger" ForeColor="White" ></asp:RequiredFieldValidator>                         
                                </div>
                                 </div>
                                 <div class="col-md-3 col-sm-3" tabindex="5" >
                                       <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">Please select</asp:ListItem>
                                        <asp:ListItem Value="A">Activated</asp:ListItem>
                                         <asp:ListItem Value="D">De-Activated</asp:ListItem>
                                       </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="*required field" ControlToValidate="ddlStatus" Font-Size="XX-Small"  CssClass="label label-danger" ForeColor="White" InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                                  <div class="col-md-3 col-sm-3">
                                     <asp:Button ID="btnUpdate" CssClass="btn btn-primary" tabindex="6" runat="server" Text="Update" />
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

</asp:Content>

