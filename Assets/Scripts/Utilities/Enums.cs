using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//枚举 房间类型
//[Flags]//枚举多选
public enum BoxType
{
}

public enum Stage
{
    PlayerTurnBegin,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurnBegin,
    EnemyTurnEnd
}