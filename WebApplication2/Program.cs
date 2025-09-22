using System;
class Program
{
    static Random random = new Random();
    class Evee
    {
        public int X { get; set; }
        public int Y { get; set; }
		public int Evolution {get; set;}
		public int LengthOfLife {get; set;}
		public char Symbol {get; set;}
		public int Health {get; set;}
        public Evee(int x, int y, int evolution, int length, char symbol, int health)
        {
            X = x;
            Y = y;
            Evolution = evolution;
            LengthOfLife = length;
            Symbol = symbol;
            Health = health;
        }
        public virtual void Move(int width, int height, List<Predator> predatores, List<Prey> preys)
        {
            int newX = (X + random.Next(-1, 2) + width) % width;
                int newY = (Y + random.Next(-1, 2) + height) % height;
                bool canMove = true;
            if (newX < 0)
            {
                newX = 0;
            }
            if (newX == width)
            {
                newX = width-1;
            }
            if (newY < 0)
            {
                newY = 0;
            }
            if (newY == width)
            {
                newY = width-1;
            }
                foreach (var p in predatores)
            {
                if (p.X == newX && p.Y == newY)
                {
                    canMove = false;
                    break;
                }
            }
                foreach (var e in preys)
                {
                    if (e.X == newX && e.Y == newY)
                    {
                        canMove = false;
                        break;
                    }
                }
            if (canMove)
            {

                X = newX;
                Y = newY;
            }
            this.LengthOfLife += 1;
        }
        
    }
    class Predator : Evee
    {
        public int Hunger { get; set; }
        public bool Ate { get; set; }
        public Predator(int x, int y, int evolution, int lengthOfLife, char symbol, int health) : base(x, y, evolution, lengthOfLife, symbol, health)
        {
            Hunger = 0;
            Ate = false;
            Symbol = symbol;
        }
        public void Eat(List<Prey> preys)
        {
            List<Prey> remainingPrey = new List<Prey>();
            bool ate = false;
            foreach (var prey in preys)
            {
                if (Math.Abs(X - prey.X) <= 1 && Math.Abs(Y - prey.Y) <= 1)
                {
                    if (this.Evolution == 0 && prey.Evolution == 1 || this.Evolution == 1 && prey.Evolution == 2 || this.Evolution == 2 && prey.Evolution == 0)
                    {
                        Hunger = 0;
                        prey.Health -= 30;
                        if (this.Evolution == 2 && this.LengthOfLife >= 3)
                        {
                            prey.Health -= 10;
                        }
                        ate = true;
                        Ate = true;
                        if (prey.Evolution == 0 && prey.LengthOfLife >= 3)
                        {
                            this.Health -= 10;
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
                        }
                        break;

                    }
                }
            }
            if (!ate)
            {
                Hunger = 1;
                Ate = false;
            }
        }
        public void Reproduce(List<Predator> predatores, List<Prey> preys, int width, int height)
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
                    int evo = random.Next(0, 3);
                    predatores.Add(new Predator(newX, newY, evo, 0, 'E', 100));
                }
            }
        }
        public void CheckDeath(List<Predator> predatores)
        {
            List<Predator> remainingPredator = new List<Predator>();
            foreach (var p in predatores)
            {
                if (p.Health > 0)
                {
                    remainingPredator.Add(p);
                }
            }
            predatores = remainingPredator;
        }
        public virtual void Evolve()
        {
            if (this.Evolution == 0)
            {
                this.Symbol = 'F';
            }
            else if (this.Evolution == 1)
            {
                this.Symbol = 'J';
            }
            else if (this.Evolution == 2)
            {
                this.Symbol = 'V';
            }
        }
    }
    class Prey : Evee
    {
        public Prey(int x, int y, int evolution, int lengthOfLife, char symbol, int health) : base(x, y, evolution, lengthOfLife, symbol, health) { }
        public void Reproduce(List<Predator> predatores, List<Prey> preys, int width, int height)
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
                int evo = random.Next(0, 3);
                preys.Add(new Prey(newX, newY, evo, 0, 'e', 100));
            }
        }
        public void Heal(Prey prey)
        {
            prey.Health += 10;
            if (prey.Health > 100)
            {
                prey.Health = 100;
            }
        }
        public virtual void Evolve()
        {
            if (this.Evolution == 0)
            {
                this.Symbol = 'f';
            }
            else if (this.Evolution == 1)
            {
                this.Symbol = 'j';
            }
            else if (this.Evolution == 2)
            {
                this.Symbol = 'v';
            }
        }
        public void CheckDeath(List<Prey> preys)
        {
            List<Prey> remainingPrey = new List<Prey>();
            foreach (var p in preys)
            {
                if (p.Health > 0)
                {
                    remainingPrey.Add(p);
                }
            }
            preys = remainingPrey;
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
            field[predator.Y, predator.X] = predator.Symbol;
        }
        foreach (var prey in preys)
        {
            if (field[prey.Y, prey.X] == '.')
            {
                field[prey.Y, prey.X] = prey.Symbol;
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
        int fieldSize = Convert.ToInt32(Console.ReadLine());
        if (fieldSize <= 0)
        {
            Console.WriteLine("Input field size higher then 0");
            fieldSize = Convert.ToInt32(Console.ReadLine());
        }
        List<Predator> predatores = new List<Predator>();
        List<Prey> preys = new List<Prey>();
        var occupied = new HashSet<Tuple<int, int>>();
        while (predatores.Count < numPredatores)
        {
            int x = random.Next(0, fieldSize);
            int y = random.Next(0, fieldSize);
            var pos = Tuple.Create(x, y);
            if (!occupied.Contains(pos))
            {
				int evo = random.Next(0,3);
                predatores.Add(new Predator(x, y, evo,0,'E',100));
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
                preys.Add(new Prey(x, y, evo,0,'e',100));
                occupied.Add(pos);
            }
        }
        int step = 0;
        while (predatores.Count > 0 || preys.Count > 0)
        {
            step++;
            Console.WriteLine($"step {step}");
            for (int i = 0; i < predatores.Count; i++)
            {
                if (predatores[i].LengthOfLife == 3)
                {
                    predatores[i].Evolve();
                }
                predatores[i].CheckDeath(predatores);
                if (predatores[i].Evolution == 0 && predatores[i].LengthOfLife >= 3)
                {
                    predatores[i].Eat(preys);
                }
                predatores[i].Move(fieldSize, fieldSize, predatores, preys);
            }
            for (int i = 0; i < preys.Count; i++)
            {
                if (preys[i].LengthOfLife == 3)
                {
                    preys[i].Evolve();
                }
                preys[i].CheckDeath(preys);
                if (preys[i].LengthOfLife == 3)
                    if (preys[i].Evolution == 2 && preys[i].LengthOfLife >= 3 && preys[i].Health < 100)
                    {
                        preys[i].Heal(preys[i]);
                    }
                preys[i].Move(fieldSize, fieldSize, predatores, preys);
            }
            for (int i = 0; i < predatores.Count; i++)
            {
                if (predatores[i].Evolution == 1 && predatores[i].LengthOfLife >= 3)
                {
                    predatores[i].Move(fieldSize, fieldSize, predatores, preys);
                }
                predatores[i].Eat(preys);
                if (predatores.Count + preys.Count <= ((fieldSize * fieldSize) / 2))
                {
                    predatores[i].Reproduce(predatores, preys, fieldSize, fieldSize);
                }
            }
            for (int i = 0; i < preys.Count; i++)
            {
                if (predatores.Count + preys.Count <= ((fieldSize * fieldSize) / 2))
                {
                    preys[i].Reproduce(predatores, preys, fieldSize, fieldSize);
                }
            }
            PrintField(predatores, preys, fieldSize, fieldSize);
        }
        Console.WriteLine("end");
    }
}