namespace ExcelData
{
	/// <summary>
	/// 资源建筑
	/// <summary>
	[System.Serializable]
	public  class ResBuilding
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
	/// 生产速度（秒）
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("生产速度（秒）")]
	public int speed;
	/// <summary>
	/// 所需资源
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("所需资源")]
	public string res;
	/// <summary>
	/// 所需资源数量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("所需资源数量")]
	public int num;
	/// <summary>
	/// 看视频获得资源
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("看视频获得资源")]
	public int ad;


		#endregion

	}
}
