using System;
using System.Collections.Generic;

namespace Lab3Patterns
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №3: ПОРОЖДАЮЩИЕ ПАТТЕРНЫ");

            Console.WriteLine("\n1. SINGLETON — единый игровой журнал");
            GameLogger logger1 = GameLogger.GetInstance();
            GameLogger logger2 = GameLogger.GetInstance();
            logger1.Log("Игра началась");
            Console.WriteLine($"Обе ссылки указывают на один объект: {ReferenceEquals(logger1, logger2)}");

            Console.WriteLine("\n2. FACTORY METHOD — создание игровых юнитов");
            UnitFactory archerFactory = new ArcherFactory();
            UnitFactory soldierFactory = new SoldierFactory();
            Console.WriteLine(archerFactory.CreateUnit("Эйра").Describe());
            Console.WriteLine(soldierFactory.CreateUnit("Бран").Describe());

            Console.WriteLine("\n3. ABSTRACT FACTORY — комплекты объектов карты");
            IMapFactory forestFactory = new ForestFactory();
            IMapFactory desertFactory = new DesertFactory();
            Console.WriteLine("Лесная карта: " + forestFactory.CreateTerrain().GetDescription() + "; " + forestFactory.CreateDecoration().GetDescription());
            Console.WriteLine("Пустынная карта: " + desertFactory.CreateTerrain().GetDescription() + "; " + desertFactory.CreateDecoration().GetDescription());

            Console.WriteLine("\n4. BUILDER — пошаговая настройка персонажа");
            CharacterBuilder builder = new CharacterBuilder();
            Character warrior = builder.SetName("Рыцарь").SetRole("Воин").SetWeapon("Меч").SetHealth(150).Build();
            Character scout = builder.SetName("Следопыт").SetRole("Разведчик").SetWeapon("Лук").SetHealth(80).Build();
            Console.WriteLine(warrior);
            Console.WriteLine(scout);
        }
    }

    public class GameLogger
    {
        private static GameLogger _instance;
        private List<string> _logs = new List<string>();

        private GameLogger() { }

        public static GameLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameLogger();
            }
            return _instance;
        }

        public void Log(string message)
        {
            _logs.Add(message);
            Console.WriteLine($"[LOG] {message}");
        }
    }

    public interface IUnit
    {
        string Describe();
    }

    public class Archer : IUnit
    {
        public string Name { get; set; }
        public Archer(string name) { Name = name; }
        public string Describe() => $"Лучник {Name}, здоровье 75";
    }

    public class Soldier : IUnit
    {
        public string Name { get; set; }
        public Soldier(string name) { Name = name; }
        public string Describe() => $"Пехотинец {Name}, здоровье 120";
    }

    public abstract class UnitFactory
    {
        public abstract IUnit CreateUnit(string name);
    }

    public class ArcherFactory : UnitFactory
    {
        public override IUnit CreateUnit(string name) => new Archer(name);
    }

    public class SoldierFactory : UnitFactory
    {
        public override IUnit CreateUnit(string name) => new Soldier(name);
    }

    public interface ITerrain { string GetDescription(); }
    public interface IDecoration { string GetDescription(); }

    public class ForestTerrain : ITerrain { public string GetDescription() => "лесная местность"; }
    public class ForestDecoration : IDecoration { public string GetDescription() => "деревья и кусты"; }
    public class DesertTerrain : ITerrain { public string GetDescription() => "песчаная местность"; }
    public class DesertDecoration : IDecoration { public string GetDescription() => "кактусы и скалы"; }

    public interface IMapFactory
    {
        ITerrain CreateTerrain();
        IDecoration CreateDecoration();
    }

    public class ForestFactory : IMapFactory
    {
        public ITerrain CreateTerrain() => new ForestTerrain();
        public IDecoration CreateDecoration() => new ForestDecoration();
    }

    public class DesertFactory : IMapFactory
    {
        public ITerrain CreateTerrain() => new DesertTerrain();
        public IDecoration CreateDecoration() => new DesertDecoration();
    }

    public class Character
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Weapon { get; set; }
        public int Health { get; set; }

        public override string ToString()
        {
            return $"{Role} {Name}: оружие {Weapon}, здоровье {Health}";
        }
    }

    public class CharacterBuilder
    {
        private Character _character = new Character();

        public CharacterBuilder SetName(string name) { _character.Name = name; return this; }
        public CharacterBuilder SetRole(string role) { _character.Role = role; return this; }
        public CharacterBuilder SetWeapon(string weapon) { _character.Weapon = weapon; return this; }
        public CharacterBuilder SetHealth(int health) { _character.Health = health; return this; }

        public Character Build()
        {
            Character result = _character;
            _character = new Character();
            return result;
        }
    }
}