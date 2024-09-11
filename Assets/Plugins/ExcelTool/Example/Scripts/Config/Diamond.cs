namespace ExcelData
{
	/// <summary>
	/// 钻石升级表
	/// <summary>
	[System.Serializable]
	public  class Diamond
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
	/// 产量升级材料
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("产量升级材料")]
	public int[] production;
	/// <summary>
	/// 速度升级材料
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("速度升级材料")]
	public int[] speed;
	/// <summary>
	/// 容量升级材料
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("容量升级材料")]
	public int[] capacity;
	/// <summary>
	/// 产量提升
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("产量提升")]
	public int[] pValue;
	/// <summary>
	/// 速度提升
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("速度提升")]
	public float[] sValue;
	/// <summary>
	/// 容量提升
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("容量提升")]
	public int[] cValue;


		#endregion

	}
}
