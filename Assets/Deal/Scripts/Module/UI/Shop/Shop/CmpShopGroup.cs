using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.UI
{
    public class CmpShopGroup : MonoBehaviour
    {
        public string ShopType = "";

        public CmpShopItem pfbItem;


        private void Start()
        {
            this._renderItems();
        }


        private void _renderItems()
        {
            ExcelData.Shop[] shops = ConfigManger.I.configS.shops;

            Data_Task _Task = TaskManager.I.GetTask();

            for (int i = 0; i < shops.Length; i++)
            {
                if (shops[i].type == this.ShopType && _Task.TaskId >= shops[i].task)
                {
                    CmpShopItem item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                    item.SetData(shops[i]);
                }
            }

            this.pfbItem.gameObject.SetActive(false);
        }
    }
}