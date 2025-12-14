using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxMono : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite BoxSprite;
    public BoxType BoxType;
    public bool disEntry;
    public bool isLock;
    public BoxDataSO boxData;
    public Vector2 boxPos;
    private bool isClick;

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
        disEntry = data.disEntry;

        boxPos = transform.position;
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
        if (!isClick)
        {
            return;
        }
        MoveManager.Instance.PlayerMove(boxPos);
    }
}