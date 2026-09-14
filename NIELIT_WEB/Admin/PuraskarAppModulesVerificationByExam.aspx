<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PuraskarApp.master" AutoEventWireup="true" CodeFile="PuraskarAppModulesVerificationByExam.aspx.cs"
    Inherits="Admin_PuraskarAppModulesVerificationByExam" Debug="true" %>

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
                    Text="Protsahan Puraskar  Candidate Modules Verification " Visible="true"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>

                <%-- added by amit start --%>
                <asp:Label
                    ID="lblpaidby"
                    runat="server"
                    Width="99%"
                    EnableTheming="False"
                    Text="Paid By:"
                    Visible ="false"
                    Style="
                        display:block;
                        text-align:center;
                        background-color:#d4edda;
                        color:#155724;
                        border:1px solid #c3e6cb;
                        padding:6px;
                        font-size:11pt;
                    ">
                </asp:Label>

                <%-- added by amit end --%>


                <asp:GridView ID="gvMain" runat="server" DataKeyNames="slno,ModuleID,Candidate_ID,RegnNo,Examid,IsPreModules" OnSorting="gvMain_Sorting" CssClass="Grid"
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
                            DataTextField="ExamCycle" HeaderText="ExamCycle" SortExpression="ExamCycle" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                    
                        <asp:HyperLinkField HeaderStyle-Width="60%"
                            DataTextField="modulesApp" HeaderText="ModulesAppeared" SortExpression="Name" Target="_self" >
                            <HeaderStyle Width="60%"   />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="modulesPass" HeaderText="ModulesPassed" SortExpression="Level" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                       <%-- <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="isProcessed" HeaderText="AlreadyPaid/Selected" SortExpression="Exams" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>--%>
                         <asp:TemplateField HeaderText="AlreadyPaid/Processed" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                            <ItemTemplate>

                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("isProcessed") %>' Width="40px"></asp:Label>
                                 </ItemTemplate>
                        </asp:TemplateField>
                       <%-- <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="isVerifiedByExam" HeaderText="CurrentSelected" SortExpression="FatherName" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>--%>
                         <asp:TemplateField HeaderText="CurrentSelected" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("isVerifiedByExam") %>' Width="110px"></asp:Label>

                                <asp:DropDownList ID="ddlVerifieds" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVerifieds_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                </asp:DropDownList>
                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"     ControlToValidate="ddlVerifieds"  
                                          ErrorMessage="Please select"  ForeColor="Red"  InitialValue="0"     Display="Dynamic">
                                               </asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        <%--<asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
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

                 <asp:Button ID="btnUpdateModules" runat="server" OnClick="btnUpdateModules_Click"
                    Text="Finalized Modules" Visible="false" /> 
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
                      <%--  <tr>
                <th colspan="3"><span style="font: 200;"><u>Documents Uploaded Details</u></span></th>
            </tr>
                        <tr ><th>Documents </th>
                            <th>Uplaoded Date</th>
                            <th>Download </th>

                        </tr>--%>

                    </thead>
                    <tr id="Income" runat="server">
                     <td><asp:Label ID="lblIncom" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblIncomCerDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label7" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                        <%--<asp:Button ID="btnIncom" runat="server" OnClick="btnIncom_Click" Text="IncomCer Download" />--%>
                         <asp:LinkButton ID="btnIncom" runat="server" onclick="btnIncom_Click">Income Certificate</asp:LinkButton>
                    </td>
                       </tr>
                    <tr id="ph" runat="server">
                     <td><asp:Label ID="lblPh" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblPhCerDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label10" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                         <%--<asp:Button ID="btnph" runat="server" OnClick="btnph_Click" Text="PhCer Download" />--%>
                        <asp:LinkButton ID="btnph" runat="server" onclick="btnph_Click">PWD Certificate</asp:LinkButton>
                    </td>
                       </tr>
                    <tr id="caste" runat="server">
                     <td><asp:Label ID="lblCaste" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblCasteDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label13" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                       <%-- <asp:Button ID="btnCaste" runat="server" OnClick="btnCaste_Click" Text="CasteCer Download" />--%>
                         <asp:LinkButton ID="btnCaste" runat="server" onclick="btnCaste_Click">Caste Certificate</asp:LinkButton>
                    </td>
                       </tr>
                </table>
       

    </div>
                   </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnIncom" />
             <asp:PostBackTrigger ControlID="btnph" />
            <asp:PostBackTrigger ControlID="btnCaste" />
           <%-- <asp:AsyncPostBackTrigger ControlID="btnIncom" EventName="Click" />--%>
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