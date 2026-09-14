<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="NielitCentreBatchwiseFinancialRepFilter.aspx.cs" Inherits="HO_NielitCentreBatchwiseFinancialRepFilter" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Nielit Centre Batchwise Financial Report Filter 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function OpenWindow() {
            // nielit centre id 
            var CtId;
            if (document.getElementById('<%=ddlCentreName.ClientID %>').value != "0")
                CtId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
            else {
                alert("Please Select NIELIT Centre Name !");
                return false;
            }

            // course category id 
            var CourseCatId;
            if (document.getElementById('<%=ddlcoursecategory.ClientID %>').value != "0")
                CourseCatId = document.getElementById('<%=ddlcoursecategory.ClientID %>').value;
            else {

                alert("Please Select Course Category !");
                return false;
            }
            // course id ddlbatchname
            var CourseId;
            if (document.getElementById('<%=ddlcourseName.ClientID %>').value != "0")
                CourseId = document.getElementById('<%=ddlcourseName.ClientID %>').value;
            else {

                alert("Please Select Course !");
                return false;
            }
            // batch name
            var BatchId;
            if (document.getElementById('<%=ddlbatchname.ClientID %>').value != "0")
                BatchId = document.getElementById('<%=ddlbatchname.ClientID %>').value;
            else {

                alert("Please Select Batch Code !");
                return false;
            }

            // Intitute name or sub centre name
            var centreType;


            var InstId;
            if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value == "0") {
                if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlSubcentreName.ClientID %>').value;
                    centreType = "S";
                }

                if (document.getElementById('<%=ddlCentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
                    centreType = "C";
                }
            }
            else {
                if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlSubcentreName.ClientID %>').value;
                    centreType = "S";
                }

            }

            //View report                       
            window.open("NielitCentreBatchwiseFinancialRep.aspx?CourseId=" + CourseId + "&InstId=" + InstId + "&CourseCatId=" + CourseCatId + "&BatchId=" + BatchId + "&centreType=" + centreType, 'Financial Report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
            return false;
        }
    </script>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td colspan="2" style="width: 66%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                            TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                            Style="height: 27px" Font-Bold="True">
                            <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                            <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="NIELIT Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>



            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Sub Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>

        </tr>
        <tr class="even">

            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSubcentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlSubcentreName_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlcourseName" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcourseName_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Batch Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                </asp:UpdatePanel>

            </td>
            <td>&nbsp;</td>

        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="NIELITCentreId" runat="server" />
            <asp:HiddenField ID="HNANFL" runat="server" />
            <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
