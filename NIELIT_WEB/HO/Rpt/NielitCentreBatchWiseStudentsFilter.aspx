<%@ Page Title="NIELIT Center BatchWise Students Details Report" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="NielitCentreBatchWiseStudentsFilter.aspx.cs" Inherits="NielitCentreBatchWiseStudentsFilter" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    NIELIT Center BatchWise Students Details Report
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <style type="text/css">
        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: Courier New;
            border: none;
            height: 20px;
            padding-left: 90px;
        }
    </style>
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">


        function OpenWindow() {

            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                        return false;
                    if (!isSelected("<%=ddlCourseCat.ClientID %>", "Course Category"))
                        return false;
                    if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                        return false;
            if (!isSelected("<%=ddlBatchCode.ClientID %>", "Batch Code"))
                return false;

                    if (document.getElementById('<%=ddlCentreName.ClientID %>')) {
                        if (!isSelected("<%=ddlCentreName.ClientID %>", "Please Select Centre Name"))
                    return false;
            }

            var centreID;
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreID = document.getElementById('<%=ddlCentreName.ClientID %>').value;

                var courseCat;
                if (!isSelected("<%=ddlCourseCat.ClientID %>", "Course Category"))
                return false;
            else
                courseCat = document.getElementById('<%=ddlCourseCat.ClientID %>').value;

            var courseID;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                courseID = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            var batchId;
            if (!isSelected("<%=ddlBatchCode.ClientID %>", "Batch Code"))
                return false;
            else
                batchId = document.getElementById('<%=ddlBatchCode.ClientID %>').value;


            var url = "NielitCentreBatchWiseStudentsRep.aspx?centreID=" + centreID + "&courseCat=" + courseCat + "&courseID=" + courseID + "&batchId=" + batchId;
            window.open(url);

            return false;
        }
 </script>

      <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 34%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Catogory &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
         
        <tr class="even">
            <td style="width: 34%;" valign="top">
                <asp:DropDownList ID="ddlCentreName" runat="server" Style="width: 270px;" AutoPostBack="True"  
                    OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged" EnableTheming="True">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlCentreName"
                    PromptText="  Select Centre Name"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
            </td>
            <td style="width: 33%;" valign="top" >

                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                <asp:DropDownList ID="ddlCourseCat" runat="server" Style="width: 270px;" AutoPostBack="True"  
                    OnSelectedIndexChanged="ddlCourseCat_SelectedIndexChanged" EnableTheming="True">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlCourseCat"
                    PromptText="  Select Course Category"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
                                </ContentTemplate>
                        </asp:UpdatePanel>
            </td>

            <td style="width: 33%;" valign="top" >
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" Height="22px" Style="width: 270px;" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlCourseName"
                                     PromptText="  Select Course Name"
                                     PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                                 </asp:ListSearchExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>

            <td width="33%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Batch Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                
            </td>
            <td style="width: 33%;" valign="top">
                
            </td>

        </tr>
        <tr>
                        <td style="width: 33%;" valign="top" >
                <asp:UpdatePanel ID="UpdatePanel121" runat="server">
                            <ContentTemplate>
                <asp:DropDownList ID="ddlBatchCode" runat="server" Style="width: 270px;" AutoPostBack="True"  
                    EnableTheming="True" >
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlBatchCode"
                    PromptText="  Select Batch Code"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
                                </ContentTemplate>
                        </asp:UpdatePanel>
            </td>

            <td width="33%">

            </td>
        </tr>

    </table>
<div style="text-align: right; margin-top: 10px">
    <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
 <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>

