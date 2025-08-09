using System;
using Eveelutionfight;
class Program
{
    static Random random = new Random();
    class Evee
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Evee(int x, int y)
        {
            X = x;
            Y = y;
            int evolution = random(0, 3);
            int lengthOfLife = 0;
            string symbol = "e";
            int health = 100;
        }
        public virtual void Move(int width, int height)
        {
            int dx = random.Next(-1, 2);
            int dy = random.Next(-1, 2);
            X = (X + dx + width) % width;
            Y = (Y + dy + height) % height;
            Evee.lengthOfLife += 1;
        }
        public virtual void Evolve()
        {
            if (Evee.lengthOfLife == 3)
            {
                if (Evee.evolution == 0)
                {
                    Evee.symbol = "f";
                }
                else if (Evee.evolution == 1)
                {
                    Evee.symbol = "j";
                }
                else if (Evee.evolution == 2)
                {
                    Evee.symbol = "v";
                }
            }
        }
    }
    class Predator : Evee
    {
        public int Hunger { get; set; }
        public bool Ate { get; set; }
        public Predator(int x, int y, int evolution, int lengthOfLife, string symbol) : base(x, y, evolution, lengthOfLife, symbol)
        {
            Hunger = 0;
            Ate = false;
            symbol = symbol.ToUpper();
        }
        public List<Prey> Eat(List<Prey> preys)
        {
            List<Prey> remainingPrey = new List<Prey>();
            bool ate = false;
            foreach (var prey in preys)
            {
                if (Math.Abs(X - prey.X) <= 1 && Math.Abs(Y - prey.Y) <= 1)
                {
                    if (Predator.evolution == 0 && prey.evolution == 1 || Predator.evolution == 1 && prey.evolution == 2 || Predator.evolution == 2 && prey.evolution == 0)
                    {
                        Hunger = 0;
                        prey.health -= 30;
                        if (Predator.evolution == 2 && Predator.lengthOfLife >= 3)
                        {
                            prey.health -= 10;
                        }
                        ate = true;
                        Ate = true;
                        if (prey.evolution == 0 && prey.lengthOfLife >= 3)
                        {
                            Predator.health -= 10;
                        }
                        if (prey.evolution == 1 && prey.lengthOfLife >= 3)
                        {
                            int dodge = random(0, 2);
                            if (dodge == 1)
                            {
                                prey.health += 30;
                            }
                        }
                        if (prey.health > 0)
                        {
                            Ate = false;
                            remainingPrey.Add(prey);
                        }
                        break;

                    }
                }
                else
                {
                    remainingPrey.Add(prey);
                }
            }
            if (!ate)
            {
                Hunger++;
                Ate = false;
                return preys;
            }
            return remainingPrey;
        }
        public List<Predator> Reproduce(List<Predator> predatores, List<Prey> preys, int width, int height)
        {
            if (Ate)
            {
                int newX = (X + random.Next(-1, 2) + width) % width;
                int newY = (Y + random.Next(-1, 2) + height) % height;
                bool canSpawn = true;
                foreach (var p in predatores)
                {
                    if (p.X == newX && p.Y == newY)
                    {
                        canSpawn = false;
                        break;
                    }
                }
                foreach (var e in preys)
                {
                    if (e.X == newX && e.Y == newY)
                    {
                        canSpawn = false;
                        break;
                    }
                }
                if (canSpawn)
                {
                    var newPredatores = new List<Predator>(predatores);
                    newPredatores.Add(new Predator(newX, newY));
                    return newPredatores;
                }
            }
            return predatores;
        }
        public List<Predator> ChekDeath()
        {
            List<Predator> remainingPredator = new List<Predator>();
            foreach (var p in predatores)
            {
                if (p.health > 0)
                {
                    remainingPredator.Add(p);
                }
            }
            return remainingPredator;
        }
    }
    class Prey : Evee
    {
        public Prey(int x, int y, int evolution, int lengthOfLife, string symbol) : base(x, y, evolution, lengthOfLife, symbol) { }
        public List<Prey> Reproduce(List<Predator> predatores, List<Prey> preys, int width, int height)
        {
            int newX = (X + random.Next(-1, 2) + width) % width;
            int newY = (Y + random.Next(-1, 2) + height) % height;
            bool canSpawn = true;
            foreach (var p in predatores)
            {
                if (p.X == newX && p.Y == newY)
                {
                    canSpawn = false;
                    break;
                }
            }
            foreach (var e in preys)
            {
                if (e.X == newX && e.Y == newY)
                {
                    canSpawn = false;
                    break;
                }
            }
            if (canSpawn)
            {
                var newPreys = new List<Prey>(preys);
                newPreys.Add(new Prey(newX, newY));
                return newPreys;
            }
            return preys;
        }
        public List<Prey> Heal()
        {
            foreach (var p in preys)
            {
                if (p.evolution == 2 && p.lengthOfLife >= 3)
                {
                    p.health += 10;
                    if (p.health > 100)
                    {
                        p.health = 100;
                    }
                }
            }
            return preys;
        }
    }
    static void PrintField(List<Predator> predatores, List<Prey> preys, int width, int height)
    {
        char[,] field = new char[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field[y, x] = '.';
            }
        }
        foreach (var predator in predatores)
        {
            field[predator.Y, predator.X] = predator.symbol;
        }
        foreach (var prey in preys)
        {
            if (field[prey.Y, prey.X] == '.')
            {
                field[prey.Y, prey.X] = prey.symbol;
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(field[y, x] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Predatores: {predatores.Count}, Preys: {preys.Count}\n");
    }
    static void Main()
    {
        int numPredatores = random.Next(1, 11);
        int numPreys = random.Next(1, 11);
        Console.WriteLine("Input field size");
        int fieldSize = int.Parse(Console.ReadLine());
        var predatores = new List<Predator>();
        var preys = new List<Prey>();
        var occupied = new HashSet<Tuple<int, int>>();
        while (predatores.Count < numPredatores)
        {
            int x = random.Next(0, fieldSize);
            int y = random.Next(0, fieldSize);
            var pos = Tuple.Create(x, y);
            if (!occupied.Contains(pos))
            {
                predatores.Add(new Predator(x, y));
                occupied.Add(pos);
            }
        }
        while (preys.Count < numPreys)
        {
            int x = random.Next(0, fieldSize);
            int y = random.Next(0, fieldSize);
            var pos = Tuple.Create(x, y);
            if (!occupied.Contains(pos))
            {
                preys.Add(new Prey(x, y));
                occupied.Add(pos);
            }
        }
        int step = 0;
        while (predatores.Count > 0 && preys.Count > 0)
        {
            step++;
            Console.WriteLine($"step {step}");
            for (int i = 0; i < predatores.Count; i++)
            {
                predatores[i].ChekDeath();
                if (predatores[i].evolution == 0 && predatores[i].lengthOfLife >= 3)
                {
                    preys = predatores[i].Eat(preys);
                }
                predatores[i].Move(fieldSize, fieldSize);
                predatores[i].Evolve();
                if (predatores[i].evolution == 1 && predatores[i].lengthOfLife >= 3)
                {
                    predatores[i].Move(fieldSize, fieldSize);
                }
                preys = predatores[i].Eat(preys);
                predatores = predatores[i].Reproduce(predatores, preys, fieldSize, fieldSize);
            }
            for (int i = 0; i < preys.Count; i++)
            {
                preys[i].heal();
                preys[i].Move(fieldSize, fieldSize);
                preys[i].Evolve();
                preys = preys[i].Reproduce(predatores, preys, fieldSize, fieldSize);
            }
            PrintField(predatores, preys, fieldSize, fieldSize);
        }
        Console.WriteLine("end");
    }
}

//добавить prey.evolution =1 возможность dodge и 2 heal