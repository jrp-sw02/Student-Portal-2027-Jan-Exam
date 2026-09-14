<%@ Page Title="Project Student Correction" Language="C#" AutoEventWireup="True" CodeFile="ProjectBasedCorrectionReport.aspx.cs"
    Inherits="HO_ProjectBasedCorrectionReport"
    Debug="True"
    MasterPageFile="~/MasterPages/Main.master" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">  

        function validateFormFields() {

             if (!isSelected("<%=ddlproject.ClientID %>", "Project"))
                return false;


            if (!isSelected("<%=ddlinstitute.ClientID %>", "School"))
                return false;

           

            if (!isBlank("<%=txtapaar.ClientID %>", "ApaarID "))
                return false;
        }

        function nameValidation(input) {
            input.value = input.value.replace(/[^A-Za-z \-]/g, '');
        }

        function rnvalidation(input) {
            input.value = input.value.replace(/[^A-Za-z0-9\-]/g, '');
        }

        function apaarvalidation(input) {
            input.value = input.value.replace(/[^0-9]/g, '').substring(0, 12);
        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Project Based Student Correction Form
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
         
    <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" id="tblInfo">

        <tr class="gdalternate1">
            <td colspan="2" align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                            Width="100%"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>Select Project</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList Width="300px" ID="ddlproject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlproject_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select Project--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>


        <tr class="gdalternate1">
            <td>Select Accredited School</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList Width="300px" ID="ddlinstitute" runat="server" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select Institute--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label runat="server" ID="lblapaar" Text="APAAR ID"></asp:Label>
            </td>
            <td>
                <asp:TextBox Placeholder="APAAR ID" ID="txtapaar" oninput="apaarvalidation(this)" runat="server" Width="290px"></asp:TextBox>
            </td>
        </tr>
        <tr id="trbtn" class="gdalternate1">
            <td></td>
            <td>
                <asp:Button ID="btnStatus" runat="server" Text="Check Status" OnClientClick="return validateFormFields()" OnClick="btn_status" Width="89px" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" Width="89px" />

            </td>
        </tr>

    </table>
    <div>
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="true"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" Width="100%" AutoGenerateColumns="False" Visible="true">
                    <Columns>
                        <asp:BoundField DataField="UDISECode" HeaderText="UDISE Code" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Class" HeaderText="Class" />
                        <asp:BoundField DataField="RollNumber" HeaderText="Roll Number" />
                        <asp:BoundField DataField="FatherName" HeaderText="Father Name" />
                        <asp:BoundField DataField="DOB" HeaderText="Date of Birth" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <table id="update_table" runat="server" visible="false" align="center" border="0" cellpadding="3" class="sample3" width="100%">
        <tr class="gdrow1">
            <td colspan="2">
                <asp:Label ID="Label2" Style="font-weight: bold;" runat="server" Text="Updation Form : "></asp:Label>
            </td>

        </tr>

        <tr class="gdalternate1">

            <td colspan="2">
                <asp:Label ID="lblinstructions" runat="server">
                    <p>
                        (i) Please enter data Carefully. Data can be only updated 2 times.
                        <br />
                        (ii) Data cannot be Updated, after Registration Form is filled.
                    </p>
                </asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblname" runat="server" Text="Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtname" oninput="nameValidation(this)" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblrollnum" runat="server" Text="Roll Number"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtrollnum" oninput="rnvalidation(this)" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>

        <tr class="gdrow1">
            <td>
                <asp:Label ID="lblfathername" runat="server" Text="Father Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtfathername" oninput="nameValidation(this)" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>

        <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblDob" runat="server" Text="Date of Birth"></asp:Label>
            </td>
            <td>
                <asp:TextBox MaxLength="11" ID="txtdob" runat="server" SkinID="txtDate" Width="99px"
                    TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                    AutoPostBack="true">
                </asp:TextBox>
                <img id="imgDob1" runat="server" src="../../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="cedate1" TargetControlID="txtdob" PopupPosition="BottomLeft"
                    Format="yyyy-MMM-dd" PopupButtonID="imgDob1" runat="server">
                </asp:CalendarExtender>

            </td>
        </tr>
        <tr class="gdrow1">
            <td></td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Update Data" OnClick="btnUpdate_Click" OnClientClick="return validateFormFields()" Width="89px" />
                <asp:Label ID="lblsuccess" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
        <div>

        <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="true"></asp:Label>
                <asp:GridView ID="gvMain2" runat="server" Width="100%" AutoGenerateColumns="False" Visible="true">
                    <Columns>
                        <asp:BoundField DataField="UDISECode" HeaderText="UDISE Code" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Class" HeaderText="Class" />   
                        <asp:BoundField DataField="RollNumber" HeaderText="Roll Number" />
                        <asp:BoundField DataField="FatherName" HeaderText="Father Name" />
                        <asp:BoundField DataField="DOB" HeaderText="Date of Birth" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>



    </div>
</asp:Content>
