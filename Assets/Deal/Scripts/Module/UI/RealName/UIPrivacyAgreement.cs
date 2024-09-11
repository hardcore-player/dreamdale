using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using System;

namespace Deal.UI
{
    public class UIPrivacyAgreement : UIBase
    {
        //public InputField inputName;
        //public InputField inputId;

        public Action okAction;

        public void OnOkClick()
        {
            PlayerPrefs.SetInt("PrivacyAgreement_Agree", 1);

            if (this.okAction != null) this.okAction();

            this.CloseSelf();
        }

        public void OnCancelClick()
        {
            Application.Quit();
        }

        public void OnOpenClick()
        {
            //Application.OpenURL("http://www.ldtgames.com/privacy");
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIUserPrivacy, UILayer.Dialog);
        }

        public void OnOpen1Click()
        {
            //Application.OpenURL("http://www.ldtgames.com/privacy");

            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIPersonalPrivacy, UILayer.Dialog);
        }

        public void SetCallback(Action action)
        {
            this.okAction = action;
        }
    }
}


