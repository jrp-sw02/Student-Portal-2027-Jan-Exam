<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PuraskarApp.master" AutoEventWireup="true" CodeFile="PuraskarAppDocsVerification.aspx.cs"
    Inherits="Admin_PuraskarAppDocsVerification" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register src="../UserControl/NormalHeader.ascx" tagname="NormalHeader" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    
     
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
   <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td align="center" valign="top">
                <table width="1000px" border="0" cellspacing="0" cellpadding="0" align="center">
                    <tr>
                        <td colspan="3" valign="top">
                            <uc5:NormalHeader ID="NormalHeader2" runat="server" />
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    <asp:Label ID="lblHeading" runat="server" Text="Online Puraskar Application"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        
    </script>
    
       <script language="javascript" type="text/javascript">
           function ValidateFormFields() {

           }

           function CheckSelectedDept() {

           }
           var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                // row.style.backgroundColor = "aqua";
                
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                   // row.style.backgroundColor = "#C2D69B";
                }
                else {
                   // row.style.backgroundColor = "white";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            //headerCheckBox.checked = checked;

        }
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        //row.style.backgroundColor = "aqua";
                        
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original 
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            //row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>
    <div id="divGrid" runat="server" visible="true">
       
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblAffiliatedInst" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Placement Candidate Documents Verification " Visible="true"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="slno,RegnNo,Examid" OnSorting="gvMain_Sorting" CssClass="Grid"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="600px" ShowHeader="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                    <RowStyle Height="40px" />
                    
                    <Columns> 
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="RegnNo" HeaderText="RegistrationNo" SortExpression="Regno" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                          <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Name" HeaderText="Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                    
                        <asp:HyperLinkField HeaderStyle-Width="60%"
                            DataTextField="FatherName" HeaderText="FatherName" SortExpression="FatherName" Target="_self" >
                            <HeaderStyle Width="60%"   />
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="AppointmentLetter" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("AppointmentLetterFileVerified") %>' Width="110px"></asp:Label>
                                <asp:DropDownList ID="ddlVerifiedsAppointmentLetter" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVerifiedsAppointmentLetter_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Verified"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                </asp:DropDownList>                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"     ControlToValidate="ddlVerifiedsAppointmentLetter"  
                                          ErrorMessage="Please select"  ForeColor="Red"  InitialValue="0"     Display="Dynamic">
                                               </asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                      <asp:TemplateField HeaderText="SalarySlip" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("SalarySlipFileVerified") %>' Width="110px"></asp:Label>

                                <asp:DropDownList ID="ddlVerifiedsSalarySlipFile" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVerifiedsSalarySlipFile_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Verified"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                </asp:DropDownList>                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"     ControlToValidate="ddlVerifiedsSalarySlipFile"  
                                          ErrorMessage="Please select"  ForeColor="Red"  InitialValue="0"     Display="Dynamic">
                                               </asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="BankStatement" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("BankStatementFileVerified") %>' Width="110px"></asp:Label>

                                <asp:DropDownList ID="ddlVerifiedsBankStatementFile" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVerifiedsBankStatementFile_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Verified"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                </asp:DropDownList>
                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"     ControlToValidate="ddlVerifiedsBankStatementFile"  
                                          ErrorMessage="Please select"  ForeColor="Red"  InitialValue="0"     Display="Dynamic">
                                               </asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" visible="false" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)"  AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                           <asp:TemplateField Visible="false" HeaderText="lblIdVisFalse">
                            <ItemTemplate>
                                 <asp:Label runat="server" Visible="true" ID="lblExamid" Text='<%# Eval("Examid") %>'></asp:Label>
                                <asp:Label runat="server" Visible="true" ID="lblRegno" Text='<%# Eval("RegnNo") %>'></asp:Label>
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:ButtonField  Text="View" CommandName="Select" Visible="false" ItemStyle-Width="30" HeaderText="Docs"  />
                    </Columns>
                    <SelectedRowStyle BackColor="#87CEFA" ForeColor="Maroon" Font-Size="10" />
                    <PagerSettings Visible="False" />
                </asp:GridView>

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
            
        </asp:UpdatePanel>
    </div>  
    
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="msg">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="hcentreID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
  
    <div style="text-align: right; margin-top: 10px; height: 80px">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblRecord" runat="server" Font-Bold="True" ForeColor="#CC6600" /><br /><br /> 

                 <asp:Button ID="btnVerifyDocuments" runat="server" OnClick="btnVerifyDocuments_Click"
                    Text="Verify Documents" Visible="false" /> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
   <div id="DivDocs" runat="server" visible="false">
        <asp:Label ID="Label161" runat="server" text="Details of Documnets:" Font-Bold="True" ForeColor="blue" />
       <br />
                <table class="sample3" style="width: 65%; text-align: left" border="1" cellpadding="1"
            cellspacing="1">
                    <thead>
                        <tr style="background-color:#5499c7   ; text-align:center"><th><asp:Label ID="Label14" runat="server" text="Documents Name" Font-Bold="True" ForeColor="black" /> </th>
                            <th><asp:Label ID="Label15" runat="server" text="Documents Date" Font-Bold="True" ForeColor="black" /> </th>
                            <th><asp:Label ID="Label16" runat="server" text="Documents Type" Font-Bold="True" ForeColor="black" /> </th>

                        </tr>
         

                    </thead>
                    <tr id="Income" runat="server">
                     <td><asp:Label ID="lblAppointmentLetter" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblAppointmentLetterDate" runat="server"  ForeColor="black" /></td>
                    <td>                       
                         <asp:LinkButton ID="btnAppointmentLetter" runat="server" onclick="btnAppointmentLetter_Click">Appointment Letter</asp:LinkButton>
                    </td>
                       </tr>
                    <tr id="ph" runat="server">
                     <td><asp:Label ID="lblSalarySlip" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblSalarySlipDate" runat="server"  ForeColor="black" /></td>
                    <td>                     
                        <asp:LinkButton ID="btnSalarySlip" runat="server" onclick="btnSalarySlip_Click">Salary Slip</asp:LinkButton>
                    </td>
                       </tr>
                    <tr id="caste" runat="server">
                     <td><asp:Label ID="lblBankStatement" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblBankStatementDate" runat="server"  ForeColor="black" /></td>
                    <td>
                         <asp:LinkButton ID="btnBankStatement" runat="server" onclick="btnBankStatement_Click">Bank Statement</asp:LinkButton>
                    </td>
                       </tr>
                </table>
       

    </div>
                   </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnAppointmentLetter" />
             <asp:PostBackTrigger ControlID="btnSalarySlip" />
            <asp:PostBackTrigger ControlID="btnBankStatement" />          
    </Triggers>
        </asp:UpdatePanel>
    <div id="Div2">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label3" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="HValueForPrevModulesExamId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
    <div style="text-align:center">
        &nbsp;
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>