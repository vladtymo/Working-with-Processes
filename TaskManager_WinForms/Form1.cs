using System.Diagnostics;

namespace TaskManager_WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var list = Process.GetProcesses().Select(x => new ProcessViewModel(x)).ToList();

            dataGridView1.DataSource = list;
        }
    }

    class ProcessViewModel
    {
        public int PID { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public long Memory { get; set; }

        public ProcessViewModel(Process x)
        {
            try
            {
                PID = x.Id;
                Name = x.ProcessName;
                Priority = x.PriorityClass.ToString();
                Memory = x.PrivateMemorySize64 / 1024;
            }
            catch {}
        }
    }
}