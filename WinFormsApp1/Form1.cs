namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        float s = 1000;
        float p = 250;
        string material = "Не выбрано";
        int pressType = 0;
        float q = 300;

        string folderPath = @"C:\Users\arina\Documents\Институт\ИнжПроект_Диплом\Гидравлический пресс\Сборка1";

        int cb1_index = 5;

        double D = 0;

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = cb1_index;
            textBox1.Text = s.ToString();
            textBox2.Text = q.ToString();
            radioButton1.Checked = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            p = float.Parse(comboBox1.SelectedItem.ToString()!.Split()[0]);
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (q == 0 || s == 0 || p == 0)
            {
                MessageBox.Show("Введите значения");
                return;
            }

            double F = q * s;
            double D = 10 * 2 * Math.Sqrt(F) / (Math.Sqrt(p * 10 * Math.PI));

            D = FindDiameter(D);

            new PressSborka().GidravlicPress(D);
        }


        private static double FindDiameter(double D)
        {
            foreach (var d in diameters)
            {
                if (d >= D)
                    return d;
            }

            return diameters[diameters.Count - 1];
        }

        private static List<double> diameters = new List<double>
        {
            100, 110, 125, 140, 160, 180, 200, 220, 250, 280, 320,
            360, 400, 450, 500, 530, 560, 630, 710, 800, 900
        };

        private static Dictionary<int, Dictionary<string, (int Min, int Max)>> qValues = new Dictionary<int, Dictionary<string, (int Min, int Max)>>()
        {
            {
                0,
                new Dictionary<string, (int Min, int Max)>()
                {
                    { "01-040-02", (250, 350) },
                    { "03-010-02", (250, 350) },
                    { "01-030-02", (250, 350) },
                    { "Э-2-330-02", (250, 350) },
                    { "У-4-080-02", (250, 350) },
                    { "Сп-1-342-02", (250, 350) },
                    { "В-4-70", (300, 400) },
                    { "Аминопласт", (250, 350) },
                    { "АГ-4В", (300, 400) },
                    { "АГ-4С", (300, 400) },
                }
            },

            {
                1,
                new Dictionary<string, (int Min, int Max)>()
                {
                    { "01-040-02", (400, 800) },
                    { "03-010-02", (400, 800) },
                    { "01-030-02", (400, 800) },
                    { "Э-2-330-02", (400, 900) },
                    { "У-4-080-02", (500, 800) },
                    { "Сп-1-342-02", (600, 800) },
                    { "В-4-70", (500, 800) },
                    { "Аминопласт", (500, 800) },
                    { "АГ-4В", (700, 1200) },
                    { "АГ-4С", (700, 1200) },
                }
            }
        };

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            s = float.Parse(textBox1.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            material = comboBox2.SelectedItem.ToString()!;

            textBox2.ReadOnly = comboBox2.SelectedIndex > 0;


            if (material != "Не выбрано")
            {
                q = (qValues[pressType][material].Max + qValues[pressType][material].Min) / 2;

                textBox2.Text = q.ToString();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pressType = 0;

            if (material != "Не выбрано")
            {
                q = (qValues[pressType][material].Max + qValues[pressType][material].Min) / 2;

                textBox2.Text = q.ToString();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            pressType = 1;


            if (material != "Не выбрано")
            {
                q = (qValues[pressType][material].Max + qValues[pressType][material].Min) / 2;

                textBox2.Text = q.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            q = float.Parse(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (material != "Не выбрано")
            {
                q = (qValues[pressType][material].Max + qValues[pressType][material].Min) / 2;
            }

            double F = q * s;
            D = 10 * 2 * Math.Sqrt(F) / (Math.Sqrt(p * 10 * Math.PI));

            D = FindDiameter(D);

            textBox3.Text = D.ToString();
            textBox4.Text = F.ToString();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}