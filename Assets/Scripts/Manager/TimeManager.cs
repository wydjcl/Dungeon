using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int time;
    public float timeTest;//测试用的转换阶段的时间
    public Stage stage;
    public PlayerMono player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        init();
    }

    private void Update()
    {
        if (stage == Stage.PlayerTurnBegin)
        {
            stage = Stage.PlayerTurn;
            return;
        }
        //if (stage == Stage.PlayerTurnEnd)
        //{
        //    timeTest += Time.deltaTime;
        //    if (timeTest >= 1f)
        //    {
        //        timeTest = 0f;
        //        stage = Stage.PlayerTurn;//测试用实现,在回合结束一秒后转到回合中
        //    }
        //    return;
        //}
        if (stage == Stage.PlayerTurn)
        {
            // Debug.Log("player.path.Count" + player.path.Count);
            if (player.path.Count != 0)
            {
                player.Move();
            }

            return;
        }
    }

    private void init()
    {
        time = 0;
        stage = Stage.PlayerTurn;
    }

    /// <summary>
    /// 转换阶段
    /// </summary>
    public void ChangeStage()//TODO增加广播
    {
        if (stage == Stage.PlayerTurnBegin)
        {
            time++;
            stage = Stage.PlayerTurn;
            return;
        }
        if (stage == Stage.PlayerTurn)
        {
            stage = Stage.PlayerTurnEnd;
            return;
        }
        if (stage == Stage.PlayerTurnEnd)
        {
            stage = Stage.EnemyTurnBegin;
            return;
        }
        if (stage == Stage.EnemyTurnBegin)
        {
            stage = Stage.EnemyTurnEnd;
            return;
        }
        if (stage == Stage.EnemyTurnEnd)
        {
            stage = Stage.PlayerTurnBegin;
            return;
        }
    }
}