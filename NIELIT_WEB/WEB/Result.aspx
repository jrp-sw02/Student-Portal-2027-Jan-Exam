<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="Result.aspx.cs" Inherits="Result" Debug="true" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sidetable a
        {
            color: #000000;
            text-decoration: none;
        }
        .sidetable a:hover
        {
            color: #3366CC;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblheading" runat="server" Text="View Result"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function printStatus() {
            //            var WindowObject = window.open('../PrintForm.aspx', 'PrintWindow', 'width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
            //            WindowObject.document.getElementById("divfresult").innerHTML = window.document.getElementById("<%=divresult.ClientID%>").innerHTML;
            //            WindowObject.document.close();
            //            WindowObject.focus();
            //            WindowObject.print();
            window.print();
        }
        function ValidateFormFields() {

            if (!isSelected("<%=Ddlexamyear.ClientID%>", "Examination Year"))
                return false;
            if (!isSelected("<%=Ddlexamcycle.ClientID%>", "Examination Name"))
                return false;
            if (!isBlank("<%=Txtrollno.ClientID %>", "Roll Number / Candidate Name / Application Number"))
                return false;
            if (!isBlank("<%=TxtDOB.ClientID %>", "Date of Birth"))
                return false;
            if (!isDate("<%=TxtDOB.ClientID %>", "Invalid Date of Birth"))
                return false;
            if (!isBlank("<%=txtcode.ClientID %>", "Captcha Code No."))
                return false;
        }
    </script>
    <asp:Label ID="Lblerror" Width="99%" class="error" EnableTheming="false" Visible="false"
        runat="server" Text=""></asp:Label>
    <table id="tbsearch" align="center" class="sample3" width="100%" runat="server">
        <tr class="head1">
            <td align="left" width="100%">
                <asp:RadioButtonList ID="Rdserachby" runat="server" RepeatDirection="Horizontal"
                    Width="100%" AutoPostBack="True" Style="text-align: left;" OnSelectedIndexChanged="Rdserachby_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True">Search By Roll Number</asp:ListItem>
                    <asp:ListItem Value="2">Search By Candidate Name</asp:ListItem>
                    <asp:ListItem Value="3">Search By Registration Number</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div id="divfilter" runat="server">
        <table id="Table3" align="center" class="sample3" width="100%" runat="server" cellpadding="3"
            cellspacing="1">
            <tr class="gdrow1">
                <td class="even" width="30%" id="td1" runat="server">
                    Examination Year
                </td>
                <td width="70%" valign="bottom">
                    <asp:DropDownList ID="Ddlexamyear" runat="server" AutoPostBack="True" Height="22px"
                        Width="260px" onselectedindexchanged="Ddlexamyear_SelectedIndexChanged">
                    </asp:DropDownList>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Examination
                        Year you appeared in)</span>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="30%">
                    Examination Name
                </td>
                <td width="70%" valign="bottom">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                     <asp:DropDownList ID="Ddlexamcycle" runat="server" Height="22px" Width="260px">
                     <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>
                      <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Examination
                        Name you appeared in)</span>
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Ddlexamyear" EventName="SelectedIndexChanged" />
                    </Triggers>
                    </asp:UpdatePanel>

                  
                </td>
            </tr>
            <tr class="gdrow1">
                <td class="even" width="30%" id="tdfilter" runat="server">
                    Enter Roll Number
                </td>
                <td>
                    <asp:TextBox ID="Txtrollno" runat="server" Width="248px" MaxLength="50"></asp:TextBox>
                    <span id="spfilter" runat="server" style="text-align: left; vertical-align: top;
                        font-size: 8pt;">(Roll No. printed on your admit card)</span>
                </td>
            </tr>
            <tr class="trgdalternate1calendar">
                <td class="odd" width="30%">
                    Enter DOB(dd-Mon-yyyy)
                </td>
                <td class="odd">
                    <asp:TextBox ID="TxtDOB" runat="server" Width="248px"></asp:TextBox>
                    <img id="imgDob" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                        vertical-align: top;" />
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Date of Birth
                        of the candidate)</span>
                    <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDOB" PopupPosition="BottomLeft"
                        Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr class="gdrow1">
                <td valign="top" width="30%">
                    Captcha Code
                </td>
                <td>
                    <%--onkeypress="checkNumber(this,6,0,event)"--%>
                    <asp:TextBox ID="txtcode" runat="server" Width="248px" MaxLength="6" ></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Enter captcha
                        code shown in image below)</span>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top" width="30%">
                </td>
                <td align="left" valign="top" style="padding: 0">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="margin: 0">
                                <tr>
                                    <td width="38%" align="left" valign="top">
                                        <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                        <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                            Width="30px" Style="vertical-align: baseline;" OnClick="ImgBtnRefresh_Click" />
                                    </td>
                                    <td width="62%">
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Click here to
                                            obtain new captcha code if are not able to see the captcha code in image.)</span>
                                    </td>
                                </tr>
                            </table>                          
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="Btnview" runat="server" Text="View" class="even" OnClientClick="return ValidateFormFields();"
                OnClick="Btnview_Click" Style="height: 26px" />
            <asp:Button ID="Btnreset" runat="server" Text="Reset" class="even" OnClick="Btnreset_Click" />
        </div>
    </div>
    <div id="divresult" runat="server" visible="false">
        <table align="center" border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="head1">
                <th colspan="3" align="left">
                    <asp:Label ID="Lblcourse" runat="server" Text="" Style="text-align: center;"></asp:Label>
                </th>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Candidate Name
                </td>
                <td>
                    <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trfathername" runat="server">
                <td width="40%">
                    Father's Name
                </td>
                <td>
                    <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trmothername" runat="server">
                <td width="40%">
                    Mother's Name
                </td>
                <td>
                    <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" runat="server" id="trgname" visble="false">
                <td width="40%">
                    Guardian Name
                </td>
                <td>
                    <asp:Label ID="Lgname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    Exam/Result Details
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Roll Number
                </td>
                <td colspan="2">
                    <asp:Label ID="LblRollno" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trcoursename" runat="server">
                <td>
                    Course Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Exam Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblexam" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trExamDate" runat="server">
                <td>
                    Date of Exam
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblexamdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trCentreName" runat="server">
                <td>
                    Exam Centre Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblexamcentre" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trCCCName" runat="server">
                <td>
                    CCC No.
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblcc" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trInstituteName" runat="server">
                <td>
                    Institute Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lbliname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" style="color: Navy; background-color: #ccff66;" id="trResultGrade" runat="server" visible="false">
                <td valign="top">
                    <b>Result (in Grade)</b>
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblresult" runat="server" Text="" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Result Published Date
                </td>
                <td colspan="2">
                    <asp:Label ID="LblResultDate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td valign="top">
                    Remarks
                </td>
                <td colspan="2">
                    <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td colspan="3">
                    <br />
                    <br />
                </td>
            </tr>
            <tr class="head1" id="trlegends" runat="server">
                <td style="font-size: 16px; padding-top: 10px;" colspan="3">
                    Meaning of Grade
                </td>
            </tr>
            <tr id="trlegends1" runat="server">
                <td width="100%" id="tdLegends" runat="server" style="font-size: 10px; padding-top: 0px;"
                    colspan="2">
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table id="tblResult" runat="server" visible="false" align="center" cellpadding="0"
            cellspacing="0" width="100%" class="preview" style="font-size: 11px;">
            <tr>
                <td width="100%" style="text-align: center; font-weight: bold; font-size: 14px;">
                    Result Card
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" AllowPaging="false" EnableTheming="false" BorderColor="Black">
                        <Columns>
                            <asp:BoundField HeaderStyle-Width="8%" HeaderStyle-BorderColor="Black" HeaderText="Code"
                                DataField="Code" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="60%" HeaderStyle-BorderColor="Black" HeaderText="Module Name"
                                DataField="name" ItemStyle-BorderColor="Black" />
                           <%-- <asp:BoundField HeaderStyle-Width="12%" HeaderText="Module Type" HeaderStyle-BorderColor="Black"
                                DataField="MType" ItemStyle-BorderColor="Black" />--%>
                             <asp:BoundField HeaderStyle-Width="8%" HeaderText="Theory Marks" HeaderStyle-BorderColor="Black"
                                DataField="TheoryMarks" ItemStyle-BorderColor="Black" />
                             <asp:BoundField HeaderStyle-Width="8%" HeaderText="Practical Marks" HeaderStyle-BorderColor="Black"
                                DataField="PracticalMarks" ItemStyle-BorderColor="Black" />
                             <asp:BoundField HeaderStyle-Width="10%" HeaderText="Weighted  Marks ( Wherever Applicable)" HeaderStyle-BorderColor="Black"
                                DataField="WeightedMarks" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" HeaderText="Result" HeaderStyle-BorderColor="Black"
                                DataField="Result" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderText="Grade Legend"
                                HeaderStyle-BorderColor="Black" DataField="Grade" ItemStyle-BorderColor="Black" />
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px" runat="server" id="divfooter" visible="false">
        <asp:Button ID="Btnprint" runat="server" Text="Print" Width="54px" />
        <asp:Button ID="Btnback" runat="server" Text="Back" Width="54px" OnClick="Btnback_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
    <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
