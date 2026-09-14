<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCandidateDetail.aspx.cs"
    Inherits="EditCandidateDetail" MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 44px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Edit Candidate(CCC) Data
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            //Course Category
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;

            //Course Name
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;

            //Application Type
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;

            //Exam cycle
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;

            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            //Exam Name
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
        }

        function Validate() {

            if (!isSelected("<%=ddlSalutation.ClientID%>", "Salutation"))
                return false;
            if (!isSelected("<%=ddlGender.ClientID%>", "Gender"))
                return false;
            if (!isDate("<%=Txt_Dob.ClientID %>", "Invalid Date of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddloccupation.ClientID%>", "Occupation"))
                return false;
            if (!isSelected("<%=ddlCategory.ClientID%>", "Cast Category"))
                return false;
            if (!isBlank("<%=TxtRemark.ClientID%>", "Remark"))
                return false;
            var GuardianName = document.getElementById("<%=Txt_GName.ClientID %>").value;
            var FatherName = document.getElementById("<%=Txt_Fname.ClientID %>").value;
            var MotherName = document.getElementById("<%=Txt_Mname.ClientID %>").value;
            if ((GuardianName == "" && FatherName == "" && MotherName == "") || (GuardianName != "" && FatherName != "" && MotherName != "")) {
                callErrorMsg("<%=Txt_Fname.ClientID %>", "Please enter either (Father Name and Mother Name) OR  Guardian Name.");
                return false;
            }
            else if (FatherName != "" && MotherName == "") {
                callErrorMsg("<%=Txt_Mname.ClientID %>", "Please enter Mother Name.");
                return false;
            }
            else if (MotherName != "" && FatherName == "") {
                callErrorMsg("<%=Txt_Fname.ClientID %>", "Please enter Father Name.");
                return false;
            }
            return true;
        }
    </script>
    <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"></asp:Label>
    <div id="divfilter" runat="server">
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td>
                    <asp:DropDownList ID="ddlRc" runat="server" Height="22px" SkinID="ddl250">
                        <asp:ListItem Value="0">--All--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCourseCategry" runat="server" AutoPostBack="True" Height="22px"
                        OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" SkinID="ddl250">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" Height="22px"
                                OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" SkinID="ddl250">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="TrExamHead" runat="server">
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TrExamInput" runat="server" class="even">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" Height="22px"
                                OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" SkinID="ddl250">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" Height="22px"
                                OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" SkinID="ddl250">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="Tr1" runat="server">
                <td>
                    <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Filter By">
                    </asp:Label>
                </td>
                <td id="tdresultgradeheader" runat="server">
                    <asp:Label ID="lblTotal1" runat="server" SkinID="CaptionLabel" Text="Result Grade Type">
                    </asp:Label>
                </td>
            </tr>
            <tr id="Tr2" runat="server" class="even">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDownloadType" runat="server" Height="22px" AutoPostBack="true"
                        SkinID="ddl250" OnSelectedIndexChanged="ddlDownloadType_SelectedIndexChanged">
                        <asp:ListItem Value="0">Result Declared</asp:ListItem>
                        <asp:ListItem Value="1">Result Not Declared</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td id="tdresultgrade" runat="server">
                    <asp:DropDownList ID="ddlResultgradeType" runat="server" SkinID="ddl250">
                        <asp:ListItem Value="1">Passed</asp:ListItem>
                        <asp:ListItem Value="2">Other than Passed</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="33%" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Search By:-"></asp:Label>
                            <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                                Style="vertical-align: top; border: 0" BorderStyle="None" RepeatLayout="Flow"
                                AutoPostBack="true" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Roll.No.</asp:ListItem>
                                <asp:ListItem Value="1">Name.</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td width="33%" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="TxtSearch" runat="server" MaxLength="50" SkinID="txt756">
                            </asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Rdsearchby" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();"
                OnClick="btnView_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
    </div>
    <div id="divGrid" runat="server" visible="false">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                            <HeaderStyle Width="1%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:HyperLinkField HeaderText="Course" DataTextField="CourseName" SortExpression="CourseName"
                            DataNavigateUrlFields="ApplID,CourseID,ExamID,index" DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}">
                            <HeaderStyle Width="6%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.No." DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" DataTextField="Applno"
                            SortExpression="Applno" Target="_self">
                            <HeaderStyle Width="9%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.Date" DataTextField="date" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="date"
                            DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="17%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Student Name" DataTextField="StudentName" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="StudentName">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="FatherName" DataTextField="fname" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="fname">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="MotherName" DataTextField="mname" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="mname">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Guardian" DataTextField="guardian" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="guardian">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="RollNo" DataTextField="Rollno" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="Rollno">
                            <HeaderStyle Width="9%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="ResultGrade" DataTextField="grade" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}" SortExpression="grade">
                            <HeaderStyle Width="6%" />
                        </asp:HyperLinkField>
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divNavigation" runat="server">
            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="divedit" runat="server" visible="false">
        <table class="sample2" cellpadding="2" cellspacing="0">
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Salutation&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text=" Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label25" runat="server" SkinID="CaptionLabel" Text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <asp:DropDownList ID="ddlSalutation" runat="server" Height="22" SkinID="ddl250" Width="102px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlSalutation_SelectedIndexChanged">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        <asp:ListItem Value="1">Mr.</asp:ListItem>
                        <asp:ListItem Value="2">Ms.</asp:ListItem>
                        <asp:ListItem Value="3">Others</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="60" SkinID="txt248">
                    </asp:TextBox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlGender" runat="server" Height="22" SkinID="ddl250" Width="102px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSalutation" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="lblUserNameCaption1" runat="server" SkinID="CaptionLabel" Text="Father Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                        Width="100%"></asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label24" runat="server" SkinID="CaptionLabel" Text="Mother Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label26" runat="server" SkinID="CaptionLabel" Text="Guardian Name"></asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <%--<asp:TextBox ID="TxtGender" runat="server" SkinID="txt248" MaxLength="8"></asp:TextBox>--%>
                    <asp:TextBox ID="Txt_Fname" runat="server" MaxLength="60" SkinID="txt248">
                    </asp:TextBox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="Txt_Mname" runat="server" MaxLength="60" SkinID="txt248">
                    </asp:TextBox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="Txt_GName" runat="server" MaxLength="60" SkinID="txt248">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="DOB &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Occupation &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="Caste Category&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                    <asp:TextBox ID="Txt_Dob" runat="server" MaxLength="100" SkinID="txtDate">
                    </asp:TextBox>
                    <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                        height: 22px; vertical-align: top;" />
                    <asp:CalendarExtender ID="ceDOB" TargetControlID="Txt_Dob" PopupPosition="BottomLeft"
                        Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                    </asp:CalendarExtender>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:DropDownList ID="ddloccupation" runat="server" Height="22" SkinID="ddl250">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" EnableTheming="True" Height="22"
                        SkinID="ddl250" TabIndex="8">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Candidate Image&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td colspan="3" class="style1">
                    <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" TabIndex="37"
                        Width="360px" />
                    <br />
                    ( JPG,JPEG,GIF,PNG image with size upto 50 KB )
                </td>
            </tr>
            <tr>
                <td colspan="3" class="style1">
                    <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Remarks For Update&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" >
                   <asp:TextBox ID="TxtRemark" runat="server" MaxLength="200" SkinID="txt248" 
                        Width="360px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnSave" OnClientClick="return Validate();" runat="server" Text="Save"
                OnClick="SaveRecord" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
