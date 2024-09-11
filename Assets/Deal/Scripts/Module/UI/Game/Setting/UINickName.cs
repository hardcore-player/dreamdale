using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;


namespace Deal.UI
{
    [System.Serializable]
    public class RandomNameItem
    {
        public string name;
        public int id;
    }

    public class RandomNameList
    {
        public RandomNameItem[] nameList;
    }

    public class UINickName : UIBase
    {
        public RandomNameList playerNameList;
        public Text userName;
        public Text txtError;
        public InputField iputName;

        public TextMeshProUGUI txtTitle;
        public GameObject btnClose;


        public override void OnUIStart()
        {
            TextAsset a = ResManager.I.GetResourcesSync<TextAsset>(AddressbalePathEnum.JSON_RandomName);

            this.playerNameList = JsonUtility.FromJson<RandomNameList>(a.text);

            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Node/Name/BtnRamdon", this.OnRandomClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Ok", this.OnChangeClick);

            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo info = user.Data.Userinfo;
            this.iputName.text = info.NickName;

            user.OnUserInfoChange += OnUserInfoChange;
        }

        public override void OnInit(UIParamStruct param)
        {
            string type = param.param as string;

            if (type == "create")
            {
                this.txtTitle.text = "创建角色";
                this.btnClose.SetActive(false);
            }
            else
            {
                this.txtTitle.text = "修改昵称";
                this.btnClose.SetActive(true);
            }
        }


        private void OnDestroy()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (user != null)
            {
                user.OnUserInfoChange -= OnUserInfoChange;
            }
        }

        public void OnRandomClick()
        {
            if (this.playerNameList == null || this.playerNameList.nameList.Length <= 0)
            {
                return;
            }


            int r = Druid.Utils.MathUtils.RandomInt(0, this.playerNameList.nameList.Length);
            string rName = this.playerNameList.nameList[r].name;

            this.iputName.text = rName;
        }

        public void OnChangeClick()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo info = user.Data.Userinfo;
            string newName = this.iputName.text.Trim();
            if (newName == "" || newName == info.NickName)
            {
                this.txtError.text = "名字不能为空或与之前相同";
                return;
            }

            if (user.Data.ChangeNameTimes <= 0)
            {
                UIManager.I.Toast("修改失败，没有修改次数");
                return;
            }

            //int newNameLen = System.Text.Encoding.Default.GetBytes(newName).Length;

            //Debug.Log($"newNameLen{newNameLen}");
            //if (newNameLen > 9)
            //{
            //    this.txtError.text = "请勿超过9个汉字";
            //    return;
            //}

            //if (Druid.Utils.GameUtils.ContainsSpecialChars(newName))
            //{
            //    this.txtError.text = "勿包含敏感词汇";
            //    return;
            //}

            this._postUserInfo(newName, info.AvatarUrl);
        }



        private void _postUserInfo(string nickName, string avatar)
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo info = user.Data.Userinfo;

            NetUtils.postUserModify(nickName, (code, message) =>
           {
               Debug.Log($"_postUserInfo{code} {message}");
               if (code == 0)
               {
                   user.Data.ChangeNameTimes--;
                   user.Save();

                   UIManager.I.Toast("修改成功");
                   this.txtError.text = "";
                   this.CloseSelf();
               }
               else
               {
                   UIManager.I.Toast("修改失败");
                   this.txtError.text = message;
               }
           });
        }

        private void OnUserInfoChange()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo info = user.Data.Userinfo;
            this.iputName.text = info.NickName;

        }
    }

}

