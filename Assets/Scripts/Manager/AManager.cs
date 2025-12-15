using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AManager : MonoBehaviour
{
    public static AManager Instance;
    public PlayerMono player;

    [SerializeField]
    public BoxMono[,] grid; // 地图网格

    [Header("网格参数")]
    public int gridWidth = 50;    // 网格宽度

    public int gridHeight = 50;   // 网格高度
    public Vector2 origin = Vector2.zero; // 初始位置 (0,0)

    [Header("地砖预制体")]
    public BoxMono tilePrefab;                  // 你的 BoxMono 预制体（必须赋值）

    public Transform gridParent;                // 生成地砖的父物体（可选）

    public GameObject bat;
    public BoxMono st;
    public BoxMono et;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitGrid();
    }

    // 初始化网格（假设你已经生成了地砖并赋值）TODO地图生成器
    [ContextMenu("生成地砖")]
    public void InitGrid()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("tilePrefab 未赋值，请在 Inspector 中指定 BoxMono 预制体。");
            return;
        }

        // 清理旧网格
        ClearGrid();

        grid = new BoxMono[gridWidth, gridHeight];

        // 如果没有父物体，就创建一个
        if (gridParent == null)
        {
            GameObject parent = new GameObject("GridParent");
            gridParent = parent.transform;
        }

        // 逐个生成地砖
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // 世界坐标 = 原点 + (x,y)
                Vector3 worldPos = new Vector3(origin.x + x, origin.y + y, 0f);

                BoxMono tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, gridParent);
                tile.name = $"Tile_{x}_{y}";

                // 设置地砖属性
                tile.posX = x;
                tile.posY = y;
                tile.disEntry = false; // 默认可通行
                tile.father = null;
                tile.gCost = 0;
                tile.hCost = 0;

                if (Random.value < 0.2f)
                {
                    tile.disEntry = true;
                    tile.BoxSprite.color = Color.red;
                }
                else
                {
                    if (Random.value < 0.1f)
                    {
                        tile.haveProp = true;
                        tile.BoxSprite.color = Color.blue;
                    }
                    else
                    {
                        if (Random.value < 0.1f)
                        {
                            tile.haveMonster = true;
                            tile.disEntry = true;
                            // tile.BoxSprite.color = Color.yellow;
                            var e = Instantiate(bat, gridParent);
                            e.transform.position = tile.transform.position;
                            var eb = e.GetComponent<EnemyBase>();
                            eb.posX = Mathf.FloorToInt(e.transform.position.x);
                            eb.posY = Mathf.FloorToInt(e.transform.position.y);
                        }
                    }
                }
                if (x == 0 & y == 0)
                {
                    player.boxStart = tile;//生成的第一个格子为角色格子
                }
                grid[x, y] = tile;
            }
        }

        Debug.Log($"InitGrid 完成：生成 {gridWidth}x{gridHeight} 地砖，起点 {origin}。");
    }

    private void ClearGrid()
    {
        if (gridParent != null)
        {
            for (int i = gridParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(gridParent.GetChild(i).gameObject);
            }
        }
        grid = null;
    }

    [ContextMenu("测试寻路")]
    public void test()
    {
        FindPath(st, et);
    }

    // A* 寻路
    public List<BoxMono> FindPath(BoxMono startNode, BoxMono endNode)
    {
        List<BoxMono> openList = new List<BoxMono>();
        HashSet<BoxMono> closedList = new HashSet<BoxMono>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 找到 fCost 最小的节点
            BoxMono currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost ||
                    (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 找到终点
            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            foreach (BoxMono neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.disEntry || closedList.Contains(neighbor))
                    continue;

                float newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.father = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // 没有找到路径
    }

    // 获取邻居节点（上下左右）
    private List<BoxMono> GetNeighbors(BoxMono node)
    {
        List<BoxMono> neighbors = new List<BoxMono>();

        int x = node.posX;
        int y = node.posY;

        if (x - 1 >= 0) neighbors.Add(grid[x - 1, y]);
        if (x + 1 < gridWidth) neighbors.Add(grid[x + 1, y]);
        if (y - 1 >= 0) neighbors.Add(grid[x, y - 1]);
        if (y + 1 < gridHeight) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    // 计算两点之间的距离
    private float GetDistance(BoxMono a, BoxMono b)
    {
        int dx = Mathf.Abs(a.posX - b.posX);
        int dy = Mathf.Abs(a.posY - b.posY);
        return dx + dy; // 曼哈顿距离（适合 4方向寻路）
    }

    // 回溯路径
    private List<BoxMono> RetracePath(BoxMono startNode, BoxMono endNode)
    {
        List<BoxMono> path = new List<BoxMono>();
        BoxMono currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.father;
        }
        path.Reverse();
        string debugPath = "路径: ";
        foreach (BoxMono node in path)
        {
            debugPath += $"({node.posX},{node.posY}) -> ";
        }
        //Debug.Log(debugPath);
        return path;
    }
}