using System;

namespace Deal.Msg
{
    /// <summary>
    /// 排行榜列表
    /// </summary>
    [Serializable]
    public class Msg_ArenaRankInfo
    {
        public int code;
        public string message;
        public Msg_Data_ArenaRankInfo data;
    }

    public class Msg_Data_ArenaRankInfo
    {
        public int current; //当前自己的排名
        public Msg_Data_ArenaRankPlayerinfo[] rank;
    }


    [Serializable]
    public class Msg_Data_ArenaRankPlayerinfo
    {
        public long uid;
        public string nickname;
        public string avatar_url;
        public int ranking;
        public double combats;
    }



    /// <summary>
    /// 战斗信息
    /// </summary>
    [Serializable]
    public class Msg_ArenaPkInfo
    {
        public int code;
        public string message;
        public Msg_Data_Arena_Playerinfo data;
    }

    [Serializable]
    public class Msg_Data_Arena_Playerinfo
    {
        public long uid;
        public string nickname;
        public string avatar_url;
        public double combats;
        public float max_hp;
        public float hp;
        public float attack;
        public float crit;
        public float dodge;
        public float hit;
        public float decrit;
        public float hpreg;
        public float attack_speed;
        public int weapon;
        public int hat;
    }


    /// <summary>
    /// 战斗胜利
    /// </summary>
    [Serializable]
    public class Msg_ArenaPkWin
    {
        public int code;
        public string message;
        public Msg_Data_ArenaPkWin data;
    }

    [Serializable]
    public class Msg_Data_ArenaPkWin
    {
        public int latest; //最新排名
        public int last; //上次排名

    }

}


