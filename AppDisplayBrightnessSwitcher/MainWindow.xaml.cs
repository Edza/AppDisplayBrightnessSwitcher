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

        static byte GetBrightness()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            //store result
            byte curBrightness = 0;
            foreach (System.Management.ManagementObject o in moc)
            {
                curBrightness = (byte)o.GetPropertyValue("CurrentBrightness");
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();

            return curBrightness;
        }

        void ControlBrightness(string title)
        {
            if (!title.Contains("devenv")) // we are in visual studio
            {
                if (br == 122)
                    return;

                byte b = GetBrightness();

                 Brightness.SetBrightness(122);
                SetBrightness((byte)Math.Min(100, (int)b - 9));
                 br = 122;
            }
            else
            {
                if (br == 200)
                    return;

                byte b = GetBrightness();

                SetBrightness((byte)Math.Max(0, (int)b + 9));
                Brightness.SetBrightness(180);
                br = 200;
            }
        }

        static byte[] GetBrightnessLevels()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);
            byte[] BrightnessLevels = new byte[0];

            try
            {
                System.Management.ManagementObjectCollection moc = mos.Get();

                //store result


                foreach (System.Management.ManagementObject o in moc)
                {
                    BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();

            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, Your System does not support this brightness control...");

            }

            return BrightnessLevels;
        }

        static void SetBrightness(byte targetBrightness)
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            foreach (System.Management.ManagementObject o in moc)
            {
                o.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();
        }
    }
}
