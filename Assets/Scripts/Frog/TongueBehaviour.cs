using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform tongueStartTrans;
    [SerializeField]
    private MeshRenderer tongueRend;

    private Vector3 tongueEndPostion = Vector3.zero;
    private Transform tongueTargetTrans = null;
    private FrogTongueState currTongueState = FrogTongueState.Resting;

    private float tongueStateTime = 0.0f;
    private float tongueT = 0.0f;

    private void Update()
    {
        TongueMovement();
    }

    private void TongueMovement()
    {
        if (currTongueState == FrogTongueState.Resting) { return; }
        tongueT = Mathf.Clamp01(tongueT + Time.deltaTime / tongueStateTime);
        TongueTransformChange();
        if (tongueT >= 1.0f)
        {
            NextTongueState();
        }
    }

    private void TongueTransformChange()
    {
        if (currTongueState == FrogTongueState.Holding) { return; }
        Vector3 endPos = (tongueTargetTrans == null ? tongueEndPostion : tongueTargetTrans.position);
        float directionalT = (currTongueState == FrogTongueState.Shooting ? tongueT : 1.0f - tongueT);

        //Scale
        float maxScale = 0.5f * Vector3.Distance(endPos, tongueStartTrans.position);
        transform.localScale = new Vector3(FrogBehaviour.Instance.FrogScripObj.FrogTongueRadiusScale, Mathf.Lerp(0.0f, maxScale, directionalT), FrogBehaviour.Instance.FrogScripObj.FrogTongueRadiusScale);

        //Position
        transform.position = Vector3.Lerp(tongueStartTrans.position, endPos, directionalT * 0.5f);

        //Rotation
        transform.up = (endPos - tongueStartTrans.position).normalized;
    }

    private void NextTongueState()
    {
        currTongueState = (FrogTongueState)((int)(currTongueState + 1) % (int)FrogTongueState.EndOfEnum);
        tongueT = 0.0f;
        switch (currTongueState)
        {
            case FrogTongueState.Shooting:
                tongueRend.enabled = true;
                tongueStateTime = FrogBehaviour.Instance.FrogScripObj.FrogTongueShootTime;
                break;
            case FrogTongueState.Holding:
                tongueStateTime = FrogBehaviour.Instance.FrogScripObj.FrogTongueHoldTime;
                break;
            case FrogTongueState.Retrieving:
                tongueStateTime = FrogBehaviour.Instance.FrogScripObj.FrogTongueRetrieveTime;
                if (tongueTargetTrans != null)
                {
                    tongueEndPostion = tongueTargetTrans.position;
                    tongueTargetTrans = null;
                }
                break;
            case FrogTongueState.Resting:
                tongueStateTime = 0.0f;
                tongueEndPostion = Vector3.zero;
                tongueRend.enabled = false;
                break;
        }
    }

    public void ShootTongue(Transform targetTrans)
    {
        tongueStartTrans = targetTrans;
        NextTongueState();
    }

    public void ShootTongue(Vector3 EndPosition)
    {
        tongueEndPostion = FrogBehaviour.Instance.FrogScripObj.FrogTongueMaxLength * (EndPosition - tongueStartTrans.position).normalized + tongueStartTrans.position;
        NextTongueState();

    }
}
