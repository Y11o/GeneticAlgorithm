using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
        }

        GenAlg genAlg = new GenAlg();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                genAlg.DecimalCode = comboBox2.SelectedItem.ToString() == "Десятичное";
                genAlg.UserFunction = new UserFunction(comboBox1.SelectedItem.ToString().Trim());
                genAlg.GenerationCount = Convert.ToInt32(textBox1.Text);
                genAlg.PopulationCount = Convert.ToInt32(textBox2.Text);
                genAlg.MutateRate = (Convert.ToDouble(textBox3.Text) / 100);
                genAlg.CrossRate = (Convert.ToDouble(textBox4.Text) / 100);
                genAlg.LeftRange = Convert.ToDouble(textBox5.Text);
                genAlg.RightRange = Convert.ToDouble(textBox6.Text);

                if (!genAlg.checkSettings()) { throw new Exception(); }
                label6.Text = "Выполнение генетического алгоритма.";
                Chromosome bestGene = genAlg.startGenAlg();
                label11.Text = Convert.ToString(bestGene.ChromosomeGenes[0]);
                label12.Text = Convert.ToString(bestGene.ChromosomeGenes[1]);
                label13.Text = Convert.ToString(bestGene.FitnessGenom);
            }
            catch (Exception)
            {
                label6.Text = "Не все параметры введены корректно";
            }
        }
    }
}
