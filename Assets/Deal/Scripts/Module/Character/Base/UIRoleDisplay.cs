using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Deal
{
    public class UIRoleDisplay : MonoBehaviour
    {
        public TextMeshPro textHp;
        public SpriteRenderer srSlideHp;


        public void UpdateHp(int cHp, int maxHp)
        {
            this.textHp.text = $"{cHp}/{maxHp}";

            float p = cHp / 1f / maxHp;
            this.srSlideHp.size = new Vector2(1.0f * p, 0.16f);
        }
    }
}

