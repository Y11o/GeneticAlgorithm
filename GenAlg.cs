using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static GeneticAlgorithm.Chromosome;

namespace GeneticAlgorithm
{
    internal class GenAlg
    {
        private bool decimalCode;
        private UserFunction userFunction;
        private int generationCount = 0;
        private int populationCount = 0;
        private double mutateRate = 0;
        private double crossRate = 0;
        private double leftRange;
        private double rightRange;
        private int chomosomeLength = 2;
        private Random rand = new Random();
        private List<Chromosome> currentGeneration = new List<Chromosome>();
        private List<Chromosome> nextGeneration = new List<Chromosome>();
        private List<double> fitnessProgress = new List<double>();
        private double totalFitness = 0;
        private Chromosome bestGene;

        public int GenerationCount { get => generationCount; set => generationCount = value; }
        public int PopulationCount { get => populationCount; set => populationCount = value; }
        public double MutateRate { get => mutateRate; set => mutateRate = value; }
        public double CrossRate { get => crossRate; set => crossRate = value; }
        public double LeftRange { get => leftRange; set => leftRange = value; }
        public double RightRange { get => rightRange; set => rightRange = value; }
        public bool DecimalCode { get => decimalCode; set => decimalCode = value; }
        internal UserFunction UserFunction { get => userFunction; set => userFunction = value; }

        public bool checkSettings()
        {

            if ((decimalCode || !decimalCode) && !userFunction.isEmpty() && generationCount > 0 && populationCount > 0 &&
                (mutateRate > 0 && mutateRate < 1) && (crossRate > 0 && crossRate < 1) && leftRange < rightRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Chromosome startGenAlg()
        {
            for (int being = 0; being < populationCount; being++)
            {
                Chromosome currChromosome = new Chromosome(chomosomeLength, userFunction, DecimalCode, leftRange, rightRange);
                for (int gene = 0; gene < chomosomeLength; gene++)
                {
                    int midOrEdge = rand.Next(0, 2);
                    if (midOrEdge == 0)
                    {
                        currChromosome.ChromosomeGenes[gene] = rand.NextDouble() * ((rightRange - leftRange) / 2) + leftRange;
                    }
                    else
                    {
                        currChromosome.ChromosomeGenes[gene] = rand.NextDouble() * (rightRange - leftRange) + leftRange;
                    }
                }
                currChromosome.calculateFitness();
                currentGeneration.Add(currChromosome);
            }
            countTotalFitness();
            bestGene = currentGeneration[populationCount - 1];
            outPut(currentGeneration, true, 0);

            for (int generation = 0; generation < generationCount; generation++)
            {
                setNextGeneration();
                countTotalFitness();
                outPut(currentGeneration, false, generation);
            }

            return bestGene;
        }

        public void setNextGeneration()
        {
            nextGeneration.Clear();
            Chromosome bestFromGeneration = currentGeneration[populationCount - 1];
            for (int being = 0; being < populationCount; being += 2)
            {
                int chosenParent1 = indexSearch();
                int chosenParent2 = indexSearch();
                Chromosome parent1 = currentGeneration[chosenParent1];
                Chromosome parent2 = currentGeneration[chosenParent2];
                Chromosome child1, child2;
                if (rand.NextDouble() < crossRate)
                {
                    List<Chromosome> childs = parent1.crossingover(parent2, rand);
                    child1 = childs[0];
                    child2 = childs[1];
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }
                child1.mutate(rand, mutateRate);
                child1.calculateFitness();
                child2.mutate(rand, mutateRate);
                child2.calculateFitness();
                nextGeneration.Add(child1);
                nextGeneration.Add(child2);
            }
            nextGeneration[rand.Next(populationCount - 1)] = bestFromGeneration;
            currentGeneration.Clear();
            for (int being = 0; being < populationCount; being++)
            {
                currentGeneration.Add(nextGeneration[being]);
            }
            if (currentGeneration[populationCount - 1].FitnessGenom < bestGene.FitnessGenom)
            {
                bestGene = bestFromGeneration;
            }
        }

        private int indexSearch()
        {
            double randomFitness = rand.NextDouble() * totalFitness;
            int chosenIndex = 0;
            while (randomFitness > fitnessProgress[chosenIndex])
            {
                chosenIndex++;
            }
            return chosenIndex;
        }

        private void countTotalFitness()
        {
            totalFitness = 0.0;
            fitnessProgress.Clear();
            currentGeneration.Sort(new ChromosomeComparer());
            for (int being = 0; being < populationCount; being++)
            {
                totalFitness += currentGeneration[being].FitnessGenom;
                fitnessProgress.Add(totalFitness);
            }
        }


        private void outPut(List<Chromosome> generation, bool firstFalg, int currIteration)
        {
            string path = @"D:\My projects\VS repos\GeneticAlgorithm\out.txt";
            string populationLine = "";
            for (int being = 0; being < generation.Count; being++)
            {
                populationLine += generation[being].ToString();
            }
            if (firstFalg)
            {
                FileStream fsAppend = new FileStream(path, FileMode.Create);
                string textOut = $"Для функции: {userFunction.Expression}\n" +
                    $"Кодирование: {(decimalCode ? "Десятичное" : "Двоичное")}\n" +
                    $"Количество поколений: {generationCount}\n" +
                    $"Размер популяции: {populationCount}\n" +
                    $"Вероятность мутации: {mutateRate * 100}%\n" +
                    $"Вероятность скрещивания: {crossRate * 100}%\n" +
                    $"Диапазон от {leftRange} до {rightRange}";
                textOut += "\n\nИнициализация генов:" + populationLine + "\n";
                using (StreamWriter writer = new StreamWriter(fsAppend))
                {
                    writer.Write(textOut);
                }
            }
            else
            {
                FileStream fsAppend = new FileStream(path, FileMode.Append);
                string textOut = $"\nПоколение {++currIteration}:" + populationLine + "\n";
                using (StreamWriter writer = new StreamWriter(fsAppend))
                {
                    writer.Write(textOut);
                }
            }


        }
    }

}
