using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Druid.Utils {
    public class FixCamera : MonoBehaviour
    {
        public GameObject uiRoot;
        public float initOrthoSize = 5.0f;
        public float initWidth = 750;
        public float initHeight = 1334;

        float factWidth;
        float factHeight;

        void Awake()
        {
            factWidth = Screen.width;
            factHeight = Screen.height;

            if (Math.Round(factWidth / factHeight, 2) > Math.Round(initWidth / initHeight, 2))
            {
                //高适配
                uiRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            }
            else
            {
                //宽适配

                //实际正交视口 = 初始正交视口 * 初始宽高比 / 实际宽高比
                Camera.main.orthographicSize = (initOrthoSize * (factHeight / factWidth)) / (initHeight / initWidth);
                uiRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}