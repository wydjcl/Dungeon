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
            if (health > 10)
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
            else
            {
                Die();
            }
        }
    }

    public void SetVisible(bool visible)
    {
        sp.enabled = visible; // 超出玩家视野就隐藏
    }

    public void Die()
    {
        Debug.Log("死了");
        BattleManager.Instance.enemyList.Remove(this);
        box.haveMonster = false;
        box.disEntry = false;
        Destroy(gameObject);
    }
}