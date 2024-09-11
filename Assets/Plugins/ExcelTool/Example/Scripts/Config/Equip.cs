namespace ExcelData
{
	/// <summary>
	/// 装备表
	/// <summary>
	[System.Serializable]
	public  class Equip
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
	/// 类型
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("类型")]
	public string region;
	/// <summary>
	/// 攻击力
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("攻击力")]
	public int atk;
	/// <summary>
	/// 血量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("血量")]
	public int hp;
	/// <summary>
	/// 暴击
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("暴击")]
	public float cri;
	/// <summary>
	/// 抗暴
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("抗暴")]
	public float decri;
	/// <summary>
	/// 闪避
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("闪避")]
	public float dodge;
	/// <summary>
	/// 命中
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("命中")]
	public float hit;
	/// <summary>
	/// 生命再生
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("生命再生")]
	public int reg;
	/// <summary>
	/// 材料
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("材料")]
	public string[] assets;
	/// <summary>
	/// 材料数量
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("材料数量")]
	public int[] num;
	/// <summary>
	/// 打造时间
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("打造时间")]
	public int time;
	/// <summary>
	/// 图标
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("图标")]
	public UnityEngine.Sprite icon;
	/// <summary>
	/// 预制体路径
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("预制体路径")]
	public UnityEngine.GameObject prefab;
	/// <summary>
	/// 类型
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("类型")]
	public string type;
	/// <summary>
	/// 消耗符文
	/// <summary>
	[UnityEngine.SerializeField, UnityEngine.Header("消耗符文")]
	public string rune;


		#endregion

	}
}
