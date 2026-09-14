<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="NielitCentreStudentCompletion.aspx.cs" Inherits="HO_NielitCentreStudentCompletion" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Nielit Centre Student BatchWise Completion Updates
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td colspan="2" style="width: 66%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">&nbsp;</td>
        </tr>
        <tr class="even">
            <td colspan="2" style="width: 66%;" valign="top">
                <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top"></td>
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
                            <asp:ListItem Value="1">Accredited Centre</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Centre</asp:ListItem>
                            <asp:ListItem Value="2">Nielit Centre</asp:ListItem>
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
            <td colspan="3">
                <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>

        </tr>
        <tr class="even">
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>


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
        <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />

    </div>
    <div>
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGridStudent" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdStudent" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdStudent_RowDataBound">
                    <Columns>

                        <asp:BoundField DataField="number" HeaderText="Ref No." />
                        <asp:BoundField DataField="name" HeaderText="Name" />
                        <asp:BoundField DataField="Father_Name" HeaderText="Father Name" />
                        <asp:BoundField DataField="mother_name" HeaderText="Mother Name" />
                        <asp:BoundField DataField="DOB" HeaderText="Date of Birth" />

                        <asp:TemplateField HeaderText="Course Complete">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbCompleted" runat="server" Checked='<%# Eval("whetherCourseComplete") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Certificate Issued">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbCertIssued" runat="server" Checked='<%# Eval("whetherCertificateIssued") %>' />
                                <br />
                                <%--<asp:TextBox ID="txtCertIssueDate" runat="server" Text='<%# Eval("certificateIssueDate") %>'></asp:TextBox>--%>
                                <%--<asp:TextBox ID="txtCertIssueDate" runat="server" Text='<%# Eval("certificateIssueDate") %>' type="date"></asp:TextBox>--%>
                                <asp:TextBox ID="txtCertIssueDate" runat="server" TextMode="Date" Text='<%# Eval("certificateIssueDate") %>' 
              />


                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Drop Out">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDropOut" runat="server" Checked='<%# Eval("whetherDropOut") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
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
        <asp:Button ID="btnSave" runat="server" Text="Save Options" OnClick="btnSave_Click" />
        <asp:Button ID="btnResetGrid" runat="server" Text="Reset Options" OnClick="btnResetOptions_Click" />
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
