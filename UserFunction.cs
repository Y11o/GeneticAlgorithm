using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    internal class UserFunction
    {
        private string expression = "";
        public string Expression { get => expression; set => expression = value; }

        public UserFunction(string expression)
        {
            this.expression = expression;
        }

        public bool isEmpty()
        {
            return expression == "";
        }

        public double findFunctionValue(double x1, double x2)
        {
            switch (this.expression)
            {
                case "x1^2+3x2^2+2x1*x2":
                    return Func1(x1, x2);
                case "100*(x2-x1^2)^2+(1-x1)^2":
                    return Func2(x1, x2);
                case "-12x2+4x1^2+4x2^2-4x1*x2":
                    return Func3(x1, x2);
                case "(x1-2)^4+(x1-2x2)^2":
                    return Func4(x1, x2);
                case "4(x1-5)^2+(x2-6)^2":
                    return Func5(x1, x2);
                case "2x1^3+4x1*x2^3-10*x1*x2+x2^2":
                    return Func7(x1, x2);
                case "100(x2-x1^2)^2+(1-x1)^2":
                    return Func10(x1, x2);
                case "8x1^2+4x1x2+5x2^2":
                    return Func12(x1, x2);
            }
            return 0;
        }

        private double Func1(double x1, double x2)
        {
            return Math.Pow(x1, 2) + 3 * Math.Pow(x2, 2) + 2 * x1 * x2;
        }

        private double Func2(double x1, double x2)
        {
            return 100 * Math.Pow((x2 - Math.Pow(x1, 2)), 2) + Math.Pow((1 - x1), 2);
        }

        private double Func3(double x1, double x2)
        {
            return (-12) * x2 + 4 * Math.Pow(x1, 2) + 4 * Math.Pow(x2, 2) - 4 * x1 * x2;
        }

        private double Func4(double x1, double x2)
        {
            return Math.Pow((x1 - 2), 4) + Math.Pow((x1 - 2 * x2), 2);
        }

        private double Func5(double x1, double x2)
        {
            return 4 * Math.Pow((x1 - 5), 2) + Math.Pow((x2 - 6), 2);
        }


        private double Func7(double x1, double x2)
        {
            return 2 * Math.Pow(x1, 3) + 4 * x1 * Math.Pow(x2, 3) - 10 * x1 * x2 + Math.Pow(x2, 2);
        }

        private double Func10(double x1, double x2)
        {
            return 100 * Math.Pow((x2 - Math.Pow(x1, 2)), 2) + Math.Pow((1 - x1), 2);
        }

        private double Func12(double x1, double x2)
        {
            return 8 * Math.Pow(x1, 2) + 4 * x1 * x2 + 5 * Math.Pow(x2, 2);
        }

    }
}
