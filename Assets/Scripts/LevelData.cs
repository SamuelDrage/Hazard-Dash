using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "level", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public string levelName;
    public string sceneName;
    public int rating = 0;
    public float score = 0;
}
