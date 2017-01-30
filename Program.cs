using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            startTheGameOfLife(9, 9);
        }

        public static void startTheGameOfLife(int height, int width)
        {
            Grid myGrid = new Grid(height, width);
            myGrid.initRandom();

            for (var i = 0; i <= myGrid.generations.Count; i++)
            {
                Generation currentGeneration = myGrid.generations[i];

                string response;
                currentGeneration.DrawGeneration();

                Generation newGeneration = currentGeneration.ProcessGeneration();
                myGrid.generations.Add(newGeneration);
                if (newGeneration.GetAliveCells() == 0)
                {
                    newGeneration.DrawGeneration();
                    Console.WriteLine("Everyone is dead :(");

                    Console.WriteLine("Do you want to start a new game [Y/N]?");
                    response = Console.ReadLine();

                    if (response == "Y" || response == "y")
                        startTheGameOfLife(9, 9);

                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Do you want to continue? [Y/N]");
                    response = Console.ReadLine();
                    if (response == "N" || response == "n")
                    {
                        break;
                    }
                    else
                        continue;
                }
            }
        }
    }
}