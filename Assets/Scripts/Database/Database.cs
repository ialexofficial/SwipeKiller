using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public abstract class Database<T> where T: new()
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
            using StreamWriter writer = new StreamWriter(fullSavesPath + dataFileName);
            serializer.Serialize(writer, data);
        }

        protected void Deserialize()
        {
            using StreamReader reader = new StreamReader(fullSavesPath + dataFileName);
            using JsonReader jsonReader = new JsonTextReader(reader);
            data = serializer.Deserialize<T>(jsonReader) ?? new T();
        }
    }
}