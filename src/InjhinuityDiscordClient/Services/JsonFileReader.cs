using InjhinuityDiscordClient.Services.Interfaces;
using System.IO;

namespace InjhinuityDiscordClient.Services
{
    public class JsonFileReader : IFileReader
    {
        public string ReadFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.OpenOrCreate)))
                return sr.ReadToEnd();
        }
    }
}
