<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DdVerification.aspx.cs" Inherits="DdVerification" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register src="../UserControl/CourseApplication.ascx" tagname="CourseApplication" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="DD Verification"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
 <script language="javascript" type="text/javascript">

     function ValidateFilter() {
     
         if (!isSelected("<%= ddlCourse.ClientID %>", "Course Name"))
             return false;       
         if (!isSelected("<%= ddlApplicationType.ClientID %>", "Application Type"))
             return false;
         if (!isSelected("<%= ddlDemandNoteType.ClientID %>", "Demand Note Type"))
             return false;
        
         return true;
     }
                
</script>
 <uc2:ToggleView ID="btnMode" runat="server" OnBtnMode_Click="ToggleViewMode_Changed" />
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
                                            Text="Please select All"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" OnClientClick="return ValidateFilter();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCategory" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourse" runat="server" Width="246px" 
                                                    onselectedindexchanged="ddlCourse_SelectedIndexChanged" 
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                <%-- <asp:AsyncPostBackTrigger ControlID="DdlCourseType" EventName="SelectedIndexChanged" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter4" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlApplicationType" Width="246px" runat="server" 
                                                    onselectedindexchanged="ddlApplicationType_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter5" Width="100%" runat="server" Text="DemandNote Type"></asp:Label>
                                        <asp:DropDownList ID="ddlDemandNoteType" Width="246px" runat="server">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by DD No."
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
 <script language="javascript" type="text/javascript">



     function TestCheckBox() {
         var TargetBaseControl = document.getElementById('<%= gvMain.ClientID %>');
         if (TargetBaseControl != null) {
             //get target child control.
             var TargetChildControl = "chk";
             //get all the control of the type INPUT in the base control.
             var Inputs = TargetBaseControl.getElementsByTagName("input");
             for (var n = 0; n < Inputs.length; ++n)
                 if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked)
                     return true;
         }

         alert('Select at least one checkbox!');
         return false;



     }


     function TestTextBox() {
         var TargetBaseControl = document.getElementById('<%= gvMain.ClientID %>');
         if (TargetBaseControl != null) {
             var TargetChildControl = "txtDDamount";
             var Inputs = TargetBaseControl.getElementsByTagName("textbox");
             alert(Inputs.length);
             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'textbox' && Inputs[n].id.indexOf(textbox, 0) >= 0 && Inputs[n].length <= 0) {
                     {
                         alert("Enter values,blank is not allowed");
                         return false;
                     }

                 }
             }
         }

         return true;
     }




     function ValidateForm() {

//         if (!TestTextBox())
//             return false;
         if (!TestCheckBox())
             return false;

         return true;
     }
     var dtgp = "<%= gvMain.ClientID %>"
     function CheckAll(Sender, CheckBoxName) {
         CheckUncheckAll(dtgp, Sender, CheckBoxName)
     }
     function PerformAction(obj, tableid) {
         document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
         ShowHideMenu(obj, tableid);
     }
    </script>
<asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
 <asp:View ID="List" runat="server">
   
    <div id="divGrid" runat="server">
    
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
                <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                    <tr id="trlinks" runat="server" visible="false">
                        <td align="right" valign="bottom">
                            <asp:Button ID="BtnKA" runat="server" Text="Update DD Details" 
                                onclick="BtnKA_Click" OnClientClick="return Validate_Checkbox('Are you sure you want to update Demand-Draft Details of selected applications!')"/>
                            <asp:Button ID="BtnVerify" runat="server" Text="Verify"  
                                OnClientClick="javascript:return ValidateForm();" onclick="BtnVerify_Click" />
                        </td>
                     </tr>
                </table>
                        
            <asp:GridView ID="gvMain" runat="server"  OnSorting="gvMain_Sorting" style ="padding:1"
                            OnRowDataBound="gvMain_RowDataBound" DataKeyNames="ID,courseID,ApplicationTypeId,DemandNoteTypeId,DemandNoteNo" AutoGenerateColumns="False" Width="100%"  >
                            <Columns>
                              <asp:BoundField HeaderText="#">                                                                          
                                    </asp:BoundField>  
                                     <asp:HyperLinkField DataNavigateUrlFields="ID,courseID,ApplicationTypeId,DemandNoteTypeId" DataNavigateUrlFormatString="?Key={0}&courseID={1}&ApplTypeId={2}&DemandNoteTypeId={3}"
                                    DataTextField="DemandNoteNo" HeaderText="DemandNote No" SortExpression="DemandNoteNo" Target="_self">                                    
                                </asp:HyperLinkField>                                         
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,courseID,ApplicationTypeId,DemandNoteTypeId" DataNavigateUrlFormatString="?Key={0}&courseID={1}&ApplTypeId={2}&DemandNoteTypeId={3}"
                                    DataTextField="DemandNoteDate" HeaderText="Date" SortExpression="DemandNoteDate" DataTextFormatString="{0:dd-MMM-yyyy}" Target="_self">                                   
                                </asp:HyperLinkField>
                                 <asp:HyperLinkField DataNavigateUrlFields="ID,courseID,ApplicationTypeId,DemandNoteTypeId" DataNavigateUrlFormatString="?Key={0}&courseID={1}&ApplTypeId={2}&DemandNoteTypeId={3}"
                                    DataTextField="PayeeName" HeaderText="Payee Name" SortExpression="PayeeName" Target="_self">
                                    <HeaderStyle Width="25%" />
                                </asp:HyperLinkField>                                         
                                    <asp:TemplateField HeaderText="DD No." SortExpression="DD No.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDDnumber" runat="server" MaxLength="6" Width="60px" Text='<%# Eval("DDnumber") %>'
                                                Style="text-align: right;" Height="14px" onkeypress="checkNumber(this,6,0,event);" ></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DD Date" SortExpression="DD Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDDdate" runat="server" Width="70px" 
                                                Text='<%# Convert.ToDateTime(Eval("DDdate")).ToString("dd-MMM-yyyy")  %>' 
                                                Height="14px"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDDdate" PopupPosition="BottomLeft"
                                                Format="dd-MMM-yyyy" PopupButtonID="txtDDdate" runat="server">
                                            </asp:CalendarExtender>
                                        </ItemTemplate>
                                        <HeaderStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DD Amt." SortExpression="DD Amt.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDDamount" MaxLength="5" runat="server" Width="70px" Text='<%# Eval("DDamount") %>'
                                                Style="text-align: right;" CssClass="txtInput" Height="14px" Enabled="false" ></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="">
                                        <HeaderStyle  />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>                       
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
</asp:View>
 <asp:View ID="New" runat="server">
     <uc4:CourseApplication ID="candidateDetail" runat="server" />
 </asp:View>
  </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
