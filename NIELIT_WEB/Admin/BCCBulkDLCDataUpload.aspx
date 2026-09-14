<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/PuraskarApp.master" CodeFile="BCCBulkDLCDataUpload.aspx.cs" Inherits="Admin_BCCBulkDLCDataUpload" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/usercontrol/sidelink.ascx" TagPrefix="uc" TagName="SideLink" %>

<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/json2/20130526/json2.min.js"></script>

    <script type="text/javascript">
        function getBCCCourseList() {

            var entryDate = $.trim($("[id*=txtdate]").val());

            $.ajax({
                type: 'POST',
                url: 'http://localhost:8080/BCCBulkUpload.asmx/getBCCCourseList',
                data: "{ entryDate: '" + entryDate + "'}",
                contentType: 'application/json; charset=utf-8',

                success: function (r) {
                    //alert("File Uploaded Successfully");
                    // getdataserror(JSON.parse(r.d)[0]);
                    getdataserror(r);
                    // document.getElementById("lblmsgs").innerHTML =JSON.parse(r.d)[0];
                    //getdatas(r); 

                },
                error: function (r, jqXHR, textStatus, errorThrown) {
                    document.getElementById("lblmsgs").innerHTML = r.responseText;
                    alert("jqXHR= " + jqXHR + ", textStatus= " + textStatus + ", errorThrown= " + errorThrown)
                }
            });
            return false;
        }

        function getdatas(insd) {
            debugger
            var json = JSON.stringify(insd.responseText);
            alert(json);
            //alert(JSON.parse(r));
            // getdataserror(insd)
        }
        var param = {};
        function getdataserror(insd) {


            $.ajax({
                type: "POST",
                url: "http://localhost:1039/NIELIT_WEB/Admin/BCCBulkDLCDataUpload.aspx/UpdateInstituteDetails",
                //data: "{ insd: '" + JSON.stringify(JSON.parse(insd.d)[0]) + "'}",
                data: "{ insd: '" + JSON.stringify(JSON.parse(insd.d)[0]) + "'}",
                contentType: 'application/json; charset=utf-8',
                datatype: "json",

                success: function (r) {
                    document.getElementById("lblmsgs").innerHTML = "Students record submitted successfully";
                },
                error: function (r) {
                    alert(r.d);
                    document.getElementById("lblmsgs").innerHTML = JSON.stringify(JSON.parse(insd.d)[0].length + " " + "Students record not submitted successfully");
                },
                failure: function (r) {
                    var json = JSON.stringify(r.responseText);
                    alert(json);
                }
            });
        }

        function getdatasfailure(r) {
            alert("HIdddfailure");
            // alert(r.d);
        }

    </script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">

    <asp:Label ID="lblHeading" runat="server" Text="CSC Bulk DLC Data Upload " meta:resourcekey="lblHeadingResource1"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                // row.style.backgroundColor = "aqua";

            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    // row.style.backgroundColor = "#C2D69B";
                }
                else {
                    // row.style.backgroundColor = "white";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            //headerCheckBox.checked = checked;

        }
    </script>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="sample2" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblDate" runat="server" SkinID="CaptionLabel" Text="Date &lt;b class='mandatory'&gt;*&lt;/b&gt;" Font-Bold="true"
                                meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdate" runat="server" />
                            <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="imgdate" TargetControlID="txtdate">
                            </asp:CalendarExtender>
                            <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                        </td>

                    </tr>
                </table>

                <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="btnUploadFile" runat="server" Text="Synchronize data with DLC" OnClick="btnUploadFile_Click" />
                    &nbsp; &nbsp;
                        <asp:Button ID="btnFinalized" runat="server" Text="Finalize Synchronized Data" OnClick="btnFinalized_Click" />
                    &nbsp; &nbsp;
            <asp:Button ID="Button1" runat="server" Text="View InValid Record " OnClick="btnCancel_Click" />
                    <asp:Label ID="lblmsg" runat="server" Text="No record found!!" ForeColor="Red" Visible="false"></asp:Label>

                </div>
                <div style="text-align: left; margin-top: 10px">
                    <br />
                    <asp:Label ID="lblMsgs" runat="server" Text="" Visible="false"></asp:Label>
                </div>
                <table width="100%" id="trdata1" runat="server" visible="true">
                    <tr id="trdata" runat="server" visible="false">
                        <td colspan="2">
                            <div id="divReportData" runat="server" style="width: 100%;" visible="false">
                                <br />
                                <asp:Label ID="lblAccrDetails" runat="server" ForeColor="blue" Font-Bold="true"
                                    Text="  List of Synchronized Data" Visible="true"></asp:Label>
                                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                                    <HeaderStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>

                                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                                    DataTextField="RefNo" HeaderText="RefNo" SortExpression="RefNo"
                                                    Target="_self">
                                                    <HeaderStyle Width="20%" />
                                                </asp:HyperLinkField>

                                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                                    DataTextField="Name" HeaderText="Institute Name" SortExpression="Name"
                                                    Target="_self">
                                                    <HeaderStyle Width="20%" />
                                                </asp:HyperLinkField>

                                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                                    DataTextField="AccreditationNumber" HeaderText="Accr No." SortExpression="AccreditationNumber"
                                                    Target="_self">
                                                    <HeaderStyle Width="20%" />
                                                </asp:HyperLinkField>

                                                <asp:TemplateField HeaderText="DISTRICT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("DISTRICT") %>' Width="110px"></asp:Label>

                                                        <asp:DropDownList ID="ddlDistrict" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                                        </asp:DropDownList>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlDistrict"
                                                            ErrorMessage="Please select" ForeColor="Red" InitialValue="0" Display="Dynamic">
                                                        </asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                                    DataTextField="CITY" HeaderText="CITY" SortExpression="ADDRESS"
                                                    Target="_self">
                                                    <HeaderStyle Width="20%" />
                                                </asp:HyperLinkField>

                                                <asp:TemplateField HeaderText="STATE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label22" runat="server" Text='<%# Bind("STATE_NAME") %>' Width="110px"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                                    DataTextField="Remarks" HeaderText="Remarks" SortExpression="Remarks"
                                                    Target="_self">
                                                    <HeaderStyle Width="40%" />
                                                </asp:HyperLinkField>
                                            </Columns>
                                            <PagerSettings Visible="False" />
                                        </asp:GridView>
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
                        </td>
                    </tr>
                    <tr id="RInvalidRecords" runat="server" visible="false">
                        <td colspan="2">
                            <asp:Label ID="lblInvalidRec" runat="server" ForeColor="blue" Font-Bold="true"
                                Text="  Invalid Records Details" Visible="true"></asp:Label>
                            <br />
                            <asp:Label ID="lblMsgss" runat="server" Text="" Visible="false"></asp:Label>
                            <div id="grdRemarks" runat="server">
                                <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>

                                        <asp:GridView ID="GridView1" runat="server" DataKeyNames="Accrediation_Number" OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing"
                                            OnRowCancelingEdit="OnRowCancelingEdit" OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" Visible="false">
                                            <RowStyle Height="30px" />
                                            <AlternatingRowStyle Height="30px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="#" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerial" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RefNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ID_RefNo" runat="server" Text='<%#Eval("RefNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="InstituteName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server" Text='<%#Eval("CENTRE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Accr No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Accrediation_Number" runat="server" Text='<%#Eval("Accrediation_Number") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DISTRICT">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DISTRICT" runat="server" Text='<%#Eval("DISTRICT") %>'></asp:Label>
                                                    </ItemTemplate>

                                                    <EditItemTemplate>
                                                        <asp:Label ID="lbl_DISTRICTl" runat="server" Text='<%#Eval("DISTRICT") %>'></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddlDistricts" runat="server" SkinID="ddl150" Visible="true">
                                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CITY">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CITY" runat="server" Text='<%#Eval("CITY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="STATE">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_STATE_NAME" runat="server" Text='<%#Eval("STATE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Remarks" runat="server" ForeColor="#996633" Font-Bold="true" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:CommandField HeaderText="Action" ShowEditButton="True"></asp:CommandField>
                                            </Columns>

                                        </asp:GridView>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>

                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
    <uc:SideLink runat="server" ID="ucSideLink" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>