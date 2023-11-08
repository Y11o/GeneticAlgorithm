using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    internal class ChromosomeComparer : IComparer<Chromosome>
    {
        public int Compare(Chromosome c1, Chromosome c2)
        {
            if (c1.FitnessGenom > c2.FitnessGenom)
                return -1;
            else if (c1.FitnessGenom == c2.FitnessGenom)
                return 0;
            else
                return 1;

        }
    }
}
