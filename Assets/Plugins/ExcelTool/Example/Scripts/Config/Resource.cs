namespace ExcelData
{
	/// <summary>
	/// 资源表
	/// <summary>
	[System.Serializable]
	public  class Resource
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
	/// 恢复时间
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("恢复时间")]
	public int restore;
	/// <summary>
	/// 采集次数
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("采集次数")]
	public int collect;


		#endregion

	}
}
