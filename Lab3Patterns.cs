using System;

namespace lab3
{

    // singleton
    public class GameLogger
    {
        private static GameLogger _instance;

        private GameLogger() { }

        public static GameLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameLogger();
            }

            return _instance;
        }

        public void Log(string text)
        {
            Console.WriteLine("[LOG] " + text);
        }
    }


    // factory method
    public interface IUnit
    {
        void ShowInfo();
    }

    public class Archer : IUnit
    {
        public void ShowInfo()
        {
            Console.WriteLine("Лучник готов к бою");
        }
    }

    public class Soldier : IUnit
    {
        public void ShowInfo()
        {
            Console.WriteLine("Солдат готов к бою");
        }
    }

    public abstract class UnitFactory
    {
        public abstract IUnit CreateUnit();

        public void SpawnUnit()
        {
            IUnit unit = CreateUnit();
            unit.ShowInfo();
        }
    }

    public class ArcherFactory : UnitFactory
    {
        public override IUnit CreateUnit()
        {
            return new Archer();
        }
    }

    public class SoldierFactory : UnitFactory
    {
        public override IUnit CreateUnit()
        {
            return new Soldier();
        }
    }


    // abstract factory
    public interface ITerrain
    {
        void ShowTerrain();
    }

    public interface IDecoration
    {
        void ShowDecoration();
    }

    public class ForestTerrain : ITerrain
    {
        public void ShowTerrain()
        {
            Console.WriteLine("Лесная местность");
        }
    }

    public class DesertTerrain : ITerrain
    {
        public void ShowTerrain()
        {
            Console.WriteLine("Пустынная местность");
        }
    }

    public class Tree : IDecoration
    {
        public void ShowDecoration()
        {
            Console.WriteLine("На карте растут деревья");
        }
    }

    public class Cactus : IDecoration
    {
        public void ShowDecoration()
        {
            Console.WriteLine("На карте стоят кактусы");
        }
    }

    public interface IMapFactory
    {
        ITerrain CreateTerrain();
        IDecoration CreateDecoration();
    }

    public class ForestFactory : IMapFactory
    {
        public ITerrain CreateTerrain()
        {
            return new ForestTerrain();
        }

        public IDecoration CreateDecoration()
        {
            return new Tree();
        }
    }

    public class DesertFactory : IMapFactory
    {
        public ITerrain CreateTerrain()
        {
            return new DesertTerrain();
        }

        public IDecoration CreateDecoration()
        {
            return new Cactus();
        }
    }

    // builder
    public class Character
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Weapon { get; set; }
        public int Hp { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine(Name + " | " + Role +
                " | " + Weapon + " | HP: " + Hp);
        }
    }

    public class CharacterBuilder
    {
        private Character _character = new Character();

        public CharacterBuilder SetName(string name)
        {
            _character.Name = name;
            return this;
        }


        public CharacterBuilder SetRole(string role)
        {
            _character.Role = role;
            return this;
        }


        public CharacterBuilder SetWeapon(string weapon)
        {
            _character.Weapon = weapon;
            return this;
        }


        public CharacterBuilder SetHp(int hp)
        {
            _character.Hp = hp;
            return this;
        }

        public Character Build()
        {
            Character result = _character;
            _character = new Character();
            return result;
        }
    }

    public class Program
    {

        public static void Main()
        {
            // singleton
            GameLogger myLogger = GameLogger.GetInstance();
            myLogger.Log("Игра началась");

            GameLogger log2 = GameLogger.GetInstance();
            Console.WriteLine("Один объект: " + (myLogger == log2));

            // factory method
            UnitFactory factory = new ArcherFactory();
            factory.SpawnUnit();

            factory = new SoldierFactory();
            factory.SpawnUnit();

            // abstract factory
            IMapFactory map = new ForestFactory();

            ITerrain terrain = map.CreateTerrain();
            IDecoration decor = map.CreateDecoration();

            terrain.ShowTerrain();
            decor.ShowDecoration();

            map = new DesertFactory();

            terrain = map.CreateTerrain();
            decor = map.CreateDecoration();

            terrain.ShowTerrain();
            decor.ShowDecoration();

            // builder
            CharacterBuilder builder = new CharacterBuilder();

            Character player1 = builder
                .SetName("Алекс")
                .SetRole("Воин")
                .SetWeapon("Меч")
                .SetHp(100)
                .Build();

            player1.ShowInfo();

            Character player2 = builder
                .SetName("Макс")
                .SetRole("Лучник")
                .SetWeapon("Лук")
                .SetHp(70)
                .Build();

            player2.ShowInfo();
        }


    }

}
