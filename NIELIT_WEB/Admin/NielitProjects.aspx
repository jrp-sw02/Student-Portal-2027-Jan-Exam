<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NielitProjects.aspx.cs"
    Inherits="Admin_NielitProjects" MasterPageFile="~/MasterPages/main.master"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NIELIT Projects"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Project Name"></asp:Label>
                                        <asp:DropDownList ID="ddlprojectName" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Budget Allocated Equal or Above"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                <asp:TextBox ID="txtBudgetAllocatedSearch" runat="server"  onkeypress="return numeric(event)" onpaste="return false;" Width="95%" MaxLength="9" BorderStyle="Double">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>

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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Project Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
           
            if (!isBlank("<%=txtProjectName.ClientID  %>", "Project Name"))
                return false;
            if (!isBlank("<%=txtProjectDescription.ClientID  %>", "Project Description"))
                return false;
            if (!isBlankDate("<%=txtstartDate.ClientID %>", "Project From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtendDate.ClientID %>", "Project To Date", "dd-MMM-yyyy"))
                return false;
           
            if (!isBlank("<%=txtbudgetAllocated.ClientID  %>", "Budget Allocated"))
                return false;
            if (!isSelected("<%=ddlisAadharAuthenticationReqd.ClientID  %>", "isAadharAuthenticationReqd"))
                return false;

            if (!isDate("txtstartDate", "Invalid date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("txtendDate", "Invalid date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=txtprojectDurationInMonths.ClientID  %>", "Project Duration Months"))
                return false;
            
            if (!IsValidMinMaxLenght("<%=txtprojectDurationInMonths.ClientID %>", 1, 12, "Invalid Months"))
                return false;

            if (!isNumber("txtprojectDurationInMonths"))
                return false;

            if (!isNumber("txtbudgetAllocated"))
                return false;

            var aadharorderdateNumberFor = document.getElementById("<%=ddlisAadharAuthenticationReqd.ClientID %>").value;
            if (aadharorderdateNumberFor = 1) {
                if (!isBlank("<%=txtAadharOrderDate.ClientID  %>", "Aadhar Order Date"))
                    return false;
                if (!isBlank("<%=txtAadharOrderNumber.ClientID  %>", "Aadhar Order Number"))
                    return false;
        }
            var frdate = document.getElementById("<%=txtstartDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtendDate.ClientID %>").value;
            
            if (!CompareDates(frdate, todate, "Start date should be less then End date", true))
                return false;

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

     <script type="text/javascript" language="javascript">
         function numeric(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && ((charCode >= 48 && charCode <= 57) || charCode == 46))
                 return true;
             else {
                 alert('Please Enter Numeric values.');
                 return false;
             }
         }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>                               
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="schemeCode" 
                                    HeaderText="Scheme Code" SortExpression="schemeCode" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="fundedByOrg"
                                    HeaderText="Funded By Organization" SortExpression="fundedByOrg" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="projectName" 
                                    HeaderText="Project Name" SortExpression="projectName" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="fundedByOrg"
                                    HeaderText="Funded By Organization" SortExpression="fundedByOrg" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="budgetAllocated"
                                    HeaderText="Budget Allocated" SortExpression="budgetAllocated" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="projectFromDate" 
                                    HeaderText="Project Start Date" SortExpression="projectFromDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="projectTodate"
                                    HeaderText="Project End Date" SortExpression="projectTodate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="AadharOrderNumber"
                                    HeaderText="AadharOrderNumber" SortExpression="AadharOrderNumber" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="AadharOrderDate"
                                    HeaderText="AadharOrderDate" SortExpression="AadharOrderDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
               
                <tr>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Project Name &lt;b class='mandatory'&gt;*&lt;/b&gt;" ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Project Description &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Project Duration (in Months) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtProjectName" runat="server" SkinID="txt248" ToolTip="Project Name" Rows="200"  MaxLength="200"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtProjectDescription" runat="server" SkinID="txt248" MaxLength="1000" ></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtprojectDurationInMonths" SkinID="txt248" runat="server" onkeypress="return numeric(event)" onpaste="return false;" Width="200px" MaxLength="3" AutoPostBack="True" OnTextChanged="txtDuration_TextChanged"></asp:TextBox>
                     
                    </td>
                </tr>

                <tr>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Project Start Date &lt;b class='mandatory'&gt;*&lt;/b&gt;" ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Project End Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Funded By Org "></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                          <asp:TextBox ID="txtstartDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px" AutoPostBack="True"  OnTextChanged="txtstartDate_TextChanged"></asp:TextBox>
                         <img id="img2" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtstartDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtendDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px"></asp:TextBox>
                        <img id="img3"  runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtendDate">
                        </asp:CalendarExtender>
                        
                    </td>
                    <td style="width: 33%;" valign="top">                       
                         <asp:TextBox ID="txtfundedByOrg" runat="server" SkinID="txt248" ToolTip="fundedByOrg" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>                    
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Budget Allocated &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="LbisAadharAuthenticationReqd" runat="server" SkinID="CaptionLabel" Text="Aadhar Authentication Required "></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Aadhar Order Date "  visible="false"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">  

                    <asp:TextBox ID="txtbudgetAllocated" runat="server" SkinID="txt248" onkeypress="return numeric(event)" onpaste="return false;" Width="200px" MaxLength="9"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                      

                        <asp:DropDownList ID="ddlisAadharAuthenticationReqd" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlisAadharAuthenticationReqd_SelectedIndexChanged">
                            <asp:ListItem>---Select One---</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                        </asp:DropDownList>
                  

                    </td>
                   <td style="width: 33%;" valign="top">  

                     <asp:TextBox ID="txtAadharOrderDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px" visible="false"></asp:TextBox>
                        <img id="img1"  runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" visible="false" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtAadharOrderDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                
                <tr id="aadharordernumber1" runat = "server"><td style="width: 33%;" valign="top" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>   
                     <asp:Label ID="lblAadharOrderNo" runat="server" SkinID="CaptionLabel"  visible="false" Text="Aadhar Order Number "></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                     <td colspan="2">
                         <asp:Label ID="lblSchemeCode" runat="server" SkinID="CaptionLabel" Text="Scheme Code" Visible ="true"></asp:Label>
                    </td>
                </tr>
               
                         
                 <tr id="aadharordernumber2" runat = "server" >
                     <td >
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                         <asp:TextBox ID="txtAadharOrderNumber" runat="server" visible="false" SkinID="txt248"  onpaste="return false;" Width="200px" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                     </td>
                     <td colspan="2">
                         <asp:TextBox ID="txtSchemeCode" runat="server" MaxLength="5" Rows="200" SkinID="txt248" ToolTip="Scheme Code"></asp:TextBox>
                     </td>
                </tr>
                
                 <tr id="lblerrordisplay" runat="server" visible="false">
                     <td colspan="3">
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                           <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False" Width="99%"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
     <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td>   
            </td>
        </tr>
    </table>
</asp:Content>
