using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Deal.Data;
using Druid;

namespace Deal
{
    /// <summary>
    /// 角色的农场行为
    /// </summary>
    public class CharacterFarm : MonoBehaviour
    {
        private Hero _hero;
        public Hero Hero { get => _hero; set => _hero = value; }

        // 树石头等需要停下在收集等建筑
        private List<CollectableRes> collectableAsset = new List<CollectableRes>();
        // 解锁的建筑
        private List<BuildingUnclockWithAsset> building4unlock = new List<BuildingUnclockWithAsset>();

        // 羊
        public FarmerHouseSheep houseSheep;
        public List<FarmerHouseChiken> houseChikens = new List<FarmerHouseChiken>();

        public List<Transform> follow = new List<Transform>();

        public bool HasSheep()
        {
            return houseSheep != null && houseSheep.sheepState == FarmerHouseSheepState.FOLLOW;
        }


        public void CatchSheep(FarmerHouseSheep Sheep)
        {
            houseSheep = Sheep;
            Sheep.SetFollow(this.follow[0]);
        }

        // 直接放回去
        public void SheepHome()
        {
            this.houseSheep.ReBorn();
            this.houseSheep = null;
        }


        #region 鸡
        public bool CatchChiken(FarmerHouseChiken Chiken)
        {
            if (houseChikens.Count < 4)
            {
                houseChikens.Add(Chiken);

                this.ResetChikenFollow();

                Hero.ChatCharacter.ShowChicken(houseChikens.Count);

                return true;
            }
            return false;
        }

        public bool HasChiken()
        {
            return houseChikens != null && houseChikens.Count > 0;
        }

        public int Chikens()
        {
            return houseChikens.Count;
        }

        // 直接放回去
        public void ChikensHome()
        {
            for (int i = 0; i < houseChikens.Count; i++)
            {
                houseChikens[i].ReBorn();

            }

            houseChikens.Clear();

            Hero.ChatCharacter.HideMark();

        }

        public void ChikenPop()
        {
            if (houseChikens.Count > 0)
            {
                houseChikens.RemoveAt(0);
                this.ResetChikenFollow();
                Hero.ChatCharacter.ShowChicken(houseChikens.Count);
            }

            if (houseChikens.Count == 0)
            {
                Hero.ChatCharacter.HideMark();
            }
        }

        private void ResetChikenFollow()
        {
            for (int i = 0; i < houseChikens.Count; i++)
            {
                houseChikens[i].SetFollow(this.follow[i]);

            }
        }

        #endregion 鸡

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("CharacterFarm " + collision.gameObject.tag);
            if (collision.gameObject.tag == "CollectableRes")
            {
                CollectableRes prop = collision.gameObject.GetComponent<CollectableRes>();
                if (prop != null)
                {
                    this.collectableAsset.Add(prop);

                    this.checkTool();
                }
            }
            else if (collision.gameObject.tag == "Land")
            {
                BuildingUnclockWithAsset prop = collision.gameObject.GetComponent<BuildingUnclockWithAsset>();
                if (prop != null)
                {
                    this.building4unlock.Add(prop);
                }
            }
            else if (collision.gameObject.tag == "Sheep")
            {
                //  抓羊
                FarmerHouseSheep sheep = collision.gameObject.GetComponent<FarmerHouseSheep>();
                if (this.HasSheep() == false && sheep != null)
                {
                    if (sheep.HasWool == true && sheep.sheepState == FarmerHouseSheepState.IDLE)
                    {
                        this.CatchSheep(sheep);
                        sheep.sheepState = FarmerHouseSheepState.FOLLOW;
                    }
                }
            }
            else if (collision.gameObject.tag == "Chiken")
            {
                //  抓鸡
                FarmerHouseChiken chiken = collision.gameObject.GetComponent<FarmerHouseChiken>();

                if (this.Chikens() < 4 && chiken != null)
                {
                    if (chiken.HasWool == true && chiken.sheepState == FarmerHouseSheepState.IDLE)
                    {
                        Debug.Log($"this.Chikens(){this.Chikens()} chiken.HasWool{chiken.HasWool} chiken.sheepState{chiken.sheepState}");

                        if (this.CatchChiken(chiken))
                        {
                            chiken.sheepState = FarmerHouseSheepState.FOLLOW;
                        }

                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerExit2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == "CollectableRes")
            {
                CollectableRes prop = collision.gameObject.GetComponent<CollectableRes>();
                if (prop != null)
                {
                    if (this.collectableAsset.Contains(prop))
                    {
                        this.collectableAsset.Remove(prop);
                        this.checkTool();
                    }

                }
            }
            else if (collision.gameObject.tag == "Land")
            {
                BuildingUnclockWithAsset prop = collision.gameObject.GetComponent<BuildingUnclockWithAsset>();
                if (prop != null)
                {
                    if (this.building4unlock.Contains(prop))
                    {
                        prop.StopDonate();
                        this.building4unlock.Remove(prop);
                    }

                }
            }
        }

        private void checkTool()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            WorkshopToolEnum lackTools = WorkshopToolEnum.None;
            for (int i = 0; i < this.collectableAsset.Count; i++)
            {
                var item = collectableAsset[i];
                Data_CollectableRes dd = item.GetData<Data_CollectableRes>();

                if (item.CanCollect() && !this.Hero.CanCollectAsset(dd.AssetId))
                {
                    lackTools = userData.GetCollectResTool(dd.AssetId);

                    break;
                }
            }

            if (lackTools != WorkshopToolEnum.None)
            {
                this.Hero.ShowLackToolBubble(lackTools);
            }
            else
            {
                this.Hero.HideToolBubble(lackTools);
            }
        }

        /// <summary>
        /// 采集
        /// </summary>
        private void DoCollectAsset()
        {
            //Debug.Log("DoFarm");

            AssetEnum collectAsset = DealUtils.getCollectAssetEnum(this.Hero, this.collectableAsset);

            if (collectAsset != AssetEnum.None)
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

                WorkshopToolEnum tool = userData.GetCollectResTool(collectAsset);
                int fallNum = userData.GetCollectRes(collectAsset);

                if (fallNum > 0)
                {
                    if (collectAsset == AssetEnum.Treasure)
                    {
                        TaskManager.I.OnTaskAction(TaskActionEnum.Treasure);
                        ActivityUtils.DoDailyTask(DailyTaskTypeEnum.action, 1);
                    }
                    this.Hero.PlayWeapon(tool);

                    DealUtils.RoleCollectAsset(this.Hero, this.collectableAsset, collectAsset, tool, fallNum);
                }
            }
        }



        /// <summary>
        /// 给建筑捐钱
        /// </summary>
        private void DoDonate()
        {
            //Debug.Log("DoDonate");

            //this.Hero.Controller.PlayAnimation();

            var item = this.building4unlock[0];
            item.StartDonate(this.Hero);
        }

        void Update()
        {
            CheckCollectAsset();
            CheckUnlockBuild();

        }

        /// <summary>
        /// 检测采集
        /// </summary>
        void CheckCollectAsset()
        {
            if (this.Hero.Controller.IsIdleNoAni())
            {
                if (this.collectableAsset.Count > 0)
                {
                    this.DoCollectAsset();
                }
            }
        }


        /// <summary>
        /// 检测解锁建筑
        /// </summary>
        void CheckUnlockBuild()
        {
            if (this.Hero.Controller.IsIdle())
            //if (this.Hero.Controller.IsIdleNoAni())
            {
                if (this.building4unlock.Count > 0)
                {
                    this.DoDonate();
                }
            }
        }
    }

}
