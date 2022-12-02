namespace AdventOfCode2022.Problems.Day02;

class Day2 : ICodingProblem
{
    public void Run()
    {
        // Part 1
        var sum = 0;
        foreach (var round in File.ReadAllLines("Problems\\Day02\\input.txt"))
        {
            var opponent = ParseHandShape(round.Substring(0, 1));
            var mine = ParseHandShape(round.Substring(2, 1));
            var result = GetResult(mine, opponent);
            if (result == Result.Win)
                sum += 6;
            else if (result == Result.Draw)
                sum += 3;
            sum += (int)mine + 1;
        }
        Console.WriteLine($"Part 1: {sum}");
        
        // Part 2
        sum = 0;
        foreach (var round in File.ReadAllLines("Problems\\Day02\\input.txt"))
        {
            var opponent = ParseHandShape(round.Substring(0, 1));
            var desiredResult = ParseResult(round.Substring(2, 1));
            var mine = PickHand(desiredResult, opponent);
            var result = GetResult(mine, opponent);
            if (result == Result.Win)
                sum += 6;
            else if (result == Result.Draw)
                sum += 3;
            sum += (int)mine + 1;
        }
        Console.WriteLine($"Part 2: {sum}");
    }

    enum Result
    {
        Draw,
        Win,
        Loss
    }

    enum HandShape
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }

    private Result ParseResult(string s)
    {
        switch (s.Trim())
        {
            case "X":
                return Result.Loss;
            case "Y":
                return Result.Draw;
            case "Z":    
                return Result.Win;
        }

        throw new Exception("Unknown result");
    }
    
    private HandShape ParseHandShape(string s)
    {
        switch (s.Trim())
        {
            case "A":
            case "X":
                return HandShape.Rock;
            case "B":
            case "Y":
                return HandShape.Paper;
            case "C":
            case "Z":    
                return HandShape.Scissors;
        }

        throw new Exception("Unknown shape");
    }

    private Result GetResult(HandShape x, HandShape y)
    {
        var diff = (int)x - (int)y;
        switch (diff)
        {
            case 0:
                return Result.Draw;
            case 1:
            case -2:
                return Result.Win;
            default:
                return Result.Loss;
        }
    }
    
    private HandShape PickHand(Result result, HandShape opponent)
    {
        switch (result)
        {
            case Result.Draw:
                return opponent;
            case Result.Win:
                var winningHand = (int)opponent + 1;
                if (winningHand <= 2)
                    return (HandShape)winningHand;
                return (HandShape)((int)opponent - 2);
            case Result.Loss:
                var losingHand = (int)opponent + 2;
                if (losingHand <= 2)
                    return (HandShape)losingHand;
                return (HandShape)((int)opponent - 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}