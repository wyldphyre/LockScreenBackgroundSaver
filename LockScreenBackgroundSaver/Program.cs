using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      // TODO: Validate that OutputFolder exists
    }
  }
}
