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
    /// 羊圈
    /// </summary>
    public class Building_FarmerHouse : BuildingBase
    {
        public TextMeshProUGUI txtNum;
        public TextMeshProUGUI txtPrice;
        public Button btnBuy;

        public GameObject sheepPfb;


        public List<Transform> bornList = new List<Transform>();
        public List<FarmerHouseSheep> sheepList = new List<FarmerHouseSheep>();

        private int[] sheepPirce = { 250, 250, 500, 750, 1000, 1500, 2000, 2500, 3000, 4000, 5000, 5500, 6000 };

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
        public FarmerHouseSheep GetIdleSheep()
        {
            for (int i = 0; i < this.sheepList.Count; i++)
            {
                if (this.sheepList[i].HasWool == true && this.sheepList[i].sheepState == FarmerHouseSheepState.IDLE)
                {
                    return this.sheepList[i];
                }
            }

            return null;
        }

        private void RenderUI()
        {
            Data_FarmerHouse data = this.GetData<Data_FarmerHouse>();

            this.txtNum.text = $"{data.SheepNum}/12";

            if (data.SheepNum < 12)
            {
                this.txtPrice.text = sheepPirce[data.SheepNum] + "";
                this.btnBuy.gameObject.SetActive(true);
            }
            else
            {
                this.btnBuy.gameObject.SetActive(false);
            }
        }


        private void InitSheepList()
        {
            Data_FarmerHouse data = this.GetData<Data_FarmerHouse>();

            for (int i = 0; i < data.SheepNum; i++)
            {
                GameObject go = Instantiate(this.sheepPfb);

                FarmerHouseSheep item = go.GetComponent<FarmerHouseSheep>();
                item.SetBorn(i, bornList[i].position);

                this.sheepList.Add(item);

            }
        }

        private async void AddSheep()
        {
            GameObject go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_Sheep);

            int i = this.sheepList.Count;
            FarmerHouseSheep item = go.GetComponent<FarmerHouseSheep>();
            item.SetBorn(i, bornList[i].position);
            this.sheepList.Add(item);

            this.RenderUI();
        }


        public void AddSheepClick()
        {
            Data_FarmerHouse data = this.GetData<Data_FarmerHouse>();
            if (data.SheepNum < data.SheepTotal)
            //if (data.SheepNum < 12)
            {

                int price = this.sheepPirce[data.SheepNum];
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                bool ok = userData.CostAsset(AssetEnum.Gold, price);
                if (ok)
                {
                    data.SheepNum++;
                    DataManager.I.Save(DataDefine.UserData);
                    DataManager.I.Save(DataDefine.MapData);

                    this.AddSheep();

                    TaskManager.I.OnTaskBuy("Sheep", data.SheepNum);
                }
            }
        }

    }
}

