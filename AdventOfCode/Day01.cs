namespace AdventOfCode;

public class Day01 : BaseDay
{
    private string[] elveLists;

    public Day01()
    {
        elveLists = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
    }

    public override ValueTask<string> Solve_1() {

        int maxCalories = 0;
        foreach(var list in elveLists)
        {
            int calorieCount = list.Split("\r\n").Select(s => int.Parse(s)).Sum();
            if (calorieCount > maxCalories)
            {
                maxCalories = calorieCount;
            }
        }

        return new(maxCalories.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        List<int> elfCalories = new List<int>();
        foreach (var list in elveLists)
        {
            elfCalories.Add(list.Split("\r\n").Select(s => int.Parse(s)).Sum());
            
        }

        int answer = elfCalories.OrderByDescending(x => x).Take(3).Sum();

        return new(answer.ToString());

    }
}
