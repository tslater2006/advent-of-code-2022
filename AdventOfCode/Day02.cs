public class Day02 : BaseDay
{
    enum Move
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    enum Result
    {
        Lose = 1,
        Draw = 2,
        Win = 3
        
    }

    List<(Move, Move)> matches = new();
    List<(Move, Result)> guides = new();
    public Day02()
    {
        foreach(var match in File.ReadLines(InputFilePath))
        {
            matches.Add(((Move)(match[0] - '@'), (Move)(match[2] - 'W')));
            guides.Add(((Move)(match[0] - '@'), (Result)(match[2] - 'W')));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int totalScore = 0;
        foreach(var match in matches)
        {
            totalScore += scoreRound(match.Item1, match.Item2);
        }

        return new(totalScore.ToString());
    }

    private int scoreRound(Move elfMove, Move myMove)
    {
        int score = (int)myMove;

        switch (myMove - elfMove)
        {
            case 0:
                score += 3;
                break;
            case 1:
                score += 6;
                break;
        }
        if (myMove == Move.Rock && elfMove == Move.Scissors)
        {
            score += 6;
        }
        return score;
    }

    public override ValueTask<string> Solve_2()
    {
        var totalScore = 0;
        foreach(var guide in guides)
        {
            var elfMove = guide.Item1;
            var result = guide.Item2;
            Move myMove = Move.Rock;
            switch(result)
            {
                case Result.Draw:
                    myMove = elfMove;
                    break;
                case Result.Lose:
                    myMove = (elfMove - 1) == 0 ? Move.Scissors : (Move)elfMove - 1;
                    break;
                case Result.Win:
                    myMove = (int)(elfMove + 1) > 3 ? Move.Rock : (Move)elfMove + 1;
                    break;
            }

            totalScore += scoreRound(elfMove, myMove);
        }


        return new(totalScore.ToString());
    }
}
