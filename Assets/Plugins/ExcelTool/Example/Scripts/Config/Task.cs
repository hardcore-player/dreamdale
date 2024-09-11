namespace ExcelData
{
	/// <summary>
	/// 任务表
	/// <summary>
	[System.Serializable]
	public  class Task
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 类型
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("类型")]
	public string type;
	/// <summary>
	/// 任务描述
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("任务描述")]
	public string content;
	/// <summary>
	/// 数量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("数量")]
	public int num;
	/// <summary>
	/// 任务目标
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("任务目标")]
	public string target;
	/// <summary>
	/// 是否开启
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("是否开启")]
	public int enable;
	/// <summary>
	/// 是否有宝箱
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("是否有宝箱")]
	public int reward;
	/// <summary>
	/// 自动完成
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("自动完成")]
	public int auto;


		#endregion

	}
}
