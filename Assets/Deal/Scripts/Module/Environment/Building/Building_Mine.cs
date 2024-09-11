using System.Collections;
using System.Collections.Generic;
using Deal.UI;
using UnityEngine;

namespace Deal.Env
{
    public class Building_Mine : BuildingAssetWarehouse
    {
        public AssetList assetList;
        public Animator houseAnimator;

        public void checkAnimation()
        {
            if (this.IsFull())
            {
                this.PlayIdle();
            }
            else
            {
                this.PlayWork();
            }

        }

        public void PlayWork()
        {
            if (houseAnimator)
            {
                houseAnimator.Play("work", 0);
            }

        }

        public void PlayIdle()
        {
            if (houseAnimator)
            {
                houseAnimator.Play("idle", 0);
            }

        }
    }
}


