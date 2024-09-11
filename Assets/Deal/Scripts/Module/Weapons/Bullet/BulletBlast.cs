using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{

    public class BulletBlast : Bullet
    {
        public override void SetAttacker(BattleRoleAtt roleAtt)
        {
            this._attacker = roleAtt;
            this._State = BulletState.Flying;
            this._moveDir = Vector3.zero;
        }

    }
}
