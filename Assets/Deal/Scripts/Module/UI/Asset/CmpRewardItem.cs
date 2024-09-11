using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Deal.UI
{
    public class CmpRewardItem : MonoBehaviour
    {
        public Image imgBg;
        public Image imgIcon;
        public TextMeshProUGUI txtNum;


        public void SetAsset(AssetEnum asset, int num)
        {
            this.txtNum.text = num + "";
            SpriteUtils.SetAssetSprite(imgIcon, asset);
        }

    }
}

