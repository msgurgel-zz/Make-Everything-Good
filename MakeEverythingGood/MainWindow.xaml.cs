using System;
using System.Diagnostics;
using System.Windows;
using System.ServiceProcess;

namespace MakeEverythingGood
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private readonly string[] _processes = {
            "Wacom_Tablet",
            "Wacom_TouchUser",
            "Wacom_TabletUser",
            "WacomHost",
            "WTabletServicePro",
        };

        private const string ServiceName = "WTabletServicePro";
        private const int TimeoutMilliseconds = 15000;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseProcesses();
                StartService();

                MessageBox.Show("Everything is now good!", "We did it", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private static void StartService()
        {
            ServiceController service = new ServiceController(ServiceName);
            TimeSpan timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }


        private void CloseProcesses()
        {
            foreach (string processName in _processes)
            {
                foreach (var proc in Process.GetProcessesByName(processName))
                {
                    proc.Kill();
                }
            }
        }
    }
}
