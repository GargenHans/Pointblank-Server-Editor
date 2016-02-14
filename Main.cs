using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Pointblank_Server_Editor
{
    public partial class Main : Form
    {
        public string FilePath;
        private byte[] DemonBuffer;
        private  System.Drawing.Point NewPoint;
        private int X, Y;
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                string str = fileName;
                FilePath = fileName;
                DemonBuffer = File.ReadAllBytes(openFileDialog.FileName);
                DoJob();
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }
        private void DoJob()
        {
            for (int i = 1; i <= 5; i++)
            {
                byte num = DecryptServer(DemonBuffer, (int)DemonBuffer.Length, i);
                Array.Resize<byte>(ref DemonBuffer, (int)DemonBuffer.Length + 1);
                DemonBuffer[(int)DemonBuffer.Length - 1] = num;
            }
            textBox1.Text = Encoding.GetEncoding(1251).GetString(DemonBuffer);
        }

        public static byte DecryptServer(byte[] a1, int a2, int a3)
        {
            byte num;
            byte num1 = (byte)a2;
            byte num2 = (byte)a3;
            int num3 = a2 - 1;
            int num4 = 8 - a3;
            byte num5 = a1[a2 - 1];
            while (num3 >= 0)
            {
                num = (num3 > 0 ? a1[num3 - 1] : num5);
                int num6 = num3;
                num3 = num6 - 1;
                num1 = (byte)(num << (num4 & 31) | a1[num6] >> (num2 & 31));
                a1[num3 + 1] = num1;
            }
            return num1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DemonBuffer = Encoding.Default.GetBytes(textBox1.Text);
            for (int i = 1; i <= 5; i++)
            {
                EncryptServer(DemonBuffer, (int)DemonBuffer.Length, i);
                Array.Resize<byte>(ref DemonBuffer, (int)DemonBuffer.Length - 1);
            }
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            var x = File.Create(FilePath);
            x.Write(DemonBuffer, 0, (int)DemonBuffer.Length);
            x.Close();
            MessageBox.Show("Saved !!");
            Clear();
        }

        public static int EncryptServer(byte[] a1, int a2, int a3)
        {
            int num = a3;
            byte[] numArray = a1;
            byte num1 = a1[0];
            int num2 = 8 - a3;
            int num3 = 0;
            int num4 = 8 - a3;
            if (a2 > 0)
            {
                while (true)
                {
                    int num5 = (num3 >= a2 - 1 ? (int)num1 : (int)numArray[num3 + 1]);
                    int num6 = num3;
                    num3 = num6 + 1;
                    int num7 = numArray[num6] << (num & 31);
                    numArray[num3 - 1] = (byte)(num7 | num5 >> (num2 & 31));
                    if (num3 >= a2)
                    {
                        break;
                    }
                    int num8 = (ushort)num2 >> 8;
                    num2 = num4 & 255 | (num8 & 255) << 8;
                }
            }
            return num3;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left){
                NewPoint = Control.MousePosition;
                NewPoint.X -= (X);
                NewPoint.Y -= (Y);
                this.Location = NewPoint;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            X = Control.MousePosition.X - this.Location.X;
            Y = Control.MousePosition.Y - this.Location.Y;
        }

        private void Clear()
        {
            FilePath = "";
            DemonBuffer = null;
            textBox1.Text = "";
            button3.Enabled = false;
            button4.Enabled = false;
        }
    }
}
