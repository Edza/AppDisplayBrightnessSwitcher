using System;
using System.Collections.Generic;
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
        Timer t;
        private int br;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            t = new Timer(350);
            t.Elapsed += t_Elapsed;
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
                if (br == 95)
                    return;

                 Brightness.SetBrightness(115);
                 br = 95;
            }
            else
            {
                if (br == 200)
                    return;

                Brightness.SetBrightness(195);
                br = 200;
            }
        }
    }
}
