namespace ExcelData
{
	/// <summary>
	/// 太空飞船表
	/// <summary>
	[System.Serializable]
	public  class SpaceShip
	{
		#region 变量

	/// <summary>
	/// 编号
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("编号")]
	public int id;
	/// <summary>
	/// 所需材料
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("所需材料")]
	public string[] assets;
	/// <summary>
	/// 材料个数
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("材料个数")]
	public int[] num;


		#endregion

	}
}
