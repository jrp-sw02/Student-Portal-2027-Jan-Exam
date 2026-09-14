<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchBar.ascx.cs" Inherits="SearchBar" %>
<div id="searchContainer">
        <%--<a href="#" id="searchButton" title="Search">
            <span></span><em></em></a>--%>
        <div style="clear: both">
        </div>
        <div id="searchBox" align="left">
            <div id="searchPannel">
                <fieldset id="body">
                    <fieldset>
                        <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="searchbar_upnlClear" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Button TabIndex="100" runat="server" ID="btnClear" ToolTip="Reset/Clear Search" 
                                    ClientIDMode="Static" Text="" onclick="btnClear_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel RenderMode="Inline" EnableViewState="true" ID="searchbar_upnlSearch" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Button TabIndex="99" runat="server" OnClientClick="return CheckText();" ToolTip="Search" ID="btnSearch" ClientIDMode="Static" Text="" 
                                    onclick="btnSearch_Click" UseSubmitBehavior="true" />
                                <asp:TextBox TabIndex="98" onkeypress="ChangeFocus();" ID="txtSearch" MaxLength="50" ClientIDMode="Static" runat="server"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="aceSearch"  TargetControlID="txtSearch" runat="server"  CompletionInterval="0" EnableCaching="false"  CompletionSetCount="10" >
                                </asp:AutoCompleteExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                </fieldset>
            </div>
        </div>
        <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
        <script src="../Script/shortcut.js" type="text/javascript"></script>
        <script language="javascript" type="text/javascript" >
            
            function CheckText() {
                if (!isBlank('<%=txtSearch.ClientID %>','<%=ViewState["ErrMsg"].ToString() %>'))
                    return false;
                return true;
            }
            function ChangeFocus() {
                if (window.event.keyCode == 13) {
                    $('#btnSearch').focus();
                   return  CheckText();
                }
            }
            //searchContainer
//            $(function () {
//                var button = $('#searchButton');
//                var box = $('#searchBox');
//                shortcut.add("Ctrl+Shift+S", function () {
//                    box.show();
//                    $('#txtSearch').focus();
//                });
//                shortcut.add("Esc", function () {
//                    box.hide();
//                });
//                var form = $('#searchPannel');
//                button.removeAttr('href');
//                button.mouseup(function (login) {
//                    box.toggle();
//                    $('#txtSearch').focus();
//                    button.toggleClass('');
//                });
//                form.mouseup(function () {
//                    return false;
//                });
//                $(this).mouseup(function (login) {
//                    if (!($(login.target).parent('#searchButton').length > 0)) {
//                        button.removeClass('active');
//                        box.hide();
//                    }
//                });
//            });
        </script>
    </div>