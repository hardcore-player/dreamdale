namespace ExcelData
{
	/// <summary>
	/// 市场售价表
	/// <summary>
	[System.Serializable]
	public  class Market
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
	/// 中文名字
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文名字")]
	public string display;
	/// <summary>
	/// 单价
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("单价")]
	public float unitPrice;
	/// <summary>
	/// 宝石买入价
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("宝石买入价")]
	public float gemPrice;
	/// <summary>
	/// 任务解锁日常市场
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("任务解锁日常市场")]
	public int task;
	/// <summary>
	/// 
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("")]
	public string scene;
	/// <summary>
	/// 资源市场个数
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("资源市场个数")]
	public int num;
	/// <summary>
	/// 资源市场售价
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("资源市场售价")]
	public int gem;


		#endregion

	}
}
