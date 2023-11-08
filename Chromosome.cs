using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace GeneticAlgorithm
{
    internal class Chromosome
    {
        private int chromosomeLength;
        private double[] chromosomeGenes;
        private UserFunction userFunction;
        private double fitnessGenom;
        private bool decimalCode;

        public Chromosome(int chromosomeLength, UserFunction userFunction, bool decimalCode)
        {
            this.chromosomeLength = chromosomeLength;
            this.userFunction = userFunction;
            ChromosomeGenes = new double[chromosomeLength];
            this.userFunction = userFunction;
            this.decimalCode = decimalCode;
        }

        public double FitnessGenom { get => fitnessGenom; set => fitnessGenom = value; }
        public double[] ChromosomeGenes { get => chromosomeGenes; set => chromosomeGenes = value; }
        public bool DecimalCode { get => decimalCode; set => decimalCode = value; }

        public void calculateFitness()
        {
            fitnessGenom = userFunction.findFunctionValue(ChromosomeGenes[0], ChromosomeGenes[1]);
        }

        public List<Chromosome> crossingover(Chromosome parent2, Random rand)
        {
            Chromosome child1 = new Chromosome(chromosomeLength, userFunction, DecimalCode);
            Chromosome child2 = new Chromosome(chromosomeLength, userFunction, DecimalCode);
            int position = (int)(rand.NextDouble() * chromosomeLength);
            for (int gene = 0; gene < chromosomeLength; gene++)
            {
                if (gene < position)
                {
                    child1.ChromosomeGenes[gene] = ChromosomeGenes[gene];
                    child2.ChromosomeGenes[gene] = parent2.ChromosomeGenes[gene];
                }
                else
                {
                    child1.ChromosomeGenes[gene] = parent2.ChromosomeGenes[gene];
                    child2.ChromosomeGenes[gene] = ChromosomeGenes[gene];
                }
            }
            child1.calculateFitness();
            child2.calculateFitness();
            return new List<Chromosome> { child1, child2 };
        }

        public void mutate(Random rand, double mutateRate, double leftRange, double rightRange)
        {
            for (int gene = 0; gene < chromosomeLength; gene++)
            {
                int sumOrSub = rand.Next(0, 2);
                if (rand.NextDouble() <= mutateRate)
                {
                    double gain = rand.NextDouble();
                    if (sumOrSub == 0)
                    {
                        ChromosomeGenes[gene] = ChromosomeGenes[gene] + gain * (rightRange - ChromosomeGenes[gene]) / 2;
                    }
                    else
                    {
                        ChromosomeGenes[gene] = ChromosomeGenes[gene] - gain * (ChromosomeGenes[gene] - leftRange) / 2;
                    }
                }
            }
        }

        public override string ToString()
        {
            string chromosome = "\n";
            if (decimalCode)
            {
                for (int gene = 0; gene < chromosomeLength; gene++)
                {
                    chromosome += $"X{gene + 1}: {chromosomeGenes[gene]} ";
                }
                chromosome += $"\nY: {fitnessGenom}";
                return chromosome;
            }
            else
            {
                for (int gene = 0; gene < chromosomeLength; gene++)
                {
                    int[] bits = DoubleToBits(chromosomeGenes[gene]);
                    string bitsS = "";
                    for (int bit = 0; bit < bits.Length; bit++)
                    {
                        bitsS += bits[bit];
                    }
                    chromosome += $"X{gene + 1}: " + bitsS + " ";
                }
                chromosome += $"\nY: {fitnessGenom}";
                return chromosome;
            }

        }

        private int[] DoubleToBits(double value)
        {
            long longBits = BitConverter.DoubleToInt64Bits(value);
            int[] bits = new int[64];

            for (int i = 0; i < 64; i++)
            {
                bits[i] = (int)((longBits >> i) & 1);
            }

            return bits;
        }
    }
}

