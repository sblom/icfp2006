using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Icfp2006
{
  class Program
  {
    static void Main(string[] args)
    {
      List<byte> bytes = new List<byte>();
      int positionTmp = 0, outputState = 0;
      bool halt = false;
      UniversalMachine umCodexExtractor = new UniversalMachine(new IOFlexible(() => "(\\b.bb)(\\v.vv)06FHPVboundvarHRAk\np\n"[positionTmp++], (c) =>
      {
        if (outputState < 2)
        {
          switch ((char)c)
          {
            case 'n':
              if (outputState == 0) { outputState = 1; }
              else { outputState = 0; }
              break;
            case ':':
              if (outputState == 1) { outputState = 2; }
              else { outputState = 0; }
              break;
            default:
              outputState = 0;
              break;
          }
          Console.Write((char)c);
        }
        else
        {
          bytes.Add((byte)c);
        }
      }, () => { halt = true; }));
      
      var arrayZero = ReadProgramFile(args);
      umCodexExtractor.Initialize(arrayZero);
      while (!halt)
      {
        umCodexExtractor.DoSpinCycle();
      }

      UniversalMachine um = new UniversalMachine();
      um.Initialize(BytesToProgram(bytes.ToArray()));
      while (true)
      {
        um.DoSpinCycle();
      }
    }

    private static uint[] ReadProgramFile(string[] args)
    {
      var bytes = File.ReadAllBytes(args[0]);
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
