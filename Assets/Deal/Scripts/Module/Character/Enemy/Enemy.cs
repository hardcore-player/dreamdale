using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Druid;
using System.Threading.Tasks;

namespace Deal
{


    /// <summary>
    /// 怪
    /// </summary>
    public class Enemy : BattleRole
    {
        public Animator aniAttackEffect;

        public int monsterType = 1;

        // id 是在地牢里的唯一编号
        private int _monsterId = 1;
        public int MonsterId { get => _monsterId; set => _monsterId = value; }

        public Transform aniDie;
        public void SetAttNum(float num)
        {
            this.OriAtt = new BattleRoleAtt();

            ExcelData.Monster monster = ConfigManger.I.GetMonsterCfg(this.monsterType);
            if (monster != null)
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

                //雕塑减怪物血
                float buffVal = MathUtils.GetStatueBuff(StatueEnum.Sacred);

                this.OriAtt.MaxHP = (int)(monster.hp * num * (1 - buffVal));
                this.OriAtt.HP = this.OriAtt.MaxHP;
                this.OriAtt.Attack = (int)monster.atk * num;
                this.OriAtt.Crit = 0;
                this.OriAtt.Dodge = 0;
                this.OriAtt.Hit = 0;
                this.OriAtt.DeCrit = 0;
                this.OriAtt.AttackSpeed = monster.speed;

            }
        }

        public override void OnAwake()
        {
            base.OnAwake();
            this.AttackWeapon = this.GetComponentInChildren<AttackWeapon>();
            if (this.AttackWeapon) this.AttackWeapon.Actor = this;

            if (this.aniAttackEffect != null)
            {
                this.aniAttackEffect.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PlayIdle()
        {
            bodyAnimator.speed = 1;
            bodyAnimator.Play($"ani_run_monster{this.skinId}", 0);
            //dust.SetActive(false);
            this.aniDie.gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PlayRun()
        {
            bodyAnimator.speed = 1;
            bodyAnimator.Play($"ani_run_monster{this.skinId}", 0);
            //dust.SetActive(true);
            this.aniDie.gameObject.SetActive(false);
        }

        public override void PlayDie()
        {
            this.aniDie.gameObject.SetActive(true);
            //Sequence s1 = DOTween.Sequence();
            //s1.AppendInterval(0.3f);
            //s1.AppendCallback(() =>
            //{
            //    Destroy(this.gameObject);
            //});

        }


        public override void OnFightExit(BattleRoleBase role)
        {
            base.OnFightExit(role);
            Destroy(this.gameObject);
        }

        public override void OnDie()
        {
            base.OnDie();
            this.PlayDie();
        }

        public override void PlayWeapon(WorkshopToolEnum workshopTool)
        {

            if (workshopTool == WorkshopToolEnum.AttackWeapon)
            {

                int lookRight = this.roleBody.transform.localScale.x > 0 ? 1 : -1;

                Sequence s = DOTween.Sequence();
                s.Append(this.roleBody.DOLocalMoveX(-0.07f * lookRight, 0.1f));
                s.Append(this.roleBody.DOLocalMoveX(0.2f * lookRight, 0.1f));
                s.AppendInterval(0.1f);
                s.Append(this.roleBody.DOLocalMoveX(0f, 0.08f));


                Sequence s1 = DOTween.Sequence();
                s1.AppendInterval(0.1f);
                s1.AppendCallback(() =>
                {
                    if (this.aniAttackEffect != null)
                    {
                        this.aniAttackEffect.gameObject.SetActive(true);
                        this.aniAttackEffect.Play("ani_Monster_AttackEffect", 0, 0);
                    }
                });
                s1.AppendInterval(0.5f);
                s1.AppendCallback(() =>
                {
                    if (this.aniAttackEffect != null)
                    {
                        this.aniAttackEffect.gameObject.SetActive(false);
                    }
                });
            }
        }
    }
}

