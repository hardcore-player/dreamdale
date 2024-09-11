using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;

namespace Deal.Env
{
    //public enum FarmerHouseSheepState
    //{
    //    NONE,
    //    IDLE,
    //    GOHOME, // 回家路上
    //    FOLLOW, //
    //}

    /// <summary>
    /// 羊
    /// </summary>
    public class FarmerHouseChiken : MonoBehaviour
    {
        public GameObject slider;
        public SpriteRenderer srSlideHp;
        public float MoveSpeed = 0.2f;
        public Transform roleBody;

        public Animator bodyAni;
        // 是否有鸡蛋
        public bool HasWool = false;

        private Transform follow;

        public FarmerHouseSheepState sheepState = FarmerHouseSheepState.NONE;

        private float _growWoolInterval = 0;

        // 出生位置
        private Vector3 bornPoint;
        private Vector3 idle2Point;

        // 回家的路
        public List<Vector3> homeWay = new List<Vector3>();
        private int wayPidx = 0;


        public int Id = 0;

        private void Start()
        {
            this.SetWoolState(true);
            this.SetRun();
        }

        /// <summary>
        /// 出生
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vector3"></param>
        public void SetBorn(int id, Vector3 vector3)
        {
            this.Id = id;
            this.bornPoint = vector3;

            this.homeWay.Add(new Vector3(37, 48.5f));
            this.homeWay.Add(new Vector3(37, 47));
            this.homeWay.Add(new Vector3(32.5f, 47));
            this.homeWay.Add(vector3);

            this.transform.position = vector3;

            this.sheepState = FarmerHouseSheepState.IDLE;
        }

        public void ReBorn()
        {
            this.transform.position = this.bornPoint;

            this.sheepState = FarmerHouseSheepState.IDLE;
        }


        public void SetWoolState(bool hasWool)
        {
            this.HasWool = hasWool;
        }

        public void SetIdle()
        {
            this.bodyAni.Play("ani_chick_idle");
            this.slider.gameObject.SetActive(true);

        }

        public void SetRun()
        {
            this.bodyAni.Play("ani_chick_run");
            this.slider.gameObject.SetActive(false);

        }


        public void GoHome()
        {
            this.roleBody.gameObject.SetActive(true);
            this.sheepState = FarmerHouseSheepState.GOHOME;
            this.wayPidx = 0;
            this.HasWool = false;
            this._growWoolInterval = 0;

            this.SetRun();
        }

        public void SetFollow(Transform _follow)
        {
            this.follow = _follow;
        }

        public void UpdateSlider(float p)
        {
            this.slider.gameObject.SetActive(true);
            this.srSlideHp.size = new Vector2(1.44f * p, 0.1f);
        }

        public void ShowBody()
        {
            this.roleBody.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (this.HasWool == false)
            {
                this._growWoolInterval += Time.deltaTime;
                if (this._growWoolInterval >= 5)
                {
                    this.SetWoolState(true);
                }
            }
            else
            {
                //this.slider.gameObject.SetActive(false);
            }

            if (this.sheepState == FarmerHouseSheepState.GOHOME)
            {
                this.MoveHome();
            }
            else if (this.sheepState == FarmerHouseSheepState.IDLE)
            {
                this.MoveIdle();
            }
            else if (this.sheepState == FarmerHouseSheepState.FOLLOW)
            {
                this.MoveFollow();
            }
        }

        private void MoveFollow()
        {
            this.roleBody.gameObject.SetActive(false);

            Hero hero = PlayManager.I.mHero;

            Vector3 dir = hero.transform.position - this.transform.position;
            this.transform.position = Vector3.Lerp(this.transform.position, this.follow.transform.position, 0.1f);

            this.LookDir(dir);
        }

        private void MoveIdle()
        {
            if (bornPoint == null) return;

            if (idle2Point == null || idle2Point == Vector3.zero)
            {
                float xx = (float)Druid.Utils.MathUtils.RandomDouble(-10, 10) / 10;
                float yy = (float)Druid.Utils.MathUtils.RandomDouble(-10, 10) / 10;
                idle2Point = bornPoint + new Vector3(xx, yy, 0);
            }

            Vector3 toPos = idle2Point;

            Vector3 dir = toPos - this.transform.position;
            Vector3 dst = dir.normalized * Time.deltaTime * this.MoveSpeed;

            this.transform.Translate(dst);

            this.LookDir(dir);

            if (Vector3.Distance(toPos, this.transform.position) < 0.1f)
            {
                idle2Point = Vector3.zero;
            }
        }


        private void MoveHome()
        {
            // 追踪
            if (this.homeWay.Count == 0) return;

            Vector3 toPos = this.homeWay[this.wayPidx];

            Vector3 dir = toPos - this.transform.position;
            Vector3 dst = dir.normalized * Time.deltaTime * this.MoveSpeed * 15;

            this.transform.Translate(dst);

            this.LookDir(dir);

            if (Vector3.Distance(toPos, this.transform.position) < 0.1f)
            {
                this.wayPidx++;
                if (this.wayPidx >= this.homeWay.Count)
                {
                    this.sheepState = FarmerHouseSheepState.IDLE;
                }
            }
        }

        public virtual void LookDir(Vector3 dir)
        {
            if (dir.x > 0)
            {
                this.roleBody.localScale = new Vector3(-1, 1, 0);
            }
            else if (dir.x < 0)
            {
                this.roleBody.localScale = new Vector3(1, 1, 0);
            }

        }

    }
}

