<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="MISQualificationEligibility.aspx.cs" Inherits="Admin_MISQualificationEligibility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
               
                 <script type="text/javascript">

                     function SearchQualifications(txtSearch, cbQualifications) {
                         if ($(txtSearch).val() != "") {
                             var count = 0;
                             $(cbQualifications).children('tbody').children('tr').each(function () {
                                 var match = false;
                                 $(this).children('td').children('label').each(function () {
                                     if ($(this).text().toUpperCase().indexOf($(txtSearch).val().toUpperCase()) > -1)
                                         match = true;
                                 });
                                 if (match) {
                                     $(this).show();
                                     count++;
                                 }
                                 else { $(this).hide(); }
                             });
                             $('#spnCount').html((count) + ' match').css({ "color": "green", "font-size": "100%","font-weight": "bold" });
                         }
                         else {
                             $(cbQualifications).children('tbody').children('tr').each(function () {
                                 $(this).show();
                             });
                             $('#spnCount').html('');
                         }
                     }


    </script>

    <script type="text/javascript">
        function checkBoxList1OnCheck(listControlRef) {
            var inputItemArray = listControlRef.getElementsByTagName('input');

            for (var i = 0; i < inputItemArray.length; i++) {
                var inputItem = inputItemArray[i];

                if (inputItem.checked) {
                    inputItem.parentElement.style.backgroundColor = 'LightGreen ';
                }
                else {
                    inputItem.parentElement.style.backgroundColor = 'White';
                }
            }
        }
</script>
 <script  type="text/javascript">
     $(function () {
         $('#multiselect').multiselect();
     });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#multiselect').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                filterPlaceholder: 'Search for something...'
            });
        });
</script>
              
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="MIS Qualification Eligibility"></asp:Label>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
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
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategoryF" Width="100%" runat="server" AutoPostBack="True"
                                             OnSelectedIndexChanged="ddlcoursecategoryF_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                      
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td> 
                                        <asp:Label ID="lblcourseNameF" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourseNameF" Width="100%" runat="server"  AutoPostBack="True">                                          
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                                </ContentTemplate>
                                        </asp:UpdatePanel>                                  
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Qualification Levels"></asp:Label>
                                        <asp:DropDownList ID="ddlQlevels" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBreadCrumb" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category Name"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=cbQualifications.ClientID %>", "Qualification Level"))
                return false;
            if (!isBlank("<%=txtExperienceYrs.ClientID %>", "Experience in Years"))
                return false;
            if (!isNumber("<%=txtExperienceYrs.ClientID %>"))
                return false;
            if (!isBlankDate("<%=txtEffectiveFromDt.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtEffectiveToDt.ClientID %>", "Effective To Date", "dd-MMM-yyyy"))
                return false;

            var frdate = document.getElementById("<%=txtEffectiveFromDt.ClientID %>").value;
            var todate = document.getElementById("<%=txtEffectiveToDt.ClientID %>").value;
            if (!CompareDates(frdate, todate, "Effective From date should be less then Effective To date", true))
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
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" 
                            Width="100%"
                             OnRowDeleting="gvMain_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblsr" runat="server" Text='<%# Bind("Id") %>'></asp:Label>                           
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="coursecatName"
                                    HeaderText="Course Category" SortExpression="coursecatName" Target="_self">
                                <HeaderStyle Width="18%" />
                                </asp:HyperLinkField>
                                 
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="Name"
                                    HeaderText="Course Name" SortExpression="Name" Target="_self">
                                <HeaderStyle Width="18%" />
                                </asp:HyperLinkField>      
                                
                                 <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="QualificationLevelName"
                                    HeaderText="Qualification Levels" SortExpression="QualificationLevelName" Target="_self">
                                <HeaderStyle Width="20%" />
                                </asp:HyperLinkField>                                                               
                               
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="experience"
                                    HeaderText="Experience" SortExpression="experience" Target="_self">
                                  
                                <HeaderStyle Width="10%" />
                                </asp:HyperLinkField> 
                                
                                 <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="EffFromDate"
                                    HeaderText="Effective From Date" SortExpression="EffFromDate" Target="_self">
                                  
                                <HeaderStyle Width="25%" />
                                </asp:HyperLinkField>  
                                
                                 <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="EffToDate"
                                    HeaderText="Effective To Date" SortExpression="EffToDate" Target="_self">
                                  
                                <HeaderStyle Width="35%" />
                                </asp:HyperLinkField>    
                               
                             
                                <asp:TemplateField HeaderText="Delete" Visible="false" >
                                    <ItemTemplate>
                                         <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                             Text="&lt;img title='Click to delete this record' src='../Images/delete.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
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
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0">

                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="lblQuallevel" runat="server" SkinID="CaptionLabel" Text="Qualification Level&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                    </td>
                                       
                </tr>
                <tr class="even">

                   <td style="width:33%;" valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>--%>
                                <asp:DropDownList ID="ddlcoursecategory" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>

                    </td>
                     <td style="width:33%;" valign="top">
                           <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlcourseName"  Width="100%" runat="server" SkinID="ddl250"> 
                             <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>  
                               </asp:UpdatePanel>  
                    </td>
                    
                   <td valign="top" style="width:33%;">
                       <%-- <asp:DropDownList ID="ddlQualLevel" runat="server" Height="22px" Width="200px">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>--%>
                        <div style=" width: 100%" >
                       <asp:TextBox ID="TextBox1" runat="server" style="width: 370px;" SkinID="ddl250" onkeyup="SearchQualifications(this,'#cbQualifications');"
                                                    placeholder="Search Qualification">
                                                </asp:TextBox>
                                                <span id="spnCount"></span>
                                                <div style="height: 100px; overflow-y: auto; overflow-x: hidden">
                                                    <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel2" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                                    <asp:CheckBox ID="allChkBox" Text="Select all" AutoPostBack="True" oncheckedchanged="allChkBox_CheckedChanged" runat="server" visible="false"  />
                                                    <asp:CheckBoxList ID="cbQualifications" runat="server" visible="false" RepeatColumns="1"
                                                        RepeatDirection="Vertical" Width="360px" ClientIDMode="Static" onclick="checkBoxList1OnCheck(this);"  >                                                    
                                                    </asp:CheckBoxList>
                                    </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                            </div>
                    </td>
                </tr> 
                <tr>                    
                    
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lbExperience" runat="server" SkinID="CaptionLabel" Text="Experience in Years&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                     <td style="width: 33%;" valign="top">
                         <asp:Label ID="lblEffectiveFromDt" runat="server" SkinID="CaptionLabel" Text="Effective Date From&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                     <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblEffectiveToDt" runat="server" SkinID="CaptionLabel" Text="Effective Date To&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">                   
                    
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExperienceYrs" runat="server" MaxLength="4" SkinID="txt248"></asp:TextBox>
                    </td>

                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEffectiveFromDt" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" AutoPostBack="True" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveFromDt">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                    </td>

                     <td style="width: 33%;" valign="top">
                       <asp:TextBox ID="txtEffectiveToDt" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" AutoPostBack="True" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveToDt">
                                </asp:CalendarExtender>
                                <img id="img1" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                    </td>
                </tr>
               
                
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
