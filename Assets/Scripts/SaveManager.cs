using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string filePath = Application.persistentDataPath + "/saveData.dat";

    public static bool SaveFileExists(){
        if(File.Exists(filePath)){
            return true;
        } else {
            return false;
        }
    }

    public static void SaveData(int playerLevel, Dictionary<int, LevelData> levels){
        // Create a reference to our BinaryFormatter and create a new file at specified path
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(filePath);

        // Create new SaveData variable
        SaveData data = new SaveData();
        // Set the playerLevel value in the SaveData variable to the playerLevel
        data.playerLevel = playerLevel;
        // Loop through each level and copy level data to save data struct
        foreach(LevelData level in levels.Values){
            LevelSaveData newLevel = new LevelSaveData();
            newLevel.rating = level.rating;
            newLevel.score = level.score;
            data.levelData.Add(newLevel);
        }

		// Serialize our file with the GameData class and close the file
		bf.Serialize(file, data);
		file.Close();
		Debug.Log("Game saved to: " + filePath + " successfully");
    }

    public static void LoadData(bool newGame){
        if(File.Exists(filePath) && !newGame){
            // Reference our BinaryFormatter, open the file and deserialize the data into a variable of class GameData, and close the file
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            // Set the playerlevel to the player level extracted from the save level
            GameManager.instance.playerLevel = data.playerLevel;

            // Create new dictionary and populate it with all the level data
            Dictionary<int, LevelData> levelData = new Dictionary<int, LevelData>();
            for(int i = 1; i < (GameManager.instance.levels.Count+1); i++){
                levelData.Add(i, GameManager.instance.levels[i-1]);
            }

            // Loop through the dictionary and set the values according to the saved data
            for(int i = 0; i < levelData.Count; i++){
                levelData[i+1].rating = data.levelData[i].rating;
                levelData[i+1].score = data.levelData[i].score;
            }

            // Assign the new level data to the game managers version
            GameManager.instance.levelData = levelData;
        } else {
            if(File.Exists(filePath)){
                File.Delete(filePath);
            }

            // Reset the data for the levels
            foreach(LevelData lvlData in GameManager.instance.levels){
                lvlData.rating = 0;
                lvlData.score = 0;
            }

            // Create new level data dictionary and populate it with default level data
            Dictionary<int, LevelData> levelData = new Dictionary<int, LevelData>();
            for(int i = 1; i < (GameManager.instance.levels.Count+1); i++){
                levelData.Add(i, GameManager.instance.levels[i-1]);
            }

            // Set the game managers level to 1
            GameManager.instance.playerLevel = 1;
            // Assign the game manager with the new level data
            GameManager.instance.levelData = levelData;
        }
    }

    public static void DeleteData(){
        if(File.Exists(filePath)){
			File.Delete(filePath);
		}
    }
}

[System.Serializable]
public class SaveData {
    public int playerLevel;
    public List<LevelSaveData> levelData = new List<LevelSaveData>();
}

[System.Serializable]
public class LevelSaveData {
    public int rating = 0;
    public float score = 0;
}
