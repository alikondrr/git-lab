using System;

//лаба 2
namespace лаба_2
{
    public abstract class GameObject
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Name { get; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        protected GameObject(string name, int x, int y)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым");

            Id = _nextId++;
            Name = name;
            X = x;
            Y = y;
        }
    }

    public interface IAttacker
    {
        void Attack(Unit unit);
    }

    public interface IMoveable
    {
        void Move(int x, int y);
    }

    public abstract class Unit : GameObject
    {
        public float Hp { get; private set; }

        protected Unit(string name, int x, int y, float hp)
            : base(name, x, y)
        {
            if (float.IsNaN(hp) || float.IsInfinity(hp) || hp < 0)
                throw new ArgumentException("Здоровье должно быть неотрицательным");

            Hp = hp;
        }

        public bool IsAlive() => Hp > 0;

        public virtual void ReceiveDamage(float damage)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentException("Урон должен быть неотрицательным");

            Hp = Math.Max(0, Hp - damage);
        }
    }

    public abstract class Building : GameObject
    {
        public bool Built { get; protected set; }

        protected Building(string name, int x, int y, bool built)
            : base(name, x, y)
        {
            Built = built;
        }
    }

    public class Archer : Unit, IAttacker, IMoveable
    {
        public float Damage { get; }

        public Archer(string name, int x, int y, float hp, float damage)
            : base(name, x, y, hp)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentException("Урон должен быть неотрицательным");

            Damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            if (!IsAlive())
            {
                Console.WriteLine($"{Name} мёртв и не может атаковать.");
                return;
            }

            if (!unit.IsAlive())
            {
                Console.WriteLine($"{unit.Name} уже мёртв.");
                return;
            }

            Console.WriteLine(
                $"{Name} атакует {unit.Name} и наносит {Damage} урона.");

            unit.ReceiveDamage(Damage);
        }

        public void Move(int x, int y)
        {
            if (!IsAlive())
            {
                Console.WriteLine($"{Name} мёртв и не может двигаться.");
                return;
            }

            Console.WriteLine(
                $"{Name} перемещается из ({X}, {Y}) в ({x}, {y}).");

            X = x;
            Y = y;
        }
    }

    public class Fort : Building, IAttacker
    {
        public float Damage { get; }

        public Fort(string name, int x, int y, bool built, float damage)
            : base(name, x, y, built)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentException("Урон должен быть неотрицательным");

            Damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            if (!Built)
            {
                Console.WriteLine(
                    $"{Name} ещё не построена и не может атаковать.");
                return;
            }

            if (!unit.IsAlive())
            {
                Console.WriteLine($"{unit.Name} уже мёртв.");
                return;
            }

            Console.WriteLine(
                $"{Name} стреляет по {unit.Name} и наносит {Damage} урона.");

            unit.ReceiveDamage(Damage);
        }
    }

    public class MobileHouse : Building, IMoveable
    {
        public MobileHouse(string name, int x, int y, bool built)
            : base(name, x, y, built)
        {
        }

        public void Move(int x, int y)
        {
            if (!Built)
            {
                Console.WriteLine(
                    $"{Name} ещё не построен и не может двигаться.");
                return;
            }

            Console.WriteLine(
                $"{Name} едет из ({X}, {Y}) в ({x}, {y}).");

            X = x;
            Y = y;
        }
    }

    public class Program
    {
        public static void Main()
        {
            Archer archer = new Archer("Лучник", 0, 0, 100, 15);
            Archer enemy = new Archer("Вражеский лучник", 5, 5, 80, 10);
            Fort fort = new Fort("Крепость", 10, 10, true, 40);
            MobileHouse house = new MobileHouse(
                "Дом на колёсах", 2, 2, true);

            Console.WriteLine("=== Игровые объекты ===");

            Console.WriteLine(
                $"{archer.Name} | id={archer.Id} | hp={archer.Hp} | ({archer.X}, {archer.Y})");

            Console.WriteLine(
                $"{fort.Name} | id={fort.Id} | built={fort.Built} | ({fort.X}, {fort.Y})");

            Console.WriteLine(
                $"{house.Name} | id={house.Id} | built={house.Built} | ({house.X}, {house.Y})");

            Console.WriteLine("\n=== Перемещение ===");

            archer.Move(3, 4);
            house.Move(7, 8);

            Console.WriteLine("\n=== Атака ===");

            archer.Attack(enemy);
            fort.Attack(enemy);

            Console.WriteLine(
                $"\nПосле атак: {enemy.Name} | hp={enemy.Hp} | alive={enemy.IsAlive()}");

            Console.WriteLine("\n=== Полиморфизм ===");

            IAttacker[] attackers = { archer, fort };

            foreach (IAttacker attacker in attackers)
            {
                attacker.Attack(enemy);
            }

            Console.WriteLine(
                $"\nПосле повторных атак: {enemy.Name} | hp={enemy.Hp} | alive={enemy.IsAlive()}");
        }
    }
}
