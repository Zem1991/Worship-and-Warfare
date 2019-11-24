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

    public static void Save(string fileName, ScenarioFile data)
    {
        string path = GetFilePath(fileName);
        string json = JsonConvert.SerializeObject(data);
        using (StreamWriter writer = File.CreateText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, json);
        }
    }

    public static ScenarioFile Load(string fileName)
    {
        string path = GetFilePath(fileName);
        Debug.Log("Loading from path: " + path);
        if (File.Exists(path))
        {
            ScenarioFile data;
            //data = JsonUtility.FromJson<ScenarioFileData>(File.ReadAllText(path));
            using (StreamReader reader = File.OpenText(path))
            {
                using (JsonTextReader file = new JsonTextReader(reader))
                {
                    Debug.Log("Activating JsonSerializer...");
                    JsonSerializer serializer = new JsonSerializer();
                    data = serializer.Deserialize<ScenarioFile>(file);
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
        string path = Application.streamingAssetsPath + separator + FOLDER_NAME;
        if (separator == '\\') path = path.Replace('/', separator);
        return path;
    }

    private static string GetFilePath(string fileName)
    {
        char separator = Path.DirectorySeparatorChar;
        string path = GetFolderPath() + separator + fileName + EXTENSION;
        if (separator == '\\') path = path.Replace('/', separator);
        return path;
    }
}
