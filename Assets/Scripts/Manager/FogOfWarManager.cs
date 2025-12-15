using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public static FogOfWarManager Instance;

    // public int gridWidth;
    //public int gridHeight;
    // public BoxMono[,] grid; // 地图格子
    // public BoxMono[,] grid;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 更新玩家视野
    /// </summary>
    public void UpdateFog(PlayerMono player, List<EnemyBase> enemies)
    {
        // 先全部设为已探索但不可见
        for (int x = 0; x < AManager.Instance.gridWidth; x++)
        {
            for (int y = 0; y < AManager.Instance.gridHeight; y++)
            {
                if (AManager.Instance.grid[x, y].fogState == FogState.Visible)
                {
                    AManager.Instance.grid[x, y].fogState = FogState.Explored;
                    AManager.Instance.grid[x, y].UpdateFogVisual();
                }
            }
        }

        // 玩家位置和视野半径
        Vector2Int playerPos = new Vector2Int(player.posX, player.posY);
        int vision = player.visionRadius;

        // 更新玩家周围格子为可见
        for (int dx = -vision; dx <= vision; dx++)
        {
            for (int dy = -vision; dy <= vision; dy++)
            {
                int nx = playerPos.x + dx;
                int ny = playerPos.y + dy;

                if (nx >= 0 && nx < AManager.Instance.gridWidth && ny >= 0 && ny < AManager.Instance.gridHeight)
                {
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    if (dist <= vision)
                    {
                        AManager.Instance.grid[nx, ny].fogState = FogState.Visible;
                        AManager.Instance.grid[nx, ny].UpdateFogVisual();
                    }
                }
            }
        }

        // 更新敌人可见性
        foreach (var enemy in enemies)
        {
            float dist = Vector2Int.Distance(playerPos, new Vector2Int(enemy.posX, enemy.posY));
            enemy.SetVisible(dist <= vision);
        }
    }
}