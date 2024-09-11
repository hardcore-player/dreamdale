using System.Collections;
using System.Collections.Generic;
using Deal.Env;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 角色的基础，有皮肤移动等配置
    /// </summary>
    public interface RoleBaseInterface
    {


        void AddWeapon(Weapon weapon);
        void RemoveWeapon(Weapon weapon);
        void HideWeapons();
        Weapon GetWeapon(WorkshopToolEnum toolEnum);


        void PlayIdle();
        void PlayRun();
        void PlayDie();
        void PlayWeapon(WorkshopToolEnum workshopTool);

        bool CanCollectAsset(AssetEnum assetEnum);

    }
}
