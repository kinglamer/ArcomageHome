using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcomage.Entity
{
    public enum TypePlayer
    {
        AI, Human
    }

    public enum GameAction
    {
        PlayCard, 
        PlayCardAgain,
        DropCard,
        EndMove
    }

    public enum Attributes
    {
        Tower,
        Wall,
        DiamondMines,
        Menagerie,
        Colliery,
        Diamonds,
        Animals,
        Rocks,
        DirectDamage
    }

    public enum Target
    {
        Player,
        Enemy
    }

    public enum Specifications
    {
        NotSet = 0,
        PlayerTower,
        PlayerWall,
        PlayerDiamondMines,
        PlayerMenagerie,
        PlayerColliery,
        PlayerDiamonds,
        PlayerAnimals,
        PlayerRocks,

        EnemyTower,
        EnemyWall,
        EnemyDiamondMines,
        EnemyMenagerie,
        EnemyColliery,
        EnemyDiamonds,
        EnemyAnimals,
        EnemyRocks,

        CostDiamonds,
        CostAnimals,
        CostRocks,

        PlayAgain,
        EnemyDirectDamage,
        PlayerDirectDamage
    }


}
