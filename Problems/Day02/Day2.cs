namespace AdventOfCode2022.Problems.Day02;

class Day2 : ICodingProblem
{
    public void Run()
    {
        var sum = 0;
        foreach (var round in File.ReadAllLines("Problems\\Day02\\input.txt"))
        {
            var opponent = Parse(round.Substring(0, 1));
            var mine = Parse(round.Substring(2, 1));
            var result = GetResult(mine, opponent);
            if (result == Result.Win)
                sum += 6;
            else if (result == Result.Draw)
                sum += 3;
            sum += (int)mine + 1;
        }
        Console.WriteLine($"Part 1: {sum}");
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

    private HandShape Parse(string s)
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
}