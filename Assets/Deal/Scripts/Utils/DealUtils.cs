using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Druid;
using System;

namespace Deal
{

    public class DealUtils
    {

        public static AssetEnum toAssetEnum(string s)
        {
            return (AssetEnum)Enum.Parse(typeof(AssetEnum), s);
        }

        /// <summary>
        /// 设置资产图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetEnum"></param>
        public static string getAssetSpriteName(AssetEnum assetEnum)
        {
            switch (assetEnum)
            {
                case AssetEnum.Gold:
                    return "img_res_gold";
                case AssetEnum.Apple:
                    return "img_res_apple";
                case AssetEnum.Pumpkin:
                    return "img_res_pumpkin";
                case AssetEnum.Stone:
                    return "img_res_stone";
                case AssetEnum.Wood:
                    return "img_res_wood";
                case AssetEnum.Exp:
                    return "img_res_exp";
                case AssetEnum.DungeonExp:
                    return "img_res_exp";
                case AssetEnum.Plank:
                    return "img_res_plank";
                case AssetEnum.Fish:
                    return "img_res_fish";
                case AssetEnum.Iron:
                    return "img_res_iron";
                case AssetEnum.Gem:
                    return "img_res_gem";
                case AssetEnum.Brick:
                    return "img_res_brick";
                case AssetEnum.Wool:
                    return "img_res_wool";
                case AssetEnum.Sheep:
                    return "img_res_sheep";
                case AssetEnum.Grain:
                    return "img_res_grain";
                case AssetEnum.Bread:
                    return "img_res_bread";
                case AssetEnum.Sapphire:
                    return "img_res_sapphire";
                case AssetEnum.Ruby:
                    return "img_res_ruby";
                case AssetEnum.Amethyst:
                    return "img_res_amethyst";
                case AssetEnum.Emerald:
                    return "img_res_emerald";
                case AssetEnum.Nail:
                    return "img_res_nail";
                case AssetEnum.Milk:
                    return "img_res_milk";
                case AssetEnum.AncientShard:
                    return "img_res_ancientshard";
                case AssetEnum.Ticket:
                    return "img_res_ticket";
                case AssetEnum.SwordRune:
                    return "img_res_swordrune";
                case AssetEnum.AxeRune:
                    return "img_res_axerune";
                case AssetEnum.BladeRune:
                    return "img_res_bladerune";
                case AssetEnum.TalismanRune:
                    return "img_res_talismanrune";
                case AssetEnum.ShieldRune:
                    return "img_res_shieldrune";
                case AssetEnum.HelmetRune:
                    return "img_res_helmetrune";
                case AssetEnum.ArmorRune:
                    return "img_res_armorrune";
                case AssetEnum.CloakRune:
                    return "img_res_cloakrune";
                case AssetEnum.Scroll:
                    return "img_res_scroll";
                case AssetEnum.Cone:
                    return "img_res_cone";
                case AssetEnum.ArenaTicket:
                    return "img_res_challenge";
                case AssetEnum.OrbShard:
                    return "img_res_orbshard";
                case AssetEnum.Cactus:
                    return "img_res_cactus";
                case AssetEnum.WinterWood:
                    return "img_res_winterwood";
                case AssetEnum.DeadWood:
                    return "img_res_deadwood";
                case AssetEnum.Bamboo:
                    return "img_res_bamboo";
                case AssetEnum.Cotton:
                    return "img_res_cotton";
                case AssetEnum.Chicken:
                    return "img_res_chicken";
                case AssetEnum.Potion:
                    return "img_res_potion";
                case AssetEnum.DeadWoodPlank:
                    return "img_res_deadwoodplank";
                case AssetEnum.Orange:
                    return "img_res_orange";
                case AssetEnum.FishSoup:
                    return "img_res_fishsoup";
                case AssetEnum.BambooTissue:
                    return "img_res_bambootissue";
                case AssetEnum.Egg:
                    return "img_res_egg";
                case AssetEnum.Carrot:
                    return "img_res_carrot";
                case AssetEnum.Bag:
                    return "img_res_bag";
                case AssetEnum.GoldAxe:
                    return "img_res_axegold";
                case AssetEnum.GoldPick:
                    return "img_res_pickaxegold";
                case AssetEnum.Rune:
                    return "img_res_randomrune";
                default:
                    return "img_res_unknown";
            }
        }


