using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxDataSO", menuName = "CreateDataSO/Box/BoxDataSO")]
public class BoxDataSO : ScriptableObject
{
    public Sprite BoxSprite;
    public BoxType BoxType;

    /// <summary>
    /// 是否可以走进去,true为不能进去,例如墙,门
    /// </summary>
    public bool disEntry;

    /// <summary>
    /// 是否上锁
    /// </summary>
    public bool isLock;
}