using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;
using Druid;

namespace Deal.Env
{

    /// <summary>
    /// 仓库,碰到玩家，给玩家资产
    /// </summary>
    public class BuildingAssetWarehouse : BuildingBase
    {

        private Hero mHero;
        private float _flyInterval;

        /// <summary>
        /// 更新资产
        /// </summary>
        /// <param name="asset"></param>
        public virtual void UpdateAsset(Data_GameAsset asset)
        {
        }

        /// <summary>
        /// 更新资产
        /// </summary>
        /// <param name="asset"></param>
        public virtual void UpdateAsset(AssetEnum assetEnum, int num)
        {
        }


        public virtual bool IsFull()
        {
            return false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (mHero)
            {
                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= 0.1f)
                {
                    Fly2Hero();
                    this._flyInterval = 0;
                }
            }
        }

        private int _curNumUnit = 1;
        private int _curTime = 0;
        private int numPerTime(int remain, int space)
        {
            if (_curTime >= 10)
                _curNumUnit = 10;
            if (_curTime >= 19)
                _curNumUnit = 100;
            _curTime++;
            int ret = _curNumUnit;
            if (remain < ret && remain > 0)
                ret = remain;
            if (ret > space)
                ret = space;
            return ret;
        }

        private void Fly2Hero()
        {

            if (mHero == null) return;

            DataResWarehouse data = this.GetData<DataResWarehouse>();


            List<AssetEnum> assetEnums = new List<AssetEnum>();

            foreach (KeyValuePair<AssetEnum, int> asset in data.Assets)
            {
                if (asset.Value > 0)
                {
                    assetEnums.Add(asset.Key);
                }
            }

            for (int i = 0; i < assetEnums.Count; i++)
            {
                // 更新资产
                UserData userData = Druid.DataManager.I.Get<UserData>(DataDefine.UserData);
                AssetEnum key = assetEnums[i];
                int num = numPerTime(data.Assets[key], userData.BagSpaceRemain());
                bool added = userData.AddAsset(key, num);
                if (added && num > 0)
                {
                    data.Assets[key] -= num;

                    this.UpdateAsset(key, data.Assets[key]);

                    DropProp item = PlayManager.I.SpawnDropItem();
                    if (center != null)
                    {
                        item.transform.position = center.position;
                    }
                    else
                    {
                        item.transform.position = transform.position;
                    }

                    item.SetProp(key, 1);
                    item.SetFlyToPlayer(mHero);
                    item.BezierToTarget(mHero.center.position);

                    SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("BuildingAssetWarehouse OnTriggerEnter2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == TagDefine.Player)
            {
                mHero = collision.gameObject.GetComponentInParent<Hero>();
                this._flyInterval = 0;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("BuildingAssetWarehouse OnTriggerExit2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == TagDefine.Player)
            {
                mHero = null;
                _curNumUnit = 1;
                _curTime = 0;
            }
        }
    }
}

