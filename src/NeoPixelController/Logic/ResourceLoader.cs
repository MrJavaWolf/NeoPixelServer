using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NeoPixelController.Logic
{
    public class ResourceLoader
    {
        private readonly string currentDllDirectory;
        public ResourceLoader()
        {
            string fullPath = Assembly.GetExecutingAssembly().Location;
            currentDllDirectory = Path.GetDirectoryName(fullPath);
        }

        public string ReadSettings()
        {
            return File.ReadAllText(Path.Combine(currentDllDirectory, "Resources", "DeviceOptions.json"));
        }

        public string CreateFullFilePath(string fileInResources)
        {
            return Path.Combine(currentDllDirectory, "Resources", fileInResources);
        }
    }
}
