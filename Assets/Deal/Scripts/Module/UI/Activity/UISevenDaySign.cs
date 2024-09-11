using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;


namespace Deal.UI
{
    public class UISevenDaySign : UIBase
    {
        public List<CmpSevenDaySignItem> list = new List<CmpSevenDaySignItem>();


        public override void OnUIStart()
        {
            Debug.Log("CmpSevenDaySignItem==== OnUIStart");


            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);
            List<Data_SevenDay> sevenDays = _user.Data.SevenSign;

            Debug.Log("CmpSevenDaySignItem==== OnUIStart" + sevenDays.Count);

            for (int i = 0; i < 7; i++)
            {
                this.list[i].SetData(sevenDays[i]);
            }
        }
    }

}

