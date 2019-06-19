using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//TODO: Use BinaryFormatter and FileStream instead of JsonUtility and JsonSerializer.
public static class ScenarioFileHandler
{
    public const string FOLDER_NAME = "Scenarios";
    public const string EXTENSION = ".json";

    public static void Save(string fileName, ScenarioFileData data)
    {
        string path = GetFilePath(fileName);
        string json = JsonConvert.SerializeObject(data);
        using (StreamWriter writer = File.CreateText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, json);
        }
    }

    public static ScenarioFileData Load(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            ScenarioFileData data;
            using (StreamReader reader = File.OpenText(path))
            {
                using (JsonTextReader file = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    data = serializer.Deserialize<ScenarioFileData>(file);
                }
            }
            return data;
        }
        else
        {
            Debug.LogError("Scenario file not found! Attempted path: " + path);
            return null;
        }
    }

    private static string GetFolderPath()
    {
        char separator = Path.DirectorySeparatorChar;
        return Application.dataPath + separator + FOLDER_NAME;
    }

    private static string GetFilePath(string fileName)
    {
        char separator = Path.DirectorySeparatorChar;
        return GetFolderPath() + separator + fileName + EXTENSION;
    }
}
