using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal
{
    public class CmpGoodsItem : MonoBehaviour
    {

        public Image imgIcon;
        public TextMeshProUGUI txtNum;
        public GameObject goSelect;

        public GameObject goOwn;
        public GameObject goNone;

        public void SetAsset(AssetEnum asset, int num)
        {
            this.goOwn.SetActive(true);
            this.goNone.SetActive(false);

            SpriteUtils.SetAssetSprite(this.imgIcon, asset);
            this.txtNum.text = "" + num;
        }

        public void SetEmpty()
        {
            this.goOwn.SetActive(false);
            this.goNone.SetActive(true);
        }
    }
}
