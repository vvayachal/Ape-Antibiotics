using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class JSONHandler : MonoBehaviour
{


    public static void SaveData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        Debug.Log("Save Data - " + json);

        using StreamWriter streamWriter = new StreamWriter("Assets/Save Data/SaveData.json");

        streamWriter.Write(json);
    }

    public static bool LoadData(PlayerData playerData) 
    {
        if (File.Exists("Assets/Save Data/SaveData.json"))
        {
            using StreamReader streamReader = new StreamReader("Assets/Save Data/SaveData.json");

            string json = streamReader.ReadToEnd();

            PlayerData fromJson = JsonUtility.FromJson<PlayerData>(json);

            if (json.Equals(""))
            {
                return false;
            }
            copyPlayerData(playerData, fromJson);
            
            Debug.Log("Load Data - " + json);
            
            return true;
        }
        else
        {
            Debug.LogError("File Does Not Exist \n");

            return false;
        }
    }

    private static void copyPlayerData(PlayerData playerData, PlayerData fromJson)
    {
        playerData.shieldHealth = fromJson.shieldHealth;
        playerData.score = fromJson.score;
    }

}
