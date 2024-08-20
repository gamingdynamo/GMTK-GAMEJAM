using System.Collections;
using System.Collections.Generic;
// using UnityEditorInternal;
using UnityEngine;

public class FlyTonguable : Tonguable
{
    [SerializeField]
    private FlyUpgradeType upgradeType = FlyUpgradeType.AllType;
    [SerializeField]
    private FlyBehaveMode behaveMode = FlyBehaveMode.Stay;
    [SerializeField]
    private FlyScriptableObject flyScriptObj;
    [SerializeField]
    [Tooltip("For Patroll mode, this will be the center. For RandomFly, this will be the original Position")]
    private Transform targetTrans;

    private bool tongued = false;
    public bool Tongued { get { return tongued; } set { tongued = value; } }

    private float flyTimer = 0.0f;
    private int posIndex = 0;
    private bool isFlying = false;
    private List<Vector3> positions = new List<Vector3>();

    private void Start()
    {
        if (behaveMode == FlyBehaveMode.Stay) { return; }
        if (flyScriptObj == null)
        {
            Debug.Log("ERROR: fly Scriptable Object is missing in gameobject: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        if (targetTrans == null)
        {
            Debug.Log("ERROR: Fly target Transform is missing in gameObject: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        if (behaveMode == FlyBehaveMode.Patroll)
        {
            transform.position = flyScriptObj.FlyPatrollRadius * targetTrans.right + targetTrans.position;
            transform.forward = (flyScriptObj.FlyPatrollClockWise ? -1.0f : 1.0f) * targetTrans.forward;
        }
        if (behaveMode != FlyBehaveMode.FlyRandom)
        {
            flyTimer = flyScriptObj.FlyRestTime;
            InitFlyPostions();
        }
    }

    private void FixedUpdate()
    {
        if (behaveMode == FlyBehaveMode.Stay || Tongued) { return; }
        switch (behaveMode)
        {
            case FlyBehaveMode.Patroll:
                FlyPatrolling();
                break;
            case FlyBehaveMode.FlyRandom:
                FlyRandom();
                break;
        }
    }

    private void FlyPatrolling()
    {
        transform.RotateAround(targetTrans.position, targetTrans.up, (flyScriptObj.FlyPatrollClockWise ? 1.0f : -1.0f) * 360.0f * Time.fixedDeltaTime / flyScriptObj.FlyPatrollTimeInterval);
    }

    private void FlyRandom()
    {
        flyTimer = Mathf.Clamp(flyTimer - Time.fixedDeltaTime, 0.0f, Mathf.Infinity);
        if (isFlying)
        {
            transform.position = Vector3.Lerp(positions[posIndex], positions[(posIndex + positions.Count - 1) % positions.Count], flyTimer / flyScriptObj.RandomPosReachTime);
            transform.forward = (positions[posIndex] - positions[(posIndex + positions.Count - 1) % positions.Count]).normalized;
        }
        if (flyTimer > 0) { return; }
        if (!isFlying)
        {
            isFlying = true;
            posIndex++;
        }
        else
        {
            isFlying = false;
        }
        if (posIndex < positions.Count)
        {
            flyTimer = isFlying ? flyScriptObj.RandomPosReachTime : flyScriptObj.RandomPosPauseTime;
        }
        else
        {
            posIndex = -1;
            flyTimer = flyScriptObj.FlyRestTime;
            transform.position = targetTrans.position;
            transform.up = targetTrans.up;
            isFlying = false;
            InitFlyPostions();
        }
    }

    private void InitFlyPostions()
    {
        int positionNumber = Random.Range(flyScriptObj.RandomPosMinNumber, flyScriptObj.RandomPosMaxNumber);
        positions = new List<Vector3>();
        for (int i = 0; i < positionNumber; i++)
        {
            float widthAngle = Random.Range(0.0f, flyScriptObj.RandomPosMaxAngle);
            float rotationAngle = Random.Range(0.0f, 360.0f);
            float distance = Random.Range(flyScriptObj.RandomPosMinDistance, flyScriptObj.RandomPosMaxDistance);
            Vector3 result = rotationAngleDirection(GameManager.AngleToVector(rotationAngle));
            result = distance * Mathf.Tan(Mathf.Deg2Rad * widthAngle) * result;
            result += distance * targetTrans.up + targetTrans.position;
            positions.Add(result);
        }
        positions.Add(targetTrans.position);
    }

    private Vector3 rotationAngleDirection(Vector2 dir)
    {
        return (dir.x * targetTrans.right + dir.y * targetTrans.forward).normalized;
    }

    public override void GotTongued()
    {
        base.GotTongued();
        tongued = true;
    }

    public override void GotRetrieved()
    {
        GameManager.Instance.FlyCount++;
        FrogBehaviour.Instance.UpgradeFrog(upgradeType);
        Destroy(gameObject);
    }
}
