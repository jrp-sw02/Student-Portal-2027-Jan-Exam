<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="NSQFFreeCourseMapping.aspx.cs" Debug="true" Inherits="Admin_NSQFFreeCourseMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
               
                 <script type="text/javascript">

                     function SearchNielitCentres(txtSearch, cbNielitCentres) {
                         if ($(txtSearch).val() != "") {
                             var count = 0;
                             $(cbNielitCentres).children('tbody').children('tr').each(function () {
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
                             $('#spnCount').html((count) + ' match').css({ "color": "green", "font-size": "100%", "font-weight": "bold" });
                         }
                         else {
                             $(cbNielitCentres).children('tbody').children('tr').each(function () {
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
        <style type="text/css">
            .auto-style1 {
                width: 33%;
                height: 42px;
            }
        </style>
</asp:Content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NSQF Free Course Mapping for Accreditation"></asp:Label>
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
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
                                            >
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
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by course name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" visible="false" />
 
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {          
           
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category Name"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlCourseMapped.ClientID %>", "Course Mapped"))
                return false;
            if (!isBlankDate("<%=txtEffectiveFromDate.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtMappingApprovalDate.ClientID %>", "Mapping Approval Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtEFileNo.ClientID %>", "E-File No"))
                return false;
            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
            ShowHideMenu(obj, tableid);
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp1, Sender, CheckBoxName)
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp1, Sender, CheckBoxName)
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
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                           AutoGenerateColumns="False" Width="600px" OnRowDataBound ="gvMain_RowDataBound" >
                            <Columns>
                                 <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                               <%-- <asp:TemplateField HeaderText="Sr No." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblsr" runat="server" Text='<%# Bind("Id") %>'></asp:Label>                           
                            </ItemTemplate>
                        </asp:TemplateField> -->
                                 
                                  <%--<asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="coursecatName" HeaderText="Course Category Name" SortExpression="Name" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>--%>
                                                               
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Course Name" SortExpression="Name" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseMapped" HeaderText="Course Mapped" SortExpression="CourseMapped" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>

                                  <asp:HyperLinkField   HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseValidity" HeaderText="Mapped Course valid Upto" SortExpression="CourseValidity"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="mappingApprovalDate" HeaderText="Mapping Approval Date" SortExpression="mappingApprovalDate"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="eFileNo" HeaderText="e File No." SortExpression="eFileNo"
                                    Target="_self" ><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="effectiveFrom" HeaderText="Effective From Date" SortExpression="effectiveFrom"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="effectiveTo" HeaderText="Effective To Date" SortExpression="effectiveTo"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="isReplacement" HeaderText="IsReplacement" SortExpression="isReplacement" Target="_self"><HeaderStyle Width="10%" />  <ItemStyle HorizontalAlign="Center" /></asp:HyperLinkField>
                                  <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseReplaced" HeaderText="Course Replaced" SortExpression="CourseReplaced" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                 
                                                              
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false"><HeaderTemplate><asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate><ItemTemplate><asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="200" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
         <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="NSQF Course mapped to &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    
                                        
                </tr>
                <tr class="even">
                     <td style="width:33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" >
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                     <td style="width:33%;" valign="top">
                           <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlcourseName"  Width="100%" runat="server" SkinID="ddl250"> 
                             <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>  
                               </asp:UpdatePanel>  
                    </td>
                    
                     <td valign="top" width="30%">
                        
                           <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseMapped"  Width="100%" runat="server" SkinID="ddl250"> 
                             <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>  
                               </asp:UpdatePanel>  
                   
                                
                               <%-- <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCenter" Width="100%" runat="server" >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel> --%>   
                         
                          <%--<div style=" width: 100%" >
                       <asp:TextBox ID="TextBox1" runat="server" style="width: 370px;" SkinID="ddl250" onkeyup="SearchNielitCentres(this,'#cbNielitCentres');"
                                                    placeholder="Search Nielit Centre">
                                                </asp:TextBox>
                                                <span id="spnCount"></span>
                                                <div style="height: 100px; overflow-y: auto; overflow-x: hidden">
                                                    <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel5" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                                    <asp:CheckBox ID="allChkBox" Text="Select all" AutoPostBack="True" oncheckedchanged="allChkBox_CheckedChanged" runat="server" visible="false"  />
                                                    <asp:CheckBoxList ID="cbNielitCentres" runat="server" visible="false" RepeatColumns="1"
                                                        RepeatDirection="Vertical" Width="360px" ClientIDMode="Static" onclick="checkBoxList1OnCheck(this);"  >                                                    
                                                    </asp:CheckBoxList>
                                    </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                            </div>                           --%>

                            </td>
                   
                   
                </tr>   
                
                
                
                 <tr id="trisactive" runat="server">
                      <td style="width: 33%;" valign="top">
                         <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Effective_From_Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                        </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Effective_To_Date " ></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Mapping Approval Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                      
                    </td>
                    
                                        
                </tr>
                <tr class="even" id="trisactive1" runat="server">
                     <td style="width: 33%;" valign="top">
                     <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveFromDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveFromDate">
                                </asp:CalendarExtender>
                                <img id="imgEFrm" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                       <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveToDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px"  ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgETo" PopupPosition="BottomLeft"  TargetControlID="txtEffectiveToDate">
                                </asp:CalendarExtender>
                                <img id="imgETo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                 <asp:TextBox ID="txtMappingApprovalDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px"  ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgMAD" PopupPosition="BottomLeft"  TargetControlID="txtMappingApprovalDate">
                                </asp:CalendarExtender>
                                <img id="imgMAD" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    
                    
                </tr>      
                <tr>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="EFile No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                      
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Whether Replacement Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                       <asp:Label ID="lblactiveerror" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="100%"></asp:Label>
                    </td>
                     <td valign="top" class="auto-style1">
                          <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                         <asp:Label ID="lblCourseReplaced" runat="server" SkinID="CaptionLabel" Text="Course Replaced" Visible ="false"></asp:Label>
                                                 </ContentTemplate>  
                               </asp:UpdatePanel>  
                        </td>
                </tr>   
                <tr>
                     <td style="width:33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                               <asp:TextBox ID ="txtEFileNo" runat ="server"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    <td style="width:33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlIsReplacement" Width="53%" runat="server" SkinID="ddl250" AutoPostBack="true" Height="16px"  OnSelectedIndexChanged ="IsReplacement">
                            <asp:ListItem Value="-1">Select</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>


                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>

                    <td style="width:33%;" valign="top">
                           <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseReplaced"  Width="100%" runat="server" SkinID="ddl250" Visible ="false"> 
                             <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>  
                               </asp:UpdatePanel>  
                    </td>
                </tr>                                                             
            </table>          
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save" OnClick="btnSave_Click"
                    />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" Visible="False"  />
            </div>
        </asp:View>    
    </asp:MultiView>         
</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>