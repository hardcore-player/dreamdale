namespace ExcelData
{
	/// <summary>
	/// 大厅升级表
	/// <summary>
	[System.Serializable]
	public  class HallUpgrade
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
	/// 金币消耗
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("金币消耗")]
	public int[] priceArr;
	/// <summary>
	/// 基础数值
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("基础数值")]
	public int baseVal;
	/// <summary>
	/// 升级增加
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("升级增加")]
	public float upgrade;
	/// <summary>
	/// 中文名称
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文名称")]
	public string chineseName;
	/// <summary>
	/// 解锁任务
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("解锁任务")]
	public int unlock;


		#endregion

	}
}
