using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Druid
{
    public enum UIMsgBoxEnum
    {
        Single,
        Double
    }

    public class UIParamStruct
    {
        public UIMsgBoxEnum type;
        public string title;
        public string message;
        public Action okFun;
        public Action canFun;
        public object param;

        public UIParamStruct()
        {
        }

        public UIParamStruct(object _param)
        {
            this.param = _param;
        }
    }

    public class UIMsgBox : UIBase
    {
        public Transform btnOK;
        public Transform btnCal;
        public Transform btnOkSingle;
        public TextMeshProUGUI txtMsg;
        public TextMeshProUGUI txtTitle;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "btn_ok", OnOkClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "btn_ok1", OnOkClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "btn_cancel", OnCancelClick);
        }

        public override void OnInit(UIParamStruct param)
        {
            Debug.Log("UIMsgBox OnInit");

            Param = param;

            if (Param != null)
            {
                txtTitle.text = Param.title == null ? "" : Param.title;
                txtMsg.text = Param.message;

                if (Param.type == UIMsgBoxEnum.Double)
                {
                    btnOK.gameObject.SetActive(true);
                    btnCal.gameObject.SetActive(true);
                    btnOkSingle.gameObject.SetActive(false);
                }
                else
                {
                    btnOK.gameObject.SetActive(false);
                    btnCal.gameObject.SetActive(false);
                    btnOkSingle.gameObject.SetActive(true);
                }
            }
        }

        public void OnOkClick()
        {
            if (Param != null && Param.okFun != null)
            {
                Param.okFun();
            }
            CloseSelf();
        }

        public void OnCancelClick()
        {
            if (Param != null && Param.canFun != null)
            {
                Param.canFun();
            }
            CloseSelf();
        }
    }
}