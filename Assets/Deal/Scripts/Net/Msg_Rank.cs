using System;

namespace Deal.Msg
{
    /// <summary>
    /// 排行榜列表
    /// </summary>
    [Serializable]
    public class Msg_Rank
    {
        public int code;
        public string message;
        public Msg_Data_Rank data;
    }

    public class Msg_Data_Rank
    {
        public Msg_Data_Rankinfo current; //当前自己的排名
        public Msg_Data_Rankinfo[] rank;
    }


    [Serializable]
    public class Msg_Data_Rankinfo
    {
        public long uid;
        public string nickname;
        public string avatar_url;
        public int ranking;
        public double score;
    }


}


