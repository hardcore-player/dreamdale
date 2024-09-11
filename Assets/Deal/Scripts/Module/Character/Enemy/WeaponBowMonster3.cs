using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Deal
{
    public class WeaponBowMonster3 : WeaponRemote
    {
        public override void onAttackEvent()
        {
            base.onAttackEvent();

            this.BowAttack();
        }


        private async void BowAttack()
        {
            for (int i = 0; i < 3; i++)
            {
                this.NewBullet();
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }
        }
    }

}
