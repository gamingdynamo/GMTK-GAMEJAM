using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrogTongueAssist : MonoBehaviour
{
    private BoxCollider[] colids;
    private List<Transform> targets = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tonguable"))
        {
            targets.Add(other.transform);
            Debug.Log(other.name + " has entered. List Count: " +  targets.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tonguable"))
        {
            targets.Remove(other.transform);
            Debug.Log(other.name + " has Exit. List Count: " + targets.Count);
        }
    }

    public void SetAimAssistRotation(Vector3 forwardDir)
    {
        transform.forward = forwardDir;
    }

    private void SetAimAssist(int level)
    {
        float halfColidDistance = FrogBehaviour.Instance.FrogScripObj.FrogTongueMaxLength / FrogBehaviour.Instance.FrogScripObj.AimAssistColliderNumber / 2.0f;
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
        SetAimAssist(0);
    }

    public Tonguable LockInTarget()
    {
        if (targets.Count <= 0) { return null; }
        Transform result = null;
        float minAngle = Mathf.Infinity;

        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Angle(transform.forward, targets[i].position - transform.position) < minAngle)
            {
                result = targets[i];
            }
        }
        Debug.Log(result.name);
        return result.GetComponent<Tonguable>();
    }

    private bool StillIntersect(Collider collider)
    {
        for (int i = 0; i < colids.Length; i++)
        {
            if (colids[i].bounds.Intersects(collider.bounds))
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveTonguable(Tonguable tonguable)
    {
        
    }
}
