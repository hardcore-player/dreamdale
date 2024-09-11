using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;
using DG.Tweening;
using Druid.Utils;

namespace Deal.UI
{
    /// <summary>
    /// 广告券价格展示
    /// </summary>
    public class AdTicketPrice : MonoBehaviour
    {

        public GameObject goFree;
        public Image imgAdIcon;
        public GameObject goAd;

        public TextMeshProUGUI txtCd;

        private Action _onAdComplete;

        // 全局广告是否冷却
        public bool GlobalVidoeCdComplete = true;

        private void Awake()
        {

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnAssetChange;
            userData.OnShareChange += OnShareChange;

            PlatformManager.I.OnVideoCacheChange += OnGlobalVidoeCd;

            long videoCdAfter = TimeUtils.TimeNowMilliseconds() - PlatformManager.I.VideoCtAt;
            if (videoCdAfter > Config.VideoCdSecond * 1000)
            {
                this.GlobalVidoeCdComplete = true;
            }
            else
            {
                this.GlobalVidoeCdComplete = false;
            }

            this._renderButton();

            Druid.Utils.UIUtils.AddBtnClick(transform, "", this.OnAdClick);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_onAdSuccess"></param>
        public void SetAdComplete(Action _onAdSuccess)
        {
            this._onAdComplete = _onAdSuccess;
        }


        private void OnDestroy()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnAssetChange -= OnAssetChange;
                userData.OnShareChange -= OnShareChange;
            }

            PlatformManager.I.OnVideoCacheChange -= OnGlobalVidoeCd;

        }

        private void OnAssetChange(AssetEnum assetEnum, int assetNum)
        {
            if (assetEnum == AssetEnum.Ticket)
            {
                this._renderButton();
            }
        }

        private void OnShareChange()
        {
            this._renderButton();
        }


        private void _renderButton()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 广告券
            int assetNum = userData.GetAssetNum(AssetEnum.Ticket);

            Debug.Log("AssetEnum.Ticket" + assetNum);

            if (assetNum > 0)
            {
                this.goAd.SetActive(false);
                this.goFree.SetActive(true);
                this.txtCd.gameObject.SetActive(false);
                this.GetComponent<Button>().interactable = true;
            }
            else
            {

                this.goFree.SetActive(false);
                if (this.GlobalVidoeCdComplete == true)
                {   //广告cd完成
                    this.goAd.SetActive(true);
                    this.txtCd.gameObject.SetActive(false);
                    this.GetComponent<Button>().interactable = true;
                }
                else
                {
                    this.GetComponent<Button>().interactable = false;
                    this.goAd.SetActive(false);
                    this.txtCd.gameObject.SetActive(true);
                    this.txtCd.text = "5秒";
                }

                bool useShare = userData.Data.TodayShareTimes <= 6;
                useShare = false;
                if (useShare)
                {
                    SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasCommon, this.imgAdIcon, "img_com_share");
                }
                else
                {
                    SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasCommon, this.imgAdIcon, "img_com_ad");
                }
            }

        }


        public void OnAdClick()
        {

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            // 广告券
            int ticketNum = userData.GetAssetNum(AssetEnum.Ticket);
            if (ticketNum > 0)
            {
                userData.CostAsset(AssetEnum.Ticket, 1);
                if (this._onAdComplete != null)
                    this._onAdComplete();
            }
            else
            {
                // 分享
                bool useShare = userData.Data.TodayShareTimes <= 6;
                useShare = false;
                if (useShare)
                {

                    WXManager.I.shareAppMessage();
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() =>
                    {
                        userData.AddShareTimes();
                        if (this._onAdComplete != null)
                            this._onAdComplete();
                    });
                }
                else
                {

                    //#if UNITY_EDITOR
                    //PlatformManager.I.OnVideoCallback();
                    //if (this._onAdComplete != null)
                    //    this._onAdComplete();
                    //#else
                    PlatformManager.I.PlayVideoAd((yes) =>
                    {
                        Debug.Log("PlayVideoAd ccc" + yes);
                        if (yes)
                        {
                            ActivityUtils.DoDailyTask(Data.DailyTaskTypeEnum.video, 1);

                            if (this._onAdComplete != null)
                            {
                                Debug.Log("PlayVideoAd _onAdComplete");

                                this._onAdComplete();
                            }

                        }
                    });

                    //WXManager.I.showVideo(() =>
                    //{
                    //    if (this._onAdComplete != null)
                    //        this._onAdComplete();
                    //}, () =>
                    //{

                    //});
                    //#endif


                }
            }
        }


        public void OnGlobalVidoeCd()
        {
            Debug.Log("OnGlobalVidoeCd");
            this.GlobalVidoeCdComplete = false;
            this._renderButton();
        }

        private void Update()
        {
            if (this.GlobalVidoeCdComplete == false)
            {
                // 冷却中
                long videoCdAfter = TimeUtils.TimeNowMilliseconds() - PlatformManager.I.VideoCtAt;
                if (videoCdAfter > Config.VideoCdSecond * 1000)
                {
                    // 已经冷却
                    this.GlobalVidoeCdComplete = true;
                    this._renderButton();
                }
                else
                {
                    this.txtCd.text = (Config.VideoCdSecond - ((int)(videoCdAfter / 1000))) + "秒";
                }
            }
        }

    }

}
