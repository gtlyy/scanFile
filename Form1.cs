using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace scanFile
{
    public partial class Form1 : Form
    {
        static int num = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public void FindFile(string url)
        {
            DirectoryInfo directory;
            // 打开 “百度网盘同步空间”时出错！略过。
            try {
                directory = new DirectoryInfo(url);
            }
            catch(Exception e)
            {
                textBox1.Text = "Error: new DirectoryInfo()" + "\r\n" + e.ToString() + "\r\n\r\n" + "请重新选择文件夹";
                return;
            }
            // 首先判断文件夹名是否过长
            if (System.Text.Encoding.UTF8.GetByteCount(directory.Name) > 255)
            {
                num += 1;
                textBox1.Text += num.ToString() + "【Dir】 : " + directory.FullName + System.Environment.NewLine + System.Environment.NewLine;
            }

            // 打开虚拟机的共享文件夹时，linux下文件夹可以有空格结尾，但windows明显不行。
            try { 
                var files = directory.GetFiles();
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        // 判断文件名是否过长
                        int lenght1 = System.Text.Encoding.UTF8.GetByteCount(file.Name);
                        if (lenght1 > 255)
                        {
                            num += 1;
                            textBox1.Text += num.ToString() + "【File】 : " + file.FullName + System.Environment.NewLine + System.Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                textBox1.Text = "Error: directory.GetFiles()" + "\r\n" + e.ToString() + "\r\n\r\n" + "请重新选择文件夹";
                return;
            }

            var dirs = directory.GetDirectories();
            if (dirs.Any())
            {
                foreach (var dir in dirs)
                {
                    FindFile(dir.FullName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = folderBrowserDialog1.SelectedPath;
                    num = 0;
                    textBox1.Text = "";
                    label2.Text = "";
                    FindFile(textBox2.Text);
                    label2.Text = "名称过长的文件（夹）有 " + num.ToString() + " 个：";
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "Error: button1_Click()" + "\r\n" + ex.ToString() + "\r\n\r\n" + "请重新选择文件夹";
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
