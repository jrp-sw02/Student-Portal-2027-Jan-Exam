<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="VirtualAcademyTentativeStartDate.aspx.cs" Inherits="Admin_VirtualAcademyTentativeStartDate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Virtual Academy Tentative Start Date"></asp:Label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcourseNameF" Width="100%" runat="server" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourseNameF" Width="100%" runat="server"  AutoPostBack="True">                                          
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                                </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategoryF" Width="100%" runat="server" AutoPostBack="True"
                                             OnSelectedIndexChanged="ddlcoursecategoryF_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                      --%>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td> 


                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="IsActive Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlIsActiveF" Width="100%" runat="server">
                                             <asp:ListItem Value="99" Text="--ALL--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                             <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:ListItem Value="99999" Text="--All--"></asp:ListItem>                                            
                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>

                                        
                                        
                                        <asp:Label ID="lblcourseNameF" Width="100%" runat="server" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourseNameF" Width="100%" runat="server"  AutoPostBack="True">                                          
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                                </ContentTemplate>
                                        </asp:UpdatePanel>    
                                                   --%>                   
                                    </td>
                                </tr>
                                                                                              
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Activity Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
   
      <script language="javascript" type="text/javascript">
          
         
</script> 
    
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (document.getElementById("<%=ddlCourseName.ClientID %>").disabled == false) {
                if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                    return false;
            }
            if (document.getElementById("<%=ddlIsActive.ClientID %>").disabled == false) {
                if (!isSelected("<%=ddlIsActive.ClientID %>", "IsActive"))
                    return false;
            }


            if (!isBlankDate("<%=txtTentativeStartDate.ClientID %>", "Tentative Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtTentativeStartDate.ClientID %>", "Invalid Tentative Start Date", "dd-MMM-yyyy"))
                return false;
            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                         <%--<table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                              
                            <tr>
                                <td align="left">
                                   
                                     <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                                 
                        </table>--%>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" 
                            Width="100%">
                            <Columns>
                               <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField 
                                    DataTextField="courseName" HeaderText="Course Name" SortExpression="Name"
                                    Target="_self" >
                                <ItemStyle Width="13%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                               
                               
                                <asp:HyperLinkField  
                                    DataTextField="TentativeStartDate" HeaderText="Tentative Start Date" SortExpression="TentativeStartDate" 
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}" >
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                 <asp:HyperLinkField  
                                    DataTextField="IsActive" HeaderText="Is_Active" SortExpression="IsActive"   
                                    Target="_self" >
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif" visible="false" >
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Tentative Start Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;" >
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel"  Text="IsActive&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" >
                        <asp:DropDownList ID="ddlCourseName" runat="server" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    
                                        <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtTentativeStartDate" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="img1" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                            vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtTentativeStartDate" PopupPosition="BottomLeft"
                            Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                        </asp:CalendarExtender>
                    </td>




                    <td valign="top" style="width: 33%;">

                        <asp:DropDownList ID="ddlIsActive" runat="server" SkinID="ddl250" 
                                    AutoPostBack="true" Enabled="false">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                    </td>
                </tr> 
                 <tr id="l1" runat="server" visible="false">
                    <td style="width: 99%;" valign="top" colspan="3">
                       <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" ForeColor="Red"  Text="Record already exists. Do you want to continue! Please Select 'YES' &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    
                    </td>
                      </tr>   
                  <tr class="even" id="l2" runat="server" visible="false">
                    <td style="width: 99%;" valign="top" colspan="3">
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RdoChoice" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="300px" 
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1">YES</asp:ListItem>
                                    <asp:ListItem Value="0" selected="true">NO</asp:ListItem>
                                   
                                </asp:RadioButtonList>
                            </ContentTemplate>                            
                        </asp:UpdatePanel>
                    </td>
                      </tr> 
                <tr class="even" id="l3" runat="server" visible="false">
                    <td style="width: 99%;" valign="top" colspan="3">
                      
                    
                    </td>
                      </tr>                               
            </table>

            &nbsp;<br />
             <asp:Label ID="lblmsg" runat="server" visible="false" SkinID="CaptionLabel" ForeColor="Green" Font-Size="Small" Font-Bold="true" Text="Record already exists.!!"></asp:Label>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
             <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="Rvalue" runat="server" />                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
