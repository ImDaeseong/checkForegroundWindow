using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Win32 API 선언
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private System.Windows.Forms.Timer timer;

        //최대 로그 개수
        private const int MaxLogCount = 100;

        public Form1()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 500; 
            timer.Tick += Timer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string log;
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero)
            {
                //프로세스 ID
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);

                //윈도우 제목
                int length = GetWindowTextLength(hWnd);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(hWnd, sb, sb.Capacity);

                string windowTitle = sb.ToString();
                log = $"[{DateTime.Now:HH:mm:ss}] {windowTitle} (PID: {processId})";
            }
            else
            {
                log = $"[{DateTime.Now:HH:mm:ss}] 포그라운드 윈도우 없음";
            }

            listBox1.Items.Add(log);

            // 로그 개수 제한
            while (listBox1.Items.Count > MaxLogCount)
            {
                //오래된 로그 제거
                listBox1.Items.RemoveAt(0); 
            }

            //최신 로그가 보이도록 스크롤 이동
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

    }
}
