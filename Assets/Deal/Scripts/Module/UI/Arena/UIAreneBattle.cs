using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Msg;
using DG.Tweening;
using Deal.Data;
using TMPro;

namespace Deal.UI
{
    public class UIAreneBattle : UIBase
    {
        public HeroPvp heroLeft;
        public HeroPvp heroRight;

        public CmpAvatar topLfet;
        public CmpAvatar topRight;

        public CmpAvatar midLfet;
        public CmpAvatar midRight;


        public Slider sliderLeft;
        public Slider sliderRight;

        public TextMeshProUGUI txtCombatLeft;
        public TextMeshProUGUI txtCombatRight;

        // 对手信息
        private Msg_Data_Arena_Playerinfo _self;
        private Msg_Data_Arena_Playerinfo _target;

        public override void OnUIAwake()
        {
            BattleController.I.OnHeroFightExit += this.OnFightExit;
        }

        public override void OnUIDestroy()
        {
            BattleController.I.OnHeroFightExit -= this.OnFightExit;
        }

        /// <summary>
        /// 血条UI更新
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="maxHp"></param>
        public void OnRoleHpUpdateLeft(int hp, int maxHp)
        {
            this.sliderLeft.value = hp / 1.0f / maxHp;
        }

        /// <summary>
        /// 血条UI更新
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="maxHp"></param>
        public void OnRoleHpUpdateRight(int hp, int maxHp)
        {
            this.sliderRight.value = hp / 1.0f / maxHp;
        }

        public override void OnInit(UIParamStruct param)
        {

            PvpSceneManager pvpScene = App.I.CurScene as PvpSceneManager;

            this.heroLeft = pvpScene.HeroLeft;
            this.heroRight = pvpScene.HeroRight;

            this.heroLeft.OnRoleHpUpdate += OnRoleHpUpdateLeft;
            this.heroRight.OnRoleHpUpdate += OnRoleHpUpdateRight;


            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            int weapon = dungeonData.GetEquip(EquipPointEnum.weapon) != null ? dungeonData.GetEquip(EquipPointEnum.weapon).equipId : 0;
            int hat = dungeonData.GetEquip(EquipPointEnum.head) != null ? dungeonData.GetEquip(EquipPointEnum.head).equipId : 0;


            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo Userinfo = userData.Data.Userinfo;
            // 设置角色属性
            Hero hero = PlayManager.I.mHero;
            BattleRoleAtt OriAtt = hero.OriAtt.Clone();
            Msg_Data_Arena_Playerinfo _self = new Msg_Data_Arena_Playerinfo();
            this._self = _self;

            _self.uid = Userinfo.UserId;
            _self.nickname = Userinfo.NickName;
            _self.avatar_url = Userinfo.AvatarUrl;
            _self.max_hp = OriAtt.MaxHP;
            _self.hp = OriAtt.MaxHP;
            _self.attack = OriAtt.Attack;
            _self.crit = OriAtt.Crit;
            _self.dodge = OriAtt.Dodge;
            _self.hit = OriAtt.Hit;
            _self.decrit = OriAtt.DeCrit;
            _self.hpreg = OriAtt.HPReg;
            _self.attack_speed = OriAtt.AttackSpeed;

            int combats = MathUtils.GetCombat(OriAtt);
            _self.combats = combats;

            _self.weapon = weapon;
            _self.hat = hat;


            Msg_Data_Arena_Playerinfo _target = param.param as Msg_Data_Arena_Playerinfo;

            Debug.Log("_target_target" + _target.nickname);
            this._target = _target;

            this.heroRight.RestBattleBaseAtt(_target);
            this.heroLeft.RestBattleBaseAtt(_self);

            // 更新ui
            this.topLfet.SetInfo(_self.nickname, _self.avatar_url);
            this.midLfet.SetInfo(_self.nickname, _self.avatar_url);

            this.topRight.SetInfo(_target.nickname, _target.avatar_url);
            this.midRight.SetInfo(_target.nickname, _target.avatar_url);

            this.txtCombatLeft.text = "战斗力:" + MathUtils.ToKBM((long)_self.combats);
            this.txtCombatRight.text = "战斗力:" + MathUtils.ToKBM((long)_target.combats);


            hero.Controller.SetStatePause();
            hero.gameObject.SetActive(false);

            // 开始
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(2.0f);
            sequence.AppendCallback(() =>
            {
                this.OnShowAnimationEnd();
            });
        }


        public void OnShowAnimationEnd()
        {
            this.heroLeft.gameObject.SetActive(true);
            this.heroRight.gameObject.SetActive(true);


            this.heroLeft.transform.position = new Vector3(-8, 0, 0);
            this.heroRight.transform.position = new Vector3(8, 0, 0);

            this.heroLeft.transform.DOMoveX(-0.5f, 1f);
            this.heroRight.transform.DOMoveX(0.5f, 1f);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1.0f);
            sequence.AppendCallback(() =>
            {
                BattleController.I.OnRoleEnter(heroLeft, heroRight);
            });
        }


        public void OnClose1Click()
        {
            this.heroLeft.gameObject.SetActive(false);
            this.heroRight.gameObject.SetActive(false);

            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStateIdle();
            hero.gameObject.SetActive(true);

            this.heroLeft.OnRoleHpUpdate -= OnRoleHpUpdateLeft;
            this.heroRight.OnRoleHpUpdate -= OnRoleHpUpdateRight;

            this.CloseSelf();
        }


        public void OnFightExit(BattleRoleBase role)
        {
            bool isWin = false;
            if (this.heroLeft.IsDie())
            {
                isWin = false;
            }
            else if (this.heroRight.IsDie())
            {
                isWin = true;
            }

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int currentRank = userData.rankInfo.data.current;

            if (isWin)
            {
                NetUtils.doArenaPkWin(this._target.uid, (res) =>
                {
                    if (res != null)
                    {
                        this.ShowResult(isWin, res.last, res.latest);
                    }
                });
            }
            else
            {
                this.ShowResult(isWin, currentRank, currentRank);
            }

        }


        public async void ShowResult(bool iswin, int befoereRank, int newRank)
        {
            UIAreneResult uIAreneResult = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIResult, UILayer.Dialog, new UIParamStruct(iswin)) as UIAreneResult;
            uIAreneResult.SetTarget(this._self, this._target);
            uIAreneResult.SetRankInfo(befoereRank, newRank);
        }
    }

}
