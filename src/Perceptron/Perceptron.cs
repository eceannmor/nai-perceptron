using helper;

namespace perceptron;
public class Perceptron
{
    private static readonly int MAX_ITER = 50000;
    public readonly double[] weights;
    public readonly double bias;
    public static readonly string[] classes = ["Iris-virginica", "Iris-versicolor"];

    /// <summary>
    ///     Main constructor, initializes the weights and trains the perceptron.
    /// </summary>
    /// <param name="trainingSet">
    ///     A matrix of weights containing the whole training set.<br/>
    ///     One row of the matrix is equivalent to one object.<br/>
    ///     Property value with index <c>i</c> of an object with index <c>j</c>
    ///     is contained at <c>trainingSet[j][i]</c>
    /// </param>
    /// <param name="classSet">
    ///     An array representing classes of objects in <c>trainingSet</c>.<br/>
    ///     Object at <c>trainingSet[i]</c> has a class of <c>classSet[i]</c>.<br/>
    ///     More specifically, <c>j = classSet[i]</c> represents the index <c>j</c> 
    ///     which can be used to find the string representation of the class 
    /// <code>
    ///     string classOfFistObject = classes[classSet[0]]
    /// </code>
    /// </param>
    /// <param name="stepSize">
    ///     Size of the adjustment performed at incorrect guess while training<br/>
    ///     Advised to keep between <c>0.01</c> and <c>0.15</c>, depending on the data
    /// </param>
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
        }
    }

    /// <summary>
    ///     Additional constructor, specifying <c>stepSize = 0.01</c> by default 
    /// </summary>
    /// <param name="trainingSet">
    ///     A matrix of weights containing the whole training set.<br/>
    ///     One row of the matrix is equivalent to one object.<br/>
    ///     Property value with index <c>i</c> of an object with index <c>j</c>
    ///     is contained at <c>trainingSet[j][i]</c>
    /// </param>
    /// <param name="classSet">
    ///     An array representing classes of objects in <c>trainingSet</c>.<br/>
    ///     Object at <c>trainingSet[i]</c> has a class of <c>classSet[i]</c>.<br/>
    ///     More specifically, <c>j = classSet[i]</c> represents the index <c>j</c> 
    ///     which can be used to find the string representation of the class 
    /// <code>
    ///     string classOfFistObject = classes[classSet[0]]
    /// </code>
    /// </param>
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
        string[] lines = File.ReadAllLines("./src/data/perceptron.test.data");
        List<List<string>> input = [];
        foreach (string line in lines)
        {
            input.Add(new List<string>(line.Split(',')));
        }
        double[][] w = new double[input.Count][];
        int[] classSet = new int[input.Count];
        int[] resultSet = new int[input.Count];
        for (int i = 0; i < input.Count; i++)
        {
            w[i] = input[i].Take(input[i].Count - 1).Select(x => double.Parse(x, System.Globalization.NumberStyles.Float)).ToArray();
            classSet[i] = input[i].Last() == classes[1] ? 1 : 0;
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
            Console.Write("Result: " + classes[resultSet[i]] + "\tExpected: " + classes[classSet[i]] + "\tWeights: ");
            foreach (double weight in w[i])
            {
                Console.Write(weight + "\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Helper.Credit();
        Helper.ExitOnEnter();
    }

    /// <summary>
    /// Goes through the entire process from receiving the input vector to determining whether the neuron fired
    /// </summary>
    /// <param name="weights">
    ///     An unclassified object in a form of a vector of parameters
    /// </param>
    /// <returns>
    ///     In a manner defined by the implementation,
    /// <list type="bullet">
    ///     <item><c>1</c> if the neuron fired</item>
    ///     <item><c>0</c> otherwise</item>
    /// </list>
    /// </returns>
    public int Classify(double[] weights)
    {
        return Activate(WeightedSum(weights), x => x > bias ? 1 : 0);
    }

    /// <returns>
    ///     The amount of inputs of the perceptron
    /// </returns>
    public int GetDimension()
    {
        return weights.Length;
    }

    /// <summary>
    /// Passes the <paramref name="sum"/> to the <paramref name="activationFunction"/> and returns the result
    /// </summary>
    /// <param name="sum">
    ///     Sum of the weighted input parameters
    /// </param>
    /// <param name="activationFunction">
    ///     function taking in a <c>double</c> and outputting either <c>0</c> or <c>1</c> <br/>
    ///     It should make use of the <c>bias</c> field
    /// </param>
    /// <returns>
    /// In a manner defined by the <paramref name="activationFunction"/>,
    /// <list type="bullet">
    ///     <item><c>1</c> if the neuron fired</item>
    ///     <item><c>0</c> otherwise</item>
    /// </list>
    /// </returns>
    public static int Activate(double sum, Func<double, int> activationFunction)
    {
        return activationFunction.Invoke(sum);
    }

    /// <summary>
    ///     Handy shortcut for computing the dot product of the input vector representing an object to be classified <br/>
    ///     and the weights of the perceptron's nodes 
    /// </summary>
    /// <param name="a">
    ///     Vector representing an unclassified object
    /// </param>
    /// <returns>
    ///     0 if the calculation cannot be performed <br/>
    ///     otherwise, the dot product of <paramref name="a"/> and the weights vector
    /// </returns>
    public double WeightedSum(double[] a)
    {
        return Product(a, weights);
    }

    /// <summary>
    ///     Computes the dot product (scalar product) of the given vectors
    /// </summary>
    /// <param name="a">
    ///     First vector
    /// </param>
    /// <param name="b">
    ///     Second vector
    /// </param>
    /// <returns>
    /// <list type="bullet">
    /// <item>
    ///     0 if the calculation cannot be performed 
    /// </item>
    /// <item>
    ///     otherwise, the dot product of <paramref name="a"/> and <paramref name="b"/>
    /// </item>
    /// </list>
    /// </returns>
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
