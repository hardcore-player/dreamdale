using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Druid;
using Druid.Utils;
using UnityEngine;


namespace Deal.Dungeon
{


    /// <summary>
    /// 传送门
    /// </summary>
    public class Arena_Teleport : MonoBehaviour
    {


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                this.OnOpen();
            }
        }


        public void OnOpen()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();
            PlayManager.I.LoadGameScene();
        }
    }

}
