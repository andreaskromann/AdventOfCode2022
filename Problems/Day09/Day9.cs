namespace AdventOfCode2022.Problems.Day09;

class Day9 : ICodingProblem
{
    public void Run()
    {
        var lines = File.ReadAllLines("Problems\\Day09\\input.txt");
        var visited = new HashSet<Position>();
        var snake = Enumerable.Range(1, 10).Select(x => new Position(0, 0)).ToArray();
        visited.Add(snake[^1]);
        foreach (var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            switch (parts[0])
            {
                case "L":
                    for (var i = 0; i < int.Parse(parts[1]); i++)
                    {
                        snake[0] = snake[0] with { X = snake[0].X - 1 };
                        for (var j = 1; j < snake.Length; j++)
                            snake[j] = Follow(snake[j], snake[j - 1], j == snake.Length - 1 ? visited : null);
                    }

                    break;
                case "R":
                    for (var i = 0; i < int.Parse(parts[1]); i++)
                    {
                        snake[0] = snake[0] with { X = snake[0].X + 1 };
                        for (var j = 1; j < snake.Length; j++)
                            snake[j] = Follow(snake[j], snake[j - 1], j == snake.Length - 1 ? visited : null);
                    }

                    break;
                case "U":
                    for (var i = 0; i < int.Parse(parts[1]); i++)
                    {
                        snake[0] = snake[0] with { Y = snake[0].Y - 1 };
                        for (var j = 1; j < snake.Length; j++)
                            snake[j] = Follow(snake[j], snake[j - 1], j == snake.Length - 1 ? visited : null);
                    }

                    break;
                case "D":
                    for (var i = 0; i < int.Parse(parts[1]); i++)
                    {
                        snake[0] = snake[0] with { Y = snake[0].Y + 1 };
                        for (var j = 1; j < snake.Length; j++)
                            snake[j] = Follow(snake[j], snake[j - 1], j == snake.Length - 1 ? visited : null);
                    }

                    break;
            }
        }

        Console.WriteLine($"Part 2: {visited.Count}");
    }

    private static Position Follow(Position tail, Position head, HashSet<Position>? visited)
    {
        if (Math.Abs(tail.X - head.X) > 1 || Math.Abs(tail.Y - head.Y) > 1)
        {
            tail = tail with { X = tail.X - Math.Sign(tail.X - head.X), Y = tail.Y - Math.Sign(tail.Y - head.Y) };
            visited?.Add(tail);
        }

        return tail;
    }

    private record Position(int X, int Y);
}