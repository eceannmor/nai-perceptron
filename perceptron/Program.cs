using perceptron;

Perceptron perceptron;

string[] lines = File.ReadAllLines("./perceptron/data/perceptron.data");
List<List<String>> input = [];
foreach (string line in lines)
{
    input.Add(new List<string>(line.Split(',')));
}
double[][] weights = new double[input.Count][];
int[] classSet = new int[input.Count];
for (int i = 0; i < input.Count; i++)
{
    weights[i] = input[i].Take(input[i].Count-1).Select(double.Parse).ToArray();
    classSet[i] = input[i].Last() == "Iris-versicolor" ? 1 : 0;
}

perceptron = new Perceptron(weights, classSet);

List<string> menu = [
    "Test custom weights",
    "Test the test set",
    "Exit"
];

int indexCurrent = 0;
Helper.Menu(menu, menu[indexCurrent]);
ConsoleKeyInfo key;
bool running = true;

do
{
    Helper.Menu(menu, menu[indexCurrent]);
    key = Console.ReadKey();
    if (key.Key == ConsoleKey.DownArrow)
    {
        if (indexCurrent + 1 < menu.Count)
        {
            indexCurrent++;
        }
    }
    else if (key.Key == ConsoleKey.UpArrow)
    {
        if (indexCurrent - 1 >= 0)
        {
            indexCurrent--;
        }
    }
    else if (key.Key == ConsoleKey.Enter)
    {
        switch (indexCurrent)
        {
            case 0:
                {
                    Helper.Test(perceptron);
                    break;
                }
            case 1:
                {
                    perceptron.Test();
                    break;
                }
            case 2:
                {
                    running = false;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
while (running);
Console.Clear();
Console.WriteLine("*paws at you*\ntnx for using the preceptron simulator~");