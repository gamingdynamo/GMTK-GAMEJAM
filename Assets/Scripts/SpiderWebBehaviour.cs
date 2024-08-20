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
    private Transform foodParentTrans;
    [SerializeField]
    private Transform popUpLocation;
    [HideInInspector]
    public float webBounceYPos = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FrogGroundCheck"))
        {
            if (FrogBehaviour.Instance.FrogScaleLevel >= GameManager.Instance.FlyNeeded)
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

    private void Start()
    {
        GameManager.Instance.FlyNeeded = foodParentTrans.childCount;
        GameManager.Instance.FlyCount = 0;
        Time.timeScale = 1.0f;
        HUDHandler.Instance.StartLevel();
        InputHandler.Instance.ControlsActive(true);
        InputHandler.Instance.FocusCursor();
    }

    private void Update()
    {
        spiderWebMat.SetFloat("_WebBounceYPos", webBounceYPos);
    }

    public void FrogDrop()
    {
        spiderWebAnmt.SetTrigger("FrogDrop");
        HUDHandler.Instance.ShowPopUpHud(popUpLocation.position);
    }

    public void FrogJump()
    {
        spiderWebAnmt.SetTrigger("FrogJump");
        HUDHandler.Instance.HidePopUpHud();
    }

    public void FrogBreak()
    {
        spiderWebAnmt.SetTrigger("FrogBreak");
        FinishLevel();
    }

    public void FinishLevel()
    {
        InputHandler.Instance.ControlsActive(false);
        FrogBehaviour.Instance.FinishLevel();
        HUDHandler.Instance.FinishLevel();
    }
}
