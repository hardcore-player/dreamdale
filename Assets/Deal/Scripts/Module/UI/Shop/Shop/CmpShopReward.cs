using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class CmpShopReward : MonoBehaviour
    {
        public TextMeshProUGUI txtNum;
        public Image imgAsset;
        public Image imgBg;

        public void SetAsset(AssetEnum asset, int num)
        {
            this.txtNum.text = "" + num;
            SpriteUtils.SetAssetSprite(this.imgAsset, asset);
        }

    }
}