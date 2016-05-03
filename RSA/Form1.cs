using System;
using System.Windows.Forms;

namespace RSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            RSA.Dvoid dkey = KeyGenerator;
            dkey.BeginInvoke(null, null);

        }

        private void KeyGenerator()
        {
            var key = RSA.KeyGenerator();
            button1.BeginInvoke(new RSA.DKey(PrintKey1), new object[] {key});
        }

        private void PrintKey1(Key key)
        {
            textBox1.Text = key.p.ToString();
            textBox2.Text = key.q.ToString();
            textBox3.Text = key.n.ToString();
            textBox4.Text = key.E.ToString();
            textBox5.Text = key.D.ToString();
            button1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ulong n, E;
            try
            {
                n = Convert.ToUInt64(textBox3.Text);
                E = Convert.ToUInt64(textBox4.Text);
            }
            catch
            {
                MessageBox.Show(@"Перед шифруванням потрібно згенерувати ключ");
                return;
            }
            var open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                string filename = open.FileName;
                RSA.DUlongUlongString encrpt = Encryption;
                encrpt.BeginInvoke(n, E, filename, null, null);
            }
        }

        private void Encryption(ulong n, ulong E, string filename)
        {
            RSA.Encryption(n, E, filename);
            RSA.DvoidInt action = Enable;
            button2.BeginInvoke(action, new object[] { 2 });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ulong n, D;
            try
            {
                n = Convert.ToUInt64(textBox3.Text);
                D = Convert.ToUInt64(textBox5.Text);
            }
            catch
            {
                MessageBox.Show(@"Введено некоректний ключ");
                return;
            }
            var open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                button3.Enabled = false;
                string filename = open.FileName;
                RSA.DUlongUlongString dcrpt = Decryption1;
                dcrpt.BeginInvoke(n, D, filename, null, null);
            }
        }

        private void Decryption1(ulong n, ulong D, string filename)
        {

            RSA.Decryption(n, D, filename);
            RSA.DvoidInt action = Enable;
            button3.BeginInvoke(action, new object[] { 3 });

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ulong n, E;
            try
            {
                n = Convert.ToUInt64(textBox7.Text);
                E = Convert.ToUInt64(textBox6.Text);
                button4.Enabled = false;
            }
            catch
            {
                button4.Enabled = true;
                MessageBox.Show(@"Перед взломом закритого ключа введіть відкритий ключ");
                return;
            }
            RSA.DUlongUlong dulong = KeyHacking;
            dulong.BeginInvoke(n, E, null, null);


        }

        private void KeyHacking(ulong n, ulong E)
        {
            var key = RSA.KeyHacking(n, E);
            button4.BeginInvoke(new RSA.DKey(PrintKey2), new object[] {key});

        }

        private void PrintKey2(Key key)
        {
            textBox8.Text = key.p.ToString();
            textBox9.Text = key.q.ToString();
            textBox10.Text = key.D.ToString();
            button4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ulong n, D;
            try
            {
                n = Convert.ToUInt64(textBox7.Text);
                D = Convert.ToUInt64(textBox10.Text);
            }
            catch
            {
                MessageBox.Show(@"Введено некоректний ключ");
                return;
            }
            var open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                button5.Enabled = false;
                string filename = open.FileName;
                RSA.DUlongUlongString dcrpt = Decryption2;
                dcrpt.BeginInvoke(n, D, filename, null, null);
            }
        }

        private void Decryption2(ulong n, ulong D, string filename)
        {
            RSA.Decryption(n, D, filename);
            RSA.DvoidInt action = Enable;
            button5.BeginInvoke(action, new object[] {5});
        }

        private void Enable(int a)
        {
            switch (a)
            {
                case 2:
                {
                    button2.Enabled = true;
                    break;
                }
                case 3:
                {
                    button3.Enabled = true;
                    break;
                }
                case 5:
                {
                    button5.Enabled = true;
                    break;
                }
            }
        }
    }
}
