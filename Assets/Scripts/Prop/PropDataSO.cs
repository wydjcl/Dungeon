using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropDataSO", menuName = "CreateDataSO/Prop/PropDataSO")]
public class PropDataSO : ScriptableObject
{
    public string propName;

    [TextArea]
    public string description;
}