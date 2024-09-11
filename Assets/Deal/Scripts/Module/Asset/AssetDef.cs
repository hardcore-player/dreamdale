using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 资产定义
    /// </summary>
    [Serializable]
    public enum AssetEnum
    {
        Task = 1000, //任务
        Gold = 1001, //金币
        Wood = 1002, //木头
        Stone = 1003,  //石头
        Pumpkin = 1004, //南瓜
        Apple = 1005, // 苹果
        Iron = 1006,  // 钢铁
        Plank = 1007, // 木板
        Fish = 1008, // 鱼
        Brick = 1009, // 砖
        Milk = 1010, //牛奶
        Bread = 1011, //面包
        Wool = 1012, // 羊毛
        Grain = 1013, // 谷物
        Treasure = 1014, // 宝藏
        Nail = 1015,  //钉子
        Gem = 1016,  // 宝石（充值的货币）
        Cactus = 1017, // 仙人掌
        Potion = 1018, // 仙人掌药水
        Carrot = 1019, // 胡萝卜
        WinterWood = 1020, // 松树
        Cone = 1021, // 松果
        Ruby = 1022, //红宝石
        None = 0,  // 
        Exp = 1023,  //  经验
        DeadWood = 1024, // 枯木
        DeadWoodPlank = 1025, //枯木板
        FishSoup = 1026, //鱼汤
        Scroll = 1027, // 卷轴
        DungeonExp = 1028, // 角色经验
        Sheep = 1029, // 羊
        Sapphire = 1030, // 蓝宝石
        Emerald = 1031, // 绿宝石
        Amethyst = 1032, // 紫宝石
        Egg = 1033, //鸡蛋
        AncientShard = 1034, //古代碎片
        OrbShard = 1035, //宝珠碎片
        SwordRune = 1036, // 剑符文
        AxeRune = 1037, //  斧符文
        BladeRune = 1038, // 刀符文
        TalismanRune = 1039, // 护身符符文
        ShieldRune = 1040, // 盾符文
        HelmetRune = 1041, // 头盔符文
        ArmorRune = 1042, // 护甲符文
        CloakRune = 1043, //  披风符文
        RareAscensionOrb = 1044, // 稀有进阶宝珠
        Orange = 1045, // 橘子
        Bamboo = 1046, //竹子
        BambooTissue = 1047, //竹纤维
        Ticket = 1048, // 广告券
        ArenaTicket = 1049, // 竞技场挑战券
        Cotton = 1050, // 莲花
        Chicken = 1051, // 鸡
        Bag = 1052, // 鸡
        SakuraWood = 1053,  // 樱树
        SakuraPlank = 1054, //  樱树木板

        GoldAxe = 1055, //  金斧头
        GoldPick = 1056, //  金镐头
        Rune = 1057, //  金镐头
    }
}