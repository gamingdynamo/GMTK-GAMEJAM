using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTonguable : Tonguable
{
    [SerializeField]
    private HookTonguableScriptableObject hookScriptObj;
    public HookTonguableScriptableObject HookScriptObj { get { return hookScriptObj; } }

    public override void GotRetrieved()
    {
        
    }
}
