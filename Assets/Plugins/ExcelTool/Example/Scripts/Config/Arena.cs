namespace ExcelData
{
	/// <summary>
	/// 竞技场表
	/// <summary>
	[System.Serializable]
	public  class Arena
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 名次
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名次")]
	public int[] price;
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


		#endregion

	}
}