        /// <summary>
        /// 设置资产图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetEnum"></param>
        public static string getToolSpriteName(WorkshopToolEnum toolEnum)
        {
            switch (toolEnum)
            {
                case WorkshopToolEnum.Axe:
                    return "img_res_axe";
                case WorkshopToolEnum.Pickaxe:
                    return "img_res_pickaxe";
                case WorkshopToolEnum.Shovel:
                    return "img_res_shovel";
                //case WorkshopToolEnum.Sword:
                //    return "img_res_sword";
                case WorkshopToolEnum.FishingRod:
                    return "img_res_fishingrod";
                case WorkshopToolEnum.Sickle:
                    return "img_res_sickle";
                default:
                    return "img_res_axe";
            }
        }

        public static string GetEquipAttrIcon(EquipAttrEnum attrEnum)
        {
            switch (attrEnum)
            {
                case EquipAttrEnum.attack:
                    return "img_character_atk";
                case EquipAttrEnum.hp:
                    return "img_character_hp";
                case EquipAttrEnum.crit:
                    return "img_character_crit";
                case EquipAttrEnum.dodge:
                    return "img_character_dodge";
                case EquipAttrEnum.hit:
                    return "img_character_hit";
                case EquipAttrEnum.decrit:
                    return "img_character_decrit";
                case EquipAttrEnum.hreg:
                    return "img_character_hpreg";

                default:
                    return "img_res_axe";
            }
        }

        /// <summary>
        /// 资产是否占用背包格子
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public static bool isAssetInBagTotal(AssetEnum assetEnum)
        {
            if (assetEnum == AssetEnum.Gold || assetEnum == AssetEnum.Gem
                || assetEnum == AssetEnum.Scroll || assetEnum == AssetEnum.Ticket || assetEnum == AssetEnum.ArenaTicket
                || assetEnum == AssetEnum.AncientShard || assetEnum == AssetEnum.Exp || assetEnum == AssetEnum.DungeonExp
                || assetEnum == AssetEnum.OrbShard || assetEnum == AssetEnum.SwordRune || assetEnum == AssetEnum.AxeRune
                || assetEnum == AssetEnum.BladeRune || assetEnum == AssetEnum.TalismanRune || assetEnum == AssetEnum.ShieldRune
                || assetEnum == AssetEnum.HelmetRune || assetEnum == AssetEnum.ArmorRune || assetEnum == AssetEnum.CloakRune)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 掉落的物品展示，分物品实际数量，和展示图标数量，一般的物品比如宝石、金币、木材等，展示图标上限是5，例如：当掉落的某种物品（1-5）个时，则出现（1-5）个图标，每个图标代表一个物品，即+1
        ///如果掉落物品大于5比如7，则固定出现5个图标，有两个图标+2，三个图标+1
        ///如果掉落物品数是23，则固定出现5个图标，有三个图标+5，两个图标+4
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="assetNum"></param>
        /// <param name="pos"></param>
        public static void newDropItem(AssetEnum asset, int assetNum, Vector3 pos, bool bigCount = false, RoleBase role = null)
        {
            int maxCount = bigCount == true ? 50 : 5;
            List<int> tmp = new List<int>();
            for (int i = 0; i < maxCount; i++)
            {
                tmp.Add(0);
            }

            while (assetNum > 0)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    if (assetNum > 0)
                    {
                        tmp[i]++;
                        assetNum--;
                    }
                }
            }

