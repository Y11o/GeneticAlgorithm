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
        private double leftRange;
        private double rightRange;

        public Chromosome(int chromosomeLength, UserFunction userFunction, bool decimalCode, double leftRange, double rightRange)
        {
            this.chromosomeLength = chromosomeLength;
            this.userFunction = userFunction;
            ChromosomeGenes = new double[chromosomeLength];
            this.userFunction = userFunction;
            this.decimalCode = decimalCode;
            LeftRange = leftRange;
            RightRange = rightRange;
        }

        public double FitnessGenom { get => fitnessGenom; set => fitnessGenom = value; }
        public double[] ChromosomeGenes { get => chromosomeGenes; set => chromosomeGenes = value; }
        public bool DecimalCode { get => decimalCode; set => decimalCode = value; }
        public double LeftRange { get => leftRange; set => leftRange = value; }
        public double RightRange { get => rightRange; set => rightRange = value; }

        public void calculateFitness()
        {
            if (decimalCode) {
                fitnessGenom = userFunction.findFunctionValue(ChromosomeGenes[0], ChromosomeGenes[1]);
            }
            else
            {
                double codedFitrness = userFunction.findFunctionValue(doubleToCode(ChromosomeGenes[0]), doubleToCode(ChromosomeGenes[1]));
                fitnessGenom = codedToDouble(codedFitrness);
            }

        }

        public List<Chromosome> crossingover(Chromosome parent2, Random rand)
        {
            Chromosome child1 = new Chromosome(chromosomeLength, userFunction, DecimalCode, LeftRange, RightRange);
            Chromosome child2 = new Chromosome(chromosomeLength, userFunction, DecimalCode, LeftRange, RightRange);
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

        public void mutate(Random rand, double mutateRate)
        {
            double mutation = rand.NextDouble();
            if (mutation >= mutateRate)
            {
                for (int gene = 0; gene < chromosomeLength; gene++)
                {
                    ChromosomeGenes[gene] = rand.NextDouble() * (RightRange - LeftRange) + LeftRange;
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

                    int[] bits = DoubleToBits(doubleToCode(chromosomeGenes[gene]));
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

        private int doubleToCode(double gene)
        {
            return Convert.ToInt32(Math.Floor((gene - Convert.ToInt32(Math.Floor(LeftRange))) * (Math.Pow(2, 10) - 1) /
                (Convert.ToInt32(Math.Floor(RightRange)) - Convert.ToInt32(Math.Floor(LeftRange)))));
        }

        private double codedToDouble(double gene)
        {
            double decodedGene = gene * (Convert.ToInt32(Math.Floor(RightRange)) - Convert.ToInt32(Math.Floor(LeftRange))) /
                (Math.Pow(2, 10) - 1) + Convert.ToInt32(Math.Floor(LeftRange));
            return decodedGene;
        }
    }
}

