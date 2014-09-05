<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="NetGain.GoogleMap" Namespace="NetGain.GoogleMap" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Google Map API Test</title>
    <style type="text/css">
        .Table
        {
            display: table;
        }
        .Title
        {
            display: table-caption;
            text-align: center;
            font-weight: bold;
            font-size: larger;
        }
        .Heading
        {
            display: table-row;
            font-weight: bold;
            text-align: center;
        }
        .Row
        {
            display: table-row;
            vertical-align: top;
        }
        .Cell
        {
            display: table-cell; /*border: solid;
        border-width: thin;*/
            padding-left: 5px;
            padding-right: 5px;
            vertical-align: top;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">  
    <div class="Table">
        <div class="Row">
            <div class="Cell" style="width: 220px">
                <asp:Repeater ID="rptAddress" runat="server">
                    <ItemTemplate>
                        <table style="background-color:#EBEFF0;border-top:1px dotted #df5015;border-bottom:1px solid #df5015;width:200px;font-family:Verdana;font-size:small;">
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "StreetAddress")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "City")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "State")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "Country")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "Zipcode")%>
                            </td></tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                    <table style="background-color:#ffffff;border-top:1px dotted #df5015;border-bottom:1px solid #df5015;width:200px;font-family:Verdana;font-size:small;">
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "StreetAddress")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "City")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "State")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "Country")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "Zipcode")%>
                            </td></tr>
                        </table>
                    </AlternatingItemTemplate><SeparatorTemplate><br /></SeparatorTemplate>
                </asp:Repeater>
            </div>                       
            <div class="Cell">
                <cc1:GoogleMap ID="GoogleMap1" Width="400" Height="200" Zoom="10" MapType="ROADMAP" Radius="40" runat="server">
                </cc1:GoogleMap>              
            </div>
            </div>
            </div>            
            <div class="Table">
             <div class="Row">
            <div class="Cell" style="width: 220px">
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <table style="background-color:#EBEFF0;border-top:1px dotted #df5015;border-bottom:1px solid #df5015;width:200px;font-family:Verdana;font-size:small;">
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "StreetAddress")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "City")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "State")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "Country")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "Zipcode")%>
                            </td></tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                    <table style="background-color:#ffffff;border-top:1px dotted #df5015;border-bottom:1px solid #df5015;width:200px;font-family:Verdana;font-size:small;">
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "StreetAddress")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "City")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "State")%>,
                            </td></tr>
                            <tr><td>
                                <%#DataBinder.Eval(Container.DataItem, "Country")%>,
                            &nbsp;&nbsp;
                                <%#DataBinder.Eval(Container.DataItem, "Zipcode")%>
                            </td></tr>
                        </table>
                    </AlternatingItemTemplate><SeparatorTemplate><br /></SeparatorTemplate>
                </asp:Repeater>
            </div>
            
            <div class="Cell">
            <cc1:GoogleMap ID="GoogleMap2" Width="400" Height="200" Zoom="10" MapType="ROADMAP" Radius="40" runat="server">
                </cc1:GoogleMap>
            </div>
            <br />
            <%--<asp:Label ID="Label1" runat="server" Text="Click on the map to add a new marker.."></asp:Label>--%>
        </div>
        </div>
        </form>
</body>
</html>
