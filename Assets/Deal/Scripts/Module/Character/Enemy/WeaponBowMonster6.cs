using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Deal
{
    public class WeaponBowMonster6 : WeaponRemote
    {
        public override void onAttackEvent()
        {
            //base.onAttackEvent();

            this.BowAttack();
        }


        private void BowAttack()
        {
            for (int i = 0; i < 3; i++)
            {
                Bullet bullet = this.NewBullet();

                Debug.Log("new BowAttack");

                Vector3 leg = bullet.transform.localEulerAngles;
                bullet.transform.localEulerAngles = new Vector3(leg.x, leg.y, leg.z + 7 * (i - 1));
            }
        }
    }

}
