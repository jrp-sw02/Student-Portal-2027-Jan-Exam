<%@ Page Title="MIS Bulk Upload Students" Language="C#" AutoEventWireup="True" CodeFile="MISBulkUploadStudents.aspx.cs"
    Inherits="HO_MISBulkUploadStudents"
    MasterPageFile="~/MasterPages/Main.master" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">  
        function validateFormFields() {
            if (!isSelected("<%=ddlCenter.ClientID %>", "Centre"))
                return false;

            if (!isSelected("<%=ddlCourse.ClientID %>", "Course"))
                return false;

            if (!isSelected("<%=ddlBatch.ClientID %>", "Batch"))
                return false;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            

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
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        //     row.style.backgroundColor = "#8ce393";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original 

                        //     row.style.backgroundColor = "#cad7e2";

                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>


    <style type="text/css">
        .gdbody .gdrow a {
            background-color: transparent;
        }

        .gdheader th{
            font-size : 9px;
        }


        .gdbody .gdalternate a {
            background-color: transparent;
        }

        .gdbody tr {
            background-color: #cad7e2;
        }

        .btnDisabled {
    background: #d3d3d3 !important;
    color: #666 !important;
    border: 1px solid #aaa !important;
    cursor: not-allowed !important;
    opacity: 1 !important;
}
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    MIS Bulk Data Upload
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />

</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">


    <asp:Label ID="txtInstitute" runat="server" Text="CENTRE NAME"
        Width="100%"></asp:Label>



    <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" id="Table1">

        <tr>
            <td colspan="2" align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblerror" runat="server" EnableTheming="false"  CssClass="error" Visible="false"
                            Width="100%"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr class="gdrow1">
            <td>Choose Institute: </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                            TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                            Style="height: 27px" Font-Bold="True">
                            <%--              <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>--%>
                            <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>


        <tr class="gdalternate1">
            <td>Select Centre: </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCenter" Width="250px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>



        <tr class="gdrow1">
            <td>Select Course: </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourse" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" TabIndex="1">
                              <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>


        <tr class="gdalternate1">
            <td>Select Batch</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatch" runat="server" Width="250px" TabIndex="1">
                              <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label runat="server" Font-Bold="true" ID="lblBatchStart" Text=""></asp:Label>
                        <asp:Label runat="server"  Font-Bold="true" ID="lblBatchEnd" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr id="trbtn" class="gdalternate1">
            <td></td>
            <td>
                <asp:Button ID="btnClick" runat="server" Text="Show Data" OnClick="btnShow_Click" OnClientClick="return validateFormFields()" Width="89px" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" Width="89px" />

            </td>
        </tr>

    </table>

    <div id="divGrid" runat="server">
        <asp:Label ID="lblimportantAlert" runat="server" EnableTheming="false"
            Width="100%">

             <strong style="color:red" >Note: Centres have to upload all required Certificates later through Edit Form.</strong>
        </asp:Label>

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="true"></asp:Label>

                <asp:GridView ID="gvMain" runat="server" OnRowDataBound="gvMain_RowDataBound" DataKeyNames="ID,Registration_No,Cast_Category_ID,Cor_District_ID"

                    AutoGenerateColumns="False" Width="100%">

                    <Columns>

                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>

                        <asp:BoundField
                            DataField="Registration_No"
                            HeaderText="Regn No."
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField
                            DataField="Name"
                            HeaderText="Name"
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField
                            DataField="Father_Name"
                            HeaderText="Father Name"
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField
                            DataField="dob"
                            HeaderText="Date of Birth"
                            DataFormatString="{0:dd-MMM-yyyy}"
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField
                            DataField="Gender"
                            HeaderText="Gender"
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField
                            DataField="caste_name"
                            HeaderText="CasteCategory"
                            NullDisplayText="-"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />

                        <%--              <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,CourseId" HeaderText="ProjectName"
                            DataTextField="ProjectName"
                            DataNavigateUrlFormatString="?Key={0}">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>--%>

                        <asp:TemplateField HeaderText="Project" runat="server">
                            <ItemTemplate>
                                <asp:DropDownList Width="100px" ID="ddlProjectId" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField Visible="false">

                            <ItemTemplate>
                                <asp:Label runat="server" Visible="true" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                <asp:Label runat="server" Visible="true" ID="lblregnno" Text='<%# Eval("Registration_No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkInstitutes" onclick="Check_Click(this)" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>
                </asp:GridView>



            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" Visible="false" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div runat="server" id="savebuttondiv" visible="false" style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnSave" runat="server"
            Text="Save" OnClick="btnSave_Click" />
    </div>


    <div id="Save_Record_section" runat="server" visible="false">


        <asp:Label runat="server" ID="lblsuccesscnt" ForeColor="Green" Font-Bold="true" Text="Success : 3 Records saved."></asp:Label>
        <asp:Label runat="server" ID="lblfailurecnt" ForeColor="Red" Font-Bold="true" Text="Failed  : 1 Records Failed."></asp:Label>

        <div>
            <asp:Label runat="server" ID="Label4" Text="Status of Saved Records, Please Check Carefully"></asp:Label>
            <asp:UpdatePanel EnableViewState="true" ID="updgv2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="true"></asp:Label>
                    <asp:GridView OnRowDataBound="gvMain2_RowDataBound" ID="gvMain2" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <%--<asp:BoundField DataField="Regn No" HeaderText="Regn No" />--%>
                            <asp:BoundField HeaderText="#" />
                            <asp:BoundField DataField="Registration_No" HeaderText="Registration Number" />
                            <asp:BoundField DataField="projectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="reason" HeaderText="Reason" />


                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>



</asp:Content>
