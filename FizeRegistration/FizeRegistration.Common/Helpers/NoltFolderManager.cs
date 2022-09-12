using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizeRegistration.Common.Helpers
{
    public static class NoltFolderManager
    {
        private static string _serverPath;

        private static string _imagelFiles = "wwwroot\\images";

        public static void InitializeFolderManager(string serverPath)
        {
            _serverPath = serverPath;

            CreateImageFilesFolderIfNotExists();
        }

        private static void CreateImageFilesFolderIfNotExists()
        {
            if (!Directory.Exists(Path.Combine(_serverPath, _imagelFiles)))
            {
                Directory.CreateDirectory(Path.Combine(_serverPath, _imagelFiles));
            }
        }

        public static string GetImageFilesFolderPath() => Path.Combine(_serverPath, _imagelFiles);
    }
   
}
