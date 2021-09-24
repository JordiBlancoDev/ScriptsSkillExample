using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Loads the player's game data.
    /// </summary>
    public Data LoadData()
    {
        return BinaryDeserialize(Application.persistentDataPath + "/playerData.dat");
    }
    
    /// <summary>
    /// Saves the player's data.
    /// </summary>
    public void SaveData(Data currentGameData)
    {
       BinarySerialize(currentGameData, Application.persistentDataPath + "/playerData.dat");
    }

    /// <summary>
    /// Converts the game's data into binary at the desired path.
    /// </summary>
    void BinarySerialize(Data data, string filePath)
    {
        FileStream fileStream;
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(filePath)) File.Delete(filePath);
        fileStream = File.Create(filePath);
        bf.Serialize(fileStream, data);
        fileStream.Close();
    }
    
    /// <summary>
    /// Converts the binary file from the path to Data.
    /// </summary>
    Data BinaryDeserialize(string filePath)
    {
        FileStream fileStream;
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(filePath))
        {
            Data data = new Data();
            fileStream = File.OpenRead(filePath);
            data = bf.Deserialize(fileStream) as Data;
            fileStream.Close();
            return data;
        }
        else return null;
    }
}
