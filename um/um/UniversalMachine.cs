using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icfp2006.UM
{
  public class UniversalMachine
  {
    private const int INIT_ARRAYS = 16;
    
    private bool halted_ = false;

    private uint executionFinger_; 
    private uint[] registers_ = new uint[8];
    private uint[][] arrays_;
    private Queue<int> arraysFree_ = new Queue<int>();
    private IOContext ioContext_;

    public UniversalMachine(IOContext ioContext)
    {
      ioContext_ = ioContext;

      arrays_ = new uint[INIT_ARRAYS][];
      for (int i = 1; i < INIT_ARRAYS; ++i)
      {
        arraysFree_.Enqueue(i);
      }
    }

    public UniversalMachine() : this(new ConsoleContext())
    {
    }

    public void Initialize(uint[] arrayZero)
    {
      arrays_[0] = arrayZero;
    }
    
    public bool DoSpinCycle() {
      uint @operator = arrays_[0][executionFinger_];
      byte operatorNumber = (byte)((@operator >> 28) & 0xff);

      uint constant;
      byte registerA, registerB, registerC;

      if (operatorNumber == 13)
      {
        constant = @operator & ((1 << 25) - 1);
        registerA = (byte)((@operator >> 25) & 0x7);
        registerB = registerC = 0;
      }
      else
      {
        constant = 0;
        registerA = (byte)((@operator >> 6) & 0x7);
        registerB = (byte)((@operator >> 3) & 0x7);
        registerC = (byte)(@operator & 0x7);
      }

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
          halted_ = true;
          break;
        case 8: // Allocation.
          if (arraysFree_.Count == 0)
          {
            int length = arrays_.Length;
            Array.Resize(ref arrays_, length * 2);
            for (int i = length; i < length * 2; ++i)
            {
              arraysFree_.Enqueue(i);
            }
          }
          int freeArray = arraysFree_.Dequeue();
          arrays_[freeArray] = new uint[registers_[registerC]];
          registers_[registerB] = (uint)freeArray;
          break;
        case 9: // Abandonment.
          arrays_[registers_[registerC]] = null;
          arraysFree_.Enqueue((int)registers_[registerC]);
          break;
        case 10: // Output.
          ioContext_.Output(registers_[registerC]);
          break;
        case 11: // Input.
          registers_[registerC] = ioContext_.Input();
          break;
        case 12: // Load Program.   
          if (registers_[registerB] != 0)
          {
            arrays_[0] = (uint[])arrays_[registers_[registerB]].Clone();
          }
          executionFinger_ = registers_[registerC] - 1;
          break;
        case 13: // Orthography.
          registers_[registerA] = constant;
          break;
        default:
          throw new Exception();
      }

      ++executionFinger_;

      return !halted_;
    }
  }
}
