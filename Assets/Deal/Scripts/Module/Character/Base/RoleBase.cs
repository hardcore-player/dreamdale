using System.Collections;
using System.Collections.Generic;
using Deal.Env;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 角色的基础，有皮肤移动等配置
    /// </summary>
    public class RoleBase : MonoBehaviour, RoleBaseInterface
    {
        [Header("视觉中心点")]
        public Transform center;

        [Header("皮肤ID")]
        public int skinId = 5;

        [Header("角色动画节点，翻转用")]
        public Transform roleBody;

        [Header("攻击动画速度")]
        public float aniSpeedRate = 1;

        [Header("身体动画")]
        public Animator bodyAnimator;

        [Header("烟")]// 
        public GameObject dust;

        private Dictionary<WorkshopToolEnum, Weapon> weaponList = new Dictionary<WorkshopToolEnum, Weapon>();

        private void Awake()
        {
            Weapon[] weapons = this.GetComponentsInChildren<Weapon>();

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Actor = this;
                this.weaponList.Add(weapons[i].weaponType, weapons[i]);
            }

            this.OnAwake();
            this.PlayIdle();
        }

        public virtual void OnAwake()
        {
        }

        public void AddWeapon(Weapon weapon)
        {
            this.weaponList.Add(weapon.weaponType, weapon);
        }

        public void RemoveWeapon(Weapon weapon)
        {
            this.weaponList.Remove(weapon.weaponType);
        }

        public virtual bool CanCollectAsset(AssetEnum assetEnum)
        {
            return true;
        }

        public Weapon GetWeapon(WorkshopToolEnum toolEnum)
        {
            return this.weaponList[toolEnum];
        }

        public void HideWeapons()
        {
            foreach (var item in this.weaponList)
            {
                if (item.Key != WorkshopToolEnum.AttackWeapon)
                {
                    item.Value.gameObject.SetActive(false);
                }
            }
        }

        public virtual void PlayWeapon(WorkshopToolEnum weapon)
        {
            bodyAnimator.speed = 2 * this.aniSpeedRate;

            if (weapon == WorkshopToolEnum.None)
            {
                bodyAnimator.Play($"ani_doing{this.skinId}", 0, 0);
            }
            else if (weapon == WorkshopToolEnum.AttackWeapon)
            {
                bodyAnimator.Play($"ani_attack_{this.skinId}", 0, 0);
            }
            else
            {
                bodyAnimator.Play($"ani_axe{this.skinId}", 0, 0);
            }


            foreach (var item in this.weaponList)
            {
                if (item.Key == weapon)
                {
                    item.Value.gameObject.SetActive(true);
                    item.Value.playAttack(2 * this.aniSpeedRate);
                }
                else
                {
                    item.Value.gameObject.SetActive(false);
                }
            }
        }

        public virtual void PlayDie()
        {
        }

        public virtual void PlayBorn()
        {
        }


        public virtual void PlayIdle()
        {
            this.HideWeapons();
            bodyAnimator.speed = 1;
            bodyAnimator.Play($"ani_idle{this.skinId}", 0);

            if (dust != null) dust.SetActive(false);
        }

        public virtual void PlayRun()
        {
            this.HideWeapons();
            bodyAnimator.speed = 1;
            bodyAnimator.Play($"ani_run{this.skinId}", 0);
            if (dust != null) dust.SetActive(true);
        }


        public virtual void LookDir(Vector3 dir)
        {
            if (dir.x > 0)
            {
                this.roleBody.localScale = new Vector3(1, 1, 0);
            }
            else if (dir.x < 0)
            {
                this.roleBody.localScale = new Vector3(-1, 1, 0);
            }

        }


    }
}
