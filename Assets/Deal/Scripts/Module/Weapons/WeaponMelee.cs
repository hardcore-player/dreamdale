using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Deal
{

    /// <summary>
    /// 近战武器
    /// </summary>
    public class WeaponMelee : AttackWeapon
    {
        // 武器持有人
        public BoxCollider2D boxCollider;


        public override void attackTarget(BattleRoleBase tagert)
        {
            weaponAnimator.speed = this.AniSpeed;
            weaponAnimator.Play("ani_attack", 0, 0);
        }

        public override void playAttack(float speed = 2)
        {
            Debug.Log("ani_attack");
            base.playAttack(speed);
            weaponAnimator.Play("ani_attack", 0, 0);
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Debug.Log("WeaponMelee Enter2D" + collision.gameObject.tag);

        //    if (collision.gameObject.tag == TagDefine.Enemy)
        //    {
        //        if (this.Actor is Hero)
        //        {
        //            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //            BattleRoleBase battleRole = this.Actor as BattleRoleBase;
        //            RoleDamageData roleDamage = battleRole.GetAttackDamage(enemy);
        //            enemy.OnAttacked(roleDamage);
        //        }

        //    }
        //    else if (collision.gameObject.tag == TagDefine.PlayerBattle)
        //    {
        //        if (this.Actor is Enemy)
        //        {
        //            Hero hero = collision.gameObject.GetComponentInParent<Hero>();
        //            BattleRoleBase battleRole = this.Actor as BattleRoleBase;
        //            RoleDamageData roleDamage = battleRole.GetAttackDamage(hero);
        //            hero.OnAttacked(roleDamage);
        //        }

        //    }
        //}
    }
}

