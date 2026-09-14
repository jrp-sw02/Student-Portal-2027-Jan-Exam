<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="Rights.aspx.cs" Inherits="Admin_Rights" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <script language="javascript" type="text/javascript" src="../Script/GlobalFunction.js"></script>
    <script language ="javascript" type="text/javascript" src="../Script/browserDetect-min.js"></script>
    <script language="javascript" type="text/javascript"  src="../Script/browserDetect.js"></script>
    <script language="javascript" type="text/javascript">
        var selectedElmnt = "";
        function ShowHideobject(elmnt) {
            var e = document.getElementById(elmnt);
            if (e.style.display == 'none') {
                if (selectedElmnt != "") { document.getElementById(selectedElmnt).style.display = "none"; }
//                e.src = 'images/' + elmnt + '_up.jpg';
                e.style.display = 'block';
                selectedElmnt = e.id;
            }
            else {
//                e.src = 'images/' + elmnt + '_down.jpg';
                e.style.display = 'none';
            }
        }
        //selectedElmnt = document.getElementById("selectedElmnt").value;
        function checkUncheck() {
            if (event.srcElement.type == 'checkbox') {
                var s = new String(event.srcElement.id);
                s.split("_")
                var prefix = s.split("_")[0] + "_" //"grdRights_ctl"
                var ctlNo = s.split("_")[1] //s.substring(13,s.indexOf("_",13));
                if (s.indexOf("chkFull") >= 0) {
                    if (document.getElementById(s.toString()).checked == true) {
                        document.getElementById(prefix + ctlNo + "_chkAdd").checked = true;
                        document.getElementById(prefix + ctlNo + "_chkEdit").checked = true;
                        document.getElementById(prefix + ctlNo + "_chkDelete").checked = true;
                        document.getElementById(prefix + ctlNo + "_chkView").checked = true;
                    }
                }

                if (s.indexOf("chkView") >= 0) {
                    if (document.getElementById(s.toString()).checked == false) {
                        document.getElementById(prefix + ctlNo + "_chkAdd").checked = false;
                        document.getElementById(prefix + ctlNo + "_chkEdit").checked = false;
                        document.getElementById(prefix + ctlNo + "_chkDelete").checked = false;
                        document.getElementById(prefix + ctlNo + "_chkFull").checked = false;
                    }
                }

                if (s.indexOf("chkAdd") >= 0 || s.indexOf("chkEdit") >= 0 || s.indexOf("chkDelete") >= 0) {
                    if (document.getElementById(s.toString()).checked == true) {
                        document.getElementById(prefix + ctlNo + "_chkView").checked = true;
                        if (document.getElementById(prefix + ctlNo + "_chkAdd").checked == true && document.getElementById(prefix + ctlNo + "_chkEdit").checked == true && document.getElementById(prefix + ctlNo + "_chkDelete").checked == true) {
                            document.getElementById(prefix + ctlNo + "_chkFull").checked = true;
                        }
                    }
                    else {
                        document.getElementById(prefix + ctlNo + "_chkFull").checked = false;
                    }
                }
            }
        }

//        function window.onload() {
//             selectedElmnt = document.getElementById("selectedElmnt").value;
//        }
    </script>
    <div style="height:650px;">
    <table id="TABLE1" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0"
        class="sample3">
        <tr class="head1">
            <td align="center" valign="top" colspan="2">
                <asp:Label ID="Lbhead" runat="server" Text="RIGHTS"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width:30%">
                Select Role
            </td>
            <td style="width:70%" align="left">
                <asp:DropDownList ID="ddlRole" runat="server" Width="530px" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="gdrow1">
            <td colspan="2" align="center">
                <asp:Panel ID="pnlObjects" runat="server">
                </asp:Panel>
                <br />
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

