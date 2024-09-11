using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using TMPro;

namespace Deal.Env
{
    /// <summary>
    /// 养鸡场
    /// </summary>
    public class Building_ChickenFarm : BuildingBase
    {
        public TextMeshProUGUI txtNum;
        public TextMeshProUGUI txtPrice;
        public Button btnBuy;

        public GameObject chickPfb;


        public List<Transform> bornList = new List<Transform>();
        public List<FarmerHouseChiken> chickList = new List<FarmerHouseChiken>();

        private int[] chickPirce = { 500, 750, 1000, 1500, 2000, 2500, 3000, 4000, 5000, 7000, 10000, 15000 };

        public override void SetData(Data_SaveBase data)
        {
            base.SetData(data);

            this.InitSheepList();
            this.RenderUI();
        }

        /// <summary>
        /// 获取一只空闲的羊
        /// </summary>
        /// <returns></returns>
        public FarmerHouseChiken GetIdleSheep()
        {
            for (int i = 0; i < this.chickList.Count; i++)
            {
                if (this.chickList[i].HasWool == true && this.chickList[i].sheepState == FarmerHouseSheepState.IDLE)
                {
                    return this.chickList[i];
                }
            }

            return null;
        }

        private void RenderUI()
        {
            Data_ChickenFarm data = this.GetData<Data_ChickenFarm>();

            this.txtNum.text = $"{data.ChickenNum}/12";

            if (data.ChickenNum < 12)
            {
                this.txtPrice.text = chickPirce[data.ChickenNum] + "";
                this.btnBuy.gameObject.SetActive(true);
            }
            else
            {
                this.btnBuy.gameObject.SetActive(false);
            }
        }


        private void InitSheepList()
        {
            Data_ChickenFarm data = this.GetData<Data_ChickenFarm>();

            for (int i = 0; i < data.ChickenNum; i++)
            {
                GameObject go = Instantiate(this.chickPfb);

                FarmerHouseChiken item = go.GetComponent<FarmerHouseChiken>();
                item.SetBorn(i, bornList[i].position);

                this.chickList.Add(item);

            }
        }

        private async void AddSheep()
        {
            GameObject go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_chick);

            int i = this.chickList.Count;
            FarmerHouseChiken item = go.GetComponent<FarmerHouseChiken>();
            item.SetBorn(i, bornList[i].position);
            this.chickList.Add(item);

            this.RenderUI();
        }


        public void AddSheepClick()
        {
            Data_ChickenFarm data = this.GetData<Data_ChickenFarm>();
            if (data.ChickenNum < data.ChickenTotal)
            //if (data.SheepNum < 12)
            {

                int price = this.chickPirce[data.ChickenNum];
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                bool ok = userData.CostAsset(AssetEnum.Gold, price);
                if (ok)
                {
                    data.ChickenNum++;
                    DataManager.I.Save(DataDefine.UserData);
                    DataManager.I.Save(DataDefine.MapData);

                    this.AddSheep();

                    TaskManager.I.OnTaskBuy("Chicken", data.ChickenNum);
                }
            }
        }

    }
}

