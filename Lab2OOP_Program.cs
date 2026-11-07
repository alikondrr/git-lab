using System;

namespace Lab2OOP
{
    public abstract class GameObject
    {
        private static int _nextId = 1;

        private readonly int _id;
        private readonly string _name;
        private int _x;
        private int _y;

        protected GameObject(string name, int x, int y)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым", nameof(name));

            _id = _nextId++;
            _name = name;
            _x = x;
            _y = y;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        protected void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
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
        private float _hp;

        protected Unit(string name, int x, int y, float hp)
            : base(name, x, y)
        {
            if (float.IsNaN(hp) || float.IsInfinity(hp) || hp < 0)
                throw new ArgumentOutOfRangeException(nameof(hp));

            _hp = hp;
        }

        public bool IsAlive()
        {
            return _hp > 0;
        }

        public float GetHp()
        {
            return _hp;
        }

        public virtual void ReceiveDamage(float damage)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _hp = Math.Max(0f, _hp - damage);
        }
    }

    public abstract class Building : GameObject
    {
        private readonly bool _built;

        protected Building(string name, int x, int y, bool built)
            : base(name, x, y)
        {
            _built = built;
        }

        public bool IsBuilt()
        {
            return _built;
        }
    }

    public class Archer : Unit, IAttacker, IMoveable
    {
        private readonly float _damage;

        public Archer(string name, int x, int y, float hp, float damage)
            : base(name, x, y, hp)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            if (!IsAlive())
            {
                Console.WriteLine($"{GetName()} мёртв и не может атаковать.");
                return;
            }

            if (!unit.IsAlive())
            {
                Console.WriteLine($"{unit.GetName()} уже мёртв.");
                return;
            }

            Console.WriteLine(
                $"{GetName()} атакует {unit.GetName()} и наносит {_damage} урона.");

            unit.ReceiveDamage(_damage);
        }

        public void Move(int x, int y)
        {
            if (!IsAlive())
            {
                Console.WriteLine($"{GetName()} мёртв и не может двигаться.");
                return;
            }

            Console.WriteLine(
                $"{GetName()} перемещается из ({GetX()}, {GetY()}) в ({x}, {y}).");

            SetPosition(x, y);
        }
    }

    public class Fort : Building, IAttacker
    {
        private readonly float _damage;

        public Fort(string name, int x, int y, bool built, float damage)
            : base(name, x, y, built)
        {
            if (float.IsNaN(damage) || float.IsInfinity(damage) || damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _damage = damage;
        }

        public void Attack(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            if (!IsBuilt())
            {
                Console.WriteLine($"{GetName()} ещё не построена и не может атаковать.");
                return;
            }

            if (!unit.IsAlive())
            {
                Console.WriteLine($"{unit.GetName()} уже мёртв.");
                return;
            }

            Console.WriteLine(
                $"{GetName()} стреляет по {unit.GetName()} и наносит {_damage} урона.");

            unit.ReceiveDamage(_damage);
        }
    }

    public class MobileHome : Building, IMoveable
    {
        public MobileHome(string name, int x, int y, bool built)
            : base(name, x, y, built)
        {
        }

        public void Move(int x, int y)
        {
            if (!IsBuilt())
            {
                Console.WriteLine($"{GetName()} ещё не построен и не может двигаться.");
                return;
            }

            Console.WriteLine(
                $"{GetName()} едет из ({GetX()}, {GetY()}) в ({x}, {y}).");

            SetPosition(x, y);
        }
    }

    public class Program
    {
        public static void Main()
        {
            Archer archer = new Archer("Лучник", 0, 0, 100, 15);
            Archer enemy = new Archer("Вражеский лучник", 5, 5, 80, 10);
            Fort fort = new Fort("Крепость", 10, 10, true, 40);
            MobileHome house = new MobileHome("Дом на колёсах", 2, 2, true);

            Console.WriteLine("=== Игровые объекты ===");

            Console.WriteLine(
                $"{archer.GetName()} | id={archer.GetId()} | hp={archer.GetHp()} | ({archer.GetX()}, {archer.GetY()})");

            Console.WriteLine(
                $"{fort.GetName()} | id={fort.GetId()} | built={fort.IsBuilt()} | ({fort.GetX()}, {fort.GetY()})");

            Console.WriteLine(
                $"{house.GetName()} | id={house.GetId()} | built={house.IsBuilt()} | ({house.GetX()}, {house.GetY()})");

            Console.WriteLine("\n=== Перемещение ===");

            archer.Move(3, 4);
            house.Move(7, 8);

            Console.WriteLine("\n=== Атака ===");

            archer.Attack(enemy);
            fort.Attack(enemy);

            Console.WriteLine(
                $"\nПосле атак: {enemy.GetName()} | hp={enemy.GetHp()} | alive={enemy.IsAlive()}");

            Console.WriteLine("\n=== Полиморфизм ===");

            IAttacker[] attackers = { archer, fort };

            foreach (IAttacker attacker in attackers)
            {
                attacker.Attack(enemy);
            }

            Console.WriteLine(
                $"\nПосле повторных атак: {enemy.GetName()} | hp={enemy.GetHp()} | alive={enemy.IsAlive()}");
        }
    }
}
