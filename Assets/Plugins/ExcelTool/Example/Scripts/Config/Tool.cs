namespace ExcelData
{
	/// <summary>
	/// 工具表
	/// <summary>
	[System.Serializable]
	public  class Tool
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 中文名字
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文名字")]
	public string chineseName;
	/// <summary>
	/// 金币
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("金币")]
	public int gold;
	/// <summary>
	/// 名字
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名字")]
	public string name;


		#endregion

	}
}
