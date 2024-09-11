using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deal.Tools
{
    public class SysToWxFont : MonoBehaviour
    {
        void Start()
        {
            Text text = this.GetComponent<Text>();
            if (text != null)
                WXManager.I.setFont(text);
        }

    }

}
