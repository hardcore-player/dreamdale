using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Deal.Data;
using Druid;

namespace Deal.Env
{
    /// <summary>
    /// 宝藏树
    /// </summary>
    public class Res_Treasure : CollectableRes
    {
        public SpriteRenderer srHole;
        public SpriteRenderer srTreasure;

        /// <summary>
        /// 更新表现
        /// </summary>
        public override void UpdateView()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.AssetLeft > 0)
            {
                this.srTreasure.gameObject.SetActive(true);
                this.srHole.gameObject.SetActive(false);
            }
            else
            {
                this.srTreasure.gameObject.SetActive(false);
                this.srHole.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 掉落触发
        /// </summary>
        public override void OnFallAsset(int fallNum, RoleBase role)
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 古代碎片
            int asNums = userData.Data.TodayAncientShard;
            int[] a = new[] { 100, 30, 10, 3, 1, 0 };

            bool isAncientShard = false;
            if (asNums < a.Length - 1)
            {
                int per = a[asNums];

                int random = Druid.Utils.MathUtils.RandomInt(0, 100);

                if (random < per)
                {
                    isAncientShard = true;
                }
            }

            if (isAncientShard)
            {
                DealUtils.newDropItem(AssetEnum.AncientShard, 5, transform.position);

                userData.Data.TodayAncientShard++;

                userData.Save();
            }
            else
            {
                //for (int i = 0; i < 10; i++)
                //{
                DealUtils.newDropItem(AssetEnum.Gold, 20, transform.position);
                //}
            }


            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(new Vector3(1.1f, 0.9f, 1), 0.1f));
            s.Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.05f));
            s.AppendCallback(() =>
            {
                this.UpdateView();
            });

        }
    }
}


