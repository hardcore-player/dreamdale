using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;


namespace Deal
{
    public class WeaponAxe : Weapon
    {


        public override void onAttackEvent()
        {
            base.onAttackEvent();

            if (this.Actor is Hero)
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_hit_wood_01);
            }
        }

        public override void playAttack(float speed = 2)
        {
            base.playAttack(speed);
            if (this.IsGold)
            {
                weaponAnimator.Play("ani_weapon_axegold", 0, 0);
            }
            else
            {
                weaponAnimator.Play("ani_weapon_axe", 0, 0);
            }

        }
    }
}

