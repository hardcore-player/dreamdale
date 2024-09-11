namespace ExcelData
{
	/// <summary>
	/// 怪物表
	/// <summary>
	[System.Serializable]
	public  class Monster
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
	/// 中文
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("中文")]
	public string chinese;
	/// <summary>
	/// 攻击
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("攻击")]
	public int atk;
	/// <summary>
	/// 生命值
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("生命值")]
	public int hp;
	/// <summary>
	/// 攻速
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("攻速")]
	public float speed;


		#endregion

	}
}
