using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Icfp2006.UM;

namespace Icfp2006.UmShell
{
  class UmShell
  {
    static int Main(string[] args)
    {
      UniversalMachine um;
      switch(args[0])
      {
        case "decrypt":
          um = new UniversalMachine(new Decryptor(args[2], args[3]));
          break;
        case "run":
          um = new UniversalMachine();
          break;
        default:
          return 1;
      }

      var arrayZero = ReadProgramFile(args[1]);
      um.Initialize(arrayZero);

      while (um.DoSpinCycle())
      {}

      return 0;
    }

    private static uint[] ReadProgramFile(string umFileName)
    {
      var bytes = File.ReadAllBytes(umFileName);
      var arrayZero = BytesToProgram(bytes);
      return arrayZero;
    }

    private static uint[] BytesToProgram(byte[] bytes)
    {
      var arrayZero = new uint[bytes.Length / 4];
      for (int i = 0; i < arrayZero.Length; ++i)
      {
        arrayZero[i] = (uint)(bytes[i * 4] << 24) | (uint)(bytes[i * 4 + 1] << 16) | (uint)(bytes[i * 4 + 2] << 8) | (uint)(bytes[i * 4 + 3]);
      }
      return arrayZero;
    }
  }
}
