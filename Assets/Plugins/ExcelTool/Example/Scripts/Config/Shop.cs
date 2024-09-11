namespace ExcelData
{
	/// <summary>
	/// 商店表
	/// <summary>
	[System.Serializable]
	public  class Shop
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 名字
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名字")]
	public string name;
	/// <summary>
	/// 价格
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("价格")]
	public int price;
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
	/// 图片背景
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("图片背景")]
	public string iconBg;
	/// <summary>
	/// 标题背景
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("标题背景")]
	public string titleBg;


		#endregion

	}
}
