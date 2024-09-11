using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Deal/ScriptableObject/刷怪配置", fileName = "SO_DungeonWave")]
public class SO_DungeonWave : ScriptableObject
{
    [Header("时长")]
    public float WaveTime = 20;
    [Header("出怪策略")]
    public List<SO_DungeonStrategy> Monsters = new List<SO_DungeonStrategy>();

}
