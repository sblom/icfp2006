using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icfp2006
{
  class UniversalMachine
  {
    private const int INIT_ARRAYS = 16;

    private uint executionFinger_; 
    private uint[] registers_ = new uint[8];
    private uint[][] arrays_;
    Func<uint> input_;
    Action<uint> output_;

    public UniversalMachine(Func<uint> input, Action<uint> output)
    {
      input_ = input;
      output_ = output;
      arrays_ = new uint[INIT_ARRAYS][];
    }

    public UniversalMachine() : this(() => { return (uint)Console.Read(); }, (uint c) => Console.Write((char)c))
    {
    }

    public void Initialize(uint[] arrayZero)
    {
      arrays_[0] = arrayZero;
    }
    
    public void DoSpinCycle() {
      uint @operator = arrays_[0][executionFinger_];
      byte operatorNumber = (byte)((@operator >> 28) & 0xff);

      byte registerA = (byte)((@operator >> 6) & 0x7);
      byte registerB = (byte)((@operator >> 3) & 0x7);
      byte registerC = (byte)(@operator & 0x7);

      uint constant = @operator & ((1 << 25) - 1);
      byte constRegisterA = (byte)((@operator >> 25) & 0x7);

      switch (operatorNumber) {
        case 0: // Conditional Move.
          if (registers_[registerC] != 0) { registers_[registerA] = registers_[registerB]; }
          break;
        case 1: // Array Index.
          registers_[registerA] = arrays_[registers_[registerB]][registers_[registerC]];
          break;
        case 2: // Array Amendment.
          arrays_[registers_[registerA]][registers_[registerB]] = registers_[registerC];
          break;
        case 3: // Addition.
          registers_[registerA] = registers_[registerB] + registers_[registerC];
          break;
        case 4: // Multiplication.
          registers_[registerA] = registers_[registerB] * registers_[registerC];
          break;
        case 5: // Division.
          registers_[registerA] = registers_[registerB] / registers_[registerC];
          break;
        case 6: // Not-And.
          registers_[registerA] = ~(registers_[registerB] & registers_[registerC]);
          break;
        case 7: // Halt.
          Environment.Exit(0);
          break;
        case 8: // Allocation.
          int length = arrays_.Length;
          int i;
          for (i = 0; i < length; ++i) {
            if (arrays_[i] == null) {
              break;
            }
          }
          if (i == length) {
            Array.Resize(ref arrays_, length * 2);
          }
          arrays_[i] = new uint[registers_[registerC]];
          registers_[registerB] = (uint)i;
          break;
        case 9: // Abandonment.
          arrays_[registers_[registerC]] = null;
          break;
        case 10: // Output.
          output_(registers_[registerC]);
          break;
        case 11: // Input.
          registers_[registerC] = input_();
          break;
        case 12: // Load Program.   
          if (registers_[registerB] != 0)
          {
            arrays_[0] = (uint[])arrays_[registers_[registerB]].Clone();
          }
          executionFinger_ = registers_[registerC] - 1;
          break;
        case 13: // Orthography.
          registers_[constRegisterA] = constant;
          break;
        default:
          throw new Exception();
      }

      ++executionFinger_;
    }
  }
}
