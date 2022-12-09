using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{

    class Rope
    {
        public HashSet<Point> VisitedPoints = new HashSet<Point>();
        Point[] knots;

        public Rope(int knotCount)
        {
            knots = new Point[knotCount];
            for(var x = 0; x < knotCount; x++)
            {
                knots[x] = new Point(0,0);
            }


            VisitedPoints.Add(knots[0]);
        }

        public void MoveHead(char direction, int count)
        {
            for (var x = 0; x < count; x++)
            {
                switch (direction)
                {
                    case 'U':
                        knots[0].Y++;
                        break;
                    case 'D':
                        knots[0].Y--;
                        break;
                    case 'L':
                        knots[0].X--;
                        break;
                    case 'R':
                        knots[0].X++;
                        break;
                }
                UpdateKnots();
            }
        }

        void UpdateKnots()
        {
            for (var i = 1; i < knots.Length; i++)
            {
                if (knots[i-1].X == knots[i].X && Math.Abs(knots[i - 1].Y - knots[i].Y) == 2)
                {
                    /* move towards head */
                    knots[i].Y += (knots[i - 1].Y - knots[i].Y) / 2;
                    continue;
                }

                if (knots[i - 1].Y == knots[i].Y && Math.Abs(knots[i - 1].X - knots[i].X) == 2)
                {
                    /* move towards head */
                    knots[i].X += (knots[i - 1].X - knots[i].X) / 2;
                    continue;
                }

                /* we must be diagonal */
                if (Math.Abs(knots[i - 1].X - knots[i].X) + Math.Abs(knots[i - 1].Y - knots[i].Y) >= 3)
                {
                    /* figure out relative direction from tail to head */
                    knots[i].X += (knots[i - 1].X - knots[i].X) > 0 ? 1 : -1;
                    knots[i].Y += (knots[i - 1].Y - knots[i].Y) > 0 ? 1 : -1;
                    
                    continue;
                }
            }
            VisitedPoints.Add(knots[knots.Length - 1]);
        }

    }

    internal class Day09 : BaseDay
    {
        List<(char Direction, int Count)> motions;
        public Day09()
        {
            motions = File.ReadAllLines(InputFilePath).Select(l =>
            {
                return l.Split(" ").Chunk(2).Select(parts => (Direction: parts[0][0], Count: int.Parse(parts[1]))).First();
            }).ToList();

        }

        public override ValueTask<string> Solve_1()
        {
            var z = new Rope(2);
            foreach(var m in motions)
            {
                z.MoveHead(m.Direction, m.Count);
            }

            return new(z.VisitedPoints.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var z = new Rope(10);
            foreach (var m in motions)
            {
                z.MoveHead(m.Direction, m.Count);
            }

            return new(z.VisitedPoints.Count.ToString());
        }
    }
}
