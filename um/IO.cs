using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icfp2006
{
  interface IO
  {
    uint Input();
    void Output(uint ch);
    void Halt();
  }

  class IOConsole: IO
  {
    public uint Input()
    {
      return (uint)Console.Read();
    }
    public void Output(uint ch)
    {
      Console.Write((char)ch);
    }
    public void Halt()
    {
      Environment.Exit(0);
    }
  }

  class IOFlexible: IO
  {
    private Func<uint> input_;
    private Action<uint> output_;
    private Action halt_;

    public IOFlexible(Func<uint> input, Action<uint> output, Action halt)
    {
      input_ = input;
      output_ = output;
      halt_ = halt;
    }

    public uint Input()
    {
      return input_();
    }

    public void Output(uint ch)
    {
      output_(ch);
    }

    public void Halt()
    {
      halt_();
    }
  }
}
