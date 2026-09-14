<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="downloadImages_course.aspx.cs" Inherits="downloadImages" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Download Candidate Photographs
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
            var CourseId;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            else
                TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;

            //Exam cycle
            if (document.getElementById('<%=ddlExamCycle.ClientID %>')) {
                if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                    return false;
            }
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            //Exam Name
            var ExamId;
            if (document.getElementById('<%=ddlExamName.ClientID %>')) {
                if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                    return false;
                else
                    ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;
            }
            else
                ExamId = "0";

            // window.open("../HO/Rpt/ApplicationReceiptReport.aspx?StatusId=" + StatusId + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId + "&applicantTypeID=" + applicantTypeID);
        }
    </script>
    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
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
             <%--   <asp:updatepanel id="UpdatePanel3" runat="server">
                    <contenttemplate>--%>
                        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    <%--</contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>--%>
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
                <%--<asp:updatepanel id="UpdatePanel5" runat="server">
                    <contenttemplate>--%>
                        <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                   <%-- </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>--%>
            </td>
            <td>
                <%--<asp:updatepanel id="UpdatePanel4" runat="server">
                    <contenttemplate>--%>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                   <%-- </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>--%>
            </td>
            <td>
              <%--  <asp:updatepanel id="UpdatePanel7" runat="server">
                    <contenttemplate>--%>
                        <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" 
                            AutoPostBack="true" onselectedindexchanged="ddlExamYear_SelectedIndexChanged"
                            >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                   <%-- </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>--%>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:label id="Label9" runat="server" skinid="CaptionLabel" text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Download By &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="lblTotal1" runat="server" skinid="CaptionLabel" text="File Name Start With">
                </asp:label>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td>
               <%-- <asp:updatepanel id="UpdatePanel1" runat="server">
                    <contenttemplate>--%>
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    <%--</contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>--%>
            </td>
            <td>
               <%-- <asp:updatepanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
                      --%>
                          <asp:DropDownList ID="ddlDownloadType" runat="server" Height="22px" AutoPostBack="true"  
                             SkinID="ddl250" 
                            onselectedindexchanged="ddlDownloadType_SelectedIndexChanged"  >
                            <asp:ListItem Value="0">All Batches</asp:ListItem>
                             <asp:ListItem Value="1">Select Batches</asp:ListItem>
                        </asp:DropDownList>
                    <%--</contenttemplate>--%>
                   <%-- <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </triggers>--%>
                <%--</asp:updatepanel>--%>
            </td>
            <td>
                <asp:dropdownlist id="ddlNameFormat" runat="server" skinid="ddl250">
                    <asp:listitem value="1">Registration Number</asp:listitem>
                    <asp:listitem value="2">Roll Number</asp:listitem>
                </asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:updatepanel id="up2" runat="server">
                    <contenttemplate>
                <div id="divbatches" runat="server">
                <asp:checkboxlist id="chkbatchlist" runat="server"  repeatdirection="Horizontal"
                    width="100%" cellpadding="1" cellspacing="0" enabletheming="False" font-size="8pt">
                </asp:checkboxlist>
                </div>
                 </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDownloadType" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <div style="float:left;background:lightcoral;color:black">
            <asp:label runat="server" id="lblImageDetailsCount" cssclass="error" width="100%"></asp:label>
            <%--<label id="lblImageDetailsCount" runat="server"/>--%>
        </div>
        <div style="float:right">
        <asp:button id="btnView" runat="server" text="Download Images" onclientclick="return OpenWindow();"
            onclick="btnView_Click" />
      
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
        </div>
        </div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
