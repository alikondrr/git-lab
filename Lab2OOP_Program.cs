namespace лаба_2
{
    using System;

    public abstract class GameObject
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Name { get; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public GameObject(string name, int x, int y)
        {
            Id = _nextId++;
            Name = name;
            X = x;
            Y = y;
        }

        public int GetId() => Id;
        public string GetName() => Name;
        public int GetX() => X;
        public int GetY() => Y;
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
        public float Hp { get; protected set; }

        public Unit(string name, int x, int y, float hp) : base(name, x, y)
        {
            Hp = hp;
        }

        public bool IsAlive() => Hp > 0;
        public float GetHp() => Hp;

        public virtual void ReceiveDamage(float damage)
        {
            if (damage < 0)
            {
                throw new ArgumentException("Урон не может быть отрицательным");
            }

            Hp -= damage;

            if (Hp < 0)
            {
                Hp = 0;
            }
        }
    }

    public abstract class Building : GameObject
    {
        public bool Built { get; protected set; }

        public Building(string name, int x, int y, bool built) : base(name, x, y)
        {
            Built = built;
        }

        public bool IsBuilt() => Built;
    }

    public class Archer : Unit, IAttacker, IMoveable
    {
        public float Damage { get; }

        public Archer(string name, int x, int y, float hp, float damage)
            : base(name, x, y, hp)
        {
            Damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (!IsAlive())
            {
                Console.WriteLine($"{Name} мёртв и не может атаковать.");
                return;
            }

            Console.WriteLine($"{Name} атакует {unit.GetName()} и наносит {Damage} урона.");
            unit.ReceiveDamage(Damage);
        }

        public void Move(int x, int y)
        {
            Console.WriteLine($"{Name} перемещается из ({X}, {Y}) в ({x}, {y}).");
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
            Damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (!IsBuilt())
            {
                Console.WriteLine($"{Name} ещё не построена и не может атаковать.");
                return;
            }

            Console.WriteLine($"{Name} стреляет по {unit.GetName()} и наносит {Damage} урона.");
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
            if (!IsBuilt())
            {
                Console.WriteLine($"{Name} ещё не построен и не может двигаться.");
                return;
            }

            Console.WriteLine($"{Name} едет из ({X}, {Y}) в ({x}, {y}).");
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
            MobileHouse house = new MobileHouse("Дом на колёсах", 2, 2, true);

            Console.WriteLine("=== Игровые объекты ===");
            Console.WriteLine($"{archer.GetName()} | id={archer.GetId()} | hp={archer.GetHp()} | ({archer.GetX()}, {archer.GetY()})");
            Console.WriteLine($"{fort.GetName()} | id={fort.GetId()} | built={fort.IsBuilt()} | ({fort.GetX()}, {fort.GetY()})");
            Console.WriteLine($"{house.GetName()} | id={house.GetId()} | built={house.IsBuilt()} | ({house.GetX()}, {house.GetY()})");

            Console.WriteLine("\n=== Перемещение ===");
            archer.Move(3, 4);
            house.Move(7, 8);

            Console.WriteLine("\n=== Атака ===");
            archer.Attack(enemy);
            fort.Attack(enemy);

            Console.WriteLine($"\nПосле атак: {enemy.GetName()} | hp={enemy.GetHp()} | alive={enemy.IsAlive()}");

            Console.WriteLine("\n=== Полиморфизм ===");
            IAttacker[] attackers = { archer, fort };

            foreach (IAttacker attacker in attackers)
            {
                attacker.Attack(enemy);
            }

            Console.WriteLine($"\nПосле повторных атак: {enemy.GetName()} | hp={enemy.GetHp()} | alive={enemy.IsAlive()}");
        }
    }

}
