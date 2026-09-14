<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BulkCertificateAdmitCard.aspx.cs"
    Inherits="Admin_BulkCertificateAdmitCard" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Download Admit Cards"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:updatepanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:updatepanel>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {


            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
        }
    </script>
    <style type="text/css">
        .trAdmitCard
        {
            text-align: left;
            padding-left: 5px;
            border: 1 solid #000000;
        }
    </style>
    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:label id="LblcourseCategory" runat="server" skinid="CaptionLabel" text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Lblcourse" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label6" runat="server" skinid="CaptionLabel" text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:dropdownlist id="ddlCourseCategry" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlCourseCategry_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel3" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlCourseName" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlCourseName_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel5" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlAppType" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlAppType_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                 </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:label id="Lblexamcycle" runat="server" skinid="CaptionLabel" text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Lblexamyear" runat="server" skinid="CaptionLabel" text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label7" runat="server" skinid="CaptionLabel" text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr runat="server" class="even">
            <td>
                <asp:updatepanel id="UpdatePanel4" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlExamCycle" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlExamCycle_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                  </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel7" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlExamYear" runat="server" skinid="ddl250" autopostback="true"
                    onselectedindexchanged="ddlExamYear_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                  </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlExamName" runat="server" skinid="ddl250" autopostback="true" 
                            onselectedindexchanged="ddlExamName_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                  </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
    </table>
    <asp:updatepanel id="UpdatePanel13" runat="server">
        <contenttemplate>
    <asp:label runat="server" text="" id="lblcount" cssclass="error" 
        EnableTheming="False" width="99%" visible="false"></asp:label>
    </contenttemplate>
        <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </triggers>
    </asp:updatepanel>
    <div style="text-align: right; margin-top: 10px">
        <asp:button runat="server" text="Download" id="btnDownload" onclick="btnDownload_Click"
            onclientclick="return validatefilter();" />
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
    </div>
    <asp:panel runat="server" style="display: none;" id="pnladmitcard">
    </asp:panel>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
