using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadFile
{
    private const string PLAYERDATA_FILENAME = "/GameData_Player.sav";
    private const string LOADDATA_FILENAME = "/GameLoadData_Player.sav";

    public static void SavePlayerData(PlayerGameData playerGameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + PLAYERDATA_FILENAME, FileMode.Create);

        bf.Serialize(stream, playerGameData);
        stream.Close();
    }

    public static PlayerGameData LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + PLAYERDATA_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + PLAYERDATA_FILENAME, FileMode.Open);

            PlayerGameData data = bf.Deserialize(stream) as PlayerGameData;

            stream.Close();

            return data;
        }
        else
        {
            PlayerGameData data = new PlayerGameData();
            SaveLoadFile.SavePlayerData(data);
            return data;
        }
    }
    public static void SaveGameData(GameLoadData gameLoadData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + LOADDATA_FILENAME, FileMode.Create);

        bf.Serialize(stream, gameLoadData);
        stream.Close();
    }

    public static GameLoadData LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath + LOADDATA_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + LOADDATA_FILENAME, FileMode.Open);

            GameLoadData data = bf.Deserialize(stream) as GameLoadData;

            stream.Close();

            return data;
        }
        else
        {
            GameLoadData data = new GameLoadData();
            SaveLoadFile.SaveGameData(data);
            return data;
        }
    }
}