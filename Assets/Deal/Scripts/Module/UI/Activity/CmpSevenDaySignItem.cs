using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;
using Deal.Env;

namespace Deal.UI
{
    public class CmpSevenDaySignItem : UIBase
    {
        public GameObject hasSign;
        public GameObject isOpen;
        public GameObject normal;

        public Image imgIcon;
        public TextMeshProUGUI txtNum;

        public Data_SevenDay data;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.isOpen.transform, "bg", this.OnSignClick);
        }

        public void SetData(Data_SevenDay data)
        {
            Debug.Log("CmpSevenDaySignItem====" + data);
            this.data = data;
            if (data.SignState == 1)
            {
                this.hasSign.SetActive(true);
                this.isOpen.SetActive(false);
                this.normal.SetActive(false);
            }
            else if (data.IsOpen == true)
            {
                this.hasSign.SetActive(false);
                this.isOpen.SetActive(true);
                this.normal.SetActive(false);
            }
            else
            {
                this.hasSign.SetActive(false);
                this.isOpen.SetActive(false);
                this.normal.SetActive(true);
            }

            if (data.DayId < 6)
            {
                this.txtNum.text = "x" + data.RewardNum;

                SpriteUtils.SetAssetSprite(this.imgIcon, data.RewardType);
            }
            else
            {
            }
        }


        public void OnSignClick()
        {
            Debug.Log("OnSignClick1");
            if (this.data == null) return;
            if (this.data.IsOpen == false) return;
            if (this.data.SignState == 1) return;

            Debug.Log("OnSignClick1" + this.data.DayId);


            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);

            this.data.SignState = 1;
            if (this.data.DayId < 6)
            {
                List<Data_GameAsset> rewards = new List<Data_GameAsset>();
                rewards.Add(new Data_GameAsset(this.data.RewardType, this.data.RewardNum));
                ShopUtils.showRewardAndToUser(rewards);
            }
            else
            {
                this._GotoShip(true);
            }

            this.SetData(this.data);
            _user.Save();

            EventManager.I.Emit(EventDefine.EVENT_ACTIVITY_SEVEN_SIGN, null);

        }

        public void GotoShip()
        {
            this._GotoShip(false);
        }

        public void _GotoShip(bool open)
        {
            UIManager.I.Pop(AddressbalePathEnum.PREFAB_UISevenday);

            if (open == true)
            {
                UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);
                _user.UnlockMapRider(-1);


            }

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            List<Data_BuildingBase> buildings = mapData.Data.buildings;

            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].BuildingEnum == BuildingEnum.SpaceShip)
                {
                    Data_SpaceShip _SpaceShip = buildings[i] as Data_SpaceShip;

                    if (open == true)
                    {
                        _SpaceShip.Open();
                        _SpaceShip.Load();
                        mapData.Save();
                    }

                    DealUtils.Guide2BuildingData(_SpaceShip);
                    break;

                }
            }
        }

    }




}

