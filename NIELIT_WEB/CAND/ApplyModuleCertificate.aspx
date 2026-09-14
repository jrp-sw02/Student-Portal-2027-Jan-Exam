<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ApplyModuleCertificate.aspx.cs" Inherits="ApplyModuleCertificate" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Module Certificate</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Apply for Module-Wise Certificate"></asp:Label>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server">
    </asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ValidateForm() {
            if (!isSelected("<%=ddlCourseLevel.ClientID %>", "Course Level"))
                return false;
            if (!isSelected("<%=ddlModule.ClientID %>", "Course Module"))
                return false;
        }
        
       
        function printStatus() {
            window.print();
            return false;
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div id="divGrid" runat="server">
                <table width="99%" cellspacing="5" cellpadding="5" id="tblheader">
                    <tr>
                        <td align="left" style="font-weight: bold; padding-left: 100px; font-size: 12px;"
                            width="40%">
                            <asp:Label ID="lblName" runat="server" Text="Course Name"></asp:Label>
                        </td>
                        <td style="font-size: 12px;" width="60%">
                            <asp:DropDownList ID="ddlCourseLevel" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCourseLevel_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-weight: bold; padding-left: 100px; font-size: 12px;"
                            width="40%">
                            <asp:Label ID="lblModule" runat="server" Text="Module Name"></asp:Label>
                        </td>
                        <td style="font-size: 12px;" width="60%">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlModule" runat="server" Width="100%">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlCourseLevel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <%--<tr>
                        <td align="left" style="font-weight: bold; padding-left: 100px; font-size: 12px;"
                            width="40%">
                            <asp:Label ID="lblModuleFee" runat="server" AssociatedControlID="TxtPayableAmount">Amount Payable (&#8377;)</asp:Label>
                        </td>
                        <td style="font-size: 12px;" width="60%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="TxtPayableAmount" ReadOnly="true" SkinID="txt248" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlCourseLevel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>--%>
                </table>
                <div style="text-align: right; margin-top: 10px; margin-right: 20px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        OnClientClick="return ValidateForm();" />
                </div>
            </div>
            <br />
            <br />
            <div>
              <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                <br />
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gbModule" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseId"
                            OnRowDataBound="gbModule_RowDataBound" Width="100%" HeaderStyle-Font-Size="11px">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="5%" DataField="CourseName" HeaderText="Course Name">
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="20%" DataField="ModuleName" HeaderText="Module Name">
                                    <HeaderStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="5%" DataField="PaymentStatus" HeaderText="Payment Status">
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                 <asp:BoundField HeaderStyle-Width="5%" DataField="RequestType" HeaderText="Request Type">
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="10%" DataField="Status" HeaderText="Request Status">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <PagerSettings Visible="False" />
                            <EmptyDataTemplate>
                                No request Found.</EmptyDataTemplate>
                            <EmptyDataRowStyle CssClass="error" />
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
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div class="txt2" style="padding-bottom: 2px; padding-top: 2px;">
                <asp:Label ID="LblSubmitConfirm" Font-Bold="true" runat="server" Text=""></asp:Label>
                <asp:Label ID="LblRequestDetail" Font-Bold="true" runat="server" Text=""></asp:Label>
                <br />
           <%--     <div>
                    <table class="box" width="100%" cellpadding="0" cellspacing="0" id="tblpaymentmode"
                        runat="server">
                        <tr class="head1">
                            <td align="left">
                                <b>Payment Options:</b>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left">
                                <asp:RadioButtonList ID="RdoPaymentMode" runat="server" Width="267px" RepeatDirection="Horizontal"
                                    Style="vertical-align: top; border: 0" BorderStyle="None" RepeatLayout="Flow"
                                    AutoPostBack="True" OnSelectedIndexChanged="RdoPaymentMode_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>--%>
            </div>
            <asp:MultiView ID="SubmltvTab" runat="server">
               <%-- <asp:View ID="subView1" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="box">
                        <tr class="head1">
                            <td style="text-align: left;">
                                <strong>Payment By CSC </strong>:
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left;" class="style1">
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left;">
                                Please Print Form and go to nearest Common Service Center and make your Payment
                                <br />
                                To find the nearest common service center in your area go to <a href="http://www.csc.gov.in"
                                    target="_blank">http://www.csc.gov.in</a> and use VLE Locator
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:View>--%>
                <%--<asp:View ID="subView2" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="box">
                        <tr class="head1">
                            <td style="text-align: left;">
                                <strong>Payment By Online:</strong>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left; height: 24px;">
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left;">
                                Please make your Payment by Clicking on this link.
                                <asp:HyperLink ToolTip="Click to pay online" Style="cursor: pointer; color: Blue;"
                                    ID="nPayNow" runat="server">Pay Online</asp:HyperLink>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="text-align: left; height: 24px;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:View>--%>
                <%--<asp:View ID="subView3" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="1">
                        <tr class="head1">
                            <td colspan="2" style="text-align: left;">
                                <strong>Payment By NEFT RTGS: </strong>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="20%" colspan="2">
                                <asp:Label ID="Label5" runat="server" Text=" NEFT / RTGS Electronic Transfer Details."></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" width="30%">
                                Account Name
                            </td>
                            <td align="left">
                                <asp:Label ID="Label6" runat="server" Text="NIELIT"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="30%">
                                Bank Name
                            </td>
                            <td align="left">
                                <asp:Label ID="Label7" runat="server" Text="BANK OF INDIA"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" width="30%">
                                Bank Account Number
                            </td>
                            <td align="left">
                                <asp:Label ID="Label8" runat="server" Text="604820100000012"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="30%">
                                Name of the Branch
                            </td>
                            <td align="left">
                                <asp:Label ID="Label9" runat="server" Text="CGO COMPLEX BRANCH"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" width="30%">
                                Branch Code
                            </td>
                            <td align="left">
                                <asp:Label ID="Label10" runat="server" Text="6048"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="30%">
                                MICR Code
                            </td>
                            <td align="left">
                                <asp:Label ID="Label11" runat="server" Text="110013052"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" width="30%">
                                IFSC Code
                            </td>
                            <td align="left">
                                <asp:Label ID="Label12" runat="server" Text="BKID0006048"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="30%" valign="top">
                                Address of the Bank
                            </td>
                            <td align="left">
                                ELECTRONICS NIKETAN
                                <br />
                                6, CGO COMPLEX, LODHI ROAD
                                <br />
                                NEW DELHI- 110 003.
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" width="20%" colspan="2">
                                <asp:Label ID="Label1" runat="server" Text="<b>Please fill below mentioned NEFT/RTGS details to complete your application.</b>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="tr2" runat="server">
                            <td align="left" width="20%" colspan="2">
                                <asp:Label ID="Lbnefterror" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" width="30%">
                                Transaction Number <b class='mandatory'>*</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="Txtnefttransno" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="30%">
                                Transaction Date <b class='mandatory'>*</b>
                            </td>
                            <td valign="bottom" align="left">
                                <asp:TextBox MaxLength="11" ID="txtneftdate" runat="server" Width="160px"></asp:TextBox>
                                <img id="imgneftdate" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                    vertical-align: top;" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtneftdate" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgneftdate" runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="tr3" runat="server">
                            <td align="left" width="30%">
                                Amount <b class='mandatory'>*</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="Txtneftamount" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="tr4" runat="server">
                            <td align="left" width="30%">
                                Bank Name <b class='mandatory'>*</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="Txtneftbank" MaxLength="50" runat="server"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="GetBankNames"
                                    FirstRowSelected="true" ServicePath="~/WS/Common.asmx" TargetControlID="Txtneftbank"
                                    runat="server" MinimumPrefixLength="1" CompletionInterval="0" EnableCaching="false"
                                    CompletionSetCount="10">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="tr5" runat="server">
                            <td align="left" width="20%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <asp:Button ID="Btnneftsubmit" runat="server" Text="Submit" OnClientClick="return ValidateFormFields1();"
                                    OnClick="Btnneftsubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>--%>
            </asp:MultiView>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <div class="txt2">
                <asp:Label ID="LblRequestStatus" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
