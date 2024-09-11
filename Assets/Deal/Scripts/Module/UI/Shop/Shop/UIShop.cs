using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;

namespace Deal.UI
{
    public class UIShop : UIBase
    {

        public TapPages tapPages;
        public List<Transform> pages = new List<Transform>();


        public override void OnUIStart()
        {
            tapPages.onPageAction = this.OnPageChanged;
        }

        protected void OnPageChanged(int page)
        {
            for (int i = 0; i < this.pages.Count; i++)
            {
                this.pages[i].gameObject.SetActive(i == page);
            }
        }

    }
}
