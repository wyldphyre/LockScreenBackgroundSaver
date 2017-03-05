using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace LockScreenBackgroundSaver
{
  class Program
  {
    static void Main(string[] args)
    {
      var MonitorForNewImages = false;
      var OutputFolder = "";

      foreach (var argument in args)
      {
        if (argument == "-monitor")
        {
          MonitorForNewImages = true;
        }
        else if (argument.StartsWith("-output:"))
        {
          OutputFolder = argument.Split(':').Last();
        }
      }

      if (OutputFolder == "")
      {
        Console.WriteLine("must provide an output folder using the -output parameter");
        return;
      }

      // TODO: Validate that OutputFolder exists

      const int MinimumBackgroundSize = 1024 * 100;
      //var MonitorInterval = TimeSpan.FromMinutes(10);
      var MonitorInterval = TimeSpan.FromSeconds(10);
      var AssetPath = string.Format(@"C:\Users\{0}\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets", Environment.UserName);

      do
      {
        var AssetDirectoryInfo = new DirectoryInfo(AssetPath);
        var AssetFiles = AssetDirectoryInfo.GetFiles();
        var PotentialBackgroundFiles = AssetFiles.Where(File => File.Length > MinimumBackgroundSize).ToArray();
        var AssetHashFilePathDictionary = new Dictionary<string, string>();

        var HashAlgorithm = SHA256Managed.Create();

        foreach (var File in PotentialBackgroundFiles)
        {
          using (var FileStream = new FileStream(File.FullName, FileMode.Open))
          {
            var HashReturnValue = HashAlgorithm.ComputeHash(FileStream);

            var Hash = BitConverter.ToString(HashReturnValue).Replace("-", String.Empty);
            if (!AssetHashFilePathDictionary.ContainsKey(Hash))
              AssetHashFilePathDictionary.Add(Hash, File.FullName);
          }
        }

        if (MonitorForNewImages)
          Thread.Sleep(MonitorInterval);
      }
      while (MonitorForNewImages);
    }
  }
}
