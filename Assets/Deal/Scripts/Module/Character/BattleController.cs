using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using DG.Tweening;

namespace Deal
{
    public enum BattleTypeEnum
    {
        NONE,
        PVE,
        PVP,
    }

    public enum BattleStateEnum
    {
        NONE,
        FIGHT_ENTER,
        FIGHT_START,
        FIGHT_END,
        FIGHT_EXIT,
        FIGHT_PAUSE,
    }

    public class BattleController : Singleton<BattleController>
    {
        // 战斗状态
        private BattleStateEnum _battleState = BattleStateEnum.NONE;
        private BattleTypeEnum _battleType = BattleTypeEnum.NONE;
        public BattleStateEnum BattleState { get => _battleState; set => _battleState = value; }

        private float _battleInterval = 0;

        private BattleRole roleSelf;  // 自己
        private BattleRole roleTaget;  // 对手

        public DelegateHeroFightEnter OnHeroFightEnter;
        public DelegateHeroFightStart OnHeroFightStart;
        public DelegateHeroFightEnd OnHeroFightEnd;
        public DelegateHeroFightExit OnHeroFightExit;

        public bool OnRoleEnter(BattleRole role1, BattleRole role2)
        {
            if (this.BattleState != BattleStateEnum.NONE) return false;

            if (role2 is HeroPvp)
            {
                this._battleType = BattleTypeEnum.PVP;
            }
            else if (role2 is Enemy)
            {
                this._battleType = BattleTypeEnum.PVE;
            }

            if (role1.IsDie() || role2.IsDie()) return false;


            if (this._battleType == BattleTypeEnum.PVE)
            {
                this._checkFight(role1, role2);
            }
            else
            {
                this.OnFightEnter(role1, role2);
            }


            return true;
        }


        public void OnFightEnter(BattleRole role1, BattleRole role2)
        {
            this.BattleState = BattleStateEnum.FIGHT_ENTER;

            Debug.Log("OnFightEnter" + this.BattleState);
            this.roleSelf = role1;
            this.roleTaget = role2;

            role2.OnFightEnter(role1);
            role1.OnFightEnter(role2);

            this._battleInterval = 0;

            if (OnHeroFightEnter != null)
            {
                this.OnHeroFightEnter(role2);
            }
        }


        public void OnFightStart()
        {
            Debug.Log("OnFightStart");

            roleSelf.OnFightStart(roleTaget);
            roleTaget.OnFightStart(roleSelf);

            this._battleInterval = 0;
            this.BattleState = BattleStateEnum.FIGHT_START;

            if (OnHeroFightStart != null)
            {
                this.OnHeroFightStart(roleTaget);
            }
        }



        public void OnFightEnd()
        {
            Debug.Log("OnFightEnd");


            roleSelf.OnFightEnd(roleTaget);
            roleTaget.OnFightEnd(roleSelf);

            this._battleInterval = 0;
            this.BattleState = BattleStateEnum.FIGHT_END;

            if (OnHeroFightEnd != null)
            {
                this.OnHeroFightEnd(roleTaget);
            }
        }


