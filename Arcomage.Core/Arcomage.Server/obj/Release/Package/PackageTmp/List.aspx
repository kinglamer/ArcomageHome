<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Arcomage.Server.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
             <asp:gridview ID="gvTable" runat="server" runat="server" AutoGenerateColumns="True"
    PageSize="10" AllowPaging="true">
            
     
        </asp:gridview>
    </div>
    </form>
</body>
</html>
