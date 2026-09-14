<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CertificateGuardain.aspx.cs"
    Inherits="HO_CertificateGuardain" MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>


<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Certificate Exam Guardian Data
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
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
    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <div id="divfilter" runat="server">
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:label id="Label1" runat="server" skinid="CaptionLabel" text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label5" runat="server" skinid="CaptionLabel" text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label6" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:dropdownlist id="ddlRc" runat="server" height="22px" skinid="ddl250">
                    <asp:listitem value="0">--All--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <asp:dropdownlist id="ddlCourseCategry" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlCourseCategry_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel3" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label3" runat="server" skinid="CaptionLabel" text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <asp:updatepanel id="UpdatePanel5" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel4" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel7" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" 
                            AutoPostBack="true" onselectedindexchanged="ddlExamYear_SelectedIndexChanged"
                            >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:label id="Label9" runat="server" skinid="CaptionLabel" text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Filter By">
                </asp:label>
            </td>
            <td id="tdresultgradeheader" runat="server">
                <asp:label id="lblTotal1" runat="server" skinid="CaptionLabel" text="Result Grade Type">
                </asp:label>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td>
                <asp:updatepanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:DropDownList ID="ddlDownloadType" runat="server" Height="22px" AutoPostBack="true"  
                    SkinID="ddl250" 
                onselectedindexchanged="ddlDownloadType_SelectedIndexChanged"  >
                <asp:ListItem Value="0">Result Declared</asp:ListItem>
                    <asp:ListItem Value="1">Result Not Declared</asp:ListItem>
            </asp:DropDownList>
            </td>
            <td id="tdresultgrade" runat="server">
                <asp:dropdownlist id="ddlResultgradeType" runat="server" skinid="ddl250">
                    <asp:listitem value="1">Passed</asp:listitem>
                    <asp:listitem value="2">Other than Passed</asp:listitem>
                </asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td width="33%" colspan="3">
                <asp:updatepanel id="UpdatePanel15" runat="server">
                    <contenttemplate>
                            <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Search By:-"></asp:Label>
                            <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                                Style="vertical-align: top; border: 0" BorderStyle="None" RepeatLayout="Flow"
                                AutoPostBack="true" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Roll.No.</asp:ListItem>
                                <asp:ListItem Value="1">Name.</asp:ListItem>
                            </asp:RadioButtonList>
                        </contenttemplate>
                </asp:updatepanel>
            </td>
        </tr>
        <tr>
            <td width="33%" colspan="3">
                <asp:updatepanel id="UpdatePanel17" runat="server">
                    <contenttemplate>
                <asp:textbox id="TxtSearch" runat="server" maxlength="50" skinid="txt756">
                </asp:textbox>
                </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="Rdsearchby" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:button id="btnView" runat="server" text="View" onclientclick="return OpenWindow();"
            onclick="btnView_Click" />
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" /></div>
    </div>
    <div id="divGrid" runat="server" visible="false">
        <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
            <contenttemplate>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                            <HeaderStyle Width="1%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:HyperLinkField HeaderText="Course" DataTextField="CourseName" SortExpression="CourseName"
                            DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}">
                            <HeaderStyle Width="6%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.No." DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            DataTextField="Applno" SortExpression="Applno" Target="_self">
                            <HeaderStyle Width="9%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.Date" DataTextField="date" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="date" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="17%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Student Name" DataTextField="StudentName" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="StudentName">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderText="FatherName" DataTextField="fname" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="fname">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderText="MotherName" DataTextField="mname" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="mname">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Guardian" DataTextField="guardian" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="guardian">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderText="RollNo" DataTextField="Rollno" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="Rollno">
                            <HeaderStyle Width="9%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="ResultGrade" DataTextField="grade" DataNavigateUrlFields="ApplID,CourseID,ExamID,index"
                            DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&ExamID={2}&index={3}"
                            SortExpression="grade">
                            <HeaderStyle Width="6%" />
                        </asp:HyperLinkField>
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
            </contenttemplate>
        </asp:updatepanel>
        <div id="divNavigation" runat="server">
            <asp:updatepanel rendermode="Inline" id="uPnlNavigation" updatemode="Conditional"
                runat="server">
                <contenttemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </contenttemplate>
            </asp:updatepanel>
        </div>
    </div>

    <div id="divedit" runat="server" visible="false">
        <table class="sample2" cellpadding="2" cellspacing="0">
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label18" runat="server" skinid="CaptionLabel" text="Salutation&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label17" runat="server" skinid="CaptionLabel" text=" Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label25" runat="server" skinid="CaptionLabel" text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <asp:dropdownlist id="ddlSalutation" runat="server" height="22" skinid="ddl250" width="102px"
                        autopostback="True" onselectedindexchanged="ddlSalutation_SelectedIndexChanged">
                        <asp:listitem value="0">--Select One--</asp:listitem>
                        <asp:listitem value="1">Mr.</asp:listitem>
                        <asp:listitem value="2">Ms.</asp:listitem>
                    </asp:dropdownlist>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:textbox id="txtName" runat="server" maxlength="60" skinid="txt248">
                    </asp:textbox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:updatepanel id="UpdatePanel10" runat="server">
                        <contenttemplate>
                                <asp:DropDownList ID="ddlGender" runat="server" Height="22" SkinID="ddl250" Width="102px"
                                    >
                                </asp:DropDownList>
                            </contenttemplate>
                        <triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSalutation" EventName="SelectedIndexChanged" />
                            </triggers>
                    </asp:updatepanel>
                </td>
            </tr>
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:label id="lblUserNameCaption1" runat="server" skinid="CaptionLabel" text="Father Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                        width="100%"></asp:label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label24" runat="server" skinid="CaptionLabel" text="Mother Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label26" runat="server" skinid="CaptionLabel" text="Guardian Name"></asp:label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <%--<asp:TextBox ID="TxtGender" runat="server" SkinID="txt248" MaxLength="8"></asp:TextBox>--%>
                    <asp:textbox id="Txt_Fname" runat="server" maxlength="60" skinid="txt248">
                    </asp:textbox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:textbox id="Txt_Mname" runat="server" maxlength="60" skinid="txt248">
                    </asp:textbox>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:textbox id="Txt_GName" runat="server" maxlength="60" skinid="txt248">
                    </asp:textbox>
                </td>
            </tr>
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label22" runat="server" skinid="CaptionLabel" text="DOB &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:label id="Label21" runat="server" skinid="CaptionLabel" text="Occupation &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
                <td>
                    <asp:label id="Label20" runat="server" skinid="CaptionLabel" text="Caste Category&lt;b class='mandatory'&gt;*&lt;/b&gt;">
                    </asp:label>
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                    <asp:textbox id="Txt_Dob" runat="server" maxlength="100" skinid="txtDate">
                    </asp:textbox>
                    <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                        height: 22px; vertical-align: top;" />
                    <asp:calendarextender id="ceDOB" targetcontrolid="Txt_Dob" popupposition="BottomLeft"
                        format="dd-MMM-yyyy" popupbuttonid="imgDob" runat="server">
                    </asp:calendarextender>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:dropdownlist id="ddloccupation" runat="server" height="22" skinid="ddl250">
                    </asp:dropdownlist>
                </td>
                <td>
                    <asp:dropdownlist id="ddlCategory" runat="server" enabletheming="True" height="22"
                        skinid="ddl250" tabindex="8">
                    </asp:dropdownlist>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:button id="btnSave" onclientclick="return Validate();" runat="server" text="Save"
                onclick="SaveRecord" />
            <asp:button id="btnCancel" runat="server" text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:View>
    </div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
