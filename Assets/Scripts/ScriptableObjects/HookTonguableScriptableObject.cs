using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookTonguableScriptableObject", menuName = "ScriptableObjects/HookTonguableScriptableObject")]
public class HookTonguableScriptableObject : ScriptableObject
{
    public float HookRushTime = 0.1f;
    public float HookRushVelocity = 5.0f;
}
