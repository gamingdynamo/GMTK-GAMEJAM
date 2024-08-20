using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTonguable : Tonguable
{
    [SerializeField]
    private Rigidbody rig;
    [SerializeField]
    private PushTonguableScriptableObject pushScriptObj;

    private void Start()
    {
        if (pushScriptObj == null)
        {
            Debug.Log("ERROR: push Scriptable Object is missing in " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        if (rig == null) { rig = GetComponent<Rigidbody>(); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Frog"))
        {
            if (pushScriptObj.IsMovable) { return; }
            rig.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Frog"))
        {
            if (pushScriptObj.IsMovable) { return; }
            rig.isKinematic = false;
        }
    }

    public override void GotTongued()
    {
        if (FrogBehaviour.Instance.FrogTongueLevel < pushScriptObj.PushRequiredLevel) { return; }
        rig.AddForce(pushScriptObj.PushForce * (transform.position - TongueBehave.transform.position).normalized, ForceMode.Impulse);
        transform.localScale = Mathf.Clamp(transform.localScale.magnitude + pushScriptObj.PushIncreaseSize, 0.0f, pushScriptObj.MaxSize) * transform.localScale.normalized;
    }

    public override void GotRetrieved()
    {
        
    }
}
