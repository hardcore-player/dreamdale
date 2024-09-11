using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{

    public class Weapon : MonoBehaviour
    {
        // 是不是金的
        private bool _isGold;

        public bool IsGold { get => _isGold; set => _isGold = value; }

        // 武器持有人
        private RoleBase _actor;
        public RoleBase Actor { get => _actor; set => _actor = value; }

        // 攻击回调
        public Action OnAttack;
        public Animator weaponAnimator;

        public WorkshopToolEnum weaponType;

        public virtual void onAttackEvent()
        {
            if (this.OnAttack != null)
            {
                this.OnAttack();
                this.OnAttack = null;
            }
        }

        public virtual void playAttack(float speed = 2)
        {
            weaponAnimator.speed = speed;
        }

        public virtual void SetGold(bool isGold)
        {
            this._isGold = isGold;
        }
    }
}

