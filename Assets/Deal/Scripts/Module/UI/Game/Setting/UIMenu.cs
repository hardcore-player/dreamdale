using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;

namespace Deal
{
    public class UIMenu : UIBase
    {
        public GameObject goRedSetting;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Mask", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Bg/BtnSetting", this.OnSettingClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Bg/BtnEmail", this.OnEmailClick);

            //PlayerPrefs.DeleteAll();

            if (!PlayerPrefs.HasKey("goRedSetting"))
            {
                this.goRedSetting.SetActive(true);
            }
            else
            {
                this.goRedSetting.SetActive(false);
            }
        }


        public void OnSettingClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UISetting, UILayer.Dialog);

            this.CloseSelf();

        }

        public void OnEmailClick()
        {
            Debug.Log("OnEmailClick");
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIEmail, UILayer.Dialog);

            PlayerPrefs.SetInt("goRedSetting", 1);

            this.CloseSelf();

        }

    }

}
