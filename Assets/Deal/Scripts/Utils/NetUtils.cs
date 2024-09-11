using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using Deal.UI;
using Druid.Utils;
using Deal.Data;
using Deal.Msg;
using Deal.Env;

namespace Deal
{

    public class NetUtils
    {

        /// <summary>/**/
        /// 
        /// </summary>
        /// <param name="wxCode"></param>
        /// <param name="nickName"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="dev">//int dev = 0; //开发环境选项 1支持游客登录 0微信登录</param>
        public static void doWxLogin(object postData, Action<Msg_WxLogin> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_wxlogin;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning(res.DataAsText);

                if (res.IsSuccess)
                {
                    Msg_WxLogin _Response = LitJsonEx.JsonMapper.ToObject<Msg_WxLogin>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        Debug.Log("UserId:" + _Response.data.id);
                        Debug.Log("OpenId:" + _Response.data.openid);
                        _Userinfo.AvatarUrl = _Response.data.avatar_url;
                        _Userinfo.NickName = _Response.data.nickname;
                        _Userinfo.UserId = _Response.data.id;
                        _Userinfo.OpenId = _Response.data.openid;
                        _Userinfo.Token = _Response.data.token;
                        _Userinfo.CreateTime = _Response.data.create_time;
                        _Userinfo.IsAuth = _Response.data.is_auth;
                        _Userinfo.UUID = _Response.data.uuid;

                        userData.Save();
                    }
                    callback(_Response);
                }
                else
                {
                    callback(null);
                }
            });
        }

        public static void doDeviceLogin(object postData, Action<Msg_WxLogin> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_devicelogin;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning(res.DataAsText);

                if (res.IsSuccess)
                {
                    Msg_WxLogin _Response = LitJsonEx.JsonMapper.ToObject<Msg_WxLogin>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        Debug.Log("UserId:" + _Response.data.id);
                        Debug.Log("OpenId:" + _Response.data.openid);
                        _Userinfo.AvatarUrl = _Response.data.avatar_url;
                        _Userinfo.NickName = _Response.data.nickname;
                        _Userinfo.UserId = _Response.data.id;
                        _Userinfo.OpenId = _Response.data.openid;
                        _Userinfo.Token = _Response.data.token;
                        _Userinfo.CreateTime = _Response.data.create_time;
                        _Userinfo.IsAuth = _Response.data.is_auth;
                        _Userinfo.UUID = _Response.data.uuid;

                        userData.Save();
                    }
                    callback(_Response);
                }
                else
                {
                    callback(null);
                }
            });
        }


        public static void postUserInfo(string nickName, string avatar, Action<int, string> action = null)
        {
            string url = Config.HttpUrl + UrlDefine.url_wxUserinfoSave;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            // 临时保存数据
            _Userinfo.AvatarUrl = avatar;
            _Userinfo.NickName = nickName;
            //_Userinfo.IsAuth = 1;

            var postData = new { nickname = nickName, avatar_url = avatar };
            UIManager.I.ShowLoading();
            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning(res.DataAsText);
                if (res.IsSuccess)
                {
                    UIManager.I.HideLoading();

                    Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);
                    if (_Response != null && _Response.code == 0)
                    {
                        _Userinfo.AvatarUrl = avatar;
                        _Userinfo.NickName = nickName;
                        _Userinfo.IsAuth = 1;
                        //_Userinfo.AvatarUrl = _Response.data.avatar_url;
                        //_Userinfo.NickName = _Response.data.nickname;
                        //_Userinfo.UserId = _Response.data.id;
                        //_Userinfo.Token = _Response.data.token;
                        //_Userinfo.CreateTime = _Response.data.create_time;
                        //_Userinfo.IsAuth = _Response.data.is_auth;
                        if (userData.OnUserInfoChange != null)
                        {
                            userData.OnUserInfoChange();
                        }
                        userData.Save();

                        if (action != null) action(_Response.code, _Response.message);
                    }
                    else
                    {
                        if (action != null) action(-1, "");
                    }
                }
                else
                {
                    if (action != null) action(-1, "");
                }
            });
        }


        /// <summary>
        /// 单纯修改名字
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="action"></param>
        public static void postUserModify(string nickName, Action<int, string> action = null)
        {
            string url = Config.HttpUrl + UrlDefine.url_wxUserinfoModify;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            // 临时保存数据
            //_Userinfo.NickName = nickName;
            //_Userinfo.IsAuth = 1;

            var postData = new { nickname = nickName };
            UIManager.I.ShowLoading();
            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning(res.DataAsText);
                if (res.IsSuccess)
                {
                    UIManager.I.HideLoading();

                    Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);

                    Debug.Log("_Response============" + _Response);
                    if (_Response != null)
                    {
                        if (_Response.code == 0)
                        {
                            _Userinfo.NickName = nickName;
                            _Userinfo.IsAuth = 1;
                            //_Userinfo.AvatarUrl = _Response.data.avatar_url;
                            //_Userinfo.NickName = _Response.data.nickname;
                            //_Userinfo.UserId = _Response.data.id;
                            //_Userinfo.Token = _Response.data.token;
                            //_Userinfo.CreateTime = _Response.data.create_time;
                            //_Userinfo.IsAuth = _Response.data.is_auth;
                            if (userData.OnUserInfoChange != null)
                            {
                                userData.OnUserInfoChange();
                            }
                            userData.Save();
                        }

                        if (action != null) action(_Response.code, _Response.message);
                    }
                    else
                    {
                        if (action != null) action(-1, "");
                    }
                }
                else
                {
                    if (action != null) action(-1, "");
                }
            });
        }


        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="wxCode"></param>
        /// <param name="nickName"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="dev">//int dev = 0; //开发环境选项 1支持游客登录 0微信登录</param>
        public static void doCreateOrder(int goods, string storeType, Action<string> callback)
        {
            Debug.Log("[NetUtils] doCreateOrder 请求订单");

            string url = Config.HttpUrl + UrlDefine.url_creatOrder;

            Debug.Log("Application.platform:" + Application.platform);

            string os = WXManager.I.getRunPlatform();

            var postData = new { os = os, goods = goods, store = storeType };


            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            UIManager.I.ShowLoading();

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning(res.DataAsText);

                UIManager.I.HideLoading();

                if (res.IsSuccess)
                {
                    Msg_CreateOrder _Response = LitJsonEx.JsonMapper.ToObject<Msg_CreateOrder>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        callback(_Response.data.order_id);

                        userData.Save();
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }


            });
        }


        /// <summary>
        /// 查询订单情况
        /// </summary>
        /// <param name="callback"></param>
        public static void doReqOrderList(Action<Msg_Data_OrderList[]> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_OrderList;

            Debug.Log("doReqOrderListL:" + url);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                Debug.LogWarning("doReqOrderListL");

                if (res.IsSuccess)
                {
                    Msg_OrderList _Response = LitJsonEx.JsonMapper.ToObject<Msg_OrderList>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        callback(_Response.data);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }

        /// <summary>
        /// 查询订单情况
        /// </summary>
        /// <param name="callback"></param>
        public static void doReqMailList(Action<Msg_Data_Mailbox[]> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_mainbox;

            Debug.Log("doReqMailList:" + url);

            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                Debug.LogWarning("doReqOrderListL");

                if (res.IsSuccess)
                {
                    Msg_Mailbox _Response = LitJsonEx.JsonMapper.ToObject<Msg_Mailbox>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        callback(_Response.data);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }

        /// <summary>
        /// 邮件已读
        /// </summary>
        /// <param name="callback"></param>
        public static void doReqMailReceive(long mailId, Action<bool> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_mailReceive;

            Debug.Log("doReqMailReceive:" + url);
            var postData = new { mail_id = mailId };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("doReqMailReceive");

                if (res.IsSuccess)
                {
                    Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(true);
                    }
                    else
                    {
                        callback(false);
                    }
                }
                else
                {
                    callback(false);
                }
            });
        }



        /// <summary>
        /// 竞技场排行版
        /// </summary>
        /// <param name="callback"></param>
        public static void reqArenaRankList(Action<Msg_ArenaRankInfo> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_arenaRankInfo;

            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                Debug.LogWarning("reqArenaRankList");

                if (res.IsSuccess)
                {
                    Msg_ArenaRankInfo _Response = LitJsonEx.JsonMapper.ToObject<Msg_ArenaRankInfo>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(_Response);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }


        /// <summary>
        /// 竞技场战斗属性保存接口
        /// </summary>
        /// <param name="callback"></param>
        public static void doPostArenaCombat(Action<bool> callback)
        {
            Hero hero = PlayManager.I.mHero;

            float max_hp = hero.OriAtt.MaxHP;
            float hp = hero.OriAtt.MaxHP;
            float attack = hero.OriAtt.Attack;
            float crit = hero.OriAtt.Crit;
            float dodge = hero.OriAtt.Dodge;
            float hit = hero.OriAtt.Hit;
            float decrit = hero.OriAtt.DeCrit;
            float hpreg = hero.OriAtt.HPReg;
            float attack_speed = hero.OriAtt.AttackSpeed;
            float combats = MathUtils.GetCombat(hero.OriAtt);

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            int weapon = dungeonData.GetEquip(EquipPointEnum.weapon) != null ? dungeonData.GetEquip(EquipPointEnum.weapon).equipId : 0;
            int hat = dungeonData.GetEquip(EquipPointEnum.head) != null ? dungeonData.GetEquip(EquipPointEnum.head).equipId : 0;

            var postData = new
            {
                combats = combats,
                max_hp = max_hp,
                hp = hp,
                attack = attack,
                crit = crit,
                dodge = dodge,
                hit = hit,
                decrit = decrit,
                hpreg = hpreg,
                attack_speed = attack_speed,
                weapon = weapon,
                hat = hat,
            };


            string url = Config.HttpUrl + UrlDefine.url_arenaCombat;

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("doPostArenaCombat");

                if (res.IsSuccess)
                {
                    Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(true);
                    }
                    else
                    {
                        callback(false);
                    }
                }
                else
                {
                    callback(false);
                }
            });
        }


        /// <summary>
        /// 竞技场战斗属性保存接口
        /// </summary>
        /// <param name="callback"></param>
        public static void doArenaChallenge(long challengeId, Action<Msg_Data_Arena_Playerinfo> callback)
        {
            string url = String.Format(Config.HttpUrl + UrlDefine.url_arenaCombatInfo, "" + challengeId);

            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                Debug.LogWarning("doPostArenaCombat");

                if (res.IsSuccess)
                {
                    Msg_ArenaPkInfo _Response = LitJsonEx.JsonMapper.ToObject<Msg_ArenaPkInfo>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(_Response.data);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }

        /// <summary>
        /// 竞技场战斗属性保存接口
        /// </summary>
        /// <param name="callback"></param>
        public static void doArenaPkWin(long challengeId, Action<Msg_Data_ArenaPkWin> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_arenaPkWin;

            var postData = new { player = challengeId };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("doArenaPkWin");

                if (res.IsSuccess)
                {
                    Msg_ArenaPkWin _Response = LitJsonEx.JsonMapper.ToObject<Msg_ArenaPkWin>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(_Response.data);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }


        /// <summary>
        /// 提交存档
        /// </summary>
        /// <param name="callback"></param>
        public static void postSlotSave(Action<bool> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_userSave;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            string userDataSlot = LitJson.JsonMapper.ToJson(userData.Data);
            string mapDataSlot = LitJson.JsonMapper.ToJson(mapData.Data);
            string dungeonDataSlot = LitJson.JsonMapper.ToJson(dungeonData.Data);

            var postData = new
            {
                userData = userDataSlot,
                mapData = mapDataSlot,
                dungeonData = dungeonDataSlot

            };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
                {
                    Debug.LogWarning("postSlotSave");

                    if (res.IsSuccess)
                    {
                        Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);

                        if (_Response != null && _Response.code == 0)
                        {
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }
                    }
                    else
                    {
                        callback(false);
                    }
                });
        }

        public static void postSlotLoad(Action<Msg_Data_Save> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_userLoad;


            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                Debug.LogWarning("postSlotSave");

                if (res.IsSuccess)
                {
                    Msg_Save _Response = LitJsonEx.JsonMapper.ToObject<Msg_Save>(res.DataAsText);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(_Response.data);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }

        /// <summary>
        /// 玩家数据统计接口
        /// </summary>
        /// <param name="callback"></param>
        public static void postTaskStats(int taskId)
        {
            string url = Config.HttpUrl + UrlDefine.url_stats;

            Debug.Log("postTaskStats:" + url);
            var postData = new { task_id = taskId, type = "task" };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("postTaskStats");

                if (res.IsSuccess)
                {
                }

            });
        }

        public static void postStats(object postData)
        {
            string url = Config.HttpUrl + UrlDefine.url_stats;

            Debug.Log("postTaskStats send:" + url);
            //var postData = new { task_id = taskId, type = "task" };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("postTaskStats callback:" + url);

                if (res.IsSuccess)
                {
                }

            });
        }

        /// <summary>
        /// 玩家数据统计接口
        /// </summary>
        /// <param name="callback"></param>
        public static void postAdStats()
        {
            string url = Config.HttpUrl + UrlDefine.url_stats;

            Debug.Log("postTaskStats:" + url);
            var postData = new { type = "ad" };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("postTaskStats");

                if (res.IsSuccess)
                {
                }

            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void postAttrSave(Action<bool> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_attrSave;


            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            int gold = user.GetAssetNum(AssetEnum.Gold);

            int home_level = user.Data.LandLv * 100000 + user.Data.LandExp;

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            int role_level = dungeonData.Data.HeroLv * 100000 + dungeonData.Data.HeroExp;

            int copy_progress = 0;

            Building_Portal _Portal = MapManager.I.GetSingleBuilding(BuildingEnum.Portal) as Building_Portal;
            if (_Portal != null)
            {
                Data_Portal _Data = _Portal.GetData<Data_Portal>();
                copy_progress = _Data.lvId;
            }

            Hero hero = PlayManager.I.mHero;
            float combats = MathUtils.GetCombat(hero.OriAtt);

            Debug.Log($"gold{gold} home_level{home_level} role_level{role_level} copy_progress{copy_progress} combats{combats}");

            Debug.Log("postAttrSave:" + url);
            var postData = new { gold = gold, home_level = home_level, role_level = role_level, copy_progress = copy_progress, combats = combats };


            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("postAttrSave");

                if (res.IsSuccess)
                {
                    Msg_Response _Response = LitJsonEx.JsonMapper.ToObject<Msg_Response>(res.DataAsText);

                    Debug.LogWarning("postAttrSave code" + _Response.code);

                    if (_Response != null && _Response.code == 0)
                    {
                        callback(true);
                    }
                    else
                    {
                        callback(false);
                    }
                }
                else
                {
                    callback(false);
                }

            });
        }

        public static void postQueryStats(string querys)
        {
            string url = Config.HttpUrl + UrlDefine.url_stats;

            Debug.Log("postQueryStats:" + url);
            var postData = new { type = "attrition", url_query = querys };

            HttpManager.HttpPost(url, postData, getHeaders(), (res) =>
            {
                Debug.LogWarning("postQueryStats=");

                if (res.IsSuccess)
                {
                }

            });
        }

        /// <summary>
        ///  商店配置
        /// </summary>
        /// <param name="callback"></param>
        public static void reqShopConfig(Action<bool> callback)
        {
            //string version = WXManager.I.getVersion();
            //string os = WXManager.I.getRunPlatform();

            string version = Config.VersionCode + "";
            string os = Application.platform.ToString();

            string url = String.Format(Config.HttpUrl + UrlDefine.url_version, version, os);

            Debug.Log("reqShopConfig Application.platform:" + Application.platform);
            Debug.Log("reqShopConfig:" + url);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            HttpManager.HttpGet(url, getHeaders(), (res) =>
            {
                if (res.IsSuccess)
                {
                    Msg_Version _Response = LitJsonEx.JsonMapper.ToObject<Msg_Version>(res.DataAsText);

                    if (_Response != null && _Response.code == 0 && _Response.data != null)
                    {
                        userData.openShopServer = _Response.data.shop;
                        userData.openIdentity = _Response.data.identity;
                        callback(_Response.data.shop);
                    }
                    else
                    {
                        callback(false);
                    }
                }
                else
                {
                    callback(false);
                }
            });
        }

        /// <summary>
        /// 排行榜 排行榜分类：1财富榜 2战力榜 3角色等级榜 4家园等级榜 5副本进度榜
        /// </summary>
        /// <param name="callback"></param>
        public static void reqRankList(int rType, Action<Msg_Rank> callback)
        {
            string url = Config.HttpUrl + UrlDefine.url_RankList;

            Debug.LogWarning("reqRankList rType" + rType);

            HttpManager.HttpPost(url, new { type = rType }, getHeaders(), (res) =>
             {
                 Debug.LogWarning("reqArenaRankList");

                 if (res.IsSuccess)
                 {
                     Msg_Rank _Response = LitJsonEx.JsonMapper.ToObject<Msg_Rank>(res.DataAsText);

                     if (_Response != null && _Response.code == 0)
                     {
                         callback(_Response);
                     }
                     else
                     {
                         callback(null);
                     }
                 }
                 else
                 {
                     callback(null);
                 }
             });
        }

        /// <summary>
        /// 消息头
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> getHeaders()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            string token = userData.Data.Userinfo.Token == null ? "" : userData.Data.Userinfo.Token;

            string appKey = "Lo@uR8B+KR8IcobEXUqbi0lkAQzVl2Td";
            //version = "version"

            //nonce = "nonce"
            string timestamp = TimeUtils.TimeNowMilliseconds() + "";

            string nonce = "";
            System.Random rand = new System.Random();
            for (int i = 0; i < 16; i++)
            {
                nonce = nonce + rand.Next(1, 9);
            }

            string sign = MD5Util.GetMD5_32(timestamp + "" + nonce + "" + appKey); ;


            Debug.Log("appKey:" + appKey);
            Debug.Log("nonce:" + nonce);
            Debug.Log("timestamp:" + timestamp);
            Debug.Log("version:" + Config.VersionCode);
            Debug.Log("sign:" + sign);

            keyValues.Add("authorization", token);
            keyValues.Add("appKey", appKey);
            keyValues.Add("version", Config.VersionCode);
            keyValues.Add("timestamp", timestamp);
            keyValues.Add("nonce", nonce);
            keyValues.Add("sign", sign);

            return keyValues;
        }
    }
}
