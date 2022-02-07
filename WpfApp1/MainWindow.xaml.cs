using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer = null;
        IList<ProcessInfo> processes = null;

        public MainWindow()
        {
            InitializeComponent();

            InitialLoad();

            grid.ItemsSource = processes;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 2);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void InitialLoad()
        {
            var result = Process.GetProcesses().Select(p => new ProcessInfo(p));
            processes = new ObservableCollection<ProcessInfo>(result);
        }
        public void RefreshList()
        {
            var currentList = Process.GetProcesses().Select(p => new ProcessInfo(p));

            foreach (var pr in currentList)
            {
                var item = processes.FirstOrDefault(p => p.Id == pr.Id);
                if (item != null)
                {
                    item.SetFields(pr);
                }
                else
                {
                    processes.Add(pr);
                }
            }

            // TODO: remove closed processes
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }
    }

    [AddINotifyPropertyChangedInterface]
    class ProcessInfo
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public ProcessPriorityClass PriorityClass { get; set; }
        public string UserName { get; set; }

        public ProcessInfo(Process pr)
        {
            Id = pr.Id;
            try { ProcessName = pr.ProcessName; } catch { }
            try { TotalProcessorTime = pr.TotalProcessorTime; } catch { }
            try { PriorityClass = pr.PriorityClass; } catch { }
            UserName = "Null"; //UserName = GetProcessOwner(pr.Id);
        }

        public void SetFields(ProcessInfo pr)
        {
            this.ProcessName = pr.ProcessName;
            this.TotalProcessorTime = pr.TotalProcessorTime;
            this.PriorityClass = pr.PriorityClass;
        }

        public string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
