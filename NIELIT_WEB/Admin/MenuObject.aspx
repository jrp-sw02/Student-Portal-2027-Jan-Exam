<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" enableEventValidation="false"  AutoEventWireup="true"
    CodeFile="MenuObject.aspx.cs" Inherits="Admin_MenuObject" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Menu Objects"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="BtnMode_Click" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                    <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                        <tr>
                            <td>
                               <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                    Text="" onclick="btnReset_Click" />
                                <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                    Text="" onclick="btnFilter_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserType" Width="100%" runat="server" Text="Menu Object Type"></asp:Label>
                                <asp:DropDownList ID="ddlSearchObjectType" Width="100%" runat="server" >
                                    <asp:ListItem Value="0" Text ="--Select One--" ></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language ="javascript" type="text/javascript">
                        var box = $('#filterBox');
                        shortcut.add("Ctrl+Shift+F", function () {
                            box.show();
                        });
                        shortcut.add("Esc", function () {
                            box.hide();
                        });
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="LnkBtnResetSearch" 
        SearchTextToolTip="Search by object name or parent name" OnLnkBtnGO="LnkBtnGO" 
        runat="server" AutoCompleteFirstRowSelected="True" 
        AutoCompleteMinimumPrefixLength="1" 
        AutoCompleteServiceMethod="GetSearchText" AutoCompleteCompletionSetCount="10" /> 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">

    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language ="javascript" type="text/javascript">
    function ValidateObject() {
        if (!isSelected("<%=ddlObjectType.ClientID %>", "Object type"))
            return false;
        if (document.getElementById("<%=ddlObjectType.ClientID %>").value != "<%=Convert.ToInt16(EConnect.URM.enmMenuObjectType.Project) %>") 
            if(!isBlank("<%=txtParent.ClientID %>","Parent object name"))
                return false;
        if (!isBlank("<%=txtObjectName.ClientID %>", "Object name"))
            return false;
        if (!isBlankNumber("<%=txtOrderNo.ClientID %>", "Order of apearance"))
            return false;
        if (!isBlank("<%=txtURL.ClientID %>", "Form URL"))
            return false;
        return true;
    }
    var SelectedChk = null;
    function CheckSelected() {
        if(!isSelected("<%=ddlObjectType.ClientID %>","Object type"))
            return false;
        if (document.getElementById("<%=ddlObjectType.ClientID %>").value == "<%=Convert.ToInt16(EConnect.URM.enmMenuObjectType.Project) %>") {
            alert("Selected menu object type does not have any parnet object");
            return false;
        }
    }
    function SelectOne(chk) {
        if(SelectedChk!=null)
            SelectedChk.checked = false;
        if (chk.checked == true) {
            SelectedChk = chk;
        }
        else {
            SelectedChk = null;
        }
    }
    function CheckChecked() {
        if (SelectedChk == null) {
            $find("ModalPopupExtender1").show();
            alert("Please select parent object form the list");
            return false;
        }
    }
    var imgDetail = null;  
function GetDetails(ctl, args)
    { 
        imgDetail = ctl;
        if(args==null)
        {
            if(document.getElementById("<%=hfParentId.ClientID %>").value !="")
            {
                args = "GetDetail$" + document.getElementById("<%=hfParentId.ClientID %>").value;
                <%= ClientScript.GetCallbackEventReference(this,"args", "ShowDetail", null) %>;
            }
        }
        else
        {
            args = "GetDetail$"+args;
            <%= ClientScript.GetCallbackEventReference(this,"args", "ShowDetail", null) %>;
        }
    }
    var divTag = document.createElement("div");
    divTag.id = "divDyn";
    divTag.style.width="300px";
    divTag.style.textAlign="left";
    divTag.style.padding = "5px 5px 5px 5px";
    divTag.style.borderRadius = "1em";
    divTag.style.boxShadow  = "10px 10px 20px #000";
    divTag.style.MozBorderRadius  = "1em";
    divTag.style.MozBoxShadow  = "10px 10px 20px #000";
    divTag.style.WebkitBorderRadius  = "1em";
    divTag.style.WebkitBoxShadow  = "10px 10px 20px #000";
    divTag.style.visibility="visible";
    divTag.style.position= "absolute"; 
    divTag.style.zIndex= "999";
    divTag.style.backgroundColor="#ccff99";
    divTag.style.border="solid 2px SteelBlue";
    divTag.style.color="Navy";
    function ShowDetail(eventArgument ,context)
    { 
        var pos = findPos(imgDetail);
        divTag.style.left=pos[0]-315+"px";
        divTag.style.top=pos[1]-7+"px";
        divTag.innerHTML = "<table  style='width:100%;text-align:left' cellpadding='2'>" +
                            "<tr><td style='width:100%; text-align:center; background-color:SteelBlue; text-transform: uppercase; color:white; font-size:10pt' ><b>Parent Object Detail</b></td></tr>" +
                            "<tr><td style='width:100%; text-align:left'>"+eventArgument+"</td></tr>" +
                            "</table>"
        document.body.appendChild(divTag);
    }
    function HideDetsils()
    {
        try
        {
            document.body.removeChild(divTag);
        }
        catch (ex)
        {
        }
         
    }
