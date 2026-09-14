<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="NSQFFreeAccrGrant.aspx.cs" Inherits="HO_NSQFFreeAccrGrant" %>

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
</asp:Content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NSQF Free Course Accreditation Grant"></asp:Label>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by institute name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10"  />
 
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {          
           
          
          
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
                           AutoGenerateColumns="False" Width="600px" Caption="&lt;b&gt;Institutes with NSQF Free Accreditation Granted&lt;/b&gt;" OnRowDataBound ="gvMain_RowDataBound"  >
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                             
                                 
                                 <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ID" HeaderText="Request Id" SortExpression="ID" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                                               
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="course" HeaderText="Course Name" SortExpression="course" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="validity" HeaderText="Course Validity" SortExpression="validity" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="AccrNo" HeaderText="Accr No." SortExpression="AccrNo" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Status" HeaderText="Accr Status" SortExpression="Status" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="InstName" HeaderText="Institute Name" SortExpression="InstName" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="InstAddress" HeaderText="Institute Address" SortExpression="InstAddress" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="EffectiveFrom" HeaderText="Effective From Date" SortExpression="EffectiveFrom"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                 <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="EffectiveTo" HeaderText="Effective To Date" SortExpression="EffectiveTo"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}" ><HeaderStyle Width="10%" /></asp:HyperLinkField>
                                   <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="grantDate" HeaderText="Grant Date" SortExpression="grantDate"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>
                                
                               
                                
                                                              
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
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Request From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Request To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
           
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom"></asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                   
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto"></asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
            </td>
           
        </tr>
                   <tr>
                       <td colspan ="2" align ="center" >
                           <asp:Button ID ="btnShow" Text ="Show" runat ="server" OnClick="btnShow_Click" />
                       </td>
                   </tr>
    




               <tr>
                   <td colspan ="2">
                        <asp:GridView ID="grdNSQFCoursesForAccr" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                           AutoGenerateColumns="False" Width="600px" OnRowDataBound ="grdNSQFCoursesForAccr_RowDataBound"  Visible ="false" >
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                               <asp:TemplateField HeaderText="Sr No." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" Visible ="false"><ItemTemplate><asp:Label ID="lblsr" runat="server" Text='<%# Bind("id") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                 
                                  <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ID" HeaderText="Request ID" SortExpression="ID" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                                               
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Course Name" SortExpression="Name" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                               <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="InstName" HeaderText="Institute Name" SortExpression="InstName" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="InstAddress" HeaderText="Institute Address" SortExpression="InstAddress" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
								   <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Accreditation_Number" HeaderText="Accr No" SortExpression="Accreditation_Number" Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="AppDate" HeaderText="Application Date" SortExpression="AppDate"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="10%" /></asp:HyperLinkField>
                               <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="true"><HeaderTemplate><asp:Label runat="server" ID="lblRemarks" Text="Remarks" /></HeaderTemplate><ItemTemplate><asp:TextBox runat="server" ID="txtRemarks" Maxlebgth="500" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                                 
                                                              
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="true"><HeaderTemplate><asp:Label runat="server" ID="lblGrant" Text="Grant" /></HeaderTemplate><ItemTemplate><asp:CheckBox runat="server" ID="chkGrant" SkinID="CheckAllInGridView" /><asp:Label ID="lblID" runat ="server" Visible ="false" Text ='<%#Eval("id")%>'></asp:Label></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                   </td>
               </tr> 
               <tr id="FileLabelBlock" runat ="server" visible ="false">
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="E File No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Approval Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
           
        </tr>
        <tr class="even" id="FileBlock" runat ="server" visible ="false">
            <td>
               
                        <asp:TextBox ID="txtEFileNo" runat="server"  
                            ToolTip="E File No."></asp:TextBox>
                       
                   
            </td>
            <td>
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                <asp:TextBox ID="txtApprovalDate" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender4" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img3" TargetControlID="txtApprovalDate"></asp:CalendarExtender>
                <img id="img3" alt="Calender" src="../images/calendaricon.jpg" />
                        </ContentTemplate>
                   
                </asp:UpdatePanel>
            </td>
           
        </tr>                                                  
            </table>          
            <div style="text-align: right; margin-top: 10px" id="saveBlock" runat ="server" visible="false">
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