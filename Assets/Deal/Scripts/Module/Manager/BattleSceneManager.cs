using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;
using Deal.Env;
using Deal.UI;
using Druid;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cinemachine;
using Deal.Dungeon;

namespace Deal
{
    /// <summary>
    /// 战斗场景管理器
    /// </summary>
    public class BattleSceneManager : DealScene
    {
        public CinemachineVirtualCamera cinemachine;
        public Transform World;

        public override async UniTask LoadScene()
        {
            // 加载地图
            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            Data_DungeonStage dataStage = dungeon.Data.DataStage;
            double dungeonLv = dataStage.DungeonBuilding;
            DataDungeonLevel dataDungeonLevel = dataStage.DataDungeonLevel;

            int mapId = 1;
            int lvId = 0;
            bool isTmpLevel = false;
            if (dataDungeonLevel != null)
            {
                mapId = dataDungeonLevel.mapId;
                lvId = dataDungeonLevel.lvId;
                isTmpLevel = dataDungeonLevel.isTmp;
            }

            string mapName = "";

            if (isTmpLevel == true)
            {
                mapName = $"Assets/Deal/GameResources/Prefabs/Dungeon/Map/Map1.prefab";
            }
            else
            {
                mapId = (lvId % 10) + 1;
                if (dataDungeonLevel != null)
                {
                    dataDungeonLevel.mapId = mapId;
                }

                mapName = $"Assets/Deal/GameResources/Prefabs/Dungeon/Map/Map1_{mapId}.prefab";
            }

            GameObject map = await ResManager.I.GetInstantiate(mapName, this.World);

            DungeonManager.I.DungeonLevel = map.GetComponent<Dungeon_Level>();

            await UniTask.DelayFrame(1);

            GameObject player = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Player, this.World);
            Hero hero = player.GetComponent<Hero>();
            PlayManager.I.mHero = hero;
            cinemachine.Follow = hero.transform;

            hero.OpenRoleBattle();
            DungeonManager.I.StartLevel();



            UIJoystick uIJoystick = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIJoyStick) as UIJoystick;
            hero.Controller.variableJoystick = uIJoystick.joystick;

            await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGameMain);
            await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIMonster);


            await UniTask.DelayFrame(1);

            DataManager.I.Save(DataDefine.DungeonData);
            DataManager.I.Save(DataDefine.MapData);

            this.SetProgress(100);

            //DungeonManager.I.WaveStart();
            //await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGameMain);

        }

    }
}