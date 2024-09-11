namespace ExcelData
{
	/// <summary>
	/// 地下城难度表
	/// <summary>
	[System.Serializable]
	public  class Dungeon
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 难度系数
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("难度系数")]
	public float num;
	/// <summary>
	/// 特殊
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("特殊")]
	public int sp;


		#endregion

	}
}
