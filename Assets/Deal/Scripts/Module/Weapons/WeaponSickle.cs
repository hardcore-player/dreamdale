using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;


namespace Deal
{
    /// <summary>
    /// 镰刀
    /// </summary>
    public class WeaponSickle : Weapon
    {
        public override void onAttackEvent()
        {
            base.onAttackEvent();

            if (this.Actor is Hero)
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_dig_01);

            }
        }

        public override void playAttack(float speed = 2)
        {
            base.playAttack(speed);
            weaponAnimator.Play("ani_sickle", 0, 0);
        }
    }
}

