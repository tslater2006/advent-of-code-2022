using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    record CPUInstruction
    {
        public OPCODE Opcode;
        public int data;
    }

    enum OPCODE : int
    {
        NOOP = 0,
        ADDX = 1
    }

    class CRT
    {
        char[,] pixels = new char[6,40];
        public int SpritePosition = 0;
        public int Clock;
        Dictionary<int, char> LETTERS = new Dictionary<int, char>()
        {
            {0b1110_1001_1001_1110_1000_1000,'P' },
            {0b0110_1001_1000_1011_1001_0111,'G'},
            {0b1001_1001_1111_1001_1001_1001,'H'},
            {0b1111_1000_1110_1000_1000_1000,'F'},
            {0b1000_1000_1000_1000_1000_1111,'L'},
            {0b1001_1001_1001_1001_1001_0110,'U'},
        };
        

        public CRT()
        {
            for (var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 40; x++)
                {
                    pixels[y, x] = '.';
                }
            }
        }

        public string GetCode()
        {
            var start = 0;
            var code = "";
            for(var c = 0; c < 8; c++)
            {
                code += LETTERS[GetLetterCode(start)];
                start += 5;
            }

            return code;
        }

        private int GetLetterCode(int start)
        {
            int code = 0;
            for(var y = 0; y < 6; y++)
            {
                for (var x = start; x < start + 4; x++)
                {
                    code *= 2;
                    code += (pixels[y, x] == '#') ? 1 : 0;
                }
            }

            return code;
        }

        public void Tick()
        {
            Clock++;
            DrawPixel();
        }

        void DrawPixel()
        {
            var pixel = (Clock - 1) % 40;
            var row = (Clock - 1) / 40;

            if (Math.Abs(SpritePosition - pixel) <= 1)
            {
                pixels[row, pixel] = '#';
            }

        }

        public void Print()
        {
            for(var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 40; x++)
                {
                    Console.Write(pixels[y, x]);
                }
                Console.WriteLine();
            }
        }
    }

    class CPU
    {
        static readonly int[] CYCLE_COUNTS = new int[]{ 1, 2 };

        List<CPUInstruction> Instructions = new();
        public int CycleCount = 0;
        int InstructionCycle = 0;
        int PC = 0;
        public int X = 1;

        public int SignalStrength = 0;
        public CRT Screen;
        public CPU(string[] program, CRT screen)
        {
            Screen = screen;
            foreach(var l in program)
            {
                CPUInstruction instruction = new CPUInstruction();
                var parts = l.Split(" ");
                switch (parts[0])
                {
                    case "addx":
                        instruction.Opcode = OPCODE.ADDX;
                        instruction.data = int.Parse(parts[1]);
                        break;
                    case "noop":
                        instruction.Opcode = OPCODE.NOOP;
                        break;
                }
                Instructions.Add(instruction);
            }
        }
        public void RunTillComplete()
        {
            while (PC < Instructions.Count)
            {
                Tick(1);
            }
        }
        public void Tick(uint cycles)
        {
            for(var x = 0; x < cycles; x++)
            {
                ProcessInstruction();
            }
        }

        private void ProcessInstruction()
        {
            InstructionCycle++;
            CycleCount++;

            SignalStrength = X * CycleCount;
            Screen.SpritePosition = X;
            Screen.Tick();

            if (InstructionCycle == CYCLE_COUNTS[(int)Instructions[PC].Opcode])
            {
                switch (Instructions[PC].Opcode)
                {
                    case OPCODE.ADDX:
                        X += Instructions[PC].data;
                        break;
                }

                PC++;
                InstructionCycle = 0;
            }
            
        }

    }
    internal class Day10 : BaseDay
    {

        public override ValueTask<string> Solve_1()
        {
            CPU cpu = new CPU(File.ReadAllLines(InputFilePath), new CRT());

            int sum = 0;
            cpu.Tick(20);
            sum += cpu.SignalStrength;
            while (cpu.CycleCount < 220)
            {
                
                cpu.Tick(40);
                sum += cpu.SignalStrength;
            }

            return new(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            CRT screen = new CRT();
            CPU cpu = new CPU(File.ReadAllLines(InputFilePath), screen);

            cpu.RunTillComplete();
            var ans = screen.GetCode();
            return new(ans);
        }
    }
}
