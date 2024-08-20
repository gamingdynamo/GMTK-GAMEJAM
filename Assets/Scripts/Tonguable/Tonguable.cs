using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonguable : MonoBehaviour
{
    private TongueBehaviour tongueBehave;
    public TongueBehaviour TongueBehave {  get { return tongueBehave; } set { tongueBehave = value; } }
    

    public virtual void GotTongued()
    {
        if (TongueBehave == null) { return; }
        TongueBehave.SetTonguePullTarget(this);
    }
    
    public virtual void GotRetrieved()
    {
        FrogBehaviour.Instance.UpgradeFrog(FlyUpgradeType.AllType);
        Destroy(gameObject);
    }
}
