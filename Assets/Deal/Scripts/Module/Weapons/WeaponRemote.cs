using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Druid;

namespace Deal
{

    /// <summary>
    /// 远程武器
    /// </summary>
    public class WeaponRemote : AttackWeapon
    {
        public Bullet pfbBullet;

        public BattleRoleBase _tagert;

        public override void onAttackEvent()
        {
            base.onAttackEvent();
            this.NewBullet();
        }

        public override float attckLength()
        {
            return 4f;
        }


        public override void attackTarget(BattleRoleBase tagert)
        {
            //base.playAttack();
            weaponAnimator.Play("ani_attack", 0, 0);

            this._tagert = tagert;
        }


        protected Bullet NewBullet()
        {
            Bullet bullet = Instantiate<Bullet>(this.pfbBullet, this.transform.position, this.transform.rotation);
            //bullet.gameObject.SetActive(true);
            //BattleRoleBase battleRole = this.Actor as BattleRoleBase;
            //bullet.SetAttacker(battleRole.roleAtt);
            return bullet;
        }

    }
}

