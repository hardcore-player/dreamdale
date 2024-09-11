namespace ExcelData
{
	/// <summary>
	/// 实验室表
	/// <summary>
	[System.Serializable]
	public  class Research
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 中文名称
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文名称")]
	public string chineseName;
	/// <summary>
	/// 宝石
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("宝石")]
	public int gem;
	/// <summary>
	/// 名称
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("名称")]
	public string name;
	/// <summary>
	/// 任务解锁
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("任务解锁")]
	public int unlock;


		#endregion

	}
}
