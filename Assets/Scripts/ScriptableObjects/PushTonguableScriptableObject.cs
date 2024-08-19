using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushTonguableScriptableObject", menuName = "ScriptableObjects/PushTonguableScriptableObject")]
public class PushTonguableScriptableObject : ScriptableObject
{
    public int PushRequiredLevel = 0;
    public float PushForce = 50.0f;
    public float PushIncreaseSize = 0.1f;
    public float MaxSize = 2.0f;
}
