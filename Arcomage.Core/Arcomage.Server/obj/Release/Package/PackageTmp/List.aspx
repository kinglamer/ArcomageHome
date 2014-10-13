﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Arcomage.Server.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
             <asp:gridview ID="gvTable" runat="server" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" CellPadding="4"
 ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowEditing="gvTable_RowEditing" OnRowUpdating="gvTable_RowUpdating">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />

        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" /> 
                <Columns>
                
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True"  />
                    <asp:BoundField DataField="name" HeaderText="name" InsertVisible="False" />

        <asp:BoundField DataField="PlayerTower" HeaderText="PlayerTower" />
        <asp:BoundField DataField="PlayerWall" HeaderText="PlayerWall"  />

        <asp:BoundField DataField="EnemyTower" HeaderText="EnemyTower"  />
        <asp:BoundField DataField="EnemyWall" HeaderText="EnemyWall"  />
                 
          <asp:BoundField DataField="PlayerAnimals" HeaderText="PlayerAnimals"  />
          <asp:BoundField DataField="PlayerDiamonds" HeaderText="PlayerDiamonds"  />    
          <asp:BoundField DataField="PlayerRocks" HeaderText="PlayerRocks"  />

          <asp:BoundField DataField="EnemyAnimals" HeaderText="EnemyAnimals"  /> 
          <asp:BoundField DataField="EnemyDiamonds" HeaderText="EnemyDiamonds"  />
          <asp:BoundField DataField="EnemyRocks" HeaderText="EnemyRocks"  /> 

          <asp:BoundField DataField="PlayerMenagerie" HeaderText="PlayerMenagerie"  />
          <asp:BoundField DataField="PlayerDiamondMines" HeaderText="PlayerDiamondMines"  /> 
          <asp:BoundField DataField="PlayerColliery" HeaderText="PlayerColliery"  />

          <asp:BoundField DataField="EnemyMenagerie" HeaderText="EnemyMenagerie"  /> 
          <asp:BoundField DataField="EnemyDiamondMines" HeaderText="EnemyDiamondMines"  />
          <asp:BoundField DataField="EnemyColliery" HeaderText="EnemyColliery"  />    

    </Columns>
     
        </asp:gridview>
    </div>
    </form>
</body>
</html>
