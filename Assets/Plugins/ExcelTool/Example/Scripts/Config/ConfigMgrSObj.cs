namespace ExcelData
{
	/// <summary>
	/// 数据管理类 : 脚本自动生成，勿手动修改
	/// <summary>
	[System.Serializable]
	public  class ConfigMgrSObj : UnityEngine.ScriptableObject
	{
		#region 变量

	/// <summary>
	/// 竞技场表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("竞技场表")]
	public Arena[] arenas;
	/// <summary>
	/// 每日任务表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("每日任务表")]
	public Daily[] dailys;
	/// <summary>
	/// 钻石升级表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("钻石升级表")]
	public Diamond[] diamonds;
	/// <summary>
	/// 地下城难度表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("地下城难度表")]
	public Dungeon[] dungeons;
	/// <summary>
	/// 装备表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("装备表")]
	public Equip[] equips;
	/// <summary>
	/// 大厅升级表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("大厅升级表")]
	public HallUpgrade[] hallUpgrades;
	/// <summary>
	/// 灯塔地图表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("灯塔地图表")]
	public Lighthouse[] lighthouses;
	/// <summary>
	/// 市场售价表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("市场售价表")]
	public Market[] markets;
	/// <summary>
	/// 怪物表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("怪物表")]
	public Monster[] monsters;
	/// <summary>
	/// 资源建筑
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("资源建筑")]
	public ResBuilding[] resBuildings;
	/// <summary>
	/// 实验室表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("实验室表")]
	public Research[] researchs;
	/// <summary>
	/// 资源表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("资源表")]
	public Resource[] resources;
	/// <summary>
	/// 7天登录奖励表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("7天登录奖励表")]
	public SevenDay[] sevenDays;
	/// <summary>
	/// 商店表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("商店表")]
	public Shop[] shops;
	/// <summary>
	/// 太空飞船表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("太空飞船表")]
	public SpaceShip[] spaceShips;
	/// <summary>
	/// 限时特惠礼包表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("限时特惠礼包表")]
	public SpecialOffer[] specialOffers;
	/// <summary>
	/// 雕像表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("雕像表")]
	public Statue[] statues;
	/// <summary>
	/// 任务表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("任务表")]
	public Task[] tasks;
	/// <summary>
	/// 工坊工具表
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("工坊工具表")]
	public Workshop[] workshops;


		#endregion

	}
}
