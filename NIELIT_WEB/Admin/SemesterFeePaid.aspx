<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="SemesterFeePaid.aspx.cs" Inherits="Admin_SemesterFeePaid" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Semester Fee Paid"></asp:Label>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
   <%-- <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />--%>
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
           <%-- //Filter--%>
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
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="ApllyFilter" />
                                    </td>
                                </tr>

                                 <tr>
                                     <td>
                                        <asp:Label ID="Label4" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseName" Width="100%" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Batch Name"></asp:Label>
                                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbatchname_SelectedIndexChanged" >
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Semester"></asp:Label>
                                        <asp:DropDownList ID="ddlsem" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexchanged">
                                         <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                                <tr>
                                   <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Student Number"></asp:Label>
                                        <asp:DropDownList ID="ddlstudentF" Width="100%" runat="server"  >
                                         <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
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
     <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Batch Name/code"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (!isSelected("<%=ddlCourse.ClientID  %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlbatchSession.ClientID  %>", "Batch Name"))
                return false;
            if (!isSelected("<%=ddlsemester.ClientID  %>", "Semester"))
                return false;

            if (!isSelected("<%=ddlStudent.ClientID  %>", "Student Number"))
                return false;
            if (!isSelected("<%=ddlfeetypemasid.ClientID  %>", "Fee Type "))
                return false;
            if (!isBlank("<%=txtAmountPaid.ClientID  %>", "Amount Paid"))
                return false;
            if (!isBlank("<%=txtPaymentDate.ClientID  %>", "Payment Date"))
                return false;
            if (!isBlank("<%=txtRemarks.ClientID  %>", "Remarks"))
                return false;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
    </script>
      <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
         <asp:View ID="New" runat="server">
    <table class="sample2" cellpadding="2" cellspacing="0" width="100%" id="trpage" runat="server">
       
      
         <tr>
            <td  colspan="2" style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                    Width="100%"></asp:Label>
            </td>

            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblstudentname" runat="server" SkinID="CaptionLabel" Text="Student Name"
                    Width="50%"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td colspan="2" style="width: 33%;" valign="top">
                <asp:TextBox Style="width: 250px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                <asp:TextBox Style="width: 200px;" ID="txtStudent" runat="server" Enabled="false" SkinID="txt248" Width="50%"></asp:TextBox>
                        </ContentTemplate>
                     </asp:UpdatePanel>
                
            </td>
        </tr> 
        
       
                             
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
              <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Semester &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>  
            </td>
                </tr>
                <tr class="even">
                    <td valign="top" class="style1">
                         <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True" Width="200px" SkinID="ddl760" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                               </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>      
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                       <asp:DropDownList ID="ddlbatchSession"  runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlbatchSession_SelectedIndexChanged">
                            <%--<asp:ListItem Value="0" Text="--All--"></asp:ListItem>--%>
                        </asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlsemester" runat="server" Width="200px" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged" >
                                </asp:DropDownList>
                        
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
                    
                </tr>
               
          
        <tr>
             <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblstudent" runat="server" SkinID="CaptionLabel" Text="Reference Number &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="100%"></asp:Label>                      
                    </td>
           
            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblfeetype" runat="server" SkinID="CaptionLabel" Text="Fee Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblamountpaid" runat="server" SkinID="CaptionLabel" Text="Amount Paid &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
           <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                           <asp:DropDownList ID="ddlStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStudent_SelectedIndexChanged"
                             SkinID="ddl250">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
            
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlfeetypemasid" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlfeetypemasid_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
             <td style="width: 33%;" valign="top">
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                   <asp:TextBox ID="txtAmountPaid" runat="server" SkinID="txt248" ToolTip="Amount Paid" MaxLength="10"></asp:TextBox>
                             </ContentTemplate>
                          </asp:UpdatePanel>
                    </td>
        </tr>

           
        <tr>
            
            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblPaymentDate" runat="server" SkinID="CaptionLabel" Text="Payment Date &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblRemarks" runat="server" SkinID="CaptionLabel" Text="Remarks &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            
        </tr>
                <tr class="even">
           
            
           <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPaymentDate" runat="server" MaxLength="12" SkinID="txt248" Width="25px"   ToolTip="Payment Date"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txtPaymentDate">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                  
                    </td>
             <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" SkinID="txt248" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
        </tr>

    </table>
     <div style="text-align: right; margin-top: 10px" id="divsave" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnSave"  runat="server" OnClientClick="return ValidateFormFields();"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />               
            </ContentTemplate>
        </asp:UpdatePanel>

          <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="NIELITCentreId" runat="server" />
                <asp:HiddenField ID="HNANFL" runat="server" />
                <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="Div1">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="hcentreID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divGrid" runat="server" >

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                              
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server"  OnSorting="gvMain_Sorting"  OnRowDataBound="gvMain_RowDataBound"
                     AutoGenerateColumns="False" Width="600px" ShowHeader="true" >
                    <RowStyle Height="40px" />
                    <Columns>
                          <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                        <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="CourseName" HeaderText="Course Name" SortExpression="Course" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="BatchName" HeaderText="Batch Name" SortExpression="Batch" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="5%"
                            DataTextField="SemId" HeaderText="Semester" SortExpression="Sem" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="Name" HeaderText="Student Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                       
                        <asp:HyperLinkField HeaderStyle-Width="13%"
                            DataTextField="Number" HeaderText="Online Ref No." SortExpression="RefNo" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>

                       <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="feeType" HeaderText="Fee Type" SortExpression="Fee" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="AmtPaid" HeaderText="Amount Paid" SortExpression="Amount" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>

                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="paymentdate" HeaderText="Payment Date" SortExpression="Paydate" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>                       
                       
                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="Remarks" HeaderText="Remarks" SortExpression="Remarks" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                                                                                                            
                    </Columns>
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
   </asp:View>
    </asp:MultiView>
</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content8" contentplaceholderid="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:content>

