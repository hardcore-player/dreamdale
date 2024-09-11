using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;

namespace Deal
{
    public class UIPrivate : UIBase
    {
        public Text text;

        public override void OnUIAwake()
        {
            WXManager.I.setFont(this.text);
        }

    }

}
