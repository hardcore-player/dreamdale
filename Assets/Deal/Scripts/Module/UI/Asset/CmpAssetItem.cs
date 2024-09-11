using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Deal.UI
{
    public class CmpAssetItem : MonoBehaviour
    {
        public Image imgIcon;
        public TextMeshProUGUI txtNum;
        public GameObject goCompolete;


        public void SetAsset(AssetEnum asset, int num)
        {
            this.txtNum.text = MathUtils.ToKBM(num) + "";
            SpriteUtils.SetAssetSprite(imgIcon, asset);
        }

        public void SetAsset(AssetEnum asset, string num)
        {
            this.txtNum.text = num + "";
            SpriteUtils.SetAssetSprite(imgIcon, asset);
        }

        public void UpdateNum(int num)
        {
            this.txtNum.gameObject.SetActive(true);
            this.goCompolete.gameObject.SetActive(false);
            this.txtNum.text = MathUtils.ToKBM(num) + "";
        }

        public void UpdateNum(string num)
        {
            this.txtNum.gameObject.SetActive(true);
            this.goCompolete.gameObject.SetActive(false);
            this.txtNum.text = num + "";
        }

        public void SetCompoleted()
        {
            this.txtNum.gameObject.SetActive(false);
            this.goCompolete.gameObject.SetActive(true);
        }

        public void SetUnCompoleted()
        {
            this.txtNum.gameObject.SetActive(true);
            this.goCompolete.gameObject.SetActive(false);
        }
    }
}

