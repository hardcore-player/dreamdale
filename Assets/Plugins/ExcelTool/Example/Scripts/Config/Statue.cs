namespace ExcelData
{
	/// <summary>
	/// 雕像表
	/// <summary>
	[System.Serializable]
	public  class Statue
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
	/// 中文
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文")]
	public string chinese;
	/// <summary>
	/// 特性
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("特性")]
	public string function;
	/// <summary>
	/// 解锁消耗资源
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("解锁消耗资源")]
	public string unlock;


		#endregion

	}
}
