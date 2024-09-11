using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal
{


    /// <summary>
    /// 掉落蓝图
    /// </summary>
    public class DropBlueprint : DropPropBase
    {
        public BluePrintEnum bluePrint = BluePrintEnum.None;

        private void Update()
        {
            UpdateBezier();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="assetNum"></param>
        public void SetProp(BluePrintEnum bluePrint)
        {
            this.bluePrint = bluePrint;
            this.PropState = DropPropState.None;
            this.box2d.enabled = false;

            //SpriteUtils.SetBlueprintSprite(this.srIcon, bluePrint);
        }

        public override void DestroyItem()
        {
            Destroy(this.gameObject);
        }

    }
}


