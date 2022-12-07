namespace AdventOfCode;

class PacketWindow
{
    char[] window;
    public PacketWindow(int size)
    {
        window = new char[size];
    }

    public void Add(char c)
    {
        for(var x = 0; x < window.Length - 1; x++)
        {
            window[x] = window[x + 1];
        }
        window[window.Length - 1] = c;
    }

    public int DistinctCount()
    {
        return window.Distinct().Count();
    }
}

public class Day06 : BaseDay
{
    string streamData;

    public Day06()
    {
        streamData = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        PacketWindow window = new PacketWindow(4);
        var endOfMarker = 0;
        for(var x = 0; x < streamData.Length; x++)
        {
            window.Add(streamData[x]);

            if (x >= 3 && window.DistinctCount() == 4)
            {
                endOfMarker = x + 1;
                break;
            }
        }

        return new(endOfMarker.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        PacketWindow window = new PacketWindow(14);
        var endOfMarker = 0;
        for (var x = 0; x < streamData.Length; x++)
        {
            window.Add(streamData[x]);

            if (x >= 13 && window.DistinctCount() == 14)
            {
                endOfMarker = x + 1;
                break;
            }
        }

        return new(endOfMarker.ToString());
    }
}
