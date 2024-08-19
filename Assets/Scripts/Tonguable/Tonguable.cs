using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonguable : MonoBehaviour
{
    private TongueBehaviour tongueBehave;

    public void GotTongued()
    {
        if (tongueBehave == null) { return; }
        tongueBehave.SetTonguePullTarget(this);
    }

    public void GotRetrieved()
    {
        FrogBehaviour.Instance.UpgradeFrog();
        Destroy(gameObject);
    }

    public void SetTongueBehave(TongueBehaviour tongueBehave)
    {
        this.tongueBehave = tongueBehave;
    }
}