            for (int i = 0; i < maxCount; i++)
            {
                if (tmp[i] > 0)
                {
                    Vector3 rp = Deal.MathUtils.GetCirclePoint(Deal.NumDefine.FallRadius);

                    DropProp dropProp = PlayManager.I.SpawnDropItem();
                    dropProp.SetProp(asset, tmp[i]);
                    dropProp.SetOwner(role);
                    dropProp.transform.position = pos;
                    dropProp.Fall2Ground(pos + rp);
                }
            }


        }

        /// <summary>
        /// 新建一个飘字
        /// </summary>
        public static void newPopNum(Vector3 startP, string str)
        {
            int rx = Druid.Utils.MathUtils.RandomInt(-5, 5);
            int ry = Druid.Utils.MathUtils.RandomInt(-5, 5);

            startP = startP + new Vector3(rx / 10f, ry / 10f);

            TextMeshPro item = PlayManager.I.SpawnDropNumItem();
            item.text = str;
            item.transform.position = startP;

            Sequence s = DOTween.Sequence();
            s.Append(item.transform.DOMove(startP + new Vector3(0, 1, 0), 0.5f));
            s.AppendCallback(() =>
            {
                PlayManager.I.DespawnDropNumItem(item.transform);
            });
        }

        /// <summary>
        /// 新建一个飘字
        /// </summary>
        public static void newPopMonsterXX(Vector3 startP)
        {

            Addressables.InstantiateAsync(AddressbalePathEnum.PREFAB_MontserXX).Completed += (obj) =>
            {
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.position = startP;

                TextMeshPro tmp = go.GetComponent<TextMeshPro>();

                Sequence s = DOTween.Sequence();
                s.Append(tmp.DOFade(0, 0.03f));
                s.AppendInterval(0.47f);
                s.Append(tmp.DOFade(1, 0.03f));
                s.AppendInterval(0.47f);
                s.Append(tmp.DOFade(0, 0.03f));
                s.AppendInterval(0.47f);
                s.Append(tmp.DOFade(1, 0.03f));
                s.AppendInterval(0.47f);
                s.AppendCallback(() =>
                {
                    GameObject.Destroy(go);
                });

            };
        }


        /// <summary>
        /// 获得采集种类
        /// </summary>
        /// <param name="role"></param>
        /// <param name="collectableAsset"></param>
        /// <returns></returns>
        public static AssetEnum getCollectAssetEnum(RoleBase role, List<CollectableRes> collectableAsset)
        {

            AssetEnum collectAsset = AssetEnum.None;

            // 是可以采集的
            for (int i = 0; i < collectableAsset.Count; i++)
            {
                var item = collectableAsset[i];
                Data_CollectableRes dd = item.GetData<Data_CollectableRes>();

                if (item.CanCollect() && role.CanCollectAsset(dd.AssetId))
                {
                    collectAsset = dd.AssetId;
                    break;
                }
            }

            return collectAsset;
        }

