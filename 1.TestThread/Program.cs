namespace _1.TestThread
{
    class Program
    {
        // Флаг для вказання потоку на скасування роботи
        private static volatile bool _isCanceled = false;
        public static void Main(String[] args)
        {
            GetSystemInfo();
            Thread yellowThread = new Thread(() => MyThreadYellowMethod(8));
            yellowThread.Start();

            //основний потік - по суті це потік, який відобрається користувачу.
            int threadId = Thread.CurrentThread.ManagedThreadId; //ідентифікатор потоку
            Console.WriteLine("Thread Main id {0}", threadId);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("--Main working Thread{0}-- {1}", threadId, i+1);
                Thread.Sleep(400);
            }

            _isCanceled = true;
            yellowThread.Join(); //очікуємо завершення потоку yellow
            //процес - основна програма
            Console.WriteLine("Програма завершила свою роботу...");
        }

        public static void GetSystemInfo()
        {
            int coreCount = Environment.ProcessorCount;
            Console.WriteLine($"Кількість доступних процесорних ядер: {coreCount}");
        }

        public static void MyThreadYellowMethod(int n)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId; //ідентифікатор потоку
            for (int i=0; i<n; i++)
            {
                if (_isCanceled)
                {
                    Console.WriteLine("---Thread break----");
                    break;
                }
                var colorDefault = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                // код для виконання в потоці
                Console.WriteLine("--Thread working Yellow Thread{0}--{1}",threadId, i+1);
                Console.ForegroundColor = colorDefault;
                Thread.Sleep(500);
            }

        }
    }
}


