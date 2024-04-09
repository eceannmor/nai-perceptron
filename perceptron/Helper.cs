namespace perceptron
{
    public class Helper
    {

        public static string GetInput()
        {
            Console.Write("> ");
            return Console.ReadLine() ?? "";
        }

        public static string GetInput(string message)
        {
            Console.WriteLine(message);
            return GetInput();
        }

        public static void Menu(List<string> menu, string selected)
        {
            Console.Clear();
            foreach (string item in menu)
            {
                if (item == selected)
                    Console.Write(" > ");
                else
                    Console.Write(" ");
                Console.WriteLine(item);
            }
            Credit();
        }

        public static void TempNote(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
            Thread.Sleep(3000);
        }

        public static void ExitOnEnter()
        {
            Console.WriteLine("Press Enter to go back");
            ConsoleKeyInfo ck;
            bool running = true;
            do
            {
                ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.Enter)
                    running = false;
            } while (running);
        }

        public static void Test(Perceptron perceptron)
        {
            int size = perceptron.GetDimension() + 1;
            List<string> menu = [];
            double[] weights = new double[size - 1];
            for (int i = 0; i < size - 1; i++)
            {
                menu.Add("w" + (i + 1) + " = 0.0");
                weights[i] = 0;
            }
            menu.Add("Exit");
            int indexCurrent = 0;
            ConsoleKeyInfo key;
            bool running = true;
            bool numericalMode = false;
            bool commaNow = false;
            do
            {
                if (numericalMode)
                {
                    MenuSelected(menu, menu[indexCurrent]);
                }
                else
                {
                    Menu(menu, menu[indexCurrent]);
                }
                Console.WriteLine("\nCurrent prediction: " + Perceptron.Classes[perceptron.Classify(weights)]);
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow && !numericalMode)
                {
                    if (indexCurrent + 1 < menu.Count)
                    {
                        indexCurrent++;
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow && !numericalMode)
                {
                    if (indexCurrent - 1 >= 0)
                    {
                        indexCurrent--;
                    }
                }
                else if (key.Key == ConsoleKey.Enter && !numericalMode)
                {
                    if (indexCurrent == size - 1)
                    {
                        running = false;
                    }
                    else
                    {
                        numericalMode = true;
                        MenuSelected(menu, menu[indexCurrent]);
                    }
                }
                else if (key.Key == ConsoleKey.Enter && numericalMode)
                {
                    numericalMode = false;
                    Menu(menu, menu[indexCurrent]);
                }
                else if (numericalMode && key.Key >= ConsoleKey.D0 && key.Key <= ConsoleKey.D9)
                {
                    weights[indexCurrent] = Double.Parse(weights[indexCurrent].ToString() + (commaNow ? "." : "") + (key.Key - ConsoleKey.D0));
                    menu[indexCurrent] = "w" + (indexCurrent + 1) + " = " + weights[indexCurrent];
                    if (commaNow)
                    {
                        commaNow = false;
                    }
                    Menu(menu, menu[indexCurrent]);
                }
                else if (numericalMode && key.Key >= ConsoleKey.NumPad0 && key.Key <= ConsoleKey.NumPad9)
                {
                    weights[indexCurrent] = Double.Parse(weights[indexCurrent].ToString() + (commaNow ? "." : "") + (key.Key - ConsoleKey.NumPad0));
                    menu[indexCurrent] = "w" + (indexCurrent + 1) + " = " + weights[indexCurrent];
                    if (commaNow)
                    {
                        commaNow = false;
                    }
                    Menu(menu, menu[indexCurrent]);
                }
                else if (numericalMode && key.Key == ConsoleKey.Backspace)
                {
                    if (commaNow)
                    {
                        commaNow = false;
                        menu[indexCurrent] = "w" + (indexCurrent + 1) + " = " + weights[indexCurrent];
                    }
                    else
                    {
                        try
                        {
                            weights[indexCurrent] = Double.Parse(weights[indexCurrent].ToString()[..^1]);
                        }
                        catch (System.Exception)
                        {
                            weights[indexCurrent] = 0;
                        }
                        menu[indexCurrent] = "w" + (indexCurrent + 1) + " = " + weights[indexCurrent];
                    }
                    Menu(menu, menu[indexCurrent]);
                }
                else if (numericalMode && (key.Key == ConsoleKey.OemPeriod || key.Key == ConsoleKey.OemComma))
                {
                    if (!commaNow)
                    {
                        commaNow = true;
                        menu[indexCurrent] = "w" + (indexCurrent + 1) + " = " + weights[indexCurrent] + ".";
                    }
                }

            }
            while (running);
        }

        public static void MenuSelected(List<string> menu, string selected)
        {
            Console.Clear();
            foreach (string item in menu)
            {
                if (item == selected)
                    Console.WriteLine("   " + item + " < ");
                else
                    Console.WriteLine(" " + item);
            }
            Credit();
        }

        public static void Credit()
        {
            var (Left, Top) = Console.GetCursorPosition();
            var consoleH = Console.WindowTop + Console.WindowHeight - 1;
            Console.SetCursorPosition(0, consoleH);
            Console.Write("Oleksandr Volkdoav, s28021, for PJATK, 2023/2024");
            Console.SetCursorPosition(Left, Top);
        }
    }
}