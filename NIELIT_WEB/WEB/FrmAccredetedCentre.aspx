<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmAccredetedCentre.aspx.cs" Debug="false" Inherits="FrmAccredetedCentre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text=""></asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (document.getElementById("<%=DdlcourseCategory.ClientID %>").disabled == false) {
                if (!isSelected("<%=DdlcourseCategory.ClientID %>", "Course Category"))
                    return false;
            }
            if (document.getElementById("<%=DdlcourseName.ClientID %>").disabled == false) {
                if (!isSelected("<%=DdlcourseName.ClientID %>", "Course Name"))
                    return false;
            }
            if (!isSelected("<%=Ddlstate.ClientID %>", "State Name"))
                return false;

        }
    </script>
    <asp:Label ID="Lblerror" Width="99%" class="error" EnableTheming="false" Visible="false"
        runat="server" Text=""></asp:Label>
    <div id="divfilter" runat="server">
        <table width="100%" class="sample3" id="Tblacc" runat="server">
            <tr class="gdalternate1">
                <td align="left" width="30%">
                    Course Category
                </td>
                <td>
                    <asp:DropDownList ID="DdlcourseCategory" runat="server" Width="536px" OnSelectedIndexChanged="DdlcourseCategory_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="gdrow1">
                <td align="left" width="30%">
                    Course Name
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DdlcourseName" runat="server" Width="536px">
                                <asp:ListItem Text="---Select One---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DdlcourseCategory" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" width="30%">
                    Accredited Institute Status
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="Ddlstatus" runat="server" AutoPostBack="False" Width="536px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DdlcourseCategory" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdrow1">
                <td align="left" width="30%">
                    State
                </td>
                <td align="left" width="69%">
                    <asp:DropDownList ID="Ddlstate" runat="server" Width="536px" AutoPostBack="True"
                        OnSelectedIndexChanged="Ddlstate_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" width="30%">
                    District
                </td>
                <td align="left" width="69%">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="Ddldistrict" runat="server" Width="536px" AutoPostBack="True">
                                <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Ddlstate" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px; margin-right: 3px;" id="divfooter"
            runat="server">
            <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" OnClientClick="return ValidateFormFields();" />
            <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" />
        </div>
    </div>
    <div id="divresult" runat="server" visible="false">
        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
            <HeaderTemplate>
                <table cellpadding="0" cellspacing="4" style="background-color: #ffffff;" width="100%"
                    class="sample3">
                    <tr class="head1">
                        <th align="left" style="margin-right: 10px;">
                            List Of Accredited Institutes
                        </th>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left">
                            <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td width="100%">
                        <div class="Course_block">
                            <div>
                                <%#DataBinder.Eval(Container,"DataItem.Name" )%>
                            </div>
                            <table cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td rowspan="5" valign="top" width="65%">
                                        <asp:Label ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.AddressLine1" )%>'></asp:Label>
                                        Phone:
                                        <%#DataBinder.Eval(Container, "DataItem.StdNumber")%>-<%#DataBinder.Eval(Container, "DataItem.PhoneNumber1")%>,
                                        Mobile-No:
                                        <%#DataBinder.Eval(Container, "DataItem.MobileNumber")%>
                                        <br />
                                        E-Mail:
                                          <%#CommonFunctions.ChangeEmailDisplay(Convert.ToString (DataBinder.Eval(Container, "DataItem.EmailAddress1")))%>, Fax-No:
                                        <%#Convert.ToString(DataBinder.Eval(Container, "DataItem.FaxNumber"))%>
                                    </td>
                                    <td width="17%">
                                        Course Category
                                    </td>
                                    <td width="17%" colspan="2">
                                        :<%# DataBinder.Eval(Container, "DataItem.Category")%></td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                        Course Name
                                    </td>
                                    <td width="20%">
                                        :<%# DataBinder.Eval(Container, "DataItem.CName")%></td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                        Accreditation Number
                                    </td>
                                    <td width="20%">
                                        :<%# DataBinder.Eval(Container, "DataItem.AccNo")%></td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                        Current Status
                                    </td>
                                    <td width="20%">
                                        :<%# DataBinder.Eval(Container, "DataItem.Status")%></td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                        Valid Upto
                                    </td>
                                    <td width="20%">
                                        :<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.validity"))==DateTime.MinValue?"--":Convert.ToDateTime(DataBinder.Eval(Container,"DataItem.Validity")).ToString("dd-MMM-yyyy")%></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div align="right">
            <asp:Button ID="BtnBack" runat="server" Text="Back to Search" OnClick="BtnBack_Click"
                Style="margin-right: 3px;" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
    <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
