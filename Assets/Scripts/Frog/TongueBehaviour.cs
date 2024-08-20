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
    private Tonguable tongueTarget = null;
    private Tonguable tonguePullTarget = null;
    private FrogTongueState currTongueState = FrogTongueState.Resting;

    private float tongueStateTime = 0.0f;
    private float tongueT = 0.0f;

    private const float TongueLength = 0.79f;

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
        Vector3 endPos = (currTongueState == FrogTongueState.Retrieving ? tongueEndPostion : (tongueTarget == null ? tongueEndPostion : tongueTarget.transform.position));
        float directionalT = (currTongueState == FrogTongueState.Holding ? 1.0f : (currTongueState == FrogTongueState.Shooting ? tongueT : 1.0f - tongueT));

        //Scale
        float maxScale = Vector3.Distance(endPos, tongueStartTrans.position) / TongueLength;
        transform.localScale = new Vector3(FrogBehaviour.Instance.FrogScripObj.FrogTongueRadiusScale, FrogBehaviour.Instance.FrogScripObj.FrogTongueRadiusScale, Mathf.Lerp(0.0f, maxScale, directionalT));

        //Position
        transform.position = Vector3.Lerp(tongueStartTrans.position, endPos, directionalT * 0.5f * TongueLength);
        TonguableMovement(endPos, directionalT);

        //Rotation
        transform.forward = (endPos - tongueStartTrans.position).normalized;
    }

    private void TonguableMovement(Vector3 endPos, float directionalT)
    {
        if (tonguePullTarget == null) { return; }
        if (tonguePullTarget is HookTonguable) { return; }
        tonguePullTarget.transform.position = Vector3.Lerp(tongueStartTrans.position, endPos, directionalT);
        if (tonguePullTarget is PullTonguable && Vector3.Distance(transform.position, tonguePullTarget.transform.position) < (tonguePullTarget as PullTonguable).PullScriptObj.PullStopDistance)
        {
            tonguePullTarget = null;
        }
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
                if (tongueTarget != null)
                {
                    tongueTarget.TongueBehave = this;
                }
                break;
            case FrogTongueState.Holding:
                tongueStateTime = FrogBehaviour.Instance.FrogScripObj.FrogTongueHoldTime;
                if (tongueTarget != null)
                {
                    tongueTarget.GotTongued();
                }
                break;
            case FrogTongueState.Retrieving:
                tongueStateTime = FrogBehaviour.Instance.FrogScripObj.FrogTongueRetrieveTime;
                if (tongueTarget != null)
                {
                    tongueEndPostion = tongueTarget.transform.position;
                    HookMovement();
                }
                break;
            case FrogTongueState.Resting:
                if (tongueTarget != null)
                {
                    tongueTarget.GotRetrieved();
                }
                tongueTarget = null;
                tonguePullTarget = null;
                tongueStateTime = 0.0f;
                tongueEndPostion = Vector3.zero;
                tongueRend.enabled = false;
                break;
        }
    }

    private void HookMovement()
    {
        if (!(tongueTarget is HookTonguable)) { return; }
        HookTonguableScriptableObject scriptObj = (tongueTarget as HookTonguable).HookScriptObj;
        FrogBehaviour.Instance.JumpAction(scriptObj.HookRushTime, scriptObj.HookRushVelocity * (tongueTarget.transform.position - transform.position).normalized);
    }

    public void SetTonguePullTarget(Tonguable pullTarget)
    {
        tonguePullTarget = pullTarget;
    }

    public void ShootTongue(Tonguable target)
    {
        tongueTarget = target;
        NextTongueState();
    }

    public void ShootTongue(Vector3 EndPosition)
    {
        tongueEndPostion = FrogBehaviour.Instance.GetTongueLength() * (EndPosition - tongueStartTrans.position).normalized + tongueStartTrans.position;
        NextTongueState();

    }
}
