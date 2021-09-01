using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : CollectibleBase
{
    [SerializeField] int _treasureAdded = 1;

    protected override void Collect(Player player)
    {
        player.IncreaseTreasure(_treasureAdded);
    }
}
