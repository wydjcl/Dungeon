using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxMono : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpriteRenderer BoxSprite;//砖块精灵
    public GameObject BoxText;//如果是障碍物就显示
    public BoxType BoxType;//砖块类型
    public bool isLock;//是否是门锁
    public BoxDataSO boxData;//砖块数据
    public Vector2 boxPos;//砖块坐标
    private bool isClick;//是否被点击

    [Header("格子上的东西,例如道具敌人")]
    public List<Prop> rewardList;//格子上的道具

    public PropDataSO testProp;//测试用道ju

    public bool haveProp;//TODO一个暂时的bool值,未来会被list的count代替
    public bool haveMonster;

    [Header("寻路算法属性")]
    public bool disEntry;//能否点击,也包括是否是障碍物

    public int posX;
    public int posY;
    public BoxMono father;

    [Header("战争迷雾")]
    public FogState fogState = FogState.Unexplored;

    public Color unexploredColor = Color.black;
    public Color exploredColor = Color.gray;
    public Color visibleColor = Color.white;

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
        BoxSprite.sprite = data.BoxSprite;
        BoxType = data.BoxType;
        isLock = data.isLock;
        //disEntry = data.disEntry;

        boxPos = transform.position;
        posX = Mathf.FloorToInt(boxPos.x);
        posY = Mathf.FloorToInt(boxPos.y);
        rewardList = new List<Prop>();
        //测试用
        Prop newProp = new Prop();

        newProp.data = testProp;
        newProp.num = 77;
        rewardList.Add(newProp);
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
        if (!isClick || isLock)//正在被点击切是门锁就不能互动
        {
            return;
        }
        if (disEntry && !haveMonster)//障碍物且不是敌人不能互动
        {
            return;
        }
        if (!(TimeManager.Instance.stage == Stage.PlayerTurn))
        {
            return;
        }
        if (BattleManager.Instance.battleMode)
        {
            //上下左右
            if ((Mathf.Abs(posX - BattleManager.Instance.player.posX) + Mathf.Abs(posY - BattleManager.Instance.player.posY) > 1))
            {
                return;//除了上下左右以外屏蔽
            }
        }

        // AManager.Instance.OnTileClicked(this);
        if (BattleManager.Instance.player.boxStart == this)//如果点自己
        {
            if (haveProp)
            {
                TimeManager.Instance.stage = Stage.PlayerTurnEnd;
                TimeManager.Instance.stage = Stage.PlayerTurnBegin;//TODOend转化为begin
                AddRewardsToBag();
                BoxText.SetActive(false);
                BoxSprite.color = Color.white;
                haveProp = false;
            }
        }
        else
        {
            MoveManager.Instance.PlayerMove(this);
        }
    }

    public void AddRewardsToBag()
    {
        foreach (Prop reward in rewardList)
        {
            // 查找背包里是否已有同名道具
            Prop existing = BattleManager.Instance.player.bagLibrary.propList.Find(p => p.data.propName == reward.data.propName);

            if (existing != null)
            {
                // 已存在 → 增加数量
                existing.num += reward.num;
            }
            else
            {
                // 不存在 → 添加新项
                Prop newProp = new Prop
                {
                    data = reward.data,
                    num = reward.num
                };
                BattleManager.Instance.player.bagLibrary.propList.Add(newProp);
            }
        }

        // 奖励列表清空（可选）
        rewardList.Clear();
    }

    public void UpdateFogVisual()
    {
        switch (fogState)
        {
            case FogState.Unexplored:
                BoxSprite.color = unexploredColor;
                break;

            case FogState.Explored:
                BoxSprite.color = exploredColor;

                break;

            case FogState.Visible:
                BoxSprite.color = visibleColor;
                break;
        }
    }
}