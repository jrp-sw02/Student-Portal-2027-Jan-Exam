<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="VirtualAcademyCalendar.aspx.cs" Inherits="HO_VirtualAcademyCalendar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 33%;
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Virtual Academy Calendar"></asp:Label>
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
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Start Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:TextBox ID="txtStartDatefilter" runat="server" AutoPostBack="True" MaxLength="11" onpaste="return false;"  ReadOnly="false" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtStartDatefilter">
                                </asp:CalendarExtender>
                                <img id="img1" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Start Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:TextBox ID="txtStartDateToFilter" runat="server" AutoPostBack="True" MaxLength="11" onpaste="return false;"  ReadOnly="false" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtStartDateToFilter">
                                </asp:CalendarExtender>
                                <img id="img4" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                        
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Admission Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlAdmissionStatusFilter" runat="server" Width="100%" Visible="true">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="O">Open</asp:ListItem>
                                                    <asp:ListItem Value="C">Closed</asp:ListItem>                                                   
                                                </asp:DropDownList>                                                
                                            </ContentTemplate>                                            
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                   <td></td>
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
    <%--<uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Course Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
     <script language="javascript" type="text/javascript">
         function ValidateFormFields() {

             if (!isSelected("<%=ddlcoursecategory.ClientID  %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourse.ClientID  %>", "Course"))
                return false;
             if (!isSelected("<%=ddlbatch.ClientID  %>", "Batch"))
                 return false;
             if (!isBlank("<%=txtLevelParticipant.ClientID  %>", "Level of participant"))
                 return false;
            if (!isBlankDate("<%=txtstartDate.ClientID %>", "Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtendDate.ClientID %>", "End Date", "dd-MMM-yyyy"))
                return false;
           
            if (!isBlank("<%=txtDurationHours.ClientID  %>", "Duration in Hours"))
                return false;
            if (!isBlank("<%=txtDurationMonths.ClientID  %>", "Durationin Months"))
                return false;
            if (!isNumber("<%=txtDurationHours.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isNumber("<%=txtDurationMonths.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isSelected("<%=ddlAdmissionStatus.ClientID  %>", "Admission Status"))
                return false;
            if (!isBlank("<%=txtName.ClientID  %>", "Course Coordinator  Name"))
                return false;
            if (!isBlank("<%=txtDesignation.ClientID  %>", "Course Coordinator Designation"))
                return false;
            if (!isBlank("<%=txtEmail.ClientID  %>", "Course Coordinator Email"))
                return false;

            if (!isNumber("<%=txtSTDCode.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isNumber("<%=txtPhone1.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isNumber("<%=txtPhone2.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isNumber("<%=txtMobile.ClientID %>", "Numeric characters are  allowed"))
                return false;
             if (!isSelected("<%=ddlLanguage.ClientID  %>", "Teaching Language"))
                 return false;
             if (!isSelected("<%=ddlFromHours.ClientID  %>", "Timings From Hours"))
                 return false;
             if (!isSelected("<%=ddlFromMinutes.ClientID  %>", "Timings From Minutes"))
                 return false;
             if (!isSelected("<%=ddlToHours.ClientID  %>", "Timings To Hours"))
                 return false;
             if (!isSelected("<%=ddlToMinutes.ClientID  %>", "Timings To Minutes"))
                 return false;
            
            var frdate = document.getElementById("<%=txtstartDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtendDate.ClientID %>").value;
            if (!CompareDates(frdate, todate, "Start date should be less then End date", true))
                return false;
            var e2;
            e2 = document.getElementById("<%=txtEmail.ClientID %>").value;
            if (e2 != "") {
                if (!isValidEmail("<%=txtEmail.ClientID %>", "Not A Valid Email Address"))
                    return false;
            }
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
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                    <%--<asp:LinkButton ID="lbdisable" OnClientClick="return ConfirmAction('Are you sure you want to change the status of the selected exam center!');"
                                        runat="server" Text="Change Status" ToolTip="click to change status of this record" 
                                        SkinID="lnkbtnAction" onclick="lbdisable_Click"></asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting" HeaderStyle-Height ="20"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%" EmptyDataText="Please select filter criteria to get records in grid">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="CourseName"
                                    HeaderText="CourseName" SortExpression="CourseName" Target="_self">
                                <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                                
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="startDate"
                                    HeaderText="Start Date" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="endDate"
                                    HeaderText="End Date" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="durationHrs"
                                    HeaderText="Duration (in months/hrs)"  Target="_self">
                                    <HeaderStyle Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&amp;CategoryID={1}" DataTextField="courseInchargeName"
                                    HeaderText="Coordinator Name" SortExpression="Course" Target="_self" Visible="true">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                
                                 <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&amp;CategoryID={1}" DataTextField="mobEmail"
                                    HeaderText="Coordinator Contact" SortExpression="Course" Target="_self" Visible="true">
                                    <HeaderStyle Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle Height="20px" />
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
                    <td colspan="2" style="width: 66%;" valign="top">
                        &nbsp;</td>
                    <td style="width: 33%;" valign="top">&nbsp;</td>
                </tr>
                <tr class="even">
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:Label ID="Label23" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top"></td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">

                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged" SkinID="ddl250">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblState0" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="20%"></asp:Label>
                                <asp:Label ID="lblerrorddlcouse" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Batch &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                       <asp:Label ID="lblerrorddlbatch" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>

                    </td>
                </tr>
                <tr class="even">
                    <td colspan="2" style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True" Width="400px" SkinID="ddl760" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourse" EventName ="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlNSQFAligned" EventName ="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                       <asp:DropDownList ID="ddlbatch"  runat="server" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                            <%--<asp:ListItem Value="0" Text="--All--"></asp:ListItem>--%>
                        </asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="NSQF Aligned"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label36" runat="server" SkinID="CaptionLabel" Text="NSQF Level"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label35" runat="server" SkinID="CaptionLabel" Text="Level of Participants / Eligibility &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlNSQFAligned" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlNSQFAligned_SelectedIndexChanged" >
                                    <asp:ListItem Value="99" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                         <asp:TextBox ID="txtNSQFLevel" runat="server" MaxLength="3" onkeypress="checkNumber(this,2,0,event);" onpaste="return false;" SkinID="txt248" Width="200px" ></asp:TextBox>
                                 </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlNSQFAligned" EventName ="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlCourse" EventName ="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                      </td>
                    <td style="width: 33%;" valign="top">
                        
                        <asp:TextBox ID="txtLevelParticipant" runat="server" MaxLength="250" SkinID="txt248" Style="text-transform: uppercase;" Rows="2" TextMode="MultiLine" ToolTip="Name"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label34" runat="server" SkinID="CaptionLabel" Text="Start Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label33" runat="server" SkinID="CaptionLabel" Text="End Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label32" runat="server" SkinID="CaptionLabel" Text="Duration in Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                           
                            <ContentTemplate>
                                <asp:TextBox ID="txtstartDate" runat="server" AutoPostBack="True" MaxLength="11" onpaste="return false;"  ReadOnly="false" Width="200px" Enabled="false"></asp:TextBox>
                                <asp:CalendarExtender ID="txtstartDate_CalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtstartDate">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                &nbsp;<asp:TextBox ID="txtendDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" Enabled="false"></asp:TextBox>
                                <asp:CalendarExtender ID="txtendDate_CalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtendDate">
                                </asp:CalendarExtender>
                                <img id="img3" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtDurationHours" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248" Width="200px" Enabled="false"></asp:TextBox>
                                 </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlNSQFAligned" EventName ="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlCourse" EventName ="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label29" runat="server" SkinID="CaptionLabel" Text="Duration in Months &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label31" runat="server" SkinID="CaptionLabel" Text="Course Admission Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                         <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                             <ContentTemplate>
                                 <asp:TextBox ID="txtDurationMonths" runat="server" MaxLength="2" onkeypress="checkNumber(this,2,0,event);" onpaste="return false;" SkinID="txt248" Width="200px" ></asp:TextBox>
                             </ContentTemplate>
                              </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlAdmissionStatus" runat="server" Height="16px" OnSelectedIndexChanged="ddlAdmissionStatus_SelectedIndexChanged" SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="O">Open</asp:ListItem>
                            <asp:ListItem Value="C">Closed</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                       
                       
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label19" runat="server" SkinID="CaptionLabel" Text="Course Coordinator Details" style="font-weight: 700"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label24" runat="server" SkinID="CaptionLabel" Text="Designation &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        &nbsp;</td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="50" SkinID="txt248" onpaste="return false;" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <Triggers>
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtDesignation" runat="server" MaxLength="50" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="STD Code" ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Phone1"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Phone2"></asp:Label>
                    </td>
                </tr>
                <tr id="Rview2" runat="server" class="even">
                    <td valign="top" class="auto-style1">
                        <asp:TextBox ID="txtSTDCode" runat="server" MaxLength="6" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:TextBox ID="txtPhone1" runat="server" MaxLength="10" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:TextBox ID="txtPhone2" runat="server" MaxLength="10" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                </tr>
               <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Mobile" ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Email &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                   <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Teaching Language (Primary / Secondary) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                   
                </tr>
                <tr id="Rview4" runat="server" class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="12" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" SkinID="txt248" Style="text-transform: uppercase;" Width="200px"></asp:TextBox>
                    </td>
                   <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlLanguage" runat="server" Height="16px"  SkinID="ddl250" Width="140px" Enabled="false">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                       <asp:DropDownList ID="ddlSecLanguage" runat="server" Height="16px"  SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Timings From" style="font-weight: 700"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Minutes &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        &nbsp;</td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlFromHours" runat="server" Height="16px"  SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlFromMinutes" runat="server" Height="16px"  SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>

                
                  <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Timings To" style="font-weight: 700"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label16" runat="server" SkinID="CaptionLabel" Text="Minutes &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        &nbsp;</td>
                   <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlToHours" runat="server" Height="16px"  SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlToMinutes" runat="server" Height="16px"  SkinID="ddl250" Width="181px">
                            <asp:ListItem Value="0">Select</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>
               
               
            </table>
            <div style="text-align: right; margin-top: 10px">

                <asp:Button ID="btnSave"  runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return ValidateFormFields();" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="NIELITCentreId" runat="server" />
                    <asp:HiddenField ID="HNANFL" runat="server" />
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    </asp:Content>
