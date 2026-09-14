<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="Message.aspx.cs" Inherits="Admin_Message" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="SMS/Email Services"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
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
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static" OnClientClick="return validfilter()"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Service Type"></asp:Label>
                                        <asp:DropDownList ID="ddlflserviceType" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                            <asp:ListItem Value="1">SMS</asp:ListItem>
                                            <asp:ListItem Value="2">Email</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Width="100%" runat="server" Text="Date From"></asp:Label>
                                         <asp:TextBox ID="txtflFromDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                         <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                             PopupButtonID="imgdatefrom" TargetControlID="txtflFromDate"></asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                         <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                             PopupButtonID="img1" TargetControlID="txtToDate"></asp:CalendarExtender>
                                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Mobile Number/Email Address"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
 <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
             <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
       <%-- </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BreadCrumb1" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<div align="right">
 <asp:RadioButtonList ID="rblServiceType" runat="server" OnSelectedIndexChanged="rblServiceType_SelectedIndexChanged"
        RepeatDirection="Horizontal" AutoPostBack="True" Visible="False">
        <asp:ListItem Selected="True" Value="1">SMS</asp:ListItem>
        <asp:ListItem Value="2">Email</asp:ListItem>
    </asp:RadioButtonList>
  </div>
    <script type="text/javascript">
        function CheckInvalidCharacters(evt) {
            alert("Hello");
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 33 || charCode == 60 || charCode == 62 || charCode == 38 || charCode == 35)
                return false;
            return true;
        }
        function ccc() {
            alert("Hello");
            return false;
        }
        function textCounter() {
            var field = document.getElementById("<%=txtSMSMessage.ClientID %>");
            var maxlimit = 160;
            var countfield = document.getElementById("<%=lblCount.ClientID %>");
            if (field.value.length > maxlimit)
                field.value = field.value.substring(0, maxlimit);
            else
                countfield.innerHTML = maxlimit - field.value.length;
        }
        function ValidatePhoneCSV() {
            if (!isBlank("<%=txtSMSMessage.ClientID %>", "Message"))
                return false;
            if (!isBlank("<%=txtSMSMobiles.ClientID %>", "Mobile Number"))
                return false;
            var value = document.getElementById("<%=txtSMSMobiles.ClientID %>").value;
            var regex = /^(\d{10},)*\d{10}$/;
            if (regex.test(value) == true) {
               
                return true;
            }
            else {
                alert("Not a valid mobile number");
                return false;
            }
            
        }

        function changetext() {
            var mobile = document.getElementById("<%=txtSMSMobiles.ClientID %>").value;
            if (mobile == "") {

                document.getElementById("<%=lblTotalmsg.ClientID %>").innerText = "Total SMS will be sent: 0";
            }
            else {
                var list = parseInt(mobile.split(",").length);
                if (list != "")
                    document.getElementById("<%=lblTotalmsg.ClientID %>").innerText = "Total SMS will be sent: " + parseInt(list);
            }

        }
        function TotalEmail() {
            var emails = document.getElementById("<%=txtEmailToAddress.ClientID %>").value;
            if (emails == "") {

                document.getElementById("<%=lblTotalEmail.ClientID %>").innerText = "Total Email will be sent: 0";
            }
            else {
                var list = parseInt(emails.split(",").length);
                if (list != "")
                    document.getElementById("<%=lblTotalEmail.ClientID %>").innerText = "Total Email will be sent: " + parseInt(list);
            }

        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 44 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function validateEmail(field) {
            var regex = /\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b/i;
            return (regex.test(field)) ? true : false;
        }
        function validfilter() {
            if (!isSelected("<%=ddlflserviceType.ClientID %>", "Service Type"))
                return false;
            if (!isBlankDate("<%=txtflFromDate.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtflFromDate.ClientID %>", "Invalid From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtToDate.ClientID %>", "Invalid To Date", "dd-MMM-yyyy"))
                return false;
            var frdate = document.getElementById("<%=txtflFromDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtToDate.ClientID %>").value;
            if (!CompareDates(frdate,todate, "From date should be less then To date",true))
            {
                return false;
            }
        }
        function validateMultipleEmailsCommaSeparated() {
            if (!isBlank("<%=txtEmailToAddress.ClientID %>", "To Email Address"))
                return false;
            var value = document.getElementById("<%=txtEmailToAddress.ClientID %>").value;
//            var valuecc = document.getElementById("<%=txtCCEmailAddress.ClientID %>").value;
            if (value != "") {
                var result = value.split(",");
                for (var i = 0; i < result.length; i++)
                    if (!validateEmail(result[i])) {
                        alert("Not a Valid Email Address");
                        return false;
                    }
                }
                if (!isBlank("<%=txtSubject.ClientID %>", "Subject"))
                    return false;
//            if (valuecc != "") {
//                var result = valuecc.split(",");
//                for (var i = 0; i < result.length; i++)
//                    if (!validateEmail(result[i])) {
//                        alert("Not a Valid Email CC Address");
//                        return false;
//                    }
//                }
                if (trim(document.getElementById("<%=txtMsg.ClientID %>").value, " ") == "") {
                    alert("Message/Email body can not be left blank.");
                    return false;
                }
                else {
                    if (!confirm("Do you want to send this Email"))
                        return false;
                }
            return true;
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
            <asp:HiddenField ID="HiddenField1" runat="server">
                                </asp:HiddenField>
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                    <asp:Label Width="99%" EnableTheming="False" CssClass="error" ID="lblError1"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Mobile" HeaderText="Mobile No." SortExpression="Mobile" 
                                    Target="_self" >
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Message" HeaderText="Message" SortExpression="Message" 
                                    Target="_self" >
                                <HeaderStyle Width="40%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Date" HeaderText="Date" SortExpression="Date" DataTextFormatString="{0:dd-MMM-yyyy}"
                                    Target="_self" >
                                <HeaderStyle Width="12%" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Status" HeaderText="Status" SortExpression="Status"
                                    Target="_self" >
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                         <asp:GridView ID="gvEmail" runat="server" DataKeyNames="ID" OnSorting="gvEmail_Sorting"
                            OnRowDataBound="gvEmail_RowDataBound" AutoGenerateColumns="False" 
                            Width="100%" Visible="False">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#"><HeaderStyle Width="5%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                 <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Subject" HeaderText="Subject" SortExpression="Subject" 
                                    Target="_self" >
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Email" HeaderText="Email Address" SortExpression="Email" 
                                    Target="_self" >
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,stype" DataNavigateUrlFormatString="?Key={0}&stype={1}"
                                    DataTextField="Date" HeaderText="Date" SortExpression="Date" DataTextFormatString="{0:dd-MMM-yyyy}"
                                    Target="_self" >
                                <ItemStyle HorizontalAlign="Center" />
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
        <table class="sample2" runat="server" id="Table1" cellpadding="2" cellspacing="0"
        width="100%">
        <tr class="heading">
            <td valign="top">
                <asp:Label ID="lblServiceType" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
    <table class="sample2" runat="server" id="tblsms" cellpadding="2" cellspacing="0"
        width="100%">
        <tr >
            <td width="60%" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Type your message here &lt;b class='mandatory'&gt;*&lt;/b&gt;(Maximum 160 characters only)"></asp:Label>
            </td>
            <td width="40%">
                <asp:Label ID="lblCount" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td colspan="2" valign="top">
                <asp:TextBox ID="txtSMSMessage" runat="server" SkinID="txt756" ToolTip="Message"
                    onkeyup="textCounter();" onkeydown="textCounter();" TextMode="MultiLine" Height="68px"
                    Width="177px" MaxLength="160" Rows="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  valign="top">
                <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="Mobile number of recipient  &lt;b class='mandatory'&gt;*&lt;/b&gt; (e.g.: 9001107701,9001107702)"
                    Width="100%"></asp:Label>
            </td>
            <td valign="top" align="right">
                <asp:Label ID="lblTotalmsg" style="text-align:right" runat="server" SkinID="CaptionLabel" Text="Total SMS will be sent: 0"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td colspan="2"  valign="top">
                <asp:TextBox ID="txtSMSMobiles" runat="server" MaxLength="160" SkinID="txt756" Height="68px"
                    TextMode="MultiLine" onkeydown="changetext();" onchange="changetext();" onkeyup="changetext();"
                    onmouseout="changetext();" ToolTip="Mobile Numbers"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td  valign="top">
                <asp:Label ID="lblDate" runat="server" SkinID="CaptionLabel" Text=""
                    Width="100%"></asp:Label>
            </td>
            <td  valign="top">
                <asp:Label ID="lblstatus" runat="server" SkinID="CaptionLabel" Text=""
                    Width="100%"></asp:Label>
            </td>
        </tr>
        </table>
    <table class="sample2" runat="server" id="tblEmail" cellpadding="2" cellspacing="0"
        width="100%">
        <tr>
            <td valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="To &lt;b class='mandatory'&gt;*&lt;/b&gt; someone@gmail.com,newone@yahoo.in"></asp:Label>
            </td>
            <td valign="top" align="right">
                <asp:Label ID="lblTotalEmail" style="text-align:right" runat="server" SkinID="CaptionLabel" Text="Total Email will be sent: 0"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td valign="top" colspan="2" id="tdEmailToAddress" runat="server">
                <asp:TextBox ID="txtEmailToAddress" runat="server" SkinID="txt756" onkeydown="TotalEmail();" onchange="TotalEmail();" onkeyup="TotalEmail();"
                    onmouseout="TotalEmail();"></asp:TextBox>
            </td>
        </tr>
        <tr id="Tr1" runat="server" visible="false">
            <td valign="top" colspan="2">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" 
                    Text="CC  someone@gmail.com,newone@yahoo.in" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr id="Tr2" class="even" runat="server" visible="false">
            <td valign="top" colspan="2">
                <asp:TextBox ID="txtCCEmailAddress" runat="server" SkinID="txt756" 
                    Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" 
                    Text="Subject (Max 100 characters) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td valign="top" colspan="2" id="tdSubject" runat="server">
                <asp:TextBox ID="txtSubject" runat="server" SkinID="txt756" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Message"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td  colspan="2" id="tdMsg" runat="server">
                <asp:TextBox ID="txtMsg" Text="" runat="server" SkinID="txt756" TextMode="MultiLine" Height="150px"  
                    ></asp:TextBox>
                <%--<asp:HtmlEditorExtender ID="htEforEmail" runat="server" TargetControlID="txtMsg"
                    DisplaySourceTab="true">
                </asp:HtmlEditorExtender>--%>
            </td>
        </tr>
        <tr>
            <td  valign="top" colspan="2">
                <asp:Label ID="lblDateEmail" runat="server" SkinID="CaptionLabel" Text=""
                    Width="100%"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnResendEmail" runat="server" Text="Resend Email" 
            Visible="False" OnClientClick="return validateMultipleEmailsCommaSeparated();" 
            onclick="btnResendEmail_Click"/>
        <asp:Button ID="btnResendSMS" runat="server" Text="Resend Sms" Visible="False" 
            OnClientClick="return ValidatePhoneCSV();" onclick="btnResendSMS_Click"/>
        <asp:Button ID="btnSendSms" runat="server" Text="Send Sms" Visible="False" OnClientClick="return ValidatePhoneCSV();" OnClick="btnSendSms_Click" />
        <asp:Button ID="btnEmail" runat="server" Text="Send Email" Visible="False" 
            OnClientClick="return validateMultipleEmailsCommaSeparated()" 
            onclick="btnEmail_Click"  />
        <asp:Button ID="btnCancel" runat="server" Text="Reset" Visible="False" OnClick="btnCancel_Click" />
        <asp:Button ID="bbtnCancel" runat="server" Text="Cancel" 
            onclick="bbtnCancel_Click" />
    </div> 
    </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
