namespace ExcelData
{
	/// <summary>
	/// 每日任务表
	/// <summary>
	[System.Serializable]
	public  class Daily
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 物品
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("物品")]
	public string goods;
	/// <summary>
	/// 数量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("数量")]
	public int num;
	/// <summary>
	/// 描述
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("描述")]
	public string desc;
	/// <summary>
	/// 数量要求
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("数量要求")]
	public int req;
	/// <summary>
	/// 类型
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("类型")]
	public string type;


		#endregion

	}
}
