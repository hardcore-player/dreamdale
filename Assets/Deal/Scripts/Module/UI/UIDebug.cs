using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using TMPro;

namespace Deal.UI
{
    public class UIDebug : UIBase
    {

        public GameObject panel;

        public TMP_Dropdown dropdownAsset;

        private int dropdownAssetId = 0;

        public void OnDropdownChange(int a)
        {
            this.dropdownAssetId = dropdownAsset.value;
        }

        public void OnOpen()
        {
            if (panel.activeInHierarchy)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
            }
        }

        /// <summary>
        /// 存档
        /// </summary>
        public void OnTestSave()
        {

            NetUtils.postSlotSave((ok) =>
            {
                Debug.Log("OnTestSave" + ok);
            });

            //UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //string aa = LitJson.JsonMapper.ToJson(userData.Data);
            //Debug.Log(aa);

            //object oo = LitJsonEx.JsonMapper.ToObject<UserDataSlot>(aa);

            //UserDataSlot dd = oo as UserDataSlot;
            //Debug.Log(dd);


            //MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            //string aa = LitJson.JsonMapper.ToJson(mapData.Data);
            //Debug.Log(aa);

            //MapDataSlot dd = LitJsonEx.JsonMapper.ToObject<MapDataSlot>(aa);

            //UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            //userData.Save();

            //MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            //mapData.Save();

            //mapData.Load();
        }

        /// <summary>
        /// 资产变化
        /// </summary>
        public void OnTestAssetsChange()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            userData.AddBagBase(99999999);

            userData.AddAsset(AssetEnum.Gold, 50000);
            userData.AddAsset(AssetEnum.Wood, 5000);
            userData.AddAsset(AssetEnum.Stone, 5000);
            userData.AddAsset(AssetEnum.Pumpkin, 5000);
            userData.AddAsset(AssetEnum.Apple, 5000);
            userData.AddAsset(AssetEnum.Plank, 5000);
            userData.AddAsset(AssetEnum.Fish, 5000);
            userData.AddAsset(AssetEnum.Gem, 5000);
            userData.AddAsset(AssetEnum.Wool, 5000);
            userData.AddAsset(AssetEnum.Brick, 5000);
            userData.AddAsset(AssetEnum.Grain, 5000);
            userData.AddAsset(AssetEnum.Bread, 5000);
            userData.AddAsset(AssetEnum.Nail, 5000);
            userData.AddAsset(AssetEnum.Iron, 5000);
            userData.AddAsset(AssetEnum.Scroll, 5000);
            userData.AddAsset(AssetEnum.Sapphire, 500);
            userData.AddAsset(AssetEnum.Ruby, 500);
            userData.AddAsset(AssetEnum.Emerald, 5000);
            userData.AddAsset(AssetEnum.Amethyst, 5000);
            userData.AddAsset(AssetEnum.Potion, 5000);
            userData.AddAsset(AssetEnum.AncientShard, 5000);
            userData.AddAsset(AssetEnum.SwordRune, 500);
            userData.AddAsset(AssetEnum.Cone, 5000);
            userData.AddAsset(AssetEnum.Cactus, 5000);
            userData.AddAsset(AssetEnum.Carrot, 5000);
            userData.AddAsset(AssetEnum.WinterWood, 5000);
            userData.AddAsset(AssetEnum.DeadWood, 5000);
            userData.AddAsset(AssetEnum.Orange, 5000);
            userData.AddAsset(AssetEnum.Bamboo, 5000);
            userData.AddAsset(AssetEnum.BambooTissue, 5000);
            userData.AddAsset(AssetEnum.FishSoup, 5000);
            userData.AddAsset(AssetEnum.Egg, 5000);
            userData.AddAsset(AssetEnum.DeadWoodPlank, 5000);
            userData.AddAsset(AssetEnum.Cactus, 5000);
            userData.AddAsset(AssetEnum.Carrot, 5000);

            userData.AddAsset(AssetEnum.Ticket, 2);

            //userData.FireUpdate();

            //MapManager.I.mapRender.OpenTiles(0, 9, 3, 3);
        }

        public void OnTestTaskComplete()
        {
            Data_Task task = TaskManager.I.GetTask();
            task.IsDone = true;
            TaskManager.I.OnCompleteTask();
        }

        public void OnTestTaskRefresh()
        {
            Data_Task task = TaskManager.I.GetTask();
            task.TaskId -= 1;
            task.IsDone = true;
            TaskManager.I.OnCompleteTask();
        }

        public void OnTestTaskLast()
        {
            Data_Task task = TaskManager.I.GetTask();
            task.TaskId -= 2;
            task.IsDone = true;
            TaskManager.I.OnCompleteTask();
        }

        public void OnBattleScne()
        {
            PlayManager.I.LoadBattleScene();
        }

        public void OnGameScne()
        {
            PlayManager.I.LoadGameScene();
        }

        public async void OnNewEnemy(int i)
        {
            GameObject go = null;
            Vector3 position = Vector3.zero;

            if (i == 1)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster1, position);
            }
            else if (i == 2)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster2, position);
            }
            else if (i == 3)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster3, position);
            }
            else if (i == 4)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster4, position);
            }
            else if (i == 5)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster5, position);
            }
            else if (i == 6)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster6, position);
            }
            else if (i == 7)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster7, position);
            }
            else if (i == 8)
            {
                go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Monster8, position);
            }

            Enemy enemy = go.GetComponent<Enemy>();
            enemy.transform.position = position;
            //PlayManager.I.mEnemies.Add(enemy);
        }

        public void OnOrderPause()
        {
            ShopUtils.CheckOrder();
        }

        public void OnMoveSpeedClick()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.speed = 15;

        }

        public void OnDebugClick()
        {
            if (Config.DebugMode == 0)
            {
                Config.DebugMode = 1;
            }
            else
            {
                Config.DebugMode = 0;
            }

        }
    }
}
