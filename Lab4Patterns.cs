using System;
using System.Collections.Generic;

namespace Lab4Patterns
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №4: ПОВЕДЕНЧЕСКИЕ ПАТТЕРНЫ");

            // 1. STRATEGY
            Console.WriteLine("\n1. STRATEGY — выбор способа атаки");
            Hero hero = new Hero();

            hero.SetAttackStrategy(new SwordAttack());
            hero.Attack();

            hero.SetAttackStrategy(new BowAttack());
            hero.Attack();

            hero.SetAttackStrategy(new MagicAttack());
            hero.Attack();

            // 2. CHAIN OF RESPONSIBILITY
            Console.WriteLine("\n2. CHAIN OF RESPONSIBILITY — проверка игрока");

            PlayerCheck nameCheck = new NameCheck();
            PlayerCheck levelCheck = new LevelCheck();
            PlayerCheck goldCheck = new GoldCheck();

            nameCheck.SetNext(levelCheck);
            levelCheck.SetNext(goldCheck);

            Player player1 = new Player("Алина", 5, 100);
            Player player2 = new Player("", 5, 100);
            Player player3 = new Player("Макс", 0, 100);
            Player player4 = new Player("Олег", 3, -10);

            Console.WriteLine("\nПроверка player1:");
            nameCheck.Handle(player1);

            Console.WriteLine("\nПроверка player2:");
            nameCheck.Handle(player2);

            Console.WriteLine("\nПроверка player3:");
            nameCheck.Handle(player3);

            Console.WriteLine("\nПроверка player4:");
            nameCheck.Handle(player4);

            // 3. ITERATOR
            Console.WriteLine("\n3. ITERATOR — перебор инвентаря");

            Inventory inventory = new Inventory();
            inventory.AddItem("Меч");
            inventory.AddItem("Щит");
            inventory.AddItem("Зелье");
            inventory.AddItem("Лук");

            IMyIterator iterator = inventory.CreateIterator();

            while (iterator.HasNext())
            {
                Console.WriteLine("Предмет: " + iterator.Next());
            }
        }
    }

    // STRATEGY 
    public interface IAttackStrategy
    {
        void Attack();
    }

    public class SwordAttack : IAttackStrategy
    {
        public void Attack()
        {
            Console.WriteLine("Герой атакует мечом.");
        }
    }

    public class BowAttack : IAttackStrategy
    {
        public void Attack()
        {
            Console.WriteLine("Герой стреляет из лука.");
        }
    }

    public class MagicAttack : IAttackStrategy
    {
        public void Attack()
        {
            Console.WriteLine("Герой использует магию.");
        }
    }

    public class Hero
    {
        private IAttackStrategy _attackStrategy;

        public void SetAttackStrategy(IAttackStrategy attackStrategy)
        {
            _attackStrategy = attackStrategy;
        }

        public void Attack()
        {
            if (_attackStrategy == null)
            {
                Console.WriteLine("Стратегия атаки не выбрана.");
                return;
            }
            _attackStrategy.Attack();
        }
    }

    // CHAIN OF RESPONSIBILITY 
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }

        public Player(string name, int level, int gold)
        {
            Name = name;
            Level = level;
            Gold = gold;
        }
    }

    public abstract class PlayerCheck
    {
        protected PlayerCheck Next;

        public void SetNext(PlayerCheck next)
        {
            Next = next;
        }

        public abstract void Handle(Player player);
    }

    public class NameCheck : PlayerCheck
    {
        public override void Handle(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Name))
            {
                Console.WriteLine("Ошибка: имя игрока не заполнено.");
                return;
            }

            Console.WriteLine("Проверка имени пройдена.");
            if (Next != null)
                Next.Handle(player);
        }
    }

    public class LevelCheck : PlayerCheck
    {
        public override void Handle(Player player)
        {
            if (player.Level <= 0)
            {
                Console.WriteLine("Ошибка: уровень игрока должен быть больше 0.");
                return;
            }

            Console.WriteLine("Проверка уровня пройдена.");
            if (Next != null)
                Next.Handle(player);
        }
    }

    public class GoldCheck : PlayerCheck
    {
        public override void Handle(Player player)
        {
            if (player.Gold < 0)
            {
                Console.WriteLine("Ошибка: количество золота не может быть отрицательным.");
                return;
            }

            Console.WriteLine("Проверка золота пройдена.");
            Console.WriteLine("Игрок успешно прошёл все проверки.");
        }
    }

    // ITERATOR 
    public interface IMyIterator
    {
        bool HasNext();
        string Next();
    }

    public class InventoryIterator : IMyIterator
    {
        private List<string> _items;
        private int _position = 0;

        public InventoryIterator(List<string> items)
        {
            _items = items;
        }

        public bool HasNext()
        {
            return _position < _items.Count;
        }

        public string Next()
        {
            if (!HasNext())
                throw new IndexOutOfRangeException();

            return _items[_position++];
        }
    }

    public class Inventory
    {
        private List<string> _items = new List<string>();

        public void AddItem(string item)
        {
            _items.Add(item);
        }

        public IMyIterator CreateIterator()
        {
            return new InventoryIterator(_items);
        }
    }
}