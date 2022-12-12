namespace AdventOfCode2022.Problems.Day12;

class Day12 : ICodingProblem
{
    public void Run()
    {
        var rows = File.ReadAllLines("Problems\\Day12\\input.txt");
        var height = rows.Length;
        var width = rows[0].Length;
        var map = new Node[width, height];
        
        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        
        var unvisitedNodes = new List<Node>();
        Node? destinationNode = null; 
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var node = new Node
                {
                    Symbol = rows[y][x], 
                    Distance = rows[y][x] == 'S' ? 0 : int.MaxValue, 
                    Visited = false, 
                    X = x, 
                    Y = y
                };
                map[x, y] = node;
                unvisitedNodes.Add(node);
                if (node.Symbol == 'E')
                    destinationNode = node;
            }
        }

        do
        {
            var current = unvisitedNodes.MinBy(x => x.Distance);
            var x = current!.X;
            var y = current.Y;
            if (x > 0 && HasEdge(map[x - 1, y].Symbol, current.Symbol))
                map[x - 1, y].Distance = Math.Min(map[x - 1, y].Distance, current.Distance + 1);
            if (x < width - 1 && HasEdge(map[x + 1, y].Symbol, current.Symbol))
                map[x + 1, y].Distance =  Math.Min(map[x + 1, y].Distance, current.Distance + 1);
            if (y > 0 && HasEdge(map[x, y - 1].Symbol, current.Symbol))
                map[x, y - 1].Distance = Math.Min(map[x, y - 1].Distance, current.Distance + 1);
            if (y < height - 1 && HasEdge(map[x, y + 1].Symbol, current.Symbol))
                map[x, y + 1].Distance =  Math.Min(map[x, y + 1].Distance, current.Distance + 1);
            current.Visited = true;
            unvisitedNodes.Remove(current);
        } while (!destinationNode!.Visited && unvisitedNodes.Min(x => x.Distance) != int.MaxValue);
        
        Console.WriteLine($"Part 1: {destinationNode.Distance}");
    }

    private bool HasEdge(char to, char from)
    {
        if (from == 'S')
            from = 'a';
        if (from == 'E')
            from = 'z';
        if (to == 'S')
            to = 'a';
        if (to == 'E')
            to = 'z';
        return to - from <= 1;
    }

    class Node
    {
        public char Symbol { get; init; } 
        public int Distance { get; set; }
        public bool Visited { get; set; }
        public int X { get; init; } 
        public int Y { get; init; }
    }
}