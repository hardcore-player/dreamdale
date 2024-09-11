using System;
using System.Collections.Generic;
using Deal;
using DG.Tweening;
using Druid;
using UnityEngine;

[Serializable]
public class SO_DungeonStrategy
{
    [Header("开始时长")]
    public float StartTime = 0;
    [Header("几秒一次")]
    public float RefreshTime = 3;
    [Header("一次几个")]
    public float RefreshCount = 3;
    [Header("小怪种类")]
    public float MonsterType = 1;

    /// <summary>
    /// 
    /// </summary>
    private float _interval = 0;
    private bool _isStart = false;

    public void update(float dt)
    {
        this._interval += dt;

        if (this._isStart == false)
        {
            if (this._interval >= this.StartTime)
            {
                this._isStart = true;
                this._interval = 0;

                this.spawnMonster();
            }
        }
        else
        {
            if (this._interval >= this.RefreshTime)
            {
                this._interval = 0;
                this.spawnMonster();
            }
        }

    }


    private void spawnMonster()
    {
        for (int i = 0; i < this.RefreshCount; i++)
        {
            int x = Druid.Utils.MathUtils.RandomInt(-3, 3);
            int y = Druid.Utils.MathUtils.RandomInt(-4, 4);
            Vector3 position = new Vector3(x, y, 0);

            DealUtils.newPopMonsterXX(position);

            Sequence s = DOTween.Sequence();
            s.AppendInterval(2f);
            s.AppendCallback(() =>
            {
                this.NewEnemey(position);
            });


        }
    }

    private async void NewEnemey(Vector3 position)
    {
        GameObject go = null;

        if (this.MonsterType == 1)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster1, position);
        }
        else if (this.MonsterType == 2)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster2, position);
        }
        else if (this.MonsterType == 3)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster3, position);
        }
        else if (this.MonsterType == 4)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster4, position);
        }
        else if (this.MonsterType == 5)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster5, position);
        }
        else if (this.MonsterType == 6)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster6, position);
        }
        else if (this.MonsterType == 7)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster7, position);
        }
        else if (this.MonsterType == 8)
        {
            go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster8, position);
        }

        Enemy enemy = go.GetComponent<Enemy>();
        enemy.transform.position = position;
        //PlayManager.I.mEnemies.Add(enemy);
    }

}
