using System.Text.Json;

namespace CoffeeShop.Repository
{
    internal class FileOperations<T>
    {
        public readonly string _filePath;

        public FileOperations(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> ReadFromFile()
        {
            List<T> list = new List<T>();
            string[] fileData = File.ReadAllLines(_filePath);
            foreach(string fileLine in fileData)
            {
                T? data = JsonSerializer.Deserialize<T>(fileLine);
                if (data is null) continue;
                list.Add(data);
            }

            return list;
        }

        public void WriteToFile(List<T> list)
        {
            List<string> jsonLines = new List<string>();
            foreach(T element in list)
            {
                string line = JsonSerializer.Serialize(element);
                jsonLines.Add(line);
            }

            File.WriteAllLines(_filePath, jsonLines);
        }

        public void AppendToFile(T line)
        {
            string jsonLine = JsonSerializer.Serialize(line);
            File.AppendAllText(_filePath, jsonLine+Environment.NewLine);
        }
    }
}