var dtgp= "<%= gvMain.ClientID %>"
function CheckAll(Sender, CheckBoxName)
{
    CheckUncheckAll(dtgp, Sender, CheckBoxName)
}
function PerformAction(obj, tableid) { 
        document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
    ShowHideMenu(obj,tableid);
    }
</script>
    
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="lbDelteteOne_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" AllowPaging="True" DataKeyNames="ID,OrganizationID" PagerSettings-Visible="false"
                            runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="3%"  HeaderText="#" />
                                <asp:HyperLinkField HeaderStyle-Width="27%" DataNavigateUrlFields="ID,OrganizationID"
                                    DataNavigateUrlFormatString="?Key={0}&OrgId={1}" DataTextField="Name" HeaderText="Menu Object Name"
                                    SortExpression="Name" Target="_self" />
                                <asp:BoundField HeaderStyle-Width="15%" NullDisplayText="--" DataField="MenuObjectType"
                                    HeaderText="Object Type" SortExpression="MenuObjectType" />
                                <asp:BoundField HeaderStyle-Width="27%" NullDisplayText="--" DataField="ParentMenuObject"
                                    HeaderText="Parent Object Name" SortExpression="ParentMenuObject" />
                                <asp:BoundField HeaderStyle-Width="15%" NullDisplayText="--" DataField="ParentMenuObjectType"
                                    HeaderText="Parent Type" SortExpression="ParentMenuObjectType" />
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/tree_view.png">
                                    <ItemTemplate>
                                        <asp:Image ToolTip="Click to view Object Hierarchy Details" runat="server" ID="imgView"
                                            ImageUrl="~/images/tree_view.png" AlternateText='<%#Eval("ID") %>' ImageAlign="Middle"
                                            Width="16px" Height="16px" Style="cursor: pointer;" onmouseout="HideDetsils()"
                                            onclick='GetDetails(this,this.alt)' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ToolTip="Action" ClientIDMode="Static" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server"  
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="0" cellspacing ="1">
                <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" Text="Object Type <b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="lblParentCaption" runat="server" Text="Is Parent Form" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" Text="Parent Object <b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList  ID="ddlObjectType" SkinID="ddl250" runat="server" 
                            AutoPostBack="True" onselectedindexchanged="ddlObjectType_SelectedIndexChanged" 
                            >
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:UpdatePanel runat="server" ID="up1">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlIsParent" SkinID="ddl250" runat="server" Enabled="False" 
                                    AutoPostBack="True" onselectedindexchanged="ddlIsParent_SelectedIndexChanged">
                                    <asp:ListItem Value ="0" Text = "No" ></asp:ListItem>
                                    <asp:ListItem Value ="1" Text = "Yes" Selected ="True"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlObjectType" EventName ="SelectedIndexChanged" />
                                <asp:PostBackTrigger ControlID="ddlIsParent" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:TextBox SkinID="txt210" ID="txtParent" style="cursor:pointer" onmouseout="HideDetsils()" onclick="GetDetails(this)"  onkeypress="return false;" onkeydown="return false;" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hfParentId" Value="" runat="server" />
                                    <asp:Button Width="0px" Height="0px" ID="btnPopup" runat="server" style="display:none;" />
                                    <asp:ImageButton ID="imgPopup" OnClientClick="return CheckSelected();" ClientIDMode="Static" ToolTip="Search Parent Object" 
                                    ImageUrl="~/App_Themes/Blue/Images/search_button_02.png" ImageAlign="Middle" 
                                    style="margin-left:3px; border: none 0px" runat="server" 
                                    onclick="imgSearch_Click" />
                                    <asp:ModalPopupExtender OnOkScript="CheckChecked();" ClientIDMode="Static"  TargetControlID ="btnPopup" BackgroundCssClass="modalBackground" ID="ModalPopupExtender1" runat="server" CancelControlID="btnCancel"  OkControlID="btnOk" PopupControlID="pnlObjects"  RepositionMode="RepositionOnWindowScroll" ViewStateMode="Enabled">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="pnlObjects" runat="server"  widht="700px" Height="350px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px" BorderColor="Navy" BackColor="White">
                                        <table cellpadding="1" cellspacing="0" width="600px"  class="sample3"  >
                                            <tr class="head1">
                                                <td colspan="2" align="left" valign="top" style="color: White">
                                                Parent Object Browser: Please select parent object form the list below
                                                </td>
                                            </tr>
                                          <%--  <tr>
                                                <td width="50%" align="left"></td>
                                                <td width="50%" align="left"></td>
                                            </tr>--%>
                                            <tr class="sample2">
                                                <td colspan="2" align="center" valign="top">
                                                    <div style="overflow:auto;height:290px; width:95%; text-align:left;">
                                                        <asp:TreeView ID="tvParents" Width="100%" runat="server" 
                                                            onselectednodechanged="tvParents_SelectedNodeChanged">
                                                        </asp:TreeView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="sample2">
                                                <td colspan="2" align="right" valign="top">
                                                    <asp:Button ID="btnOk" style="display:none;" runat="server" Text="Select" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>    
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" Text="Object Name <b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label5" runat="server" Text="Abbreviation" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label4" runat="server" Text="Order Of Appearance<b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">    
                    <td style="width:33%;" valign="top">
                        <asp:TextBox SkinID="txt248" ID="txtObjectName" MaxLength="50" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtAbr" MaxLength="10" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtOrderNo" onblur="checkDot(this);" onkeypress="checkNumber(this,3,0);" MaxLength="3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trForm" runat="server" visible ="false" >  
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label13" runat="server" Text="Form URL<b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>  
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" Text="Open in New Window<b class='mandatory'>*</b>" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label12" runat="server" Text="New Window Parameters" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr id="trForm1" runat="server" visible ="false" class="even">  
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtURL" MaxLength="200" runat="server"></asp:TextBox>
                    </td>  
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList ID="ddlIsNewWindow" SkinID="ddl250" runat="server" >
                            <asp:ListItem Value ="0" Text = "No"  Selected ="True"></asp:ListItem>
                            <asp:ListItem Value ="1" Text = "Yes"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtParameters" MaxLength="300" runat="server"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>  
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label6" runat="server" Text="Icon Image Path" SkinID="CaptionLabel"></asp:Label>
                    </td>  
                    <td style="width:33%;" valign="top" >
                        <asp:Label ID="Label7" runat="server" Text="Thumbnail Image Path" SkinID="CaptionLabel"></asp:Label>
                    </td> 
                    <td style="width:33%;" valign="top" >
                         
                    </td>
                </tr>
                <tr class="even">  
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtIconPath" MaxLength="100" runat="server"></asp:TextBox>
                    </td>  
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox SkinID="txt248" ID="txtThumbPath" MaxLength="100" runat="server"></asp:TextBox>
                    </td> 
                    <td style="width:33%;" valign="top" >
                         
                    </td>
                </tr>
            </table>
             <div style="text-align:right; margin-top:10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateObject();" runat="server" 
                            Text="Save" onclick="SaveRecord" />
                        <asp:Button ID="Button1" runat="server" 
                            Text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
