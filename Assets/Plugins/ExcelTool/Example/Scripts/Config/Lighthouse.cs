namespace ExcelData
{
	/// <summary>
	/// 灯塔地图表
	/// <summary>
	[System.Serializable]
	public  class Lighthouse
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 名称
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名称")]
	public string name;
	/// <summary>
	/// 宝石
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("宝石")]
	public int gem;


		#endregion

	}
}
