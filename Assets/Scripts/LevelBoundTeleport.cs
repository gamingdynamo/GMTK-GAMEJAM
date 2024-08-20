using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundTeleport : MonoBehaviour
{
    [SerializeField]
    private Transform[] teleportObjectsTrans;
    private Vector3[] teleportPositions;

    private void Start()
    {
        InitTeleportPositions();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.position.y < transform.position.y)
        {
            int index = FindTransforIndex(other.transform);
            if (index < 0) { return; }
            TeleportDropedObject(index);
        }
    }

    private int FindTransforIndex(Transform dropedObject)
    {
        for (int i = 0; i < teleportObjectsTrans.Length; i++)
        {
            if (teleportObjectsTrans[i] == dropedObject)
            {
                return i;
            }
        }
        return -1;
    }

    private void TeleportDropedObject(int i)
    {
        teleportObjectsTrans[i].position = teleportPositions[i];
    }

    private void InitTeleportPositions()
    {
        teleportPositions = new Vector3[teleportObjectsTrans.Length];
        for (int i = 0; i < teleportObjectsTrans.Length; ++i)
        {
            teleportPositions[i] = teleportObjectsTrans[i].position;
        }
    }
}
