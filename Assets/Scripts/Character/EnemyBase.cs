using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int attack;
    public int posX;
    public int posY;
    public SpriteRenderer sp;
    public BoxMono box;

    private void Awake()
    {
        health = maxHealth;
        posX = Mathf.FloorToInt(transform.position.x);
        posY = Mathf.FloorToInt(transform.position.y);
    }

    public void TakeDamage(Vector2 pos)
    {
        if (posX == pos.x && posY == pos.y)
        {
            health -= 10;
            UpTextManager.Instance.CreateUpText(this.transform.position, "-10");
            sp.DOFade(0f, 0.2f)
            .SetLoops(1, LoopType.Yoyo)
            .OnComplete(() =>
            {
                // 动画完成后执行 DOKill
                sp.DOFade(1f, 0.1f).OnComplete(() =>
                {
                    sp.DOKill();
                });
            });
        }
    }
}