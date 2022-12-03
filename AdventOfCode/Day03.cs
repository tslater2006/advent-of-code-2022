namespace AdventOfCode;

public class Day03 : BaseDay
{
    string[] rucksacks = null;
    public Day03()
    {
        rucksacks = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<char> foundDuplicates = new();
        foreach(var ruck in rucksacks)
        {
            List<char> firstHalfChars = new();

            for(var x = 0; x < ruck.Length; x++)
            {
                if (x < ruck.Length/2)
                {
                    firstHalfChars.Add(ruck[x]);
                } else
                {
                    if (firstHalfChars.Contains(ruck[x]))
                    {
                        /* found the dup */
                        foundDuplicates.Add(ruck[x]);
                        break;
                    }
                }
            }
        }

        var answer = foundDuplicates.Sum(x =>
        {
            if (x >= 97)
            {
                return x - 96;
            } else
            {
                return x - 64 + 26;
            }
        });


        return new(answer.ToString());

    }
    public override ValueTask<string> Solve_2()
    {
        List<char> badges = new();
        foreach (var elfs in rucksacks.Chunk(3))
        {
            List<char> workingSet = new(elfs[0].ToCharArray());
            List<char> commonChars = new();
            foreach (var elf in elfs.Skip(1))
            {
                foreach( var c in elf)
                {
                    if (workingSet.Contains(c))
                    {
                        commonChars.Add(c);
                    }
                }
                workingSet.Clear();
                workingSet.AddRange(commonChars);
                commonChars.Clear();
            }

            badges.Add(workingSet[0]);
        }

        var answer = badges.Sum(x =>
        {
            if (x >= 97)
            {
                return x - 96;
            }
            else
            {
                return x - 64 + 26;
            }
        });

        return new(answer.ToString());
    }
}
