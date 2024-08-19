using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonguable : MonoBehaviour
{
    private TongueBehaviour tongueBehave;
    public TongueBehaviour TongueBehave {  get { return tongueBehave; } set { tongueBehave = value; } }
    private bool tongued = false;
    public bool Tongued { get { return tongued; } set { tongued = value; } }

    public virtual void GotTongued()
    {
        Debug.Log("Warning: Tonguable should not be used directly");
        if (TongueBehave == null) { return; }
        TongueBehave.SetTonguePullTarget(this);
        tongued = true;
    }
    
    public virtual void GotRetrieved()
    {
        Debug.Log("Warning: Tonguable should not be used directly");
        FrogBehaviour.Instance.UpgradeFrog(FlyUpgradeType.AllType);
        Destroy(gameObject);
    }
}
