using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebBehaviour : MonoBehaviour
{
    [SerializeField]
    private Material spiderWebMat;
    [SerializeField]
    private Animator spiderWebAnmt;
    [SerializeField]
    private int webBreakSizeLevel = 0;
    [HideInInspector]
    public float webBounceYPos = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FrogGroundCheck"))
        {
            if (FrogBehaviour.Instance.FrogScaleLevel >= webBreakSizeLevel)
            {
                FrogBreak();
                return;
            }
            FrogDrop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FrogGroundCheck"))
        {
            FrogJump();
        }
    }

    private void Update()
    {
        spiderWebMat.SetFloat("_WebBounceYPos", webBounceYPos);
    }

    public void FrogDrop()
    {
        spiderWebAnmt.SetTrigger("FrogDrop");
    }

    public void FrogJump()
    {
        spiderWebAnmt.SetTrigger("FrogJump");
    }

    public void FrogBreak()
    {
        spiderWebAnmt.SetTrigger("FrogBreak");
    }
}
