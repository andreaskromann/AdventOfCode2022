namespace AdventOfCode2022.Problems.Day10;

class Day10 : ICodingProblem
{
    public void Run()
    {
        var lines = File.ReadAllLines("Problems\\Day10\\input.txt");
        var register = 1;
        var cycle = 0;
        var sum = 0;
        var crt = Enumerable.Range(1, 240).Select(_ => '.').ToArray();

        void print()
        {
            Console.WriteLine($"Cycle: {cycle}");
            Console.WriteLine("Sprite position:");
            for (var i = 0; i < 40; i++)
            {
                if (i >= register - 1 && i <= register + 1)
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.Write('\n');

            Console.WriteLine("CRT status:");
            for (var i = 0; i < crt.Length; i++)
            {
                Console.Write(crt[i]);
                if ((i + 1) % 40 == 0)
                    Console.Write('\n');
            }
        }

        void incCycle()
        {
            cycle++;
            if (cycle == 20 || (cycle >= 60 && (cycle - 60) % 40 == 0))
                sum += cycle * register;
            
            var currentPosition = cycle - 1;
            var row = currentPosition / 40;
            if (currentPosition >= row * 40 + register - 1 && currentPosition <= row * 40 + register + 1)
                crt[currentPosition] = '#';
            
            //print();
        }
        
        foreach (var line in lines)
        {
            if (line.StartsWith("noop"))
                incCycle();
            
            if (line.StartsWith("addx"))
            {
                var value = int.Parse(line.Split(' ')[1]);
                for (var i = 0; i < 2; i++)
                    incCycle();
                register += value;
            }
        }
        
        Console.WriteLine($"Part 1: {sum}");
        Console.WriteLine("Part 2:");
        print();
    }
}