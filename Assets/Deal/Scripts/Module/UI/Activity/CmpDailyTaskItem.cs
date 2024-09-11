using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;


namespace Deal.UI
{
    public class CmpDailyTaskItem : UIBase
    {
        public GameObject disable;
        public GameObject highlight;
        public GameObject normal;

        public Data_DailyTask data;

        public Text txtInfo;
        public Text txtInfo1;

        public TextMeshProUGUI txtNum;
        public Image imgIcon;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.highlight.transform, "Bg", this.OnSignClick);
        }

        public void SetData(Data_DailyTask data)
        {
            this.data = data;
            if (data.HasReward == true)
            {
                // 领取了
                this.disable.SetActive(true);
                this.highlight.SetActive(false);
                //this.normal.SetActive(false);
            }
            else if (data.IsDone == true)
            {
                this.disable.SetActive(false);
                this.highlight.SetActive(true);
                //this.normal.SetActive(false);
            }
            else
            {
                this.disable.SetActive(false);
                this.highlight.SetActive(false);
                //this.normal.SetActive(true);
            }

            this.txtInfo.text = $"{data.TaskInfo} ({data.CurProgress}/{data.TotalProgress})";
            this.txtInfo1.text = $"{data.TaskInfo} ({data.CurProgress}/{data.TotalProgress})";

            this.txtNum.text = "x" + data.RewardNum;
            SpriteUtils.SetAssetSprite(this.imgIcon, data.RewardType);
        }


        public void OnSignClick()
        {
            Debug.Log("OnSignClick1");
            if (this.data == null) return;
            if (this.data.IsDone == false) return;
            if (this.data.HasReward == true) return;

            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);


            if (this.data.RewardType == AssetEnum.Rune)
            {
                AssetEnum RewardType = AssetEnum.SwordRune;
                int r = Druid.Utils.MathUtils.RandomInt(0, 8);
                if (r == 0)
                {
                    RewardType = AssetEnum.SwordRune;
                }
                else if (r == 1)
                {
                    RewardType = AssetEnum.AxeRune;
                }
                else if (r == 2)
                {
                    RewardType = AssetEnum.BladeRune;
                }
                else if (r == 3)
                {
                    RewardType = AssetEnum.TalismanRune;
                }
                else if (r == 4)
                {
                    RewardType = AssetEnum.ShieldRune;
                }
                else if (r == 5)
                {
                    RewardType = AssetEnum.HelmetRune;
                }
                else if (r == 6)
                {
                    RewardType = AssetEnum.ArmorRune;
                }
                else if (r == 7)
                {
                    RewardType = AssetEnum.CloakRune;
                }

                List<Data_GameAsset> rewards = new List<Data_GameAsset>();
                rewards.Add(new Data_GameAsset(RewardType, this.data.RewardNum));
                ShopUtils.showRewardAndToUser(rewards);
            }
            else
            {
                List<Data_GameAsset> rewards = new List<Data_GameAsset>();
                rewards.Add(new Data_GameAsset(this.data.RewardType, this.data.RewardNum));
                ShopUtils.showRewardAndToUser(rewards);

                if (this.data.RewardType == AssetEnum.GoldAxe)
                {
                    _user.UnlockGlodAxe(24 * 60 * 60);
                }
                else if (this.data.RewardType == AssetEnum.GoldPick)
                {
                    _user.UnlockGlodPickAxe(24 * 60 * 60);
                }
            }


            this.data.HasReward = true;
            this.SetData(this.data);

            _user.Save();

            EventManager.I.Emit(EventDefine.EVENT_ACTIVITY_DAILY_TASK, null);
        }
    }

}

