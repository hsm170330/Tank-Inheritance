using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : PowerUpBase
{
    [SerializeField] Material body;
    protected override void PowerUp(Player player)
    {
        player.invincible = true;
        if (body != null)
        {
            body.SetColor("_Color", Color.white);
        }
    }

    protected override void PowerDown(Player player)
    {
        player.invincible = false;
        body.SetColor("_Color", Color.green);
    }
}
