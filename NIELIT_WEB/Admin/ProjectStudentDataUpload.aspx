    <%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdmitCardUpload_DLC.aspx.cs" Inherits="Admin_CertificateExamAdmitCard" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" Async="true" AutoEventWireup="true"
    CodeFile="ProjectStudentDataUpload.aspx.cs" Inherits="Admin_ProjectStudentDataUpload" Debug="true" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Upload Project Student Data" meta:resourcekey="lblHeadingResource1"></asp:Label>
    <br />
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">


    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function ValidateFormFields() {
            if (!isSelected("<%=ddlProject.ClientID %>", "Project"))
                return false;
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;

            showPleaseWait();
            return true;

        }


        function showPleaseWait() {
            document.getElementById('<%= lblPleaseWait.ClientID %>').style.display = 'inline';
        }
 
    </script>

   <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">

        <asp:View ID="New" runat="server">

            <table class="sample2" cellpadding="2" cellspacing="0">

                <tr>
                    <td class="auto-style1">
                        <div style="width: 33%; float: left">
                            <asp:Label ID="lblProject" runat="server" Text="Project &lt;font color='RED'&gt;*&lt;/font&gt; : "></asp:Label>
                        </div>
                    </td>

                    <td class="auto-style1">
                        <div style="width: 33%; float: left">
                            <asp:DropDownList ID="ddlProject" runat="server" SkinID="ddl253" Width="100%" AutoPostBack="True">
                                <asp:ListItem>-- Select Project --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.XLS/.XLSX)" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>

                    <td colspan="2">
                        <asp:FileUpload ID="flUpload" Width="100%" onchange="triggerServerClick()" AutoPostBack="True"  runat="server" />

                        <asp:HyperLink
                            ID="lnkDownloadFormat" runat="server" NavigateUrl="~/download/Project_Sample.xlsx" Text="Download Excel Format" Target="_blank" rel="noopener noreferrer" />

                        <asp:HyperLink
                            ID="HyperLink1" runat="server" NavigateUrl="~/download/upload_instructions.pdf" Text="Download Upload Instructions" Target="_blank" rel="noopener noreferrer" />

                    </td>
                </tr>


                <tr>
                    <td colspan="3" style="text-align: right">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload File" OnClientClick="return ValidateFormFields()" />
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
						<asp:Button ID="btnreset" Text = "Reset"  runat="server" OnClick="btnreset_Click"  />
                    </td>
                </tr>


                <tr class="even">
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblTotal" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
                        <asp:Label ID="lblCount" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
                        <asp:Label ID="lblLastId" runat="server" SkinID="CaptionLabel" Text="" Visible="false"></asp:Label>
                        <asp:Button ID="DownloadFailureData" runat="server" OnClick="DownloadFailureData_Click" Text="Download UnImported Data" Visible="false" />
                    </td>
                </tr>

                <asp:HiddenField ID ="hf_uploadedfilename" runat ="server" />


            </table>
        </asp:View>
    </asp:MultiView>

         <asp:Label ID="lblPleaseWait" runat="server" Text="Please wait...Data is being Processed" 
     Style="display:none;color:red;float: right" />
           

</asp:Content>
