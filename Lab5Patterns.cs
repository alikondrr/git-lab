using System;

namespace Lab5Patterns
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №5: СТРУКТУРНЫЕ ПАТТЕРНЫ");

            // 1. PROXY
            Console.WriteLine("\n1. PROXY — доступ к игровому складу");

            IStorage playerStorage = new StorageProxy(false);
            IStorage adminStorage = new StorageProxy(true);

            playerStorage.ShowResources();
            adminStorage.ShowResources();

            // 2. ADAPTER
            Console.WriteLine("\n2. ADAPTER — подключение внешнего логгера");

            ExternalLogger externalLogger = new ExternalLogger();
            ILogger logger = new LoggerAdapter(externalLogger);
            logger.Log("Игрок вошёл в игру");

            // 3. BRIDGE
            Console.WriteLine("\n3. BRIDGE — вывод сообщений на разные устройства");

            IDevice screen = new ScreenDevice();
            IDevice file = new FileDevice();

            Message normalScreenMessage = new NormalMessage(screen);
            Message urgentScreenMessage = new UrgentMessage(screen);
            Message normalFileMessage = new NormalMessage(file);

            normalScreenMessage.Show("Добро пожаловать в игру");
            urgentScreenMessage.Show("Внимание! База атакована!");
            normalFileMessage.Show("Сохранение игры выполнено");
        }
    }

    //  PROXY 
    public interface IStorage
    {
        void ShowResources();
    }

    public class RealStorage : IStorage
    {
        public void ShowResources()
        {
            Console.WriteLine("Ресурсы склада: золото, дерево, камень.");
        }
    }

    public class StorageProxy : IStorage
    {
        private RealStorage _realStorage = new RealStorage();
        private bool _hasAccess;

        public StorageProxy(bool hasAccess)
        {
            _hasAccess = hasAccess;
        }

        public void ShowResources()
        {
            if (_hasAccess)
            {
                _realStorage.ShowResources();
            }
            else
            {
                Console.WriteLine("Доступ к складу запрещён.");
            }
        }
    }

    //  ADAPTER 
    public class ExternalLogger
    {
        public void WriteLog(string text)
        {
            Console.WriteLine("Внешний логгер: " + text);
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class LoggerAdapter : ILogger
    {
        private ExternalLogger _externalLogger;

        public LoggerAdapter(ExternalLogger externalLogger)
        {
            _externalLogger = externalLogger;
        }

        public void Log(string message)
        {
            _externalLogger.WriteLog(message);
        }
    }

    //  BRIDGE 
    public interface IDevice
    {
        void Print(string text);
    }

    public class ScreenDevice : IDevice
    {
        public void Print(string text)
        {
            Console.WriteLine("Экран: " + text);
        }
    }

    public class FileDevice : IDevice
    {
        public void Print(string text)
        {
            Console.WriteLine("Файл: " + text);
        }
    }

    public abstract class Message
    {
        protected IDevice device;

        public Message(IDevice device)
        {
            this.device = device;
        }

        public abstract void Show(string text);
    }

    public class NormalMessage : Message
    {
        public NormalMessage(IDevice device) : base(device) { }

        public override void Show(string text)
        {
            device.Print("Обычное сообщение: " + text);
        }
    }

    public class UrgentMessage : Message
    {
        public UrgentMessage(IDevice device) : base(device) { }

        public override void Show(string text)
        {
            device.Print("СРОЧНО: " + text);
        }
    }
}