        public void OnFightExit()
        {
            Debug.Log("OnFightExit");

            roleSelf.OnFightExit(roleTaget);
            roleTaget.OnFightExit(roleSelf);

            this._battleInterval = 0;
            this.BattleState = BattleStateEnum.FIGHT_EXIT;

            if (true)
            {
                roleSelf = null;
                roleTaget = null;
                this.BattleState = BattleStateEnum.NONE;
            }

            if (OnHeroFightExit != null)
            {
                this.OnHeroFightExit(roleTaget);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void _updateFightStart()
        {
            if (roleSelf.IsDie()) return;
            if (roleTaget.IsDie()) return;

            if (roleSelf.UpdateAttack())
            {
                Debug.Log("我打死对手");

                this.OnFightEnd();

            }

            if (roleTaget.UpdateAttack())
            {
                Debug.Log("对手打死我");
                if (this._battleType == BattleTypeEnum.PVE)
                {
                    // 自己死了，会视频复活
                    Debug.Log("我打死对手 BattleTypeEnum.PVE");
                }
                else if (this._battleType == BattleTypeEnum.PVP)
                {
                    // pvp 自己死了，结束战斗
                    this.OnFightEnd();
                }

                return;
            }

        }

        private void _updateFightEnter()
        {
            this._battleInterval += Time.deltaTime;

            if (this._battleInterval >= 0.5f)
            {
                this.OnFightStart();
                this._battleInterval = 0;
            }
        }

        private void _updateFightEnd()
        {
            this._battleInterval += Time.deltaTime;

            if (this._battleInterval >= 0.5f)
            {
                this.OnFightExit();
                this._battleInterval = 0;
            }
        }


        private void Update()
        {

            if (this.roleSelf == null || this.roleTaget == null) return;
            //Debug.Log("this.BattleState2  " + this.BattleState);


            if (this.BattleState == BattleStateEnum.FIGHT_START)
            {
                this._updateFightStart();
            }
            else if (this.BattleState == BattleStateEnum.FIGHT_ENTER)
            {
                this._updateFightEnter();
            }
            else if (this.BattleState == BattleStateEnum.FIGHT_END)
            {
                this._updateFightEnd();
            }
        }


        private void _checkFight(BattleRole self, BattleRole enemy)
        {
            int ex = (int)(enemy.transform.position.x - 0.5f);
            int ey = (int)(enemy.transform.position.y - 0.5f);

            float _ex = enemy.transform.position.x;
            float _ey = enemy.transform.position.y;
            float _hx = self.transform.position.x;
            float _hy = self.transform.position.y;

            int xxx = 0; //左中右
            int yyy = 0; //上中下

            if (Mathf.Abs(_hx - _ex) <= 0.5)
            {
                xxx = 0;
            }
            else if (_hx > _ex)
            {
                xxx = 1;
            }
            else
            {
                xxx = -1;
            }

            if (Mathf.Abs(_hy - _ey) <= 0.5)
            {
                yyy = 0;
            }
            else if (_hy > _ey)
            {
                yyy = 1;
            }
            else
            {
                yyy = -1;
            }

            Vector3 tryDir = new Vector3(xxx, yyy, 0);

            Debug.Log("xxx" + xxx + "  yyy" + yyy);

            if (xxx == 0 && yyy != 0)
            {
                yyy = 0;
            }

            RaycastHit2D hit2D = Physics2D.Raycast(enemy.transform.position, tryDir, 1, LayerMask.GetMask("Building"));
            if (hit2D.collider != null)
            {
                xxx = 0;
                yyy = 0;
            }

            //hero.Controller.SetStatePause();


            Vector3 heroPos;
            Vector3 enemyPos;

            if (xxx == 0 && yyy == 0)
            {
                int right = _hx > _ex ? 1 : -1;
                int top = _hy > _ey ? 1 : -1;

                float hhx = ex + xxx + 0.5f + right * 0.3f;
                float eex = ex + xxx + 0.5f - right * 0.3f;

                heroPos = new Vector3(ex + xxx + 0.5f + right * 0.3f, ey + yyy + 0.5f + top * 0.2f, 0);
                enemyPos = new Vector3(ex + xxx + 0.5f - right * 0.3f, ey + yyy + 0.5f - top * 0.2f, 0);
            }
            else
            {

                heroPos = new Vector3(ex + xxx + 0.5f, ey + yyy + 0.5f, 0);
                enemyPos = enemy.transform.position;
            }

            self.SetBattleGrid(new Vector3(_ex, _ey));
            enemy.SetBattleGrid(new Vector3(_ex, _ey));

            self.transform.DOMove(heroPos, 0.1f);
            enemy.transform.DOMove(enemyPos, 0.1f);
            this.OnFightEnter(self, enemy);
        }


    }
}