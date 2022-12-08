namespace AdventOfCode2022.Problems.Day08;

public class Day8 : ICodingProblem
{
    public void Run()
    {
        var lines = File.ReadAllLines("Problems\\Day08\\input.txt");

        var length = lines.Length;
        var width = lines[0].Length;
        var forest = new int[width, length];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < length; y++)
                forest[x, y] = int.Parse(lines[y][x].ToString());
        }

        var count = 0;
        var maxScore = int.MinValue;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < length; y++)
            {
                var (isVisible, score) = isVisibleAndScenicScore(x, y, forest, width, length);
                if (isVisible)
                    count++;
                if (score > maxScore)
                    maxScore = score;
            }
        }

        Console.WriteLine($"Part 1: {count}");
        Console.WriteLine($"Part 2: {maxScore}");
    }

    private static (bool, int) isVisibleAndScenicScore(int x, int y, int[,] forest, int width, int length)
    {
        var scoreLeft = 0;
        var isVisibleLeft = true;
        for (var i = x - 1; i >= 0; i--)
        {
            scoreLeft++;
            if (forest[i, y] >= forest[x, y])
            {
                isVisibleLeft = false;
                break;
            }
        }

        var scoreRight = 0;
        var isVisibleRight = true;
        for (var i = x + 1; i < width; i++)
        {
            scoreRight++;
            if (forest[i, y] >= forest[x, y])
            {
                isVisibleRight = false;
                break;
            }
        }

        var scoreUp = 0;
        var isVisibleUp = true;
        for (var i = y - 1; i >= 0; i--)
        {
            scoreUp++;
            if (forest[x, i] >= forest[x, y])
            {
                isVisibleUp = false;
                break;
            }
        }

        var scoreDown = 0;
        var isVisibleDown = true;
        for (var i = y + 1; i < length; i++)
        {
            scoreDown++;
            if (forest[x, i] >= forest[x, y])
            {
                isVisibleDown = false;
                break;
            }
        }

        var isVisible = isVisibleLeft || isVisibleRight || isVisibleDown || isVisibleUp;
        var scenicScore = scoreLeft * scoreRight * scoreUp * scoreDown;
        return (isVisible, scenicScore);
    }
}