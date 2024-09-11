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
    public class BuildingChickenRoost2Hero : MonoBehaviour
    {

        public Building_ChickenRoost factory;
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

            Data_ChickenRoost data = factory.GetData<Data_ChickenRoost>();

            //if (data.ToNum <= 0) return;


            for (int i = 0; i < data.RoostItem.Count; i++)
            {
                if (data.RoostItem[i].State == CollectableResState.DONE)
                {
                    // 更新资产
                    UserData userData = Druid.DataManager.I.Get<UserData>(DataDefine.UserData);
                    AssetEnum key = AssetEnum.Egg;
                    int num = 1;
                    bool added = userData.AddAsset(key, num);
                    if (added)
                    {
                        DropPropPlank item = this.factory.OnHeroPick(i);

                        item.SetFlyToPlayer(mHero);
                        item.BezierToTarget(mHero.center.position);

                        SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

                    }
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
            //    Debug.Log("BuildingAssetWarehouse OnTriggerExit2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == TagDefine.Player)
            {
                mHero = null;
            }
        }
    }
}

