using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropLibrarySO", menuName = "CreateDataSO/Prop/PropLibrarySO")]
public class PropLibrarySO : ScriptableObject
{
    [SerializeField]
    public List<Prop> propList;
}

[System.Serializable]
public class Prop
{
    public PropDataSO data;
    public int num;
}