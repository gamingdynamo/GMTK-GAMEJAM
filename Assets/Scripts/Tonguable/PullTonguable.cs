using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTonguable : Tonguable
{
    [SerializeField]
    private Rigidbody rig;
    [SerializeField]
    private PullTonguableScriptableObject pullScriptObj;
    public PullTonguableScriptableObject PullScriptObj { get { return pullScriptObj; } }

    private void Start()
    {
        if (pullScriptObj == null)
        {
            Debug.Log("ERROR: Pull Scriptable Object is missing in " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        if (rig == null) { rig = GetComponent<Rigidbody>(); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Frog"))
        {
            if (PullScriptObj.IsMovable) { return; }
            rig.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Frog"))
        {
            if (PullScriptObj.IsMovable) { return; }
            rig.isKinematic = false;
        }
    }

    public override void GotTongued()
    {
        if (FrogBehaviour.Instance.FrogTongueLevel < pullScriptObj.PullRequiredLevel) { return; }
        if (TongueBehave == null) { return; }
        TongueBehave.SetTonguePullTarget(this);
    }

    public override void GotRetrieved()
    {

    }
}
