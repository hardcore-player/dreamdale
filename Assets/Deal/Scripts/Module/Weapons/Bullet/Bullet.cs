using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    public enum BulletState
    {
        Idle,
        Flying,
        Dead
    }

    public enum BulletDamageType
    {
        BULLET, //子弹伤害
        BLAST,  // 爆炸伤害
        BULLET_BLAST, // 子弹伤害 和 爆炸伤害
    }

    public class Bullet : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        public float LifeTime = 10f;
        public BulletDamageType _DamagetType = BulletDamageType.BULLET;
        public GameObject pfbBlast;


        // 攻击回调
        protected BattleRoleAtt _attacker;
        protected Vector3 _moveDir;

        protected float _flyInterval = 0f;

        protected BulletState _State = BulletState.Idle;


        public virtual void SetAttacker(BattleRoleAtt roleAtt)
        {
            this._attacker = roleAtt;

            this._flyInterval = 0f;
            this.GetComponent<BoxCollider2D>().enabled = true;

            //this._moveDir = (this._target.transform.position - this._attacker.transform.position).normalized;
            this._moveDir = Vector3.up;

            this._State = BulletState.Flying;
        }

        public virtual void UpdateFly()
        {
            this.transform.Translate(this._moveDir * Time.deltaTime * this.MoveSpeed);
        }


        private void Update()
        {
            if (this._State == BulletState.Flying)
            {
                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= this.LifeTime)
                {
                    this._State = BulletState.Dead;
                    Destroy(this.gameObject);
                }
                else
                {
                    this.UpdateFly();
                }
            }
        }

        protected virtual void DOAttack(BattleRoleBase role)
        {
            //if (this._DamagetType == BulletDamageType.BULLET || this._DamagetType == BulletDamageType.BULLET_BLAST)
            //{
            //    // 子弹伤害
            //    RoleDamageData roleDamage = this._attacker.GetDamage(role.roleAtt);
            //    role.OnAttacked(roleDamage);
            //}

            //this.OnBlasted();

            //this._State = BulletState.Dead;
            //Destroy(this.gameObject);
        }

        protected virtual void OnBlasted()
        {
            if (this.pfbBlast != null)
            {
                GameObject gameObject = Instantiate(this.pfbBlast, this.transform.position, Quaternion.identity);

                if (this._DamagetType == BulletDamageType.BLAST || this._DamagetType == BulletDamageType.BULLET_BLAST)
                {
                    BulletBlast blastBullet = gameObject.GetComponent<BulletBlast>();
                    if (blastBullet != null)
                    {
                        blastBullet.SetAttacker(this._attacker);
                    }
                }

            }
        }


        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Debug.Log("bullet OnTriggerEnter2D" + collision.gameObject.tag);
        //    if (collision.gameObject.tag == TagDefine.PlayerBattle)
        //    {
        //        if (this._attacker.tag == TagDefine.Enemy)
        //        {
        //            Hero hero = collision.gameObject.GetComponentInParent<Hero>();
        //            this.DOAttack(hero);
        //        }

        //    }
        //    else if (collision.gameObject.tag == TagDefine.Enemy)
        //    {
        //        if (this._attacker.tag == TagDefine.PlayerBattle)
        //        {
        //            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //            this.DOAttack(enemy);
        //        }
        //    }
        //}

    }
}
