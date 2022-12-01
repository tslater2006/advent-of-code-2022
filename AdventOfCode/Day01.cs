namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] values;

    public Day01()
    {
        values = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() {

        List<int> elfCalories = new List<int>();
        int currentSum = 0;
        foreach (var s in values)
        {
            if (s == "")
            {
                elfCalories.Add(currentSum);    
                currentSum = 0;
            } else
            {
                currentSum += int.Parse(s);
            }
        }

        elfCalories.Add(currentSum);
        return new(elfCalories.Max().ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        List<int> elfCalories = new List<int>();
        int currentSum = 0;
        foreach (var s in values)
        {
            if (s == "")
            {
                elfCalories.Add(currentSum);
                currentSum = 0;
            }
            else
            {
                currentSum += int.Parse(s);
            }
        }

        elfCalories.Add(currentSum);

        int answer = elfCalories.OrderByDescending(x => x).Take(3).Sum();

        return new(answer.ToString());

    }
}
