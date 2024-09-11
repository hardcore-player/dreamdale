using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using Deal.UI;
using Druid.Utils;
using Deal.Msg;

namespace Deal
{

    public class ShopUtils
    {

        public static void pushShop()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_Task _Task = TaskManager.I.GetTask();
            if (_Task.TaskId >= 30 && userData.openShopServer)
            {
                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIShop, UILayer.Dialog);
            }

        }

        public static void pushGemGuide()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_Task _Task = TaskManager.I.GetTask();
            //if (_Task.TaskId >= 30 && userData.openShopServer)
            //{
            //    UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGuideShop, UILayer.Dialog);
            //}

            if (_Task.TaskId >= 30)
            {
                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIGuideShop, UILayer.Dialog);
            }

        }

        /// <summary>
        /// 当前是否有这种礼包了
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public static bool hasSpecialOfferType(SpecialOfferEnum stype)
        {
            // 限时礼包弹出
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //存在的限时礼包
            List<Data_SpecialOffer> SpecialOffers = userData.Data.SpecialOffers;

            for (int i = 0; i < SpecialOffers.Count; i++)
            {
                Data_SpecialOffer data_ = SpecialOffers[i];
                ExcelData.SpecialOffer special = ConfigManger.I.GetSpecialOfferCfg(data_.Id);
                if (special.type == stype.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否已经买过这个限时礼包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool hasBuySpecialOfferId(int id)
        {
            // 限时礼包弹出
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //存在的限时礼包
            List<int> HasSpecialOffers = userData.Data.HasSpecialOffers;

            for (int i = 0; i < HasSpecialOffers.Count; i++)
            {
                if (HasSpecialOffers[i] == id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 新增一个限时礼包
        /// </summary>
        /// <param name="offerId"></param>
        public static void newSpecialOffer(int offerId)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_SpecialOffer _SpecialOffer = new Data_SpecialOffer();
            _SpecialOffer.Id = offerId;
            _SpecialOffer.EndSeconds = TimeUtils.TimeNowSeconds() + 8 * 60 * 60; ;

            userData.NewSpecialOffer(_SpecialOffer);

            if (userData.openShopServer)
            {
                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UISpecialOffer, UILayer.Dialog, new UIParamStruct(_SpecialOffer));

            }

        }

        /// <summary>
        /// 新增一个限时礼包
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public static void newSpecialOfferGem(int taskId)
        {
            // 限时礼包弹出
            ExcelData.SpecialOffer[] specialOffers = ConfigManger.I.configS.specialOffers;

            for (int i = 0; i < specialOffers.Length; i++)
            {
                if (specialOffers[i].task == taskId)
                {
                    // 没买过
                    if (specialOffers[i].once == 1 && !hasBuySpecialOfferId(specialOffers[i].id))
                    {
                        newSpecialOffer(specialOffers[i].id);
                        break;
                    }
                    else if (specialOffers[i].once == 0)
                    {
                        newSpecialOffer(specialOffers[i].id);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 新增一个限时礼包
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public static void newSpecialOfferScroll()
        {
            Data_Task _Task = TaskManager.I.GetTask();

            // 限时礼包弹出
            ExcelData.SpecialOffer[] specialOffers = ConfigManger.I.configS.specialOffers;

            for (int i = 0; i < specialOffers.Length; i++)
            {
                if (_Task.TaskId >= specialOffers[i].task)
                {
                    if (specialOffers[i].type == SpecialOfferEnum.scroll.ToString())
                    {
                        // 没买过
                        if (specialOffers[i].once == 1 && !hasBuySpecialOfferId(specialOffers[i].id))
                        {
                            newSpecialOffer(specialOffers[i].id);
                            break;
                        }
                        else if (specialOffers[i].once == 0)
                        {
                            newSpecialOffer(specialOffers[i].id);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增一个限时礼包
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public static void newSpecialOfferGoldAndResource()
        {
            Data_Task _Task = TaskManager.I.GetTask();

            // 限时礼包弹出
            ExcelData.SpecialOffer[] specialOffers = ConfigManger.I.configS.specialOffers;

            for (int i = 0; i < specialOffers.Length; i++)
            {
                if (_Task.TaskId == specialOffers[i].task)
                {
                    if (specialOffers[i].type == SpecialOfferEnum.gold.ToString() || specialOffers[i].type == SpecialOfferEnum.resource.ToString())
                    {
                        // 没买过
                        if (specialOffers[i].once == 1 && !hasBuySpecialOfferId(specialOffers[i].id))
                        {
                            newSpecialOffer(specialOffers[i].id);
                            break;
                        }
                        else if (specialOffers[i].once == 0)
                        {
                            newSpecialOffer(specialOffers[i].id);
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 支付 https://queen-1318030669.cos.ap-beijing.myqcloud.com/queen/release
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="shopType"></param>
        /// <param name="price"></param>
        public static void payByOs(int goodsId, string shopType, int price)
        {

            Debug.Log("[ShopUtils] 发起支付");

            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                // 直接发奖
                Debug.Log("[ShopUtils] 直接发奖");
                Msg_Data_OrderList OrderData = new Msg_Data_OrderList();
                OrderData.orderno = "";
                OrderData.status = 0;
                OrderData.goods_id = goodsId;
                OrderData.store = shopType;
                OrderData.price = price;

                orderReward(OrderData);
            }
            else
            {
                NetUtils.doCreateOrder(goodsId, shopType, (orderId) =>
                {
                    if (orderId != null)
                    {
                        onOrderCreate(orderId, goodsId, price);
                    }
                    else
                    {
                        Debug.Log("[ShopUtils] orderId = null");
                    }
                });
            }

            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    // wx支付
            //    WXManager.I.pay(price);
            //}
            //else if (Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    // 生成会话签名
            //    //const ret = await ShopHttp.sendOrderH5Sign(orderId);
            //    //if (!ret || ret.code != 0)
            //    //{
            //    //    c(false);
            //    //    return;
            //    //}

            //    // 发起微信客服支付
            //    string sign = "";
            //    WXManager.I.openService(price, sign);
            //}
        }


        /// <summary>
        /// 拉起客服
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="price"></param>
        public static void onOrderCreate(string orderId, int goods_id, int price)
        {
            if (orderId == null && orderId == "")
            {
                Debug.Log("[ShopUtils] 订单为空");
                return;
            }


            // 保存本地订单
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Msg_Data_OrderList OrderData = new Msg_Data_OrderList();
            OrderData.orderno = orderId;
            OrderData.status = 0;
            OrderData.goods_id = goods_id;
            OrderData.price = price;

            userData.NewOrderList(OrderData);

            var sign = new { order_id = orderId };
            string orderSign = LitJson.JsonMapper.ToJson(sign);

            Debug.Log("orderSign" + orderSign);
            WXManager.I.openService(price, orderSign);
        }


        /// <summary>
        /// 检查订单
        /// </summary>
        public static void CheckOrder()
        {

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            List<Msg_Data_OrderList> ClientOrderList = userData.Data.ClientOrderList;

            if (ClientOrderList == null || ClientOrderList.Count == 0) return;

            // 查询是否有待支付的订单
            bool waitPay = false;

            for (int i = 0; i < ClientOrderList.Count; i++)
            {
                if (ClientOrderList[i].status == 0)
                {
                    waitPay = true;
                    break;
                }
            }

            if (waitPay == false) return;


            NetUtils.doReqOrderList((res) =>
            {
                Debug.Log("OnOrderPause res" + res);

                if (res != null && res.Length > 0)
                {
                    checkReward(res);
                }
            });
        }


        /// <summary>
        /// 检查发奖
        /// </summary>
        /// <param name="dataServer"></param>
        public static void checkReward(Msg_Data_OrderList[] dataServer)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            List<Msg_Data_OrderList> ClientOrderList = userData.Data.ClientOrderList;

            if (ClientOrderList == null || ClientOrderList.Count == 0) return;

            for (int i = 0; i < ClientOrderList.Count; i++)
            {
                string orderno = ClientOrderList[i].orderno;
                int status = ClientOrderList[i].status;

                if (status == 0)
                {
                    Msg_Data_OrderList sData = Array.Find(dataServer, v => v.orderno == orderno);
                    if (sData != null)
                    {
                        if (sData.status == 2)
                        {
                            //  发奖
                            orderReward(sData);
                        }
                        ClientOrderList[i].status = sData.status;
                    }
                }
            }


            // 删除状态不是0的
            for (int i = ClientOrderList.Count - 1; i >= 0; i--)
            {
                if (ClientOrderList[i].status != 0)
                {
                    ClientOrderList.RemoveAt(i);
                }
            }


            userData.Save();
        }

        /// <summary>
        /// 订单发奖
        /// </summary>
        /// <param name="order"></param>
        public static void orderReward(Msg_Data_OrderList order)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            string storeType = order.store;
            int goods_id = order.goods_id;

            WXManager.I.showToast("购买成功");
            if (storeType == "shop")
            {
                ExcelData.Shop shopData = ConfigManger.I.GetShopCfg(goods_id);
                if (shopData.type == "VIP")
                {
                    // VIP
                    int day = shopData.num[0];
                    userData.UnlockVip(day);
                }
                else
                {
                    List<Data_GameAsset> rewards = new List<Data_GameAsset>();

                    // 增加奖励
                    for (int i = 0; i < shopData.goods.Length; i++)
                    {

                        string goods = shopData.goods[i];
                        AssetEnum assetEnum = DealUtils.toAssetEnum(goods);
                        int num = shopData.num[i];
                        userData.AddAssetForce(assetEnum, num);

                        rewards.Add(new Data_GameAsset(assetEnum, num));
                    }

                    UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRewards, UILayer.Dialog, new UIParamStruct(rewards));

                }
            }
            else if (storeType == "specialOffer")
            {
                ExcelData.SpecialOffer shopData = ConfigManger.I.GetSpecialOfferCfg(goods_id);

                List<Data_GameAsset> rewards = new List<Data_GameAsset>();

                // 增加奖励
                for (int i = 0; i < shopData.goods.Length; i++)
                {

                    string goods = shopData.goods[i];
                    AssetEnum assetEnum = DealUtils.toAssetEnum(goods);
                    int num = shopData.num[i];
                    userData.AddAssetForce(assetEnum, num);

                    rewards.Add(new Data_GameAsset(assetEnum, num));
                }

                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRewards, UILayer.Dialog, new UIParamStruct(rewards));

                userData.HasSpecialOffer(goods_id);
                userData.DelSpecialOffer(goods_id);

                userData.Save();
            }
        }


        public static void showRewardAndToUser(List<Data_GameAsset> rewards)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            // 增加奖励
            for (int i = 0; i < rewards.Count; i++)
            {

                AssetEnum assetEnum = rewards[i].assetType;
                int num = rewards[i].assetNum;
                userData.AddAssetForce(assetEnum, num);

                //rewards.Add(new Data_GameAsset(assetEnum, num));
            }

            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRewards, UILayer.Dialog, new UIParamStruct(rewards));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void vipFreeTicket()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (userData.Data.IsVip)
            {
                userData.AddAssetForce(AssetEnum.Ticket, 5);

                List<Data_GameAsset> rewards = new List<Data_GameAsset>();
                rewards.Add(new Data_GameAsset(AssetEnum.Ticket, 5));

                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRewards, UILayer.Dialog, new UIParamStruct(rewards));
            }
        }

    }
}
