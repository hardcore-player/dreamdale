using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 页签组件
    /// </summary>
    public class TapPages : MonoBehaviour
    {
        [UnityEngine.SerializeField, UnityEngine.Header("是否用一个页面")]
        public bool IsSharePage = false;

        public List<Toggle> toggles = new List<Toggle>();
        public List<Transform> pages = new List<Transform>();
        public List<string> titles = new List<string>();

        public TextMeshProUGUI txtTitle;

        public Action<int> onPageAction;

        private int _cPage = -1;

        private void Start()
        {
            for (int i = 0; i < this.toggles.Count; i++)
            {
                Toggle toggle = this.toggles[i];

                int page = i;
                toggle.onValueChanged.AddListener((bool yes) =>
                {
                    if (yes == true)
                    {
                        this.OnTogChange(page);
                    }

                });
            }
            this.OnTogChange(0);
        }


        private void OnTogChange(int page)
        {
            Debug.Log("OnTogChange this._cPage" + this._cPage + "  " + page);

            if (this._cPage == page) return;

            this._cPage = page;

            if (this.txtTitle != null && titles.Count >= page)
            {
                this.txtTitle.text = titles[page];
            }

            if (this.IsSharePage == false)
            {
                for (int i = 0; i < this.pages.Count; i++)
                {
                    this.pages[i].gameObject.SetActive(i == page);
                }
            }


            if (this.onPageAction != null) onPageAction(page);

        }
    }
}

