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
    public class PvpSceneManager : DealScene
    {
        public CinemachineVirtualCamera cinemachine;
        public Transform World;

        public HeroPvp HeroLeft;
        public HeroPvp HeroRight;

        public override async UniTask LoadScene()
        {

            GameObject player = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Player, this.World);
            Hero hero = player.GetComponent<Hero>();
            hero.transform.position = new Vector3(0, 0, 0);
            PlayManager.I.mHero = hero;
            //cinemachine.Follow = hero.transform;

            hero.CloseRoleBattle();

            UIJoystick uIJoystick = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIJoyStick, UILayer.Ground) as UIJoystick;
            hero.Controller.variableJoystick = uIJoystick.joystick;

            this.SetProgress(100);
        }

    }
}