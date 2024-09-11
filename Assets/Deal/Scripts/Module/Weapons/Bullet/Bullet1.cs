using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Deal
{

    public class Bullet1 : Bullet
    {
        private BezierPath _bezier;

        public override void SetAttacker(BattleRoleAtt roleAtt)
        {
            this._attacker = roleAtt;
            this._flyInterval = 0f;

            // this._State = BulletState.Flying;

            // //_bezier = new BezierPath
            // //{
            // //    targetObj = this.transform,
            // //    startPos = transform.position,
            // //    endPos = _target.transform.position,
            // //    percentSpeed = 3.0f,
            // //    cb = () =>
            // //    {
            // //        this._State = BulletState.Dead;
            // //        Destroy(this.gameObject);
            // //    }
            // //};
            // //_bezier.Init();

            // Vector3 start = transform.position;
            // Vector3 target = _target.transform.position;

            // DOTween.To(setter: value =>
            // {
            //     this.transform.position = Parabola(start, target, 3, value);
            // }, startValue: 0, endValue: 1, duration: 2)
            //.SetEase(Ease.Linear)
            //.OnComplete(() =>
            //{
            //    this._State = BulletState.Dead;
            //    Destroy(this.gameObject);
            //});

        }

        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => 4 * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        private void Update()
        {
            if (this._State == BulletState.Flying)
            {
                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= 10)
                {
                    this._State = BulletState.Dead;
                    Destroy(this.gameObject);
                }
                else
                {
                    //if (_bezier != null)
                    //{
                    //    _bezier.Update();
                    //}
                }
            }
        }


    }
}
