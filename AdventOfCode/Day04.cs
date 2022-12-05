namespace AdventOfCode;

public class Day04 : BaseDay
{
    List<(int Start, int Stop )[]> cleaningPairs = new();
    public Day04()
    {
        /* Input is 1 to many lines like \d+-\d+,\d+-\d+ */
        cleaningPairs = File.ReadAllLines(InputFilePath).Select(line =>
        {
            return line.Split(",").Select(section =>
            {
                return section.Split("-").Select(int.Parse).Chunk(2).Select(z => (Start: z[0],Stop : z[1])).First();
            }).Chunk(2).ToArray().First();
        }).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var completeOverlaps = 0;
        foreach(var pair in cleaningPairs)
        {
            if (pair[1].Start >= pair[0].Start && pair[1].Stop  <= pair[0].Stop )
            {
                completeOverlaps++;
            } else if (pair[0].Start >= pair[1].Start && pair[0].Stop  <= pair[1].Stop )
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
            if (pair[0].Start >= pair[1].Start && pair[0].Start <= pair[1].Stop )
            {
                partialOverlaps++;
            }
            else if (pair[0].Stop  >= pair[1].Start && pair[0].Stop  <= pair[1].Stop )
            {
                partialOverlaps++;
            }
            else if (pair[1].Start >= pair[0].Start && pair[1].Start <= pair[0].Stop )
            {
                partialOverlaps++;
            }
            else if (pair[1].Stop  >= pair[0].Start && pair[1].Stop  <= pair[0].Stop )
            {
                partialOverlaps++;
            }
        }

        return new(partialOverlaps.ToString());
    }
}
