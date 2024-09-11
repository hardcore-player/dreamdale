namespace ExcelData
{
	/// <summary>
	/// 限时特惠礼包表
	/// <summary>
	[System.Serializable]
	public  class SpecialOffer
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 价格
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("价格")]
	public int price;
	/// <summary>
	/// 原价
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("原价")]
	public int origin;
	/// <summary>
	/// 物品
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("物品")]
	public string[] goods;
	/// <summary>
	/// 数量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("数量")]
	public int[] num;
	/// <summary>
	/// 类型
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("类型")]
	public string type;
	/// <summary>
	/// 开启任务
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("开启任务")]
	public int task;
	/// <summary>
	/// 是否只能买一次
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("是否只能买一次")]
	public int once;
	/// <summary>
	/// 图片
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("图片")]
	public string icon;
	/// <summary>
	/// 背景
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("背景")]
	public string bg;
	/// <summary>
	/// 名称
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名称")]
	public string name;


		#endregion

	}
}
