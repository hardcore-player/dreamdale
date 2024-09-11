using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;
using Deal.Env;
using Deal.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cinemachine;

namespace Deal
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public class GameSceneManager : DealScene
    {
        public CinemachineVirtualCamera cinemachine1;
        public CinemachineVirtualCamera cinemachine2;
        public Transform World;

        private bool IsLoadOver = false;
        private bool IsInit = false;


        public override async UniTask LoadScene()
        {
            this.IsLoadOver = false;
            this.IsInit = false;

            this.SetProgress(0);
            this.SetProgress(10);
            await UniTask.DelayFrame(1);


            // 加载地图
            GameObject map = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Map, this.World);
            MapRender mapRender = map.GetComponent<MapRender>();

            MapManager.I.mapRender = mapRender;

            this.SetProgress(10);

            await UniTask.DelayFrame(1);

            //渲染地图
            MapManager.I.LoadMap();

            this.SetProgress(30);

            await UniTask.DelayFrame(1);

            //GameObject player = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Player, this.World);
            //Hero hero = player.GetComponent<Hero>();
            //PlayManager.I.mHero = hero;

            //// 如果保存了地点，回到地点
            //if (PlayManager.I.enterDungeonPos != Vector3.zero)
            //{
            //    hero.transform.position = PlayManager.I.enterDungeonPos;
            //    PlayManager.I.enterDungeonPos = Vector3.zero;
            //}
            //else
            //{
            //    hero.transform.position = new Vector3(4, 4, 0);
            //}

            //cinemachine1.Follow = hero.transform;

            //hero.CloseRoleBattle();

            this.SetProgress(10);

            //UIJoystick uIJoystick = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIJoyStick, UILayer.Ground) as UIJoystick;
            //hero.Controller.variableJoystick = uIJoystick.joystick;

            //await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGameMain);

            this.IsInit = true;

        }

        /// <summary>
        /// 建筑已经加载完了
        /// </summary>
        /// <returns></returns>
        public bool isSceneLoaded()
        {
            //int loadBuildings = MapManager.I.mapRender.buildings.Count;

            //MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            //int needBuildings = mapData.Data.buildings.Count;

            //Debug.Log("loadBuildings" + loadBuildings);
            //Debug.Log("loadBuildings = needBuildings" + needBuildings);

            //return loadBuildings == needBuildings;

            return true;
        }

        private async void OnMapLoaded()
        {
            GameObject player = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Player, this.World);
            Hero hero = player.GetComponent<Hero>();
            PlayManager.I.mHero = hero;

            // 如果保存了地点，回到地点
            if (PlayManager.I.enterDungeonPos != Vector3.zero)
            {
                hero.transform.position = PlayManager.I.enterDungeonPos;
                PlayManager.I.enterDungeonPos = Vector3.zero;
            }
            else
            {
                hero.transform.position = new Vector3(4, 4, 0);
            }

            cinemachine1.Follow = hero.transform;

            hero.CloseRoleBattle();

            UIJoystick uIJoystick = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIJoyStick, UILayer.Ground) as UIJoystick;
            hero.Controller.variableJoystick = uIJoystick.joystick;

            await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGameMain);

            MapRender mapManager = MapManager.I.mapRender;
            mapManager.MapInited();

            this.SetProgress(20);
            await UniTask.Delay(1000);

            mapManager.MapInited1();

            SoundManager.I.Setup();
            SoundManager.I.playMusic(AddressbalePathEnum.OGG_BGM_Main);
        }

        private void Update()
        {
            if (this.IsInit == true && this.IsLoadOver == true) return;
            if (this.IsInit == false) return;


            if (this.IsLoadOver == false)
            {
                if (this.isSceneLoaded())
                {
                    this.IsLoadOver = true;

                    this.SetProgress(20);
                }
            }

            if (this.IsInit == true && this.IsLoadOver == true)
            {
                this.OnMapLoaded();
                //MapRender mapManager = MapManager.I.mapRender;
                //mapManager.MapInited();
            }
        }
    }
}