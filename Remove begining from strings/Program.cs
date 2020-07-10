using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using Leaf.xNet;
using Colorful;
using System.Windows.Forms;

namespace Remove_begining_from_strings
{
    class Program
    {

        public static string text;

        public static int totalCounter = 0;

        public static int checkedCounter = 0;

        public static int lineNumber = -1;

        public static int stopCount = 0;

        public static List<string> fileList = new List<string>();

        public static int CPM = 0;

        public static int CPM_aux = 0;

        public static int j = 0;

        public static string fileWrite;
        [STAThread]
        static void Main(string[] args)
        {
            Colorful.Console.WriteLine("How many charators do you want to remove?");
            j = Convert.ToInt32(Colorful.Console.ReadLine());
            string path = System.IO.Directory.GetCurrentDirectory();
            Colorful.Console.WriteLine("Select Your File", Color.GhostWhite);
            Thread.Sleep(500);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File List";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files|*.txt";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            string fileName = openFileDialog.FileName;
            Program.fileList = new List<string>(File.ReadAllLines(fileName));

            int maxDegreeOfParallelism = 1;


            string date3 = DateTime.Now.ToString("MM");
            string date2 = DateTime.Now.ToString("dd");
            string date1 = DateTime.Now.ToString("yyyy");
            string Path = Directory.GetCurrentDirectory();
            string time1 = DateTime.Now.ToString("HH");
            string time2 = DateTime.Now.ToString("mm");
            string time3 = DateTime.Now.ToString("ss");
            Directory.CreateDirectory(path + "\\Cleaned Files\\" + date1 + "\\" + date2 + "\\" + date3 + "\\" + time1 + "-" + time2 + "-" + time3);


            Task.Factory.StartNew(delegate ()
            {
                for (; ; )
                {
                    Program.CPM = Program.CPM_aux;
                    Program.CPM_aux = 0;
                    Colorful.Console.Title = string.Format("[Prefix Remover By : Bui] | Checked: {0}/{1} | CPM: ", new object[]
                    {
                        Program.checkedCounter,
                        Program.totalCounter,
                    }) + Program.CPM * 60;
                    Thread.Sleep(1000);
                }
            });
            Task.Factory.StartNew(delegate ()
            {
                while (Program.stopCount != maxDegreeOfParallelism && Program.stopCount - 1 != maxDegreeOfParallelism - 1)
                {
                    if (Program.fileWrite != "")
                    {
                        File.AppendAllText(@$"{path + "\\Cleaned Files\\" + date1 + "\\" + date2 + "\\" + date3 + "\\" + time1 + "-" + time2 + "-" + time3 + "\\"}Cleaned File.txt", Program.fileWrite);
                    }
                    Program.fileWrite = "";
                    Thread.Sleep(1500);
                }
                Colorful.Console.WriteLine("\n> Done.");
                Thread.Sleep(100000000);
                Environment.Exit(0);
            });


            while (!File.Exists(fileName)) ;
            Program.fileList = new List<string>(File.ReadAllLines(fileName));
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            Program.totalCounter++;
                        }
                    }
                }
            }

            for (int i = 1; i <= maxDegreeOfParallelism; i++)
            {
                new Thread(new ThreadStart(Program.PStart)).Start();
            }
        }

        public static void PStart()
        {
            for (; ; )
            {
                try
                {


                    Interlocked.Increment(ref Program.lineNumber);
                    string juy = Program.fileList[Program.lineNumber];
                    string text = juy.Substring(j + 1);
                    CheckProxy(text);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public static void CheckProxy(string line)
        {

                            Colorful.Console.WriteLine(string.Format("[+] {0}", line), Color.Lime);
                            Program.CPM_aux++;
                            Program.checkedCounter++;
                            Program.fileWrite = Program.fileWrite + string.Format("{0}\n", line);     
        }


        private static void CPM_Worker(object sender, ElapsedEventArgs e)
        {
            Program.CPM = Program.CPM_aux;
            Program.CPM_aux = 0;
        }
    }
}
