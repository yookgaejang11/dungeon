using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    battle,
    shop
}

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public StageType type;
    public string stageValue;
    public List<Enemy> enemys;
    public bool isCleared = false;
}
