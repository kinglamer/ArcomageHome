<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Arcomage.Server.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <asp:gridview ID="gvTable" runat="server" runat="server" AutoGenerateColumns="False"
    PageSize="10" AllowPaging="true">
            
                <Columns>
        <asp:BoundField DataField="id" HeaderText="Customer Id" />
        <asp:BoundField DataField="PlayerTower" HeaderText="Contact Name" />
        <asp:BoundField DataField="PlayerWall" HeaderText="City" />
    </Columns>
        </asp:gridview>
    </div>
    </form>
</body>
</html>
