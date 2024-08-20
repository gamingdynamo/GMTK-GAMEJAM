using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTonguable : Tonguable
{
    public override void GotTongued()
    {
        base.GotTongued();
        GameManager.Instance.FlyCount++;
    }
}
