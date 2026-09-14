<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master"  CodeFile="NielitCentreFormalStudent_Verify_Info.aspx.cs" Inherits="Admin_NielitCentreFormalStudent_Verify_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NielitCentreFormalStudent: Application Status"></asp:Label>
    <asp:HiddenField ID="HBatchID1" runat="server" />
    <asp:HiddenField ID="HVerifystatus" runat="server" /> 
    <asp:HiddenField ID="HCourseID" runat="server" />
    <asp:HiddenField ID="HStudentID" runat="server" />    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
     <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"  Visible="false"  />
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                             
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button OnClientClick="return ValidateLogin1();" runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategoryName" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCourseCategoryName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                               <asp:ListItem Value="100" Text="Formal Course"></asp:ListItem>
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--<asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:DropDownList ID="ddlAppType" Width="100%" runat="server">
                                            <asp:ListItem>--All--</asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcourseName" Width="100%" runat="server" AutoPostBack="True"
                                             OnSelectedIndexChanged="ddlcourseName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Batch Name"></asp:Label>
                                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Verified Status &lt;b class='mandatory'&gt;*&lt;/b&gt;" Visible="true"></asp:Label>
                                      
                                                <asp:DropDownList ID="ddlCourseVerifiedStatus" runat="server" Width="100%" Visible="true"
                                                    AutoPostBack="True"  OnSelectedIndexChanged="ddlCourseVerifiedStatus_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Select One--</asp:ListItem> 
                                                     <asp:ListItem Value="99">Both</asp:ListItem>                                                    
                                                    <asp:ListItem Value="1">Verified</asp:ListItem>
                                                    <asp:ListItem Value="2">Not Verified</asp:ListItem>                                                    
                                                </asp:DropDownList>                                                
                                           
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
     <uc4:breadcrumb ID="BreadCrumb1" runat="server" />  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isSelected("<%=ddlIsverifiedStatus.ClientID %>", ""))
                return false;

            return true;
        }
        function ValidateLogin1() {
            if (!isSelected("<%=ddlCourseVerifiedStatus.ClientID %>", "Verified Status"))
                return false;

            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
            ShowHideMenu(obj, tableid);
        }


    </script>
            <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
    <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                runat="server"></asp:Label>
           <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>                               
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID,BatchID"
                                     DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Appno" HeaderText="Appl. No." SortExpression="Appno" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                               
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Appdate" HeaderText="Appl. Date" DataTextFormatString="{0:dd-MMM-yyyy}"
                                     SortExpression="Appdate" Target="_self"><HeaderStyle Width="15%" /></asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="23%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Name" SortExpression="Name"
                                    Target="_self"><HeaderStyle Width="23%" /></asp:HyperLinkField>                            
                          <asp:HyperLinkField HeaderStyle-Width="9%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="FatherName" HeaderText="Father_Name" SortExpression="FatherName"
                                    Target="_self"><HeaderStyle Width="9%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="DOB" HeaderText="DOB" SortExpression="DOB"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="15%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="SemNo" HeaderText="Semester" SortExpression="SemNo"
                                    Target="_self" ><HeaderStyle Width="15%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="9%" DataNavigateUrlFields="ID,BatchID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="VerifiedStatus" HeaderText="Verified Status" SortExpression="VerifiedStatus"
                                    Target="_self"><HeaderStyle Width="9%" /></asp:HyperLinkField>                               
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif"><ItemTemplate><asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false"><HeaderTemplate><asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate><ItemTemplate><asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hfActionID" runat="server" Value="" />
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
             <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="NIELITCentreId" runat="server" />
                     <asp:HiddenField ID="HBatchId" runat="server" />
                     <asp:HiddenField ID="HVerifystatus1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            </asp:View>
         <asp:View ID="New" runat="server"> 
             <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
    cellspacing="1">
                  <tr class="head1">
        <td align="left" colspan="2">
            Application Details
            <asp:Label ID="lblID" runat ="server" Visible ="false" />
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Application No.
        </td>
        <td width="44%">
            <asp:Label ID="lblAppno" runat="server"></asp:Label>
        </td></tr>
                 <tr class="gdalternate1">
        <td width="36%">
            Admission Date
        </td>
        <td width="44%">
            <asp:Label ID="lblApplicationDate" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Course
        </td>
        <td width="44%">
            <asp:Label ID="lblCourse" runat="server"></asp:Label>
        </td>
    </tr>
                  <tr class="gdalternate1">
        <td width="36%">
            Application Status
        </td>
        <td width="44%">
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Applicant Name
        </td>
        <td width="44%">
            <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="TrFatherName" runat="server">
        <td width="36%">
            Father's Name
        </td>
        <td width="44%">
            <asp:Label ID="LblFatherName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1" id="TrMotherName" runat="server">
        <td width="36%">
            Mother's Name
        </td>
        <td width="44%">
            <asp:Label ID="LblMotherName" runat="server"></asp:Label>
        </td>
    </tr>
                 <tr class="gdalternate1" id="trgender" runat="server">
        <td width="36%">
            <asp:Label ID="Label8" runat="server" Text="Gender"></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </td>
    </tr>
                  <tr class="gdalternate1" id="trEWS" runat="server">
        <td width="36%">
            <asp:Label ID="Label6" runat="server" Text="Whether EWS"></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="lblEWS" runat="server"></asp:Label>
        </td>
    </tr>
                 <tr class="gdrow1" id="trdob" runat="server">
        <td width="36%">
            <asp:Label ID="Label12" runat="server" Text="Date of Birth "></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="Label2" runat="server"></asp:Label>
        </td>
    </tr>
                  <tr class="gdalternate1">
        <td width="36%">
            <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="lblmobile" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email "></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="lblemail" runat="server"></asp:Label>
        </td>
    </tr>
                 <tr class="gdalternate1">
        <td width="36%">
            <asp:Label ID="Label3" runat="server" EnableTheming="True" Text="IsVerify "></asp:Label>
        </td>
        <td width="44%">
             <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlIsverifiedStatus" runat="server" SkinID="ddl250" Enabled="true"
                                    AutoPostBack="True"  OnSelectedIndexChanged="ddlIsverifiedStatus_SelectedIndexChanged" > 
                                    <asp:ListItem Value="0">---Select One---</asp:ListItem>
                            <asp:ListItem Value="1">Mark as Verified</asp:ListItem>
                            <asp:ListItem Value="2" >Mark as Not Verified</asp:ListItem>                                  
                                </asp:DropDownList>
                                <asp:Label ID="Label4" runat="server" EnableTheming="True" Text="Required "  visible="false" ></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>   ;
        </td>
    </tr>
                  <tr class="gdalternate1">
                      <td width="36%">
                          <asp:Label ID="lblAadhaar" runat="server" Visible="False"></asp:Label>
                      </td>
                      <td width="44%">&nbsp;</td>
                  </tr>
                 </table>        
            <div style="text-align: right; margin-top: 10px"> 
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblmsg" runat="server" EnableTheming="True" Text=""  visible="false"  Font="bold" ></asp:Label>   
                 <asp:Button ID="btnCaptureBiometric"  runat="server" Text="Capture Biometric &amp; Verify" visible="false" OnClick="btnCaptureBiometric_Click" />         
                <asp:Button ID="btnVerify" OnClientClick="return ValidateLogin();"  runat="server" Text="Verify" OnClick="VerifyRecord" Visible="False" />
                <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnback_Click" />
                                 </ContentTemplate>
                        </asp:UpdatePanel>                
            </div>
           </asp:View>    
    </asp:MultiView>  
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
