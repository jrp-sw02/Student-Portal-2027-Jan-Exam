<%@ Page Title="Exam Detail" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="admcertiexamdetail.aspx.cs" Inherits="admcertiexamdetail" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Details"></asp:Label>
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlFilterYear" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCycle" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:DropDownList ID="ddlCycle" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">

    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="99%" ID="lblerror" CssClass="error" runat="server" EnableTheming="false"
        Visible="false" ForeColor="Red"></asp:Label>

    <style type="text/css">
    .modalPopup 
    { 
     border: 3px solid #31597C; 
     background-color: #E6F0F0; 
     padding-top:0px; 
     padding-right: 0px; 
     width: auto; 
     height:auto; 
     top:0px; 
     left:-110px; 
     position:relative;
    }
  table.GridView tr td, table.GridView tr th
  {
  height:70px;
  text-align:center;
  vertical-align:top;
  }
    </style>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isBlank("<%=txtExamName.ClientID %>", "Exam Name"))
                return false;
            if (!isBlankDate("<%=txtExamDate.ClientID %>", "Exam Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtExamDate.ClientID %>", "Invalid Exam Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtpubdate.ClientID %>", "Date of Publishing", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtpubdate.ClientID %>", "Invalid Date of Publishing", "dd-MMM-yyyy"))
                return false;

            return true;

        }
        function validateFields() {
            var ExamCycle=document.getElementById("<%=hfScheduleID.ClientID %>").value ;
           
            if (!isSelected("<%=ddlExamcycle.ClientID %>", "Exam Cycle"))
                return false;
            if(ExamCycle != <%=(Int32)EConnect.NIELIT.enmExamSchedule.Weekly %>)
            {
               if (!isSelected("<%=ddlOccurance.ClientID %>", "Starting day of exam"))
                return false;
            }
            if (!isBlankNumber("<%=txtYear.ClientID %>", "Year of Exam"))
                return false;
            if (!isNumber("<%=txtYear.ClientID %>"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtYear.ClientID %>", 4, 4, "Invalid Year of Exam "))
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
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" >
                              <HeaderStyle height="70px" backcolor="#26486685"/>
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField  DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="name" HeaderText="Exam Name" SortExpression="name" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="examCycle" HeaderText="Exam Cycle" SortExpression="examCycle"
                                    Target="_self">
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="examMonth" DataTextFormatString="{0:MMMM}"  HeaderText="Exam Month" SortExpression="examMonth"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="year" HeaderText="Exam Year" SortExpression="year" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                  </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="examDate" HeaderText="Exam Start Date"
                                    SortExpression="examDate" Target="_self" />
                                <asp:BoundField HeaderText="Time Table Date" DataField="PubDate" NullDisplayText="NA"
                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                <asp:BoundField HeaderText="OffLine Publishing Date"  DataField="OfflinePubDate" NullDisplayText="NA"
                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                 <asp:BoundField HeaderText="OnLine Publishing Date"  DataField="OnlinePubDate" NullDisplayText="NA"
                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                 <asp:BoundField HeaderText="Practical Publishing Date" DataField="PracPubDate" NullDisplayText="NA"
                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                <asp:BoundField HeaderText="Result Publishing Date"  DataField="ResultDate" NullDisplayText="NA"
                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>                              
                            </Columns>
                            <HeaderStyle Font-Bold="True" Font-Size="11px" />
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hidHeaders" runat="server" />
                        <asp:HiddenField ID="hfSendDate" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="15" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
       
            <table id="Table1" class="sample2" cellpadding="2" cellspacing="0" runat="server">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblExamCycle" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblSchedule" runat="server" SkinID="CaptionLabel" Text="Exam Schedule "
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblStartMonth" runat="server" SkinID="CaptionLabel" Text="Starting Exam Month"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlExamcycle" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlExamcycle_SelectedIndexChanged">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbSchedule" runat="server" Width="100%"></asp:Label>
                                <asp:HiddenField ID="hfScheduleID" Value="0" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamcycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbStartMonth" runat="server" Width="100%"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamcycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblOccurance" runat="server" SkinID="CaptionLabel" Text="Starting Day and Week"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblDate" runat="server" SkinID="CaptionLabel" Text="Year of Exam &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList Width="49%" ID="ddlOccurance" runat="server">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                 
                                </asp:DropDownList>
                                <asp:DropDownList Width="49%" ID="ddlWeek" runat="server" OnSelectedIndexChanged="ddlWeek_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="0">Sunday</asp:ListItem>
                                    <asp:ListItem Value="1">Monday</asp:ListItem>
                                    <asp:ListItem Value="2">Tuesday</asp:ListItem>
                                    <asp:ListItem Value="3">Wednesday</asp:ListItem>
                                    <asp:ListItem Value="4">Thursday</asp:ListItem>
                                    <asp:ListItem Value="5">Friday</asp:ListItem>
                                    <asp:ListItem Value="6">Saturday</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamcycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtYear" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top" align="center">
                        <asp:Button ID="btnCreate" runat="server" Text="Create Exams" OnClick="btnCreate_Click"
                            OnClientClick="return validateFields();" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;display:none" valign="top">
                        <asp:label id="lblneftperiod" runat="server" skinid="CaptionLabel" text="Neft Extension Period (in Days)"
                            width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top" id="tddispatch" visible="false" runat="server">
                        <asp:label id="lbldisptach" runat="server" skinid="CaptionLabel" text="Dispatching by Institute"
                            width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top" id="tddispatch3" visible="false" runat="server">
                        <asp:label id="lblbatch" runat="server" skinid="CaptionLabel" text="Batch Processing by Regional Centre"
                            width="100%"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;display:none" valign="top">
                        <asp:textbox runat="server" id="txtneftperiod" maxlength="2" onkeypress="checkNumber(this,2,0,event);"
                            skinid="txt248">
                        </asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top" id="tddispatch1" visible="false" runat="server">
                        <asp:dropdownlist runat="server" id="ddldispatch" skinid="ddl250">
                            <asp:listitem value="1" selected="True">Yes</asp:listitem>
                            <asp:listitem value="2">No</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                    <td style="width: 33%;" valign="top" id="tddispatch2" visible="false" runat="server">
                        <asp:dropdownlist runat="server" id="ddlbatchprocessing" runat="server" skinid="ddl250">
                            <asp:listitem value="1" selected="True">Yes</asp:listitem>
                            <asp:listitem value="2">No</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                <td style="width: 33%;" valign="top">
                    <asp:label id="lblresultversion" runat="server" skinid="CaptionLabel" text="Result Grade Version"
                        style="float:left; width:52%;">
                     </asp:label>
                    <asp:imagebutton id="ImgBtnPopupFee" runat="server" imageurl="~/images/DisablePopup.PNG"
                        enabled="False" style="clear:both;" />
                     </td>
                     <%--deep add code--%>
                     <td style="width: 33%;" valign="top">
                     <asp:label id="lblfeesubmissionInstituteExtPeriod" runat="server" skinid="CaptionLabel" text="Fee Submission of Institute Extension Period (in Days)"
                            width="100%"></asp:label>
                     
                     </td>
                     <%--deep  end add code--%>
                     <td></td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:dropdownlist id="ddlversion" runat="server" skinid="ddl250" 
                            autopostback="true" onselectedindexchanged="ddlversion_SelectedIndexChanged">
                        </asp:dropdownlist>
                    </td>
                    <%--deep add code--%>
                    <td style="width: 33%;" valign="top">
                    
                    <asp:textbox runat="server" id="txtfeesubmissionInstituteExtPeriod" maxlength="2" onkeypress="checkNumber(this,2,0,event);"
                            skinid="txt248">
                        </asp:textbox>

                    </td>
                    <%--deep  end add code--%>
                    <td>
                    </td>
                </tr>
                <tr id="trShow" runat="server" visible="false">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblExamName" runat="server" SkinID="CaptionLabel" Text="Exam Name&lt;b class='mandatory'&gt;*&lt;/b&gt; "
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblExamDate" runat="server" SkinID="CaptionLabel" Text="Exam Start Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lblpubdate" runat="server" SkinID="CaptionLabel" Text="Time Table Publishing Date"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even" runat="server" id="trShow1" visible="false">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExamName" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtExamDate" runat="server" SkinID="txt210"></asp:TextBox>
                                <img id="imgDateFrom" alt="Calender" src="../images/calendaricon.jpg" />
                                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="imgDateFrom" TargetControlID="txtExamDate">
                                </asp:CalendarExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWeek" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top" align="left">
                        <asp:TextBox Visible="false" ID="txtpubdate" runat="server" SkinID="txt210"></asp:TextBox>
                        <img visible="false" id="imgPublishDate" alt="Calender" runat="server" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgPublishDate" TargetControlID="txtpubdate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr id="tr1" runat="server" visible="false">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="OffLine Admit Card Publishing Date"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" runat="server" id="rpubdatelabel">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="OnLine Admit Card Publishing Date&lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" id="tdpract" runat="server">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Practical Admit Card Publishing Date&lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    
                </tr>
                <tr class="even" runat="server" id="tr2" visible="false">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtOfflinePubdate" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="img2" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender5" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img2" TargetControlID="txtOfflinePubdate">
                        </asp:CalendarExtender>
                    </td>
                      <td style="width: 33%;" valign="top" runat="server" id="rpubdatetext">
                        <asp:TextBox ID="txtOnlinePubdate" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender3" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtOnlinePubdate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top" align="left" id="tdpract2" runat="server">
                        <asp:TextBox ID="txtpracAdmitCard" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="img3" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender4" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img3" TargetControlID="txtpracAdmitCard">
                        </asp:CalendarExtender>
                    </td>
                  
                </tr>
                <tr id="trResultpubdate" runat="server">
                    <td style="width: 23%;" valign="top" runat="server">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Result Publishing Date&lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 23%;" valign="top" runat="server">
                        <asp:TextBox ID="txtResultPubDate" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="img4" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender6" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtResultPubDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr id="trShowTbl" visible="false" runat="server">
                    <td colspan="3" style="width: 100%">
                        <div id="divdetail" runat="server" style="width: 100%">
                        </div>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    Visible="false" />
            </div>
            <asp:modalpopupextender id="ModalPopupExtender2" runat="server" dropshadow="true"
                cancelcontrolid="ImgCancle1" targetcontrolid="ImgBtnPopupFee" popupcontrolid="InfoDiv">
            </asp:modalpopupextender>
            <div id="InfoDiv" runat="server" class="modalPopup">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="5%">
                            &nbsp;
                        </td>
                        <td width="90%">
                        </td>
                        <td width="5%">
                            <asp:imagebutton id="ImgCancle1" runat="server" imageurl="~/images/cancel.gif" style="float: right;"
                                tooltip="click to close" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td colspan="2" style="color: Navy; font-weight: bold;">
                            Grade Legends
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            <div class="box" id="divreport" runat="server">
                            </div>
                        </td>
                        <td width="5%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc5:SideLink ID="SideLink1" runat="server" />
</asp:Content>
