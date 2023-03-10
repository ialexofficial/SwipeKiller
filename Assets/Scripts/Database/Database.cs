using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public abstract class Database<T> where T: struct
    {
        protected const string SavesDir = "/Saves";
        
        protected readonly string fullSavesPath = Application.persistentDataPath + SavesDir;
        protected readonly JsonSerializer serializer = new JsonSerializer();
        protected readonly string dataFileName;
        protected T data;

        protected Database(string dataFileName)
        {
            this.dataFileName = dataFileName;

            if (!Directory.Exists(fullSavesPath))
                Directory.CreateDirectory(fullSavesPath);
            
            Deserialize();
        }

        protected void Serialize()
        {
            using FileStream file = new FileStream(fullSavesPath + dataFileName, FileMode.OpenOrCreate);
            using StreamWriter writer = new StreamWriter(file);
            serializer.Serialize(writer, data);
        }

        protected void Deserialize()
        {
            using FileStream file = new FileStream(fullSavesPath + dataFileName, FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file);
            using JsonReader jsonReader = new JsonTextReader(reader);
            data = serializer.Deserialize<T?>(jsonReader) ?? new T();
        }
    }
}