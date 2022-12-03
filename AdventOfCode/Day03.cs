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
        var ans = rucksacks.Select(ruck => ruck[0..(ruck.Length / 2)].Intersect(ruck[(ruck.Length / 2)..]).Distinct().First()).Sum(r => r >= 97 ? r - 96 : r - 64 + 26);
        return new(ans.ToString());

    }
    public override ValueTask<string> Solve_2()
    {
        var ans = rucksacks.Chunk(3).Select(elfs => elfs[0].Intersect(elfs[1]).Intersect(elfs[2]).Distinct().First()).Sum(r => r >= 97 ? r - 96 : r - 64 + 26);

        return new(ans.ToString());
    }
}
