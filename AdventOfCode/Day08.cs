using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day08 : BaseDay
    {
        byte[,] trees;
        int cols;
        int rows;
        public Day08()
        {
            var lines = File.ReadAllLines(InputFilePath);
            rows = lines.Length;
            cols = lines[0].Length;

            trees = new byte[rows, cols];


            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    trees[y, x] = (byte)(lines[y][x] - '0');
                }
            }
        }

        public int getScenicScore(int x, int y)
        {
            var treeHeight = trees[y, x];
            var scores = new int[4];
            /* go left */
            var scanX = x;
            while (scanX-- > 0)
            {
                scores[0]++;
                if (trees[y, scanX] >= treeHeight)
                    break;
                
            }

            /* go right */
            scanX = x;
            while (scanX++ < cols - 1)
            {
                scores[1]++;
                if (trees[y, scanX] >= treeHeight)
                    break;
                
            }


            /* go up */
            var scanY = y;
            while (scanY-- > 0)
            {
                scores[2]++;
                if (trees[scanY, x] >= treeHeight)
                    break;
                
            }


            /* go down */
            scanY = y;
            while (scanY++ < cols - 1)
            {
                scores[3]++;
                if (trees[scanY, x] >= treeHeight)
                    break;
                
            }

            return scores[0] * scores[1] * scores[2] * scores[3];
        }

        public bool isTreeVisible(int x, int y)
        {
            var treeHeight = trees[y, x];

            /* go left */
            var scanX = x;
            while (scanX-- > 0)
            {
                if (trees[y,scanX] >= treeHeight)
                    break;
                if (scanX == 0)
                    return true;
            }

            /* go right */
            scanX = x;
            while (scanX++ < cols - 1)
            {
                if (trees[y, scanX] >= treeHeight)
                    break;
                if (scanX == cols - 1)
                    return true;
            }
            

            /* go up */
            var scanY = y;
            while (scanY-- > 0)
            {
                if (trees[scanY, x] >= treeHeight)
                    break;
                if (scanY == 0)
                    return true;
            }
            

            /* go down */
            scanY = y;
            while (scanY++ < cols - 1)
            { 
                if (trees[scanY, x] >= treeHeight)
                    break;
                if (scanY == rows - 1)
                    return true;
            }
            
            return false;
        }

        public override ValueTask<string> Solve_1()
        {
            var visibleCount = 0;
            for(var y = 1; y < rows-1; y++)
            {
                for(var x = 1; x < cols-1; x++)
                {
                    if (isTreeVisible(x, y)) { visibleCount++; }
                }
            }

            visibleCount += cols * 2;
            visibleCount += rows * 2 - 4;

            return new(visibleCount.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var highestScenicScore = 0;

            for (var y = 1; y < rows - 1; y++)
            {
                for (var x = 1; x < cols - 1; x++)
                {
                    var score = getScenicScore(x, y);
                    if (score > highestScenicScore)
                    {
                        highestScenicScore = score;
                    }
                }
            }

            return new(highestScenicScore.ToString());
        }
    }
}
