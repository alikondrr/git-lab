using System;
using System.Collections.Generic;

namespace lab4
{
    // стратегия
    public interface IAttackStrategy
    {
        void Attack();
    }

    public class SwordAttack : IAttackStrategy
    {
        public void Attack()
        {
            Console.WriteLine("Атака мечом");
        }
    }

    public class BowAttack : IAttackStrategy
    {
        public void Attack()
        {
            Console.WriteLine("Атака из лука");
        }
    }

    public class Player
    {
        private IAttackStrategy _strategy;

        public Player(IAttackStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IAttackStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Attack()
        {
            _strategy.Attack();
        }
    }


    // цепочка обязанностей
    public abstract class EventHandler
    {
        protected EventHandler _next;

        public void SetNext(EventHandler next)
        {
            _next = next;
        }

        public abstract void Handle(string eventType);
    }

    public class BattleHandler : EventHandler
    {
        public override void Handle(string eventType)
        {
            if (eventType == "battle")
            {
                Console.WriteLine("Начался бой");
            }
            else if (_next != null)
            {
                _next.Handle(eventType);
            }
        }
    }

    public class TradeHandler : EventHandler
    {
        public override void Handle(string eventType)
        {
            if (eventType == "trade")
            {
                Console.WriteLine("Открыта торговля");
            }
            else if (_next != null)
            {
                _next.Handle(eventType);
            }
        }
    }

    public class QuestHandler : EventHandler
    {
        public override void Handle(string eventType)
        {
            if (eventType == "quest")
            {
                Console.WriteLine("Получено новое задание");
            }
            else if (_next != null)
            {
                _next.Handle(eventType);
            }
        }
    }


    // итератор
    public interface IGameIterator
    {
        bool HasNext();
        string Next();
    }

    public class InventoryIterator : IGameIterator
    {
        private List<string> _items;
        private int _pos;

        public InventoryIterator(List<string> items)
        {
            _items = items;
            _pos = 0;
        }

        public bool HasNext()
        {
            return _pos < _items.Count;
        }

        public string Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("Предметы закончились");
            }

            string item = _items[_pos];
            _pos++;
            return item;
        }
    }

    public class Inventory
    {
        private List<string> _items = new List<string>();

        public void AddItem(string item)
        {
            _items.Add(item);
        }

        public IGameIterator CreateIterator()
        {
            return new InventoryIterator(_items);
        }
    }

    public class Program
    {
        public static void Main()
        {

            // стратегия
            Player player1 = new Player(new SwordAttack());
            player1.Attack();

            player1.SetStrategy(new BowAttack());
            player1.Attack();


            // цепочка обязанностей
            EventHandler battle = new BattleHandler();
            EventHandler trade = new TradeHandler();
            EventHandler quest = new QuestHandler();

            battle.SetNext(trade);
            trade.SetNext(quest);

            battle.Handle("battle");
            battle.Handle("trade");
            battle.Handle("quest");


            // итератор
            Inventory bag = new Inventory();

            bag.AddItem("Меч");
            bag.AddItem("Зелье");
            bag.AddItem("Ключ");

            IGameIterator iter = bag.CreateIterator();

            while (iter.HasNext())
            {
                Console.WriteLine(iter.Next());
            }
        }

    }


}