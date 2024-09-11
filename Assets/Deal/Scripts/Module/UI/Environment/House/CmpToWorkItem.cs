using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class CmpToWorkItem : MonoBehaviour
    {
        public Image imgIcon;
        public GameObject goOn;
        public TextMeshProUGUI txtNum;

        public AssetEnum asset;

        public void SetData(AssetEnum asset)
        {
            this.asset = asset;

            SpriteUtils.SetAssetSprite(this.imgIcon, asset);
        }

        public void SetSelect(AssetEnum select)
        {
            this.goOn.SetActive(select == this.asset);
        }

        public void SetNum(int nums)
        {
            this.txtNum.text = $"{nums}/3";
        }

    }
}