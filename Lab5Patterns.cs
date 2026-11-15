using System;

namespace lab5
{

    // прокси
    public interface IChest
    {
        void Open();
    }

    public class Chest : IChest
    {
        public void Open()
        {
            Console.WriteLine("Сундук открыт. Получено золото");
        }
    }

    public class ChestProxy : IChest
    {
        private Chest _chest;
        private bool _hasKey;

        public ChestProxy(bool hasKey)
        {
            _hasKey = hasKey;
        }

        public void Open()
        {
            if (!_hasKey)
            {
                Console.WriteLine("Нужен ключ");
                return;
            }

            if (_chest == null)
            {
                _chest = new Chest();
            }

            _chest.Open();
        }
    }


    // адаптер
    public interface IGameSound
    {
        void PlaySound(string name);
    }

    public class OldSoundSystem
    {
        public void StartAudio(string file)
        {
            Console.WriteLine("Звук: " + file);
        }
    }

    public class SoundAdapter : IGameSound
    {
        private OldSoundSystem _sound;

        public SoundAdapter(OldSoundSystem sound)
        {
            _sound = sound;
        }

        public void PlaySound(string name)
        {
            _sound.StartAudio(name);
        }
    }


    // мост
    public interface IDamageType
    {
        void DealDamage(int damage);
    }

    public class PhysicalDamage : IDamageType
    {
        public void DealDamage(int damage)
        {
            Console.WriteLine("Физический урон: " + damage);
        }
    }

    public class MagicDamage : IDamageType
    {
        public void DealDamage(int damage)
        {
            Console.WriteLine("Магический урон: " + damage);
        }
    }

    public abstract class Weapon
    {
        protected IDamageType _damageType;

        public Weapon(IDamageType damageType)
        {
            _damageType = damageType;
        }

        public abstract void Attack();
    }

    public class Sword : Weapon
    {
        public Sword(IDamageType damageType) : base(damageType)
        {
        }

        public override void Attack()
        {
            Console.WriteLine("Удар мечом");
            _damageType.DealDamage(20);
        }
    }

    public class Bow : Weapon
    {
        public Bow(IDamageType damageType) : base(damageType)
        {
        }

        public override void Attack()
        {
            Console.WriteLine("Выстрел из лука");
            _damageType.DealDamage(15);
        }
    }


    public class Program
    {

        public static void Main()
        {

            // прокси
            IChest chest1 = new ChestProxy(false);
            IChest chest2 = new ChestProxy(true);

            chest1.Open();
            chest2.Open();


            // адаптер
            OldSoundSystem oldSound = new OldSoundSystem();
            IGameSound sound = new SoundAdapter(oldSound);

            sound.PlaySound("battle.mp3");


            // мост
            IDamageType physical = new PhysicalDamage();
            IDamageType magic = new MagicDamage();

            Weapon sword = new Sword(physical);
            Weapon magicSword = new Sword(magic);
            Weapon bow = new Bow(physical);


            sword.Attack();
            magicSword.Attack();
            bow.Attack();
        }

    }


}

