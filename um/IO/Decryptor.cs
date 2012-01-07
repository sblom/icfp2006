using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Icfp2006;

namespace Icfp2006.UM.IO
{
  class Decryptor: IOContext
  {
    private string key_;
    private string script_;
    private FileStream output_;

    private int outputState_ = 0;
    private int inputPosition_ = 0;

    public Decryptor(string key, string outputFile)
    {
      key_ = key;
      script_ = key_ + "\np\n";
      output_ = File.OpenWrite(outputFile);
    }

    public uint Input()
    {
      return script_[inputPosition_++];
    }

    public void Output(uint ch)
    {
      if (outputState_ < 2)
      {
        switch ((char)ch)
        {
          case 'n':
            if (outputState_ == 0) { outputState_ = 1; }
            else { outputState_ = 0; }
            break;
          case ':':
            if (outputState_ == 1) { outputState_ = 2; }
            else { outputState_ = 0; }
            break;
          default:
            outputState_ = 0;
            break;
        }
        Console.Write((char)ch);
      }
      else
      {
        output_.WriteByte((byte)ch);
      }
    }
  }
}