        /// <summary>
        /// 角色采集一种资源
        /// </summary>
        /// <param name="role"></param>
        /// <param name="collectableAsset"></param>
        /// <param name="collectAsset"></param>
        /// <param name="tool"></param>
        /// <param name="fallNum"></param>
        public static List<CollectableRes> RoleCollectAsset(RoleBase role, List<CollectableRes> collectableAsset, AssetEnum collectAsset, WorkshopToolEnum tool, int fallNum, bool fall = true)
        {
            List<CollectableRes> _list = new List<CollectableRes>();

            for (int i = 0; i < collectableAsset.Count; i++)
            {
                var item = collectableAsset[i];
                Data_CollectableRes dd = item.GetData<Data_CollectableRes>();

                if (dd.AssetId == collectAsset && dd.AssetLeft > 0)
                {
                    _list.Add(item);
                }
            }


            Weapon weapon = role.GetWeapon(tool);
            if (weapon)
            {
                weapon.OnAttack = null;
                if (fall == true)
                {
                    weapon.OnAttack = () =>
                    {
                        for (int i = 0; i < _list.Count; i++)
                        {
                            var item = _list[i];
                            item.FallAsset(fallNum, role, weapon.IsGold);
                        }
                    };
                }
                else
                {
                    weapon.OnAttack = () =>
                    {
                        for (int i = 0; i < _list.Count; i++)
                        {
                            var item = _list[i];
                            item.OnFallAsset(0, role);
                        }
                    };
                }

            }

            return _list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="num"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void NewDropPropBezierToTarget(AssetEnum asset, int num, Vector3 from, Vector3 to)
        {
            DropProp item = PlayManager.I.SpawnDropItem();
            item.transform.position = from;
            item.SetProp(asset, num);
            item.BezierToTarget(to);
        }

        /// <summary>
        /// 升级奖励
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="num"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static List<Data_GameAsset> GetHeroLvupReword()
        {
            List<Data_GameAsset> reward = new List<Data_GameAsset>();
            reward.Add(new Data_GameAsset(AssetEnum.Gem, 4));
            reward.Add(new Data_GameAsset(AssetEnum.Scroll, 2));

            int r = Druid.Utils.MathUtils.RandomInt(0, 8);
            if (r == 0)
            {
                reward.Add(new Data_GameAsset(AssetEnum.SwordRune, 5));
            }
            else if (r == 1)
            {
                reward.Add(new Data_GameAsset(AssetEnum.AxeRune, 5));
            }
            else if (r == 2)
            {
                reward.Add(new Data_GameAsset(AssetEnum.BladeRune, 5));
            }
            else if (r == 3)
            {
                reward.Add(new Data_GameAsset(AssetEnum.TalismanRune, 5));
            }
            else if (r == 4)
            {
                reward.Add(new Data_GameAsset(AssetEnum.ShieldRune, 5));
            }
            else if (r == 5)
            {
                reward.Add(new Data_GameAsset(AssetEnum.HelmetRune, 5));
            }
            else if (r == 6)
            {
                reward.Add(new Data_GameAsset(AssetEnum.ArmorRune, 5));
            }
            else if (r == 7)
            {
                reward.Add(new Data_GameAsset(AssetEnum.CloakRune, 5));
            }

            return reward;
        }

        private static List<Data_GameAsset> GetDungeonDrop()
        {
            List<Data_GameAsset> reward = new List<Data_GameAsset>();

            int r = Druid.Utils.MathUtils.RandomInt(0, 100);
            if (r < 10)
            {
                reward.Add(new Data_GameAsset(AssetEnum.Scroll, 1));
            }
            else
            {
                int gold = Druid.Utils.MathUtils.RandomInt(15, 25);

                reward.Add(new Data_GameAsset(AssetEnum.Gold, gold));

            }
            return reward;

        }

        /// <summary>
        /// 地牢怪物掉落
        /// </summary>
        /// <param name="postion"></param>
        public static void DungeonMonsterDrop(Vector3 postion)
        {
            List<Data_GameAsset> reward = GetDungeonDrop();
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //雕塑减怪物血
            float buffVal = MathUtils.GetStatueBuff(StatueEnum.Triumphal);

            int baseExp = 10;
            int exp = (int)(baseExp * (1 + buffVal));

            reward.Add(new Data_GameAsset(AssetEnum.DungeonExp, exp));

            foreach (var item in reward)
            {
                DealUtils.newDropItem(item.assetType, item.assetNum, postion);
            }

        }

        public static void DungeonBuildingDrop(Vector3 postion)
        {
            List<Data_GameAsset> reward = GetDungeonDrop();

            foreach (var item in reward)
            {
                DealUtils.newDropItem(item.assetType, item.assetNum, postion);
            }

        }

        public static void DungeonChestDrop(Vector3 postion)
        {
            List<Data_GameAsset> reward = new List<Data_GameAsset>();

            reward.Add(new Data_GameAsset(AssetEnum.Gem, 4));

            int r = Druid.Utils.MathUtils.RandomInt(0, 100);
            if (r < 10)
            {
                int gold = Druid.Utils.MathUtils.RandomInt(15, 25);
                reward.Add(new Data_GameAsset(AssetEnum.Gold, gold));
            }
            else if (r < 30)
            {
                reward.Add(new Data_GameAsset(AssetEnum.Scroll, 1));
            }

            foreach (var item in reward)
            {
                DealUtils.newDropItem(item.assetType, item.assetNum, postion);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public async static void newDamageEffect(BattleRoleBase role)
        {
            Vector3 vpos = role.transform.position + new Vector3(0, 0.5f, 0);

            GameObject effect = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_HitEffect, vpos);
            effect.transform.localScale = role.roleBody.localScale;

            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.AppendCallback(() =>
            {
                GameObject.Destroy(effect);
            });
        }

        /// <summary>
        /// 攻击伤害飘字
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="role"></param>
        public static async void newDamageNum(RoleDamageData damage, BattleRoleBase role)
        {
            string path = null;

            if (damage.Hit)
            {
                if (damage.Crit)
                {

                    path = AddressbalePathEnum.PREFAB_CritDamageNum;
                }
                else
                {
                    path = AddressbalePathEnum.PREFAB_DamageNum;
                }
            }
            else
            {
                path = AddressbalePathEnum.PREFAB_MissDamageNum;
            }

            Vector3 startP = role.transform.position;
            GameObject go = await ResManager.I.GetInstantiate(path, startP);
            TextMeshPro textMesh = go.GetComponent<TextMeshPro>();

            if (damage.Hit)
            {
                if (damage.Crit)
                {
                    textMesh.text = $"暴击 -{damage.Damage}";
                }
                else
                {
                    textMesh.text = $"-{damage.Damage}";
                }
            }
            else
            {
                textMesh.text = "闪避";
            }

            textMesh.transform.position = startP;

            int flyX = -1;
            if (role.roleBody.lossyScale.x < 0)
            {
                flyX = 1;
            }

            Sequence s = DOTween.Sequence();
            s.Append(textMesh.transform.DOMove(startP + new Vector3(flyX, 1, 0), 0.8f));
            s.AppendCallback(() =>
            {
                GameObject.Destroy(textMesh.gameObject);
            });
        }

        /// <summary>
        /// 回血
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="role"></param>
        public async static void newRecoverHpNum(float hp, BattleRoleBase role)
        {
            Transform pfb = null;
            //PlayManager.I.recoverHpItem;

            Vector3 startP = role.transform.position;
            GameObject go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_AddDamageNum, startP);

            TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
            textMesh.text = $"+{hp}";

            textMesh.transform.position = startP;

            int flyX = -1;
            if (role.roleBody.lossyScale.x < 0)
            {
                flyX = 1;
            }

            GameObject effect = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_HpRestoreEffect);

            effect.transform.position = role.transform.position;

            Sequence s = DOTween.Sequence();
            s.Append(textMesh.transform.DOMove(startP + new Vector3(flyX, 1, 0), 0.8f));
            s.AppendCallback(() =>
            {
                GameObject.Destroy(textMesh.gameObject);
                GameObject.Destroy(effect);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quality">品质白绿蓝紫橙12345</param>
        public static string getQualityName(int quality)
        {
            if (quality == 1)
            {
                return "普通";
            }
            else if (quality == 2)
            {
                return "罕见";
            }
            else if (quality == 3)
            {
                return "稀有";
            }
            else if (quality == 4)
            {
                return "史诗";
            }
            else if (quality == 5)
            {
                return "传奇";
            }
            return "";
        }

        public static Color getQualityColor(int quality)
        {
            if (quality == 1)
            {
                return new Color(1, 1, 1);
            }
            else if (quality == 2)
            {
                return new Color(1, 1, 1);
            }
            else if (quality == 3)
            {
                return new Color(1, 1, 1);
            }
            else if (quality == 4)
            {
                return new Color(1, 1, 1);
            }
            else if (quality == 5)
            {
                return new Color(1, 1, 1);
            }
            return new Color(1, 1, 1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="quality">品质白绿蓝紫橙12345</param>
        public static string getAttrName(EquipAttrEnum attrEnum)
        {
            switch (attrEnum)
            {
                case EquipAttrEnum.hp:
                    return "生命值";
                case EquipAttrEnum.attack:
                    return "攻击力";
                case EquipAttrEnum.crit:
                    return "暴击率";
                case EquipAttrEnum.decrit:
                    return "抗暴率";
                case EquipAttrEnum.hit:
                    return "命中率";
                case EquipAttrEnum.dodge:
                    return "闪避率";
                case EquipAttrEnum.hreg:
                    return "再生值";
                default:
                    return "";
            }

        }

        public static string getAttrDisplay(EquipAttrEnum attrEnum, float n)
        {
            switch (attrEnum)
            {
                case EquipAttrEnum.hp:
                    return n.ToString("#0");
                case EquipAttrEnum.attack:
                    return n.ToString("#0");
                case EquipAttrEnum.crit:
                    return n.ToString("#0.0") + "%";
                case EquipAttrEnum.decrit:
                    return n.ToString("#0.0") + "%";
                case EquipAttrEnum.hit:
                    return n.ToString("#0.0") + "%";
                case EquipAttrEnum.dodge:
                    return n.ToString("#0.0") + "%";
                case EquipAttrEnum.hreg:
                    return n.ToString("#0.0") + "%";
                default:
                    return n.ToString("#0.0") + "";
            }

        }


        public static void Guide2BuildingData(Data_BuildingBase target)
        {
            if (App.I.CurScene.sceneName == SceneEnum.dungeon)
            {
                return;
            }

            //StatueEnum statueEnum = statueEnums[this._pageId];
            MapRender mapRender = MapManager.I.mapRender;

            //// 目标雕塑
            //Data_BuildingBase target = null;
            //for (int i = 0; i < mapRender.SO_MapData.Builds.Count; i++)
            //{
            //    Data_BuildingBase builds = mapRender.SO_MapData.Builds[i];

            //    if (builds.BuildingEnum == BuildingEnum.Statue && builds.StatueEnum == statueEnum)
            //    {
            //        target = builds;
            //        break;
            //    }
            //}

            if (target != null)
            {
                GameObject guideArrow = GameObject.Instantiate(MapManager.I.mapRender.guideArrow);

                guideArrow.SetActive(true);


                BindingSaveData go = mapRender.GetBuyUniqueId(target.UniqueId());
                if (go != null)
                {
                    if (go.guide != null)
                    {
                        guideArrow.transform.position = go.guide.transform.position;
                    }
                    else
                    {
                        guideArrow.transform.position = go.transform.position + new Vector3(0, 1, 0);
                    }

                }
                else
                {
                    guideArrow.transform.position = target.WorldPos + new Vector3(0, 1, 0);

                }

                GameSceneManager gameScene = App.I.CurScene as GameSceneManager;


                // 镜头移动

                gameScene.cinemachine2.Follow = guideArrow.transform;

                Hero hero = PlayManager.I.mHero;

                hero.Controller.SetStatePause();

                Sequence s = DOTween.Sequence();
                s.AppendInterval(0);
                s.AppendCallback(() =>
                {
                    gameScene.cinemachine1.gameObject.SetActive(false);
                    gameScene.cinemachine2.gameObject.SetActive(true);
                });
                s.AppendInterval(1f);
                s.AppendInterval(0.5f);
                s.AppendCallback(() =>
                {
                    gameScene.cinemachine1.gameObject.SetActive(true);
                    gameScene.cinemachine2.gameObject.SetActive(false);

                });
                s.AppendInterval(0);
                s.AppendCallback(() =>
                {
                    hero.Controller.SetStateIdle();

                    GameObject.Destroy(guideArrow);
                });

            }

        }


    }
}