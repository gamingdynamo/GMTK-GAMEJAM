using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FrogTongueAssist : MonoBehaviour
{
    [SerializeField]
    private RectTransform lookSignTrans;
    [SerializeField]
    private Image[] lookSignImages;
    [SerializeField]
    private Transform tongueStartTrans;

    private BoxCollider[] colids;
    private Transform closestTarget = null;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Tonguable") && !other.CompareTag("TonguableGround")) { return; }
        if (closestTarget == null)
        {
            closestTarget = other.transform;
        }
        else
        {
            if (Vector3.Angle(transform.forward, other.transform.position - transform.position) < Vector3.Angle(transform.forward, closestTarget.position - transform.position))
            {
                closestTarget = other.transform;
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f) { return; }
        if (!InAngles(closestTarget))
        {
            closestTarget = null;
        }
        RaycastHit hit;
        if (closestTarget != null 
            && Physics.Raycast(transform.position, (closestTarget.position - transform.position).normalized, out hit, FrogBehaviour.Instance.GetTongueLength())
            && hit.transform == closestTarget)
        {
            SetLockOnActive(true);
            SetLookSignPosition();
        }
        else
        {
            SetLockOnActive(false);
            closestTarget = null;
        }

    }

    public void SetLockOnActive(bool tF)
    {
        lookSignTrans.gameObject.SetActive(tF);
    }

    public void SetAimAssistRotation(Vector3 forwardDir)
    {
        transform.forward = forwardDir;
    }

    public void SetAimAssist(float halfColidDistance)
    {
        for (int i = 0; i < colids.Length; i++)
        {
            colids[i].center = (1.0f + 2.0f * i) * halfColidDistance * Vector3.forward;
            float colidZsize = 2.0f * halfColidDistance;
            float colidXYsize = 2.0f * (1.0f + 2.0f * i) * halfColidDistance * Mathf.Tan(FrogBehaviour.Instance.FrogScripObj.AimAssistAngle * 0.5f * Mathf.Deg2Rad);
            colids[i].size = new Vector3(colidXYsize, colidXYsize, colidZsize);
        }
    }

    public void InitBoxColliders()
    {
        for (int i = 0; i < FrogBehaviour.Instance.FrogScripObj.AimAssistColliderNumber; i++)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }
        colids = GetComponents<BoxCollider>();
    }

    public Tonguable LockInTarget()
    {
        return InAngles(closestTarget) ? closestTarget.GetComponent<Tonguable>() : null;
    }

    private void SetLookSignPosition()
    {
        lookSignTrans.position = FrogBehaviour.Instance.CameraMain.WorldToScreenPoint(closestTarget.position);
        Tonguable targetTonguable = closestTarget.GetComponent<Tonguable>();
        Color targetColor = Color.white;
        switch (targetTonguable)
        {
            case PushTonguable:
                targetColor = (targetTonguable as PushTonguable).PushScriptObj.PushRequiredLevel > FrogBehaviour.Instance.FrogTongueLevel ? Color.red : Color.green;    
                break;
            case PullTonguable:
                targetColor = (targetTonguable as PullTonguable).PullScriptObj.PullRequiredLevel > FrogBehaviour.Instance.FrogTongueLevel ? Color.red : Color.green;
                break;
        }
        for (int i = 0; i < lookSignImages.Length; i++)
        {
            lookSignImages[i].color = targetColor;
        }
    }
    
    private bool InAngles(Transform trans)
    {
        if (trans == null) { return false; }
        return Vector3.Angle(FrogBehaviour.Instance.AimPosition() - transform.position, trans.position - transform.position) <= FrogBehaviour.Instance.FrogScripObj.AimAssistAngle * 0.5f;
    }
}
