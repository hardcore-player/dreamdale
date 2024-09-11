using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Druid;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deal.Env
{
    /// <summary>
    /// 交互的基础类
    /// </summary>
    public class InteractiveBase : BindingSaveData
    {

        // 碰到弹出等
        public GameObject popUIPrefabs;
        // 弹出UI
        protected UIBase _popUI;

        // 常驻UI
        public GameObject _displayUI;
        // ui多远自动显示
        public float DistanceDisplayShow = 2.5f;


        public virtual void OnUIPop()
        {

        }

        public virtual void OnUIDisplayShow()
        {

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                Hero mHero = collision.gameObject.GetComponentInParent<Hero>();
                if (mHero.Controller.GetMoveState() == CharacterState.Moving)
                {
                    OnHeroEnter(mHero);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                Hero mHero = collision.gameObject.GetComponentInParent<Hero>();
                OnHeroExit(mHero);
            }
        }

        public virtual void OnHeroEnter(Hero mHero)
        {
            if (this.popUIPrefabs != null && this._popUI == null)
            {

                GameObject go = Instantiate(this.popUIPrefabs, UIManager.I.DialogLayer);
                this._popUI = UIManager.I.Push(go, go.name);
                this.OnUIPop();
            }
        }


        public virtual void OnHeroExit(Hero mHero)
        {
            if (this._popUI != null)
            {
                this._popUI.CloseSelf();
                this._popUI = null;
            }
        }

        public virtual void OnUpdate()
        {

        }


        private void Update()
        {
            this.OnUpdate();
            this.UpdateDisplay();
        }

        private float displayInterval = 0;
        private void UpdateDisplay()
        {
            this.displayInterval += Time.deltaTime;
            if (this.displayInterval < 0.5f) return;
            this.displayInterval = 0;

            Hero hero = PlayManager.I.mHero;
            if (hero == null) return;
            if (_displayUI == null) return;

            float distance = Vector3.Distance(hero.transform.position, this.transform.position);

            if (distance > this.DistanceDisplayShow)
            {
                //隐藏
                if (_displayUI.activeInHierarchy == true)
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(_displayUI.transform.DOScale(new Vector3(0, 0, 1), 0.1f));
                    s.AppendCallback(() =>
                    {
                        _displayUI.SetActive(false);
                    });
                }
            }
            else
            {
                if (_displayUI.activeInHierarchy == false)
                {
                    _displayUI.SetActive(true);
                    this.OnUIDisplayShow();
                    _displayUI.transform.localScale = new Vector3(0, 0, 1);
                    Sequence s = DOTween.Sequence();
                    s.Append(_displayUI.transform.DOScale(new Vector3(1f, 1f, 1), 0.1f));
                    s.AppendCallback(() =>
                    {

                    });
                }
            }
        }
    }
}
