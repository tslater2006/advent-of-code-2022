namespace AdventOfCode;

public class Day04 : BaseDay
{
    List<((int, int), (int, int))> cleaningPairs = new();
    public Day04()
    {
        /* Input is 1 to many lines like \d+-\d+,\d+-\d+ */
        cleaningPairs = File.ReadAllLines(InputFilePath).Select(line =>
        {
            return line.Split(",").Select(section =>
            {
                return section.Split("-").Select(int.Parse).Chunk(2).Select(z => (z[0],z[1])).First();
            }).Chunk(2).Select(q => (q[0],q[1])).First();
        }).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var completeOverlaps = 0;
        foreach(var pair in cleaningPairs)
        {
            if (pair.Item2.Item1 >= pair.Item1.Item1 && pair.Item2.Item2 <= pair.Item1.Item2)
            {
                completeOverlaps++;
            } else if (pair.Item1.Item1 >= pair.Item2.Item1 && pair.Item1.Item2 <= pair.Item2.Item2)
            {
                completeOverlaps++;
            }
        }

        return new(completeOverlaps.ToString());

    }
    public override ValueTask<string> Solve_2()
    {

        var partialOverlaps = 0;
        foreach (var pair in cleaningPairs)
        {
            if (pair.Item1.Item1 >= pair.Item2.Item1 && pair.Item1.Item1 <= pair.Item2.Item2)
            {
                partialOverlaps++;
            }
            else if (pair.Item1.Item2 >= pair.Item2.Item1 && pair.Item1.Item2 <= pair.Item2.Item2)
            {
                partialOverlaps++;
            }
            else if (pair.Item2.Item1 >= pair.Item1.Item1 && pair.Item2.Item1 <= pair.Item1.Item2)
            {
                partialOverlaps++;
            }
            else if (pair.Item2.Item2 >= pair.Item1.Item1 && pair.Item2.Item2 <= pair.Item1.Item2)
            {
                partialOverlaps++;
            }
        }

        return new(partialOverlaps.ToString());
    }
}
