using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal
{

    public enum DropPropState
    {
        None, Idle, Droping, Pick, Despawn
    }


    /// <summary>
    /// 掉落道具，是一中资产，离着玩家一定距离会自动拾取
    /// </summary>
    public class DropPropBase : MonoBehaviour
    {
        public SpriteRenderer srIcon;
        public GameObject shadow;

        public DropPropState PropState = DropPropState.None;
        public BoxCollider2D box2d;

        private BezierPath _bezier;
        private float _distance = 0;
        protected GameObject _shadowIns;
        private bool _flyToPlayer = false;
        protected RoleBase _mPlayer;
        // 谁创造的
        protected RoleBase _mOwner;

        protected float _dropingInterval = 0;


        public virtual void OnSpawn()
        {
            this._mOwner = null;
            this.PropState = DropPropState.None;
        }

        public virtual void OnDespawn()
        {
            this.PropState = DropPropState.Despawn;
            this._mOwner = null;
        }

        protected void UpdateBezier()
        {
            if (_bezier != null)
            {
                if (!_flyToPlayer || _bezier.percent < 0.8)
                {
                    _bezier.Update();
                }
                else
                {
                    Vector2 a = transform.position;
                    Vector2 b = _mPlayer.center.position;
                    if (_distance == 0) { _distance = (b - a).magnitude; }
                    if (_distance == 0.01f)
                    {
                        if (_bezier != null)
                        {
                            _flyToPlayer = false;
                            _bezier.alive = false;
                            _distance = 0;
                            _bezier.cb();
                        }
                    }
                    else
                    {
                        _distance = System.Math.Max(_distance - Time.deltaTime * 4, 0.01f);
                        transform.position = b + (a - b).normalized * _distance;
                    }
                }
            }
        }

        public void SetFlyToPlayer(RoleBase player)
        {
            _flyToPlayer = true;
            _mPlayer = player;
        }


        /// <summary>
        /// 掉在地上
        /// </summary>
        public void Fall2Ground(Vector3 endPos)
        {
            Vector2 mid = Vector2.Lerp(transform.position, endPos, 0.8f);
            _shadowIns = Instantiate(shadow, transform.position - new Vector3(0, 0.25f), transform.rotation);
            _shadowIns.transform.DOMove(endPos - new Vector3(0, 0.25f), 0.5f);
            _bezier = new BezierPath
            {
                targetObj = this.transform,
                startPos = transform.position,
                endPos = mid,
                percentSpeed = 3.0f,
                cb = () =>
                {

                    _bezier = new BezierPath
                    {
                        targetObj = this.transform,
                        startPos = transform.position,
                        endPos = endPos,
                        percentSpeed = 6.0f,
                        cb = () =>
                        {
                            _bezier = null;
                            this.OnFall2Ground();
                        }
                    };
                    _bezier.Init();
                }
            };
            _bezier.Init();
        }

        /// <summary>
        /// 落地以后的逻辑
        /// </summary>
        public virtual void OnFall2Ground()
        {
            this.PropState = DropPropState.Idle;
            this.box2d.enabled = true;
            this._dropingInterval = 0;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public virtual void Fl2Picker(Vector3 pos)
        {
            if (this.PropState != DropPropState.None)
            {
                this.box2d.enabled = false;
                this.PropState = DropPropState.None;
                Destroy(_shadowIns);
                Sequence s = DOTween.Sequence();
                s.Append(transform.DOMove(pos, 0.2f));
                s.AppendCallback(() =>
                {
                    DestroyItem();
                });
            }

        }

        public void BezierToTarget(Vector2 endPos)
        {
            this.box2d.enabled = false;
            this.PropState = DropPropState.None;

            _bezier = new BezierPath
            {
                targetObj = this.transform,
                startPos = transform.position,
                endPos = endPos,
                cb = () =>
                {
                    _bezier = null;
                    DestroyItem();
                }
            };
            _bezier.Init();
        }



        void OnTriggerEnter2D(Collider2D collision)
        {

        }


        public virtual void DestroyItem()
        {
            if (this.PropState != DropPropState.Despawn)
            {
                PlayManager.I.DespawnDropItem(transform);
            }
        }
    }
}


