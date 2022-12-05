using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day05 : BaseDay
    {
        Stack<char>[] stacks;
        (int count, int from, int to)[] instructions;
        string[] lines;
        int bucketCount;
        int crateStartLine;

        public Day05()
        {
            lines = File.ReadAllLines(InputFilePath);
            bucketCount = (lines[0].Length + 1) / 4;
            crateStartLine = 0;
            /* Figure out tallest stack */
            for (var x = 0; x < lines.Length; x++)
            {
                if (lines[x] == "")
                {
                    crateStartLine = x - 2;
                }
            }

            var instructionStartLine = crateStartLine + 3;
            var instructionCount = lines.Length - instructionStartLine;
            instructions = lines.Skip(instructionStartLine).Select(l =>
            {
                var replaced = l.Replace("move ", "").Replace("from ", "").Replace("to ", "");
                var numbers = replaced.Split(" ").Select(int.Parse).ToArray();
                return (count: numbers[0], from: numbers[1], to: numbers[2]);

            }).ToArray();

        }

        public void init_stacks()
        {
            /* Figure out how many buckets */
            
            stacks = new Stack<char>[bucketCount];

            for (var x = 0; x < bucketCount; x++)
            {
                stacks[x] = new Stack<char>();
            }

            for(var x = crateStartLine; x >= 0; x--)
            {
                for (var y = 1; y < lines[x].Length; y+= 4)
                {
                    var stackIndex = y / 4;
                    if (lines[x][y] != ' ')
                    {
                        stacks[stackIndex].Push(lines[x][y]);
                    }
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            init_stacks();

            foreach (var instr in instructions)
            {
                for(var x = 0; x < instr.count; x++)
                {
                    stacks[instr.to - 1].Push(stacks[instr.from - 1].Pop());
                }
            }

            var ans = "";
            foreach(var stack in stacks)
            {
                ans += stack.Peek();
            }

            return new(ans);
        }

        public override ValueTask<string> Solve_2()
        {

            init_stacks();

            foreach (var instr in instructions)
            {
                Stack<char> tempStack = new();
                for (var x = 0; x < instr.count; x++)
                {
                    tempStack.Push(stacks[instr.from - 1].Pop());
                }

                while (tempStack.Count > 0)
                {
                    stacks[instr.to - 1].Push(tempStack.Pop());
                }
            }

            var ans = "";
            foreach (var stack in stacks)
            {
                ans += stack.Peek();
            }

            return new(ans);
        }
    }
}
