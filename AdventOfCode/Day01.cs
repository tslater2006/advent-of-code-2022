namespace AdventOfCode;

public class Day01 : BaseDay
{
    private string[] elveLists;

    public Day01()
    {
        elveLists = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
    }

    public override ValueTask<string> Solve_1() {

        return new(elveLists.Select(list => list.Split("\r\n").Select(s => int.Parse(s)).Sum()).Max().ToString());
        
    }
    public override ValueTask<string> Solve_2()
    {
        return new(elveLists.Select(list => list.Split("\r\n").Select(s => int.Parse(s)).Sum()).OrderByDescending(x => x).Take(3).Sum().ToString());
    }
}
