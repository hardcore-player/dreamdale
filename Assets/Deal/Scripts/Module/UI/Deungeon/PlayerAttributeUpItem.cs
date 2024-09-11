using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class PlayerAttributeUpItem : MonoBehaviour
    {
        public TextMeshProUGUI txtBefore;
        public TextMeshProUGUI txtAfter;
        public Image imgIcon;

        public void SetData(EquipAttrEnum equipAttr, float c, float n)
        {
            this.txtBefore.text = c + "";
            this.txtAfter.text = n + "";
        }
    }
}

