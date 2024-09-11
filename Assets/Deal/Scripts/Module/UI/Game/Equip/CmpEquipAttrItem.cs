using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 装备属性强化
    /// </summary>
    public class CmpEquipAttrItem : MonoBehaviour
    {
        public Image imgIcon;
        public TextMeshProUGUI txtAttName;
        public TextMeshProUGUI txtAttValue;
        public TextMeshProUGUI txtAttValueAdd;


        public void SetAttr(EquipAttrEnum equipAttr, float c, float n)
        {
            this.txtAttName.text = DealUtils.getAttrName(equipAttr) + ":";
            this.txtAttValue.text = DealUtils.getAttrDisplay(equipAttr, c) + "";
            if (n == c)
            {
                this.txtAttValueAdd.text = "";
            }
            else
            {
                this.txtAttValueAdd.text = "+" + DealUtils.getAttrDisplay(equipAttr, n - c);
            }

            SpriteUtils.SetEquipIconSprite(this.imgIcon, equipAttr);
        }
    }
}


