using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//枚举 房间类型
//[Flags]//枚举多选
public enum BoxType
{
}

/// <summary>
/// 回合阶段
/// </summary>
public enum Stage
{
    PlayerTurnBegin,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurnBegin,
    EnemyTurnEnd
}

/// <summary>
/// 迷雾类型
/// </summary>
public enum FogState//迷雾类型
{
    Unexplored,   // 黑色
    Explored,     // 灰色
    Visible       // 可见
}