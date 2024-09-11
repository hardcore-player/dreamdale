using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;


namespace Deal
{
    public class WeaponFishingRod : Weapon
    {

        public override void playAttack(float speed = 2)
        {
            //base.playAttack(speed);
            weaponAnimator.speed = 2;
            weaponAnimator.Play("ani_fishing_ok", 0, 0);


            //weaponAnimator.Play("ani_fishing_go", 0, 0);
        }

        public void playStartAttack(float speed = 2)
        {
            weaponAnimator.speed = 2;
            weaponAnimator.Play("ani_fishing_go", 0, 0);
            //weaponAnimator.Play("ani_fishing_ok", 0, 0);
        }

        public void playAttackStrong(float speed = 2)
        {
            weaponAnimator.speed = 2;
            weaponAnimator.Play("ani_fishing_idle", 0, 0);
            //weaponAnimator.Play("ani_fishing_ok", 0, 0);
        }

    }
}

