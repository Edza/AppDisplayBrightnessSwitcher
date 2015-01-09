using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDisplayBrightnessSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer t, t2;
        private int br;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {    
            t2 = new Timer(20000);
            t2.Elapsed += t2_Elapsed;
            t2.Start();
            t = new Timer(350);
            t.Elapsed += t_Elapsed;
            SetupTimer();
        }

        void t2_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetupTimer();
        }

        private void SetupTimer()
        {
            Process[] pname = Process.GetProcessesByName("devenv");
            if (pname.Length == 0)
                t.Stop();
            else
                t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            t.Stop();
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Title = Monitor.GetTopWindowName();
                ControlBrightness(this.Title);
                tb2.Text = br.ToString();
            }));
            t.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Brightness.SetBrightness(short.Parse(tb.Text));
        }

        void ControlBrightness(string title)
        {
            if (!title.Contains("devenv")) // we are in visual studio
            {
                if (br == 122)
                    return;

                 Brightness.SetBrightness(122);
                 br = 122;
            }
            else
            {
                if (br == 200)
                    return;

                Brightness.SetBrightness(200);
                br = 200;
            }
        }
    }
}
