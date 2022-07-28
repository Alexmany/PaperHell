using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Collections;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad control;
    
    public GameData gData;   

    void Awake()
    {
        control = this;
    }    

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamedata"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamedata", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), gData);           

            file.Close();                  
        }
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamedata");
        var json = JsonUtility.ToJson(gData);

        bf.Serialize(file, json);
        file.Close();        
    }       
}
