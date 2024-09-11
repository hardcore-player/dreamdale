using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    public class BulletMonter6 : Bullet
    {
        public override void UpdateFly()
        {
            this.transform.Translate(this._moveDir * Time.deltaTime * this.MoveSpeed);

            if (this._flyInterval <= 0.05f)
            {
                this.MoveSpeed = 5f;
            }
            else
            {
                // 5 秒减10
                this.MoveSpeed -= Time.deltaTime * (5 / this.LifeTime);
            }
        }
    }
}

