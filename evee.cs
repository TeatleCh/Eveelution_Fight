using System;
using Eveelutionfight;
class Program
{
    static Random random = new Random();
    class Evee
    {
        public int X { get; set; }
        public int Y { get; set; }
		public int Evolution {get; set;}
		public int LengthOfLife {get; set;}
		public string Symbol {get; set;}
		public int Health {get; set;}
        public Evee(int x, int y, int evolution, int length, string symbol, int health)
        {
            X = x;
            Y = y;
            Evolution = evolution;
            LengthOfLife = length;
            Symbol = symbol;
            Health = health;
        }
        public virtual void Move(int width, int height)
        {
            int dx = random.Next(-1, 2);
            int dy = random.Next(-1, 2);
            X = (X + dx + width) % width;
            Y = (Y + dy + height) % height;
            Evee.LengthOfLife += 1;
        }
        public virtual void Evolve()
        {
            if (Evee.LengthOfLife == 3)
            {
                if (Evee.Evolution == 0)
                {
                    Evee.Symbol = "f";
                }
                else if (Evee.Evolution == 1)
                {
                    Evee.Symbol = "j";
                }
                else if (Evee.Evolution == 2)
                {
                    Evee.Symbol = "v";
                }
            }
        }
    }
    class Predator : Evee
    {
        public int Hunger { get; set; }
        public bool Ate { get; set; }
        public Predator(int x, int y, int evolution, int lengthOfLife, string symbol, int health) : base(x, y, evolution, lengthOfLife, symbol, health)
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
                    if (Predator.Evolution == 0 && prey.Evolution == 1 || Predator.Evolution == 1 && prey.Evolution == 2 || Predator.Evolution == 2 && prey.Evolution == 0)
                    {
                        Hunger = 0;
                        prey.Health -= 30;
                        if (Predator.Evolution == 2 && Predator.LengthOfLife >= 3)
                        {
                            prey.Health -= 10;
                        }
                        ate = true;
                        Ate = true;
                        if (prey.Evolution == 0 && prey.LengthOfLife >= 3)
                        {
                            Predator.Health -= 10;
                        }
                        if (prey.Evolution == 1 && prey.LengthOfLife >= 3)
                        {
                            int dodge = random.Next(0, 2);
                            if (dodge == 1)
                            {
                                prey.Health += 30;
                            }
                        }
                        if (prey.Health > 0)
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
					int evo = random.Next(0,3);
                    var newPredatores = new List<Predator>(predatores);
                    newPredatores.Add(new Predator(newX, newY,evo,0,"e",100));
                    return newPredatores;
                }
            }
            return predatores;
        }
        public List<Predator> ChekDeath(List<Predator> predatores)
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
        public Prey(int x, int y, int evolution, int lengthOfLife, string symbol, int health) : base(x, y, evolution, lengthOfLife, symbol, health) { }
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
				int evo = random.Next(0,3);
                var newPreys = new List<Prey>(preys);
                newPreys.Add(new Prey(newX, newY, evo, 0,"e",100));
                return newPreys;
            }
            return preys;
        }
        public List<Prey> Heal(List<Prey> preys)
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
				int evo = random.Next(0,3);
                predatores.Add(new Predator(x, y, evo,0,"e",100));
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
				int evo = random.Next(0,3);
                preys.Add(new Prey(x, y, evo,0,"e",100));
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
            for (int i = 0; i <= preys.Count; i++)
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