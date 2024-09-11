using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Druid
{
    public class UIToast : MonoBehaviour
    {
        public Text txtMsg;

        public void Show(string msg)
        {
            RectTransform rect = GetComponent<RectTransform>();
            txtMsg.text = msg;

            rect.anchoredPosition = new Vector2(0, 140);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(rect.DOAnchorPosY(0, 0.5f));
            sequence.AppendInterval(1);
            sequence.Append(rect.DOAnchorPosY(140, 0.3f));
            sequence.AppendCallback(() => { Destroy(gameObject); });
        }
    }
}