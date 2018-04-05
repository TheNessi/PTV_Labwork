﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace labwork
{
    public struct experiment:IComparable
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

            if ((mu >= 0) && (lam>0))
            {
                etta = new experiment[count_exp];


                for (int i = 0; i < count_exp; i++)
                {
                    etta[i].number = i + 1;
                    etta[i].sv = 0;
                    for (int j = 0; j < count_b; j++)
                    {
                        tmp = Rand.NextDouble();
                        etta[i].sv += (mu - (Math.Log(tmp)/lam));
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

        public void form_cotrol(TextBox textBox1,KeyPressEventArgs e)
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
            form_cotrol(textBox2,e);
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
            board = new double[size + 1];
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            for (int i = 1; i < size; i++)
            {
                zi[i] = ((max + min) / size) + i + min;
                dataGridView3.Rows[i].Cells[0].Value = Convert.ToString(zi[i]);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            charact.mean = count_b * mean;
            charact.disp = count_b * disp;
            
            //среднее выборочное
            for(int i=0;i<count_exp;i++)
            {
                charact.v_mean += etta[i].sv;
            }
            charact.v_mean = charact.v_mean / count_exp;

            //выборочная дисперсия
            for (int i = 0; i < count_exp; i++)
            {
                charact.v_disp += Math.Pow((etta[i].sv - charact.v_mean),2);
            }
            charact.v_disp = charact.v_disp / count_exp;

            //размах
            charact.razm = - etta[0].sv  + etta[count_exp - 1].sv;
            
            //медиана

            if(count_exp % 2 == 1)
            {
                k = count_exp / 2 + 1;
                charact.v_median = etta[k].sv;
            }
            else if(count_exp % 2 == 0)
            {
                k = count_exp / 2;
                charact.v_median = (etta[k].sv + etta[k - 1].sv) / 2;
            }
            

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
            common_lam = 1.0 / (Math.Sqrt(charact.disp));
            common_mu = charact.mean - Math.Sqrt(charact.disp);

            v_lam = 1.0 / (Math.Sqrt(charact.v_disp));
            v_mu = charact.v_mean - Math.Sqrt(charact.v_disp);

            for (int i = 0; i < count_exp; i++)
            {
                double x = common_mu + i * 0.2;
                double y = 1 - Math.Exp(-common_lam * (x - common_mu));
                
                chart1.Series[0].Points.AddXY(x, y);
               
            }

            chart1.Series[1].Points.AddXY(common_mu, 0);

            for (int i = 0; i < count_exp; i++)
            {

                double x = common_mu + i * 0.2;
             
                double y = 1 - Math.Exp(-common_lam * (x - common_mu)) + eps;

                    chart1.Series[1].Points.AddXY(x, y);
            }

            //chart1.Series[1].Points.AddXY(dataGridView1.Rows[count_exp-1].Cells[1].Value, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < size; i++)
            {
                if (Convert.ToString(dataGridView3.Rows[i].Cells[0]) != "")
                {
                    zi[i] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0]);
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
        }
    }
}
