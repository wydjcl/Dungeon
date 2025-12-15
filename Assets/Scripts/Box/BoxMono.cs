using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxMono : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpriteRenderer BoxSprite;
    public BoxType BoxType;
    public bool isLock;
    public BoxDataSO boxData;
    public Vector2 boxPos;
    private bool isClick;

    [Header("寻路算法属性")]
    public bool disEntry;//能否点击,也包括是否是障碍物

    public int posX;
    public int posY;
    public BoxMono father;

    // A* 算法需要的代价
    public float gCost;     // 从起点到当前节点的代价

    public float hCost;     // 从当前节点到终点的预估代价
    public float fCost => gCost + hCost; // 总代价

    private void Start()
    {
        Init(boxData);
    }

    private void Update()
    {
        if (MoveManager.Instance.isMoveMap)
        {
            isClick = false;
        }
    }

    private void Init(BoxDataSO data)
    {
        BoxSprite = data.BoxSprite;
        BoxType = data.BoxType;
        isLock = data.isLock;
        //disEntry = data.disEntry;

        boxPos = transform.position;
        posX = Mathf.FloorToInt(boxPos.x);
        posY = Mathf.FloorToInt(boxPos.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MoveManager.Instance.isMoveMap)
        {
            return;
        }
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isClick || disEntry || isLock)
        {
            return;
        }

        // AManager.Instance.OnTileClicked(this);
        MoveManager.Instance.PlayerMove(this);
    }
}