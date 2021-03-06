﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace labwork
{
    public struct experiment : IComparable
    {
        public int number;
        public double sv;

        public int CompareTo(Object obj)
        {
            experiment arr = (experiment)obj;
            return this.sv.CompareTo(arr.sv);
        }
    }


    public struct specifications
    {
        public double mean;
        public double v_mean;
        public double disp;
        public double v_disp;
        public double v_median;
        public double razm;
    }

    public partial class Form1 : Form
    {
        int count_b = 0;
        int count_exp = 0;
        int k = 0;
        double lam;
        double disp;
        double mean;
        double tmp;
        double mu;
        private int size;
        double[] ni, zi, board;
        double min, max;
        double common_lam;
        double common_mu;
        double v_lam;
        double v_mu;

        experiment[] etta;
        specifications charact;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Random Rand = new Random();



            count_b = Convert.ToInt32(textBox1.Text);
            mean = Convert.ToDouble(textBox2.Text);
            disp = Convert.ToDouble(textBox3.Text);
            count_exp = Convert.ToInt32(textBox4.Text);
            lam = 1.0 / (Math.Sqrt(disp));
            mu = mean - Math.Sqrt(disp);

            label5.Text = "lambda: " + Convert.ToString(lam);
            label6.Text = "mu: " + Convert.ToString(mu);

            if ((mu >= 0) && (lam > 0))
            {
                etta = new experiment[count_exp];


                for (int i = 0; i < count_exp; i++)
                {
                    etta[i].number = i + 1;
                    etta[i].sv = 0;
                    for (int j = 0; j < count_b; j++)
                    {
                        tmp = Rand.NextDouble();
                        etta[i].sv += (mu - (Math.Log(tmp) / lam));
                    }
                }

                Array.Sort(etta);

                for (int i = 0; i < count_exp; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Convert.ToString(etta[i].number);
                    dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(etta[i].sv);
                }
            }
            else
            {
                var result = MessageBox.Show("Mu<0, попробуйте задать другое среднее или дисперсию.", "Ошибка", MessageBoxButtons.OK);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {

                    // Closes the parent form.

                    this.Close();

                }
            }
        }

        public void form_cotrol(TextBox textBox1, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (e.KeyChar == '.')
            {
                // точку заменим запятой
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {

                    // запятая уже есть в поле редактирования
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }

        public void mod_form_conrtol(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            form_cotrol(textBox2, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            form_cotrol(textBox3, e);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            min = etta[0].sv;
            max = etta[count_exp - 1].sv;

            size = Convert.ToInt32(textBox5.Text);
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.Rows.Clear();
            dataGridView3.RowCount = size + 1;
            dataGridView3.ColumnCount = 1;
            dataGridView3.Rows[0].Cells[0].Value = Convert.ToString(min);
            dataGridView3.Rows[size].Cells[0].Value = Convert.ToString(max);

            dataGridView4.Rows.Clear();
            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView4.ColumnCount = size;
            dataGridView4.RowCount = 3;
            ni = new double[size + 1];
            zi = new double[size + 1];

            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }

            zi[0] = min;
            zi[size] = max; ;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5_TextChanged(sender, e);
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }

            zi[0] = min;
            zi[size] = max; ;
            for (int i = 1; i < size; i++)
            {
                zi[i] = ((max - min) / (size)) + zi[i - 1];
                dataGridView3.Rows[i].Cells[0].Value = Convert.ToString(zi[i]);

            }
            charts_zi();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            charact.mean = count_b * mean;
            charact.disp = count_b * disp;


            charact.v_mean = 0;
            charact.v_disp = 0;

            //среднее выборочное
            for (int i = 0; i < count_exp; i++)
            {
                charact.v_mean += etta[i].sv;
            }
            charact.v_mean = charact.v_mean / count_exp;

            //выборочная дисперсия
            for (int i = 0; i < count_exp; i++)
            {
                charact.v_disp += Math.Pow((etta[i].sv - charact.v_mean), 2);
            }
            charact.v_disp = charact.v_disp / count_exp;

            //размах
            charact.razm = -etta[0].sv + etta[count_exp - 1].sv;

            //медиана

            if (count_exp % 2 == 1)
            {
                k = count_exp / 2 + 1;
                charact.v_median = etta[k - 1].sv;
            }
            else if (count_exp % 2 == 0)
            {
                k = count_exp / 2;
                charact.v_median = (etta[k].sv + etta[k - 1].sv) / 2;
            }


            common_lam = 1.0 / (Math.Sqrt(charact.disp));
            common_mu = charact.mean - Math.Sqrt(charact.disp);

            label10.Text = "common_lam: " + Convert.ToString(common_lam);
            label11.Text = "common_mu: " + Convert.ToString(common_mu);

            dataGridView2.Rows.Add();

            dataGridView2.Rows[0].Cells[0].Value = Convert.ToString(charact.mean);
            dataGridView2.Rows[0].Cells[1].Value = Convert.ToString(charact.v_mean);
            dataGridView2.Rows[0].Cells[2].Value = Convert.ToString(Math.Abs(charact.mean - charact.v_mean));
            dataGridView2.Rows[0].Cells[3].Value = Convert.ToString(charact.disp);
            dataGridView2.Rows[0].Cells[4].Value = Convert.ToString(charact.v_disp);
            dataGridView2.Rows[0].Cells[5].Value = Convert.ToString(Math.Abs(charact.disp - charact.v_disp));
            dataGridView2.Rows[0].Cells[6].Value = Convert.ToString(charact.v_median);
            dataGridView2.Rows[0].Cells[7].Value = Convert.ToString(charact.razm);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            double eps = 0.001;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            v_lam = 1.0 / (Math.Sqrt(charact.v_disp));
            v_mu = charact.v_mean - Math.Sqrt(charact.v_disp);
            double offset = common_mu - etta[0].sv;

            double local_max = -10;

            double x2 = 0;

            for (int i = 0; i < 150; i++)
            {
                x2 = x2 + i * 0.02;
                double y = normaldistr.normaldistribution(
                    (x2 - charact.mean) / (Math.Sqrt(charact.disp)));
                chart1.Series[0].Points.AddXY(x2, y);
            }

            chart1.Series[1].Points.AddXY(Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value), 0);
            local_max = (Math.Abs(0 - normaldistr.normaldistribution(
                                      (Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value) - charact.mean) /
                                      (Math.Sqrt(charact.disp)))));

            for (int i = 0; i < count_exp; i++)
            {

                double x1 = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value) + eps;
                double y = normaldistr.normaldistribution(
                    (x1 - charact.mean) / (Math.Sqrt(charact.disp)));


                double y1 = ((double)(i + 1) / count_exp);
                chart1.Series[1].Points.AddXY(x1, y1);

                double rash = Math.Abs(((double)(i) / count_exp) - y);


                if (local_max < rash)
                    local_max = rash;

                rash = Math.Abs(y1 - y);


                if (local_max < rash)
                    local_max = rash;

            }

            

            chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value) - 2;
            chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(dataGridView1.Rows[count_exp - 1].Cells[1].Value) + 2;

            double sdvig = Convert.ToDouble(dataGridView1.Rows[count_exp - 1].Cells[1].Value) + 5;
            chart1.Series[1].Points.AddXY(sdvig, 1);

            double razn = (Math.Abs(1 - normaldistr.normaldistribution(
                                        (sdvig - charact.mean) /
                                        (Math.Sqrt(charact.disp)))));
            if (local_max < razn)
                local_max = razn;

            label9.Text = "D: " + Convert.ToDouble(local_max);
        }





        private void button4_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }

            zi[0] = min;
            zi[size] = max; ;

            for (int i = 1; i < size; i++)
            {
                if (Convert.ToString(dataGridView3.Rows[i].Cells[0]) != "")
                {
                    zi[i] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0].Value);

                }
                else
                {
                    var result = MessageBox.Show("Введены не все промежутки", "Ошибка", MessageBoxButtons.OK);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {

                        // Closes the parent form.

                        this.Close();

                    }
                }
            }

            charts_zi();
        }



        void charts_zi()
        {


            chart2.Series[0].Points.Clear();

            for (int i = 0; i < size; i++)
            {

                dataGridView4.Rows[0].Cells[i].Value = (zi[i + 1] + zi[i]) / 2;
                dataGridView4.Rows[1].Cells[i].Value = normaldistr.get_plotn(
                    (zi[i + 1] + zi[i]) / 2, charact.disp, charact.mean);

                for (int j = 0; j < count_exp; j++)
                {
                    if ((zi[i] <= etta[j].sv) && (zi[i + 1] > etta[j].sv))
                    {
                        ni[i] += 1;
                    }
                }

                ni[i] = ni[i] * 1.0 / (count_exp * Math.Abs(zi[i + 1] - zi[i]));
                dataGridView4.Rows[2].Cells[i].Value = ni[i];

                chart2.Series[0].SetCustomProperty("PixelPointWidth", Convert.ToString(Math.Round(zi[i + 1] - zi[i]) * 10));
                chart2.Series[0].Points.AddXY((zi[i + 1] + zi[i]) / 2, ni[i]);


            }
            chart2.Series[0].Points.AddXY(size, ni[size]);

            double abs_max = -99;
            int pos = 0;
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(ni[i] - normaldistr.get_plotn(
                                 (zi[i + 1] + zi[i]) / 2, charact.disp, charact.mean)) > abs_max)
                {
                    abs_max = Math.Abs(ni[i] - normaldistr.get_plotn(
                                           (zi[i + 1] + zi[i]) / 2, charact.disp, charact.mean));
                    pos = i;
                }
            }

            label8.Text = "max: " + Convert.ToString(abs_max) + ". On pos: " + Convert.ToString(pos);

            chart2.ChartAreas[0].AxisX.Minimum = zi[0] - 5;
            chart2.ChartAreas[0].AxisX.Maximum = zi[size] + 5;
        }
    }
}
