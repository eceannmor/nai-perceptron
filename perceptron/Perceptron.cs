namespace perceptron
{
    public class Perceptron
    {
        private static readonly int MAX_ITER = 50000;
        public readonly double[] weights;
        public readonly double bias;

        private static string[] classes = ["Iris-virginica", "Iris-versicolor"];
        public static string[] Classes { get => classes; set => classes = value; }

        public Perceptron(double[][] trainingSet, int[] classSet, double stepSize)
        {
            weights = new double[trainingSet[0].Length];
            Random random = new();
            for (int i = 0; i < trainingSet[0].Length; i++)
            {
                weights[i] = random.NextDouble();
            }
            bias = random.NextDouble();
            for (int i = 0; i < MAX_ITER; i++)
            {
                int error = 0;
                for (int j = 0; j < trainingSet.Length; j++)
                {
                    int output = Activate(WeightedSum(trainingSet[j]), x => x > bias ? 1 : 0);
                    error += Math.Abs(classSet[j] - output);
                    for (int k = 0; k < trainingSet[0].Length; k++)
                    {
                        weights[k] += stepSize * trainingSet[j][k] * (classSet[j] - output);
                        bias -= stepSize * (classSet[j] - output);
                    }
                }
                // if (i > 100 && error / i < 0.001)
                // {
                //     Console.WriteLine("Training took " + (i + 1) + " iterations");
                //     Thread.Sleep(1000);
                //     break;
                // }
            }
        }
        public Perceptron(double[][] trainingSet, int[] classSet) : this(trainingSet, classSet, 0.01) { }

        public void Test()
        {
            Console.Clear();
            Console.WriteLine("Internal:");
            for (int i = 0; i < weights.Length; i++)
            {
                Console.WriteLine("w" + i + " = \t " + weights[i]);
            }
            Console.WriteLine("bias = \t" + bias);
            Console.WriteLine("Test set results:");
            int testsPassed = 0;
            string[] lines = File.ReadAllLines("./perceptron/data/perceptron.test.data");
            List<List<String>> input = [];
            foreach (string line in lines)
            {
                input.Add(new List<string>(line.Split(',')));
            }
            double[][] w = new double[input.Count][];
            int[] classSet = new int[input.Count];
            int[] resultSet = new int[input.Count];
            for (int i = 0; i < input.Count; i++)
            {
                w[i] = input[i].Take(input[i].Count - 1).Select(double.Parse).ToArray();
                classSet[i] = input[i].Last() == Classes[1] ? 1 : 0;
            }
            for (int i = 0; i < w.Length; i++)
            {
                resultSet[i] = Classify(w[i]);
                if (resultSet[i] == classSet[i])
                {
                    testsPassed++;
                }
            }
            Console.WriteLine("\tPassed " + testsPassed + "/" + w.Length + " tests\n Detailed:");

            for (int i = 0; i < resultSet.Length; i++)
            {
                Console.Write("Result: " + Classes[resultSet[i]] + "\tExpected: " + Classes[classSet[i]] + "\tWeights: ");
                foreach (double weight in w[i])
                {
                    Console.Write(weight + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Helper.ExitOnEnter();
        }

        public int Classify(double[] weights)
        {
            return Activate(WeightedSum(weights), x => x > bias ? 1 : 0);
        }

        public int GetDimension()
        {
            return weights.Length;
        }

        public static int Activate(double sum, Func<double, int> activationFunction)
        {
            return activationFunction.Invoke(sum);
        }

        public double WeightedSum(double[] a)
        {
            return Product(a, weights);
        }

        public static double Product(double[] a, double[] b)
        {
            double product = 0;
            if (a.Length != b.Length)
                return product;
            for (int i = 0; i < a.Length; i++)
            {
                product += a[i] * b[i];
            }
            return product;
        }

    }
}