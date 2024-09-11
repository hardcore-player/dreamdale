using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;
using DG.Tweening;
using TMPro;

namespace Deal.UI
{
    public class UIRankDescription : UIBase
    {
        public CmpRankDescriptionItem pfbItem;
        public CmpRankDescriptionItem myRankItem;

        public override void OnUIStart()
        {
            ExcelData.Arena[] arenas = ConfigManger.I.configS.arenas;

            for (int i = 0; i < arenas.Length; i++)
            {
                CmpRankDescriptionItem item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                item.gameObject.SetActive(true);
                item.SetData(arenas[i]);

            }

            myRankItem.SetMyData();
        }

    }

}
