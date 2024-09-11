using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Deal.Tools
{
    public class SortingLayerUp : MonoBehaviour
    {
        void Start()
        {
            Druid.Utils.UnityUtils.ReSortRendererInUI(this.gameObject);

        }

    }
}


