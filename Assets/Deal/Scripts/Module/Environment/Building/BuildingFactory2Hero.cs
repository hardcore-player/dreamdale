using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;
using Druid;

namespace Deal.Env
{

    /// <summary>
    /// 工厂,碰到玩家，给玩家资产
    /// </summary>
    public class BuildingFactory2Hero : MonoBehaviour
    {

        public BuildingChangeFactory factory;
        private Hero mHero;
        private float _flyInterval;


        public void Update()
        {
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



        private void Fly2Hero()
        {

            if (mHero == null) return;

            DataChangeFactory data = factory.GetData<DataChangeFactory>();

            if (data.ToNum <= 0) return;

            // 更新资产
            UserData userData = Druid.DataManager.I.Get<UserData>(DataDefine.UserData);
            AssetEnum key = data.ToAsset;
            int num = 1;
            bool added = userData.AddAsset(key, num);
            if (added)
            {
                DropPropPlank item = this.factory.OnHeroPick();

                item.SetFlyToPlayer(mHero);
                item.BezierToTarget(mHero.center.position);

                SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

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
            //    Debug.Log("BuildingAssetWarehouse OnTriggerExit2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == TagDefine.Player)
            {
                mHero = null;
            }
        }
    }
}

