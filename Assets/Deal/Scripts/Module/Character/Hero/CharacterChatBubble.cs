using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Deal
{
    public class CharacterChatBubble : MonoBehaviour
    {
        private Hero _hero;
        public Hero Hero { get => _hero; set => _hero = value; }

        // 道具
        public SpriteRenderer srTool;
        public GameObject pnl0;

        // 文字
        public GameObject pnl1;
        public TextMeshPro txtMsg;
        //感叹号
        public GameObject pnl2;
        public GameObject pnl3;

        public TextMeshPro txtChicken;


        private void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void ShowLabel(string msg)
        {
            this.gameObject.SetActive(true);
            this.pnl1.SetActive(true);
            this.pnl0.SetActive(false);
            this.pnl2.SetActive(false);
            this.pnl3.SetActive(false);

            this.txtMsg.text = msg;
        }

        public void ShowLackTool(WorkshopToolEnum toolEnum)
        {
            this.gameObject.SetActive(true);
            this.pnl0.SetActive(true);
            this.pnl1.SetActive(false);
            this.pnl2.SetActive(false);
            this.pnl3.SetActive(false);
            SpriteUtils.SetToolSprite(this.srTool, toolEnum);
        }

        public void ShowMark()
        {
            this.gameObject.SetActive(true);
            this.pnl2.SetActive(true);
            this.pnl1.SetActive(false);
            this.pnl0.SetActive(false);
            this.pnl3.SetActive(false);
        }

        public void ShowChicken(int count)
        {
            this.gameObject.SetActive(true);
            this.pnl2.SetActive(false);
            this.pnl1.SetActive(false);
            this.pnl0.SetActive(false);
            this.pnl3.SetActive(true);

            this.txtChicken.text = "" + count;

            if (count >= 4)
            {
                this.txtChicken.color = new Color(255 / 255f, 53 / 255f, 53 / 255f);
            }
            else
            {
                this.txtChicken.color = new Color(1, 1, 1);
            }
        }

        public void HideMark()
        {
            this.gameObject.SetActive(false);
        }

        public void HideLackTool(WorkshopToolEnum toolEnum)
        {
            this.gameObject.SetActive(false);
        }
    }
}

