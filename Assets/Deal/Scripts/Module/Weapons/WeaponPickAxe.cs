using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;


namespace Deal
{
    public class WeaponPickAxe : Weapon
    {
        public override void onAttackEvent()
        {
            base.onAttackEvent();

            if (this.Actor is Hero)
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_stone_hit_02);

            }
        }

        public override void playAttack(float speed = 2)
        {
            base.playAttack(speed);

            if (this.IsGold)
            {
                weaponAnimator.Play("ani_weapon_mininggold", 0, 0);
            }
            else
            {
                weaponAnimator.Play("ani_weapon_mining", 0, 0);
            }
        }
    }
}

