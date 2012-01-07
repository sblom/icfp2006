using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icfp2006.UM.IO
{
  interface IOContext
  {
    uint Input();
    void Output(uint ch);
  }

  class ConsoleContext: IOContext
  {
    public uint Input()
    {
      return (uint)Console.Read();
    }
    public void Output(uint ch)
    {
      Console.Write((char)ch);
    }
  }

  class ClosureContext: IOContext
  {
    private Func<uint> input_;
    private Action<uint> output_;

    public ClosureContext(Func<uint> input, Action<uint> output)
    {
      input_ = input;
      output_ = output;
    }

    public uint Input()
    {
      return input_();
    }

    public void Output(uint ch)
    {
      output_(ch);
    }
  }
}
