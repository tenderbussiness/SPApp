using Infrastraction.Services;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppThread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource ctSource;
        private CancellationToken cancelletaionToken;

        //маємо можливість блокувати роботу потоку.
        private static ManualResetEvent _manualEvent = new(false); // Initialize as unsignaled
        private bool _isPause = false;
        Thread thread;


        public MainWindow()
        {
            InitializeComponent();

        }

        private async void btnRun_Click(object sender, RoutedEventArgs e)
        {
            ctSource = new CancellationTokenSource();
            cancelletaionToken = ctSource.Token;
            //MessageBox.Show("Add items " + txtCount.Text);
            btnRun.IsEnabled = false;
            double count = double.Parse(txtCount.Text);

            //thread = new Thread(() =>
            //    InsertItems(count));

            //thread.Start(); //стартуємо вториний потік, який додає користувачів в БД
            UserService userService = new UserService(cancelletaionToken);
            userService.InsertUserEvent += UserService_InsertUserEvent;

            pbStatus.Maximum = count;
            btnRun.IsEnabled = true;
            //Запускаємо, але потік поки не блокуємо.
            _manualEvent.Set();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //виходимо із методу у вториний потік, але запамятовуємо точну повернення.
            await userService.InsertRnadomUserAsync((int)count);
            //повертаємося, яколи асихнроний метод відпрацював.

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            MessageBox.Show("RunTime " + elapsedTime);

        }

        private void UserService_InsertUserEvent(int count)
        {
            Dispatcher.Invoke(() => { pbStatus.Value = count; });
            _manualEvent.WaitOne(Timeout.Infinite); //якщо потік заблоковано, то очікуємо
            //поки його буде розблоковано
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if(_isPause)
            {
                _manualEvent.Set(); // розблоковуємо потік
                btnPause.Content = "Призупинити";
            }
            else
            {
                _manualEvent.Reset(); //лочимо потік
                btnPause.Content = "Продовжити";
            }

            _isPause = !_isPause;
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e)
        {
            
            ctSource.Cancel();
            btnRun.IsEnabled = true;
            pbStatus.Value = 0;
        }

        private async void btnRun_Copy_Click(object sender, RoutedEventArgs e)
        {
            ctSource = new CancellationTokenSource();
            cancelletaionToken = ctSource.Token;
            //MessageBox.Show("Add items " + txtCount.Text);
            btnRun_Copy.IsEnabled = false;
            double count = double.Parse(txtCount.Text);
            UserService userService = new UserService(cancelletaionToken);
            userService.InsertUserEvent += UserService_InsertUserEvent;

            pbStatus.Maximum = count;
            btnRun_Copy.IsEnabled = true;
            _manualEvent.Set();


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            await userService.InsertDapperRandomUserAsync((int)count);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            MessageBox.Show("RunTime " + elapsedTime);
        }
    }
}