using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public int posX;
    public int posY;
    public BoxMono boxStart;//正在踩的格子
    public List<BoxMono> path = new List<BoxMono>();
    public PropLibrarySO bagLibrary;//背包里的道具
    public PropLibrarySO bagStartLibrary;//背包里的道具

    public bool haveMoved;//移动过
    public float speed;

    public Vector2EventSO attackEvent;

    [Header("战斗属性")]
    public int attackArea = 2;//使用曼哈顿距离

    public float attack = 10;
    public int visionRadius = 3; // 玩家视野半径

    private void Start()
    {
        bagLibrary.propList.Clear();
        foreach (Prop item in bagStartLibrary.propList)
        {
            bagLibrary.propList.Add(item);
        }

        BattleManager.Instance.playerObject = gameObject;
        BattleManager.Instance.player = this;
    }

    /// <summary>
    ///  移动到某个地砖
    /// </summary>
    /// <param name="tile"></param>
    public void Move()
    {
        if (haveMoved)
        {
            return;
        }
        haveMoved = true;
        //transform.position = new Vector3(tile.posX, tile.posY, 0f);
        MoveManager.Instance.vcam.Follow = BattleManager.Instance.player.transform;//先相机回正,回正后移动,移动后follow置空

        transform.DOMove(path[0].transform.position, 1 / speed).OnComplete(() =>
        {
            FogOfWarManager.Instance.UpdateFog(this, BattleManager.Instance.enemyList);//迷雾更新
            TimeManager.Instance.stage = Stage.PlayerTurnBegin;//不应该在这里转换阶段有敌人后
            haveMoved = false;
            MoveManager.Instance.vcam.Follow = null;//先相机回正,回正后移动,移动后follow置空
        });

        posX = path[0].posX;
        posY = path[0].posY;
        //Debug.Log("移动目的地" + posX + "//" + posY);
        boxStart = path[0];
        path.RemoveAt(0);
    }

    public void Attack(BoxMono box)
    {
        attackEvent.RaisEvent(new Vector2(box.posX, box.posY), this);
    }
}