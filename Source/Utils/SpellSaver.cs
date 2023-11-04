using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static DiagnosticsManager.DiagnosticsManager;

namespace SpellCompiler.Utils
{
    public class SpellSaver
    {
        private const string _ext = ".json";
        private readonly string _savePath;

        public SpellSaver(string savePath)
        {
            _savePath = savePath;
        }

        public bool SaveSpellJson(string fileName, object objectData, Action<string> savePathHandle) 
        {
            string filePath = $"{_savePath}\\{fileName}{_ext}";

            using (FileStream createStream = File.Create(filePath))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    Converters = new List<JsonConverter>
                    {
                        new DepletionConverter(),
                        new EffectConverter(),
                        new ResourceConverter(),
                        new TranslationConverter()
                    }
                };

                string json = JsonConvert.SerializeObject(objectData, settings);
                byte[] info = UTF8Encoding.UTF8.GetBytes(json);
                createStream.Write(info, 0, info.Length);
            }

            bool hasfile = File.Exists(filePath);

            if (hasfile && DebugType == EDebugType.Explicit) 
            {
                string fileText = File.ReadAllText(filePath);

                savePathHandle.Invoke(filePath);


                LogMessage(fileText);
            }

            return hasfile;
        }

        public void Load<T>(string fileName, Action<T> readCallback) 
        {
            string filePath = $"{_savePath}\\{fileName}{_ext}";

            Assert(File.Exists(filePath), $"File does not exist at path {filePath}");

            string jsonInText;
            T readInstance;

            using (FileStream fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Read the source file into a byte array.
                byte[] bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;

                jsonInText = UTF8Encoding.UTF8.GetString(bytes, 0, numBytesRead);

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    Converters = new List<JsonConverter>
                    {
                        new DepletionConverter(),
                        new EffectConverter(),
                        new ResourceConverter(),
                        new TranslationConverter()
                    }
                };

                readInstance = JsonConvert.DeserializeObject<T>(jsonInText, settings);
            }

            Assert(readInstance != null, "File failure to convert to type from JSON.");

            readCallback.Invoke(readInstance);
        }
    }
}