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
    private const int MinimumBackgroundSize = 1024 * 250;

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

      var MonitorInterval = TimeSpan.FromMinutes(10);
      //var MonitorInterval = TimeSpan.FromSeconds(10);
      var AssetPath = string.Format(@"C:\Users\{0}\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets", Environment.UserName);

      do
      {
        var AssetImageDetails = LoadImageDetails(AssetPath, ConsiderFileSize: true);
        var OutputFolderImageDetails = LoadImageDetails(OutputFolder, ConsiderFileSize: false);

        var AssetsToCopyList = AssetImageDetails.Where(Asset => !OutputFolderImageDetails.Exists(Output => Output.Hash == Asset.Hash));

        foreach (var AssetImageDetail in AssetsToCopyList)
        {
          var DestinationPath = Path.Combine(OutputFolder, AssetImageDetail.Hash + ".jpg");

          if (!File.Exists(DestinationPath))
            File.Copy(AssetImageDetail.FilePath, DestinationPath);
        }
        
        if (MonitorForNewImages)
          Thread.Sleep(MonitorInterval);
      }
      while (MonitorForNewImages);
    }

    private static List<ImageDetails> LoadImageDetails(string FolderPath, bool ConsiderFileSize)
    {
      var Result = new List<ImageDetails>();
      var HashAlgorithm = SHA256Managed.Create();
      var PotentialBackgroundFiles = new DirectoryInfo(FolderPath)
        .GetFiles()
        .Where(File => File.Length > MinimumBackgroundSize);

      foreach (var File in PotentialBackgroundFiles)
      {
        using (var FileStream = new FileStream(File.FullName, FileMode.Open))
        {
          var HashBytes = HashAlgorithm.ComputeHash(FileStream);
          var Hash = BitConverter.ToString(HashBytes).Replace("-", String.Empty);
        
          Result.Add(new ImageDetails(Hash, File.FullName));
        }
      }

      return Result;
    }
  }

  sealed class ImageDetails
  {
    public ImageDetails(string Hash, string FilePath)
    {
      this.Hash = Hash;
      this.FilePath = FilePath;
    }

    public readonly string Hash;
    public readonly string FilePath;
  }
}
