using System;
using System.IO;
using UnityEngine;

namespace DarknessInside.Core.Save
{
    /// <summary>
    /// Сохранение в JSON-файл в Application.persistentDataPath.
    /// Запись через временный файл: если игру закроют посреди записи, старое сохранение не испортится.
    /// </summary>
    public sealed class JsonFileSaveStorage : ISaveStorage
    {
        private const string FileName = "save.json";

        private readonly string _path;

        public JsonFileSaveStorage(string path)
        {
            _path = path;
        }

        public static string DefaultPath => Path.Combine(Application.persistentDataPath, FileName);

        public bool TryLoad(out SaveData data)
        {
            data = null;
            if (!File.Exists(_path))
            {
                return false;
            }

            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(_path));
                return data != null;
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Save file is corrupted, starting fresh: " + exception.Message);
                data = null;
                return false;
            }
        }

        public void Save(SaveData data)
        {
            string tempPath = _path + ".tmp";
            try
            {
                File.WriteAllText(tempPath, JsonUtility.ToJson(data));
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                File.Move(tempPath, _path);
            }
            catch (IOException exception)
            {
                Debug.LogError("Failed to write save file: " + exception.Message);
            }
        }

        public void Delete()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}
