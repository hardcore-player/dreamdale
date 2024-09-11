using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using System.Threading.Tasks;
using System;

namespace Deal
{

    public class SpriteUtils
    {
        // 资产图集
        private static Dictionary<string, SpriteAtlas> atlasList = new Dictionary<string, SpriteAtlas>();


        private async static Task<SpriteAtlas> LoadSpriteAtlasAsync(string path)
        {
            AsyncOperationHandle<SpriteAtlas> handle = Addressables.LoadAssetAsync<SpriteAtlas>(path);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!atlasList.ContainsKey(path))
                {
                    atlasList.Add(path, handle.Result);
                }
                return handle.Result;
            }

            Debug.Log("GetInstantiateAsync err:" + handle.Status);
            return null;
        }


        public static async void SetSprite(string atlasPath, Image image, string spName, Action callback = null)
        {
            if (!atlasList.ContainsKey(atlasPath))
            {
                await LoadSpriteAtlasAsync(atlasPath);
            }

            SpriteAtlas atlas = atlasList[atlasPath];

            if (atlas != null)
            {
                Sprite sp = atlas.GetSprite(spName);
                if (sp != null)
                {
                    if (image)
                    {
                        image.sprite = sp;
                    }
                    if (callback != null) callback();
                }
                return;
            }
        }

        public static async void SetSprite(string atlasPath, SpriteRenderer image, string spName, Action callback = null)
        {
            if (!atlasList.ContainsKey(atlasPath))
            {
                await LoadSpriteAtlasAsync(atlasPath);
            }

            SpriteAtlas atlas = atlasList[atlasPath];

            if (atlas != null)
            {
                Sprite sp = atlas.GetSprite(spName);
                if (sp != null)
                {
                    image.sprite = sp;
                }
                return;
            }
        }


        /// <summary>
        /// 设置资产图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetEnum"></param>
        public static void SetAssetSprite(Image image, AssetEnum assetEnum)
        {
            string spName = DealUtils.getAssetSpriteName(assetEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, image, spName);
        }


        public static void SetAssetSprite(SpriteRenderer sr, AssetEnum assetEnum)
        {
            string spName = DealUtils.getAssetSpriteName(assetEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, sr, spName);
        }

        public static void SetEquipIconSprite(SpriteRenderer sr, EquipAttrEnum assetEnum)
        {
            string spName = DealUtils.GetEquipAttrIcon(assetEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasCommon, sr, spName);
        }

        public static void SetEquipIconSprite(Image sr, EquipAttrEnum assetEnum)
        {
            string spName = DealUtils.GetEquipAttrIcon(assetEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasCommon, sr, spName);
        }

        public static void SetAssetSprite1(SpriteRenderer spriteRenderer, AssetEnum assetEnum)
        {

            string spName = DealUtils.getAssetSpriteName(assetEnum) + "0";
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, spriteRenderer, spName);

        }

        /// <summary>
        /// 设置蓝图图片
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="assetEnum"></param>
        public static void SetBlueprintSprite(SpriteRenderer spriteRenderer, BluePrintEnum bluePrint)
        {
            //string spName = DealUtils.getAssetSpriteName(assetEnum) + "0";
            string spName = "img_res_apple";
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, spriteRenderer, spName);
        }

        public static void SetBlueprintSprite(Image img, BluePrintEnum bluePrint)
        {
            //string spName = DealUtils.getAssetSpriteName(assetEnum) + "0";
            string spName = "img_res_apple";
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, img, spName);
        }


        /// <summary>
        /// 设置资产图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetEnum"></param>
        public static void SetToolSprite(Image image, WorkshopToolEnum toolEnum)
        {
            string spName = DealUtils.getToolSpriteName(toolEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, image, spName);
        }

        /// <summary>
        /// 设置资产图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetEnum"></param>
        public static void SetToolSprite(SpriteRenderer image, WorkshopToolEnum toolEnum)
        {
            string spName = DealUtils.getToolSpriteName(toolEnum);
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, image, spName);
        }

        /// <summary>
        /// 工具带描边带icon
        /// </summary>
        /// <param name="image"></param>
        /// <param name="toolEnum"></param>
        public static void SetToolSprite1(SpriteRenderer image, WorkshopToolEnum toolEnum)
        {
            string spName = DealUtils.getToolSpriteName(toolEnum) + "0";
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, image, spName);
        }

        /// <summary>
        /// 大厅能力icon
        /// </summary>
        /// <param name="image"></param>
        /// <param name="hallAbility"></param>
        public static void SetHallAbilitySprite(Image image, HallAbilityEnum hallAbility)
        {

            string spName = "img_HallIcon" + (int)hallAbility;
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasHall, image, spName);
        }


        public static void SetEquipIcon(Image image, int equipId, Action action = null)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasEquip, image, "equip" + equipId, action);
        }

        public static void SetEquipIcon(SpriteRenderer image, int equipId, Action action = null)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasEquip, image, "equip" + equipId, action);
        }

        public static void SetRoleEquipIcon(Image image, int equipId, Action action = null)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasEquip, image, "img_equip" + equipId, action);
        }

        public static void SetRoleEquipIcon(SpriteRenderer image, int equipId, Action action = null)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasEquip, image, "img_equip" + equipId, action);
        }

        public static void SetBlueprintSprite(SpriteRenderer spriteRenderer)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, spriteRenderer, "img_res_blueprint");

        }


        public static void SetStaueBlueprintSprite(SpriteRenderer spriteRenderer)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasAssets, spriteRenderer, "img_res_statue");
        }

        /// <summary>
        /// 雕塑
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="statueEnum"></param>
        public static void SetStatueSprite(SpriteRenderer spriteRenderer, StatueEnum statueEnum)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasHouse, spriteRenderer, getSuatueSpName(statueEnum));
        }

        public static void SetStatueSprite(Image spriteRenderer, StatueEnum statueEnum)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasHouse, spriteRenderer, getSuatueSpName(statueEnum));
        }

        public static void SetDiamondMineSprite(SpriteRenderer spriteRenderer, BuildingEnum buildingEnum)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasHouse, spriteRenderer, getDiamonMonie(buildingEnum));
        }

        public static void SetDiamondMineSprite(Image spriteRenderer, BuildingEnum buildingEnum)
        {
            SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasHouse, spriteRenderer, getDiamonMonie(buildingEnum));
        }

        private static string getDiamonMonie(BuildingEnum buildingEnum)
        {
            if (buildingEnum == BuildingEnum.DiamondMine)
            {
                return "img_house_gemmine_b0";
            }
            else if (buildingEnum == BuildingEnum.RubyMine)
            {
                return "img_house_gemmine_r0";
            }
            else if (buildingEnum == BuildingEnum.EmeraldMine)
            {
                return "img_house_gemmine_g0";
            }
            else if (buildingEnum == BuildingEnum.AmethystMine)
            {
                return "img_house_gemmine_p0";
            }
            return "img_house_gemmine_b0";
        }

        /// <summary>
        /// 雕塑图片
        /// </summary>
        /// <param name="statueEnum"></param>
        /// <returns></returns>
        private static string getSuatueSpName(StatueEnum statueEnum)
        {
            string sprite = "";
            if (statueEnum == StatueEnum.Triumphal)
            {
                sprite = "img_statue14";
            }
            else if (statueEnum == StatueEnum.Colossus)
            {
                sprite = "img_statue1";
            }
            else if (statueEnum == StatueEnum.GoldApple)
            {
                sprite = "img_statue2";
            }
            else if (statueEnum == StatueEnum.Miner)
            {
                sprite = "img_statue3";
            }
            else if (statueEnum == StatueEnum.MusicalKey)
            {
                sprite = "img_statue4";
            }
            else if (statueEnum == StatueEnum.Excalibur)
            {
                sprite = "img_statue5";
            }
            else if (statueEnum == StatueEnum.Totem)
            {
                sprite = "img_statue6";
            }
            else if (statueEnum == StatueEnum.Dinosaur)
            {
                sprite = "img_statue7";
            }
            else if (statueEnum == StatueEnum.Elemental)
            {
                sprite = "img_statue8";
            }
            else if (statueEnum == StatueEnum.Time)
            {
                sprite = "img_statue9";
            }
            else if (statueEnum == StatueEnum.Sacred)
            {
                sprite = "img_statue10";
            }
            else if (statueEnum == StatueEnum.Lumber)
            {
                sprite = "img_statue11";
            }
            else if (statueEnum == StatueEnum.Heart)
            {
                sprite = "img_statue12";
            }
            else if (statueEnum == StatueEnum.Scarecrow)
            {
                sprite = "img_statue13";
            }

            //return "Statue/" + sprite;
            return sprite;
        }

    }
}