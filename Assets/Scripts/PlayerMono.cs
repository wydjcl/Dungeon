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

    public float speed;

    /// <summary>
    ///  移动到某个地砖
    /// </summary>
    /// <param name="tile"></param>
    public void Move()
    {
        //transform.position = new Vector3(tile.posX, tile.posY, 0f);
        MoveManager.Instance.vcam.Follow = GameManager.Instance.player.transform;//先相机回正,回正后移动,移动后follow置空
        TimeManager.Instance.stage = Stage.PlayerTurnEnd;
        transform.DOMove(path[0].transform.position, 1 / speed).OnComplete(() =>
        {
            TimeManager.Instance.stage = Stage.PlayerTurnBegin;
            MoveManager.Instance.vcam.Follow = null;//先相机回正,回正后移动,移动后follow置空
        });

        posX = path[0].posX;
        posY = path[0].posY;
        //Debug.Log("移动目的地" + posX + "//" + posY);
        boxStart = path[0];
        path.RemoveAt(0);
    }

    public void textMove()
    {
    }
}