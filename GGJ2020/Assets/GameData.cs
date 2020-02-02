using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public RobotDefinition[] Definitions;
    public RobotDefinition[] Enemies;
}

[System.Serializable]
public class RobotDefinition
{
    public Sprite[] IdleAnim;
    public Sprite Shadow;
    public Sprite Broken;
    [Header("Energy / Action / Chance / Speed")]
    public int[] StatsMax;
}


