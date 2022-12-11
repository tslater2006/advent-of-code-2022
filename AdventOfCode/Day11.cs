using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    enum MonkeyOperation
    {
        ADD, MULTIPLY
    }
    class Monkey
    {
        public static Dictionary<int, Monkey> Group = new();
        public static long mod = 1;
        public int ID;
        public Queue<long> Items;
        public MonkeyOperation Operation;
        public int OperationAmount;
        public int DivisibleBy;
        public int MonkeyTrueIndex;
        public int MonkeyFalseIndex;

        public long InspectionCount;

        public Monkey(int ID, int divisbleBy)
        {
            DivisibleBy = divisbleBy;
            mod *= divisbleBy;
            Group.Add(ID, this);
        }

        public void TakeTurn(bool decreaseWorry)
        {
            //Console.WriteLine($"Monkey {ID}:");
            while (Items.Count > 0) { 
                InspectItem(decreaseWorry);
                InspectionCount++;
            }
        }

        private void InspectItem(bool decreaseWorry)
        {
            long worry = Items.Dequeue();
            //Console.WriteLine($"  Monkey inspects an item with a worry level of {worry}");
            switch (Operation)
            {
                case MonkeyOperation.ADD:
                    if (OperationAmount == -1)
                    {
                        worry += worry;
                    }
                    else
                    {
                        worry += OperationAmount;
                    }
                    break;
                case MonkeyOperation.MULTIPLY:
                    if (OperationAmount == -1)
                    {
                        worry *= worry;
                        //Console.WriteLine($"    Worry level is multiplied by itself to {worry}.");
                    }
                    else
                    {
                        worry *= OperationAmount;
                        //Console.WriteLine($"    Worry level is multiplied by {OperationAmount} to {worry}.");
                    }
                    break;
            }

            worry = worry % mod;

            if (decreaseWorry)
            {
                worry /= 3;
            }
            //Console.WriteLine($"    Monkey gets bored with item. Worry level is divided by 3 to {worry}.");
            if (worry % DivisibleBy == 0)
            {
                //Console.WriteLine($"    Current worry level is divisible by {DivisibleBy}.");
                //Console.WriteLine($"    Item with worry level {worry} is thrown to monkey {MonkeyTrueIndex}.");
                Group[MonkeyTrueIndex].Items.Enqueue(worry);
            } else
            {
                //Console.WriteLine($"    Current worry level is not divisible by {DivisibleBy}.");
                //Console.WriteLine($"    Item with worry level {worry} is thrown to monkey {MonkeyFalseIndex}.");
                Group[MonkeyFalseIndex].Items.Enqueue(worry);
            }
        }
    }

    internal class Day11 : BaseDay
    {
        public Day11()
        {

        }

        private void InitMonkeys()
        {
            Monkey.Group.Clear();
            Monkey.mod = 1;
            var monkeyLines = File.ReadAllLines(InputFilePath).Where(l => l != "").Chunk(6);

            foreach (var monkeyInfo in monkeyLines)
            {
                int monkeyID = int.Parse(Regex.Match(monkeyInfo[0], @"\d+").Value);
                var divisibleBy = int.Parse(Regex.Match(monkeyInfo[3], @"\d+").Value);
                Monkey m = new Monkey(monkeyID, divisibleBy);
                m.Items = new Queue<long>(Regex.Matches(monkeyInfo[1], @"\d+").Select(m => long.Parse(m.Value)).ToList());
                m.Operation = monkeyInfo[2].Contains("+") ? MonkeyOperation.ADD : MonkeyOperation.MULTIPLY;
                if (Regex.Matches(monkeyInfo[2], "old").Count == 2)
                {
                    m.OperationAmount = -1;
                }
                else
                {
                    m.OperationAmount = int.Parse(Regex.Match(monkeyInfo[2], @"\d+").Value);
                }

                m.MonkeyTrueIndex = int.Parse(Regex.Match(monkeyInfo[4], @"\d+").Value);
                m.MonkeyFalseIndex = int.Parse(Regex.Match(monkeyInfo[5], @"\d+").Value);
            }
        }

        public override ValueTask<string> Solve_1()
        {
            InitMonkeys();
            const int ROUND_COUNT = 20;
            for (var x = 0; x < ROUND_COUNT; x++)
            {
                for (var y = 0; y < Monkey.Group.Count; y++)
                {
                    Monkey.Group[y].TakeTurn(true);
                }
            }

            var top2Monkeys = Monkey.Group.Values.OrderByDescending(m => m.InspectionCount).Take(2).ToArray();

            var ans = top2Monkeys[0].InspectionCount * top2Monkeys[1].InspectionCount;

            return new(ans.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            InitMonkeys();
            const int ROUND_COUNT = 10_000;
            for (var x = 0; x < ROUND_COUNT; x++)
            {
                for (var y = 0; y < Monkey.Group.Count; y++)
                {
                    Monkey.Group[y].TakeTurn(false);
                }
            }

            for(var x = 0; x < Monkey.Group.Count; x++)
            {
                Console.WriteLine($"Monkey {x} inspected items {Monkey.Group[x].InspectionCount} times.");
            }

            var top2Monkeys = Monkey.Group.Values.OrderByDescending(m => m.InspectionCount).Take(2).ToArray();

            var ans = top2Monkeys[0].InspectionCount * top2Monkeys[1].InspectionCount;

            return new(ans.ToString());
        }
    }
}
