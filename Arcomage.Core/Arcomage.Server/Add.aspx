<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Arcomage.Server.Add" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Style.css">
</head>
<body>
   <form id="frmAdd" runat="server">
    <div>
        
    </div>
        
        <h1> <asp:Label CssClass="error" ID="lbError" runat="server" Text=""></asp:Label></h1> 
        
        <p/>

        <asp:Label ID="lbName" runat="server" Text="Название карты"></asp:Label> 
        <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
        <p/>

        <table>
            <tr>
                <td>Атрибуты применяемые к характеристикам игрока</td>
                <td></td>
                <td>Атрибуты применяемые к характеристикам противника</td>
            </tr>
            <tr>
                <td>Башня</td>
                <td><asp:TextBox ID="tbTowerPlayer" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="tbTowerEnemy" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <td>Стена</td>
                <td><asp:TextBox ID="tbWallPlayer" runat="server" ></asp:TextBox></td>
                <td><asp:TextBox ID="tbWallEnemy" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Брилиантовый прииск</td>
                <td><asp:TextBox ID="tbDiamondMinesPlayer" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="tbDiamondMinesEnemy" runat="server"></asp:TextBox></td>
            </tr>
              <tr>
                <td>Зверинец</td>
                <td><asp:TextBox ID="tbMenageriePlayer" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="tbMenagerieEnemy" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <td>Каменоломня</td> 
                <td><asp:TextBox ID="tbCollieryPlayer" runat="server"></asp:TextBox></td>
                 <td><asp:TextBox ID="tbCollieryEnemy" runat="server"></asp:TextBox></td>
             </tr>
             <tr>
                <td>Брилианты</td>
                <td><asp:TextBox ID="tbDiamondsPlayer" runat="server"></asp:TextBox></td>
                 <td><asp:TextBox ID="tbDiamondsEnemy" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <td>Звери</td>
                <td><asp:TextBox ID="tbAnimalsPlayer" runat="server"></asp:TextBox></td>
                 <td><asp:TextBox ID="tbAnimalsEnemy" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Камни</td>
                <td><asp:TextBox ID="tbRocksPlayer" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="tbRocksEnemy" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        
        <p/>
        <table>
            <tr>
                <td>Стоимость карты</td>
            </tr>
               <tr>
                <td>Брилианты</td>
                <td><asp:TextBox ID="tbCostDiamonds" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <td>Звери</td>
                <td><asp:TextBox ID="tbCostAnimals" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Камни</td>
                <td><asp:TextBox ID="tbCostRocks" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <asp:button ID="btSave" runat="server" text="Сохранить" OnClick="btSave_Click" />
    </form>
</body>
</html>
