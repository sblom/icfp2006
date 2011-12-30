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
      UniversalMachine um = new UniversalMachine();
      var bytes = File.ReadAllBytes(args[0]);
      var arrayZero = new uint[bytes.Length / 4];
      for (int i = 0; i < arrayZero.Length; ++i)
      {
        arrayZero[i] = (uint)(bytes[i * 4] << 24) | (uint)(bytes[i * 4 + 1] << 16) | (uint)(bytes[i * 4 + 2] << 8) | (uint)(bytes[i * 4 + 3]);
      }
      um.Initialize(arrayZero);
      while (true)
      {
        um.DoSpinCycle();
      }
    }
  }
}
