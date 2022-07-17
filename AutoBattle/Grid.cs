using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;
        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;
            Console.WriteLine($"{xLenght}, {yLength}\n");
            Console.WriteLine("The battle field has been created\n");
            int count = 0;
            for (int y = 0; y < Columns; y++)
            {
                for(int x = 0; x < Lines; x++)
                {
                    GridBox newBox = new GridBox((Team)0, x, y, false, count);
                    grids.Add(newBox);
                    //Console.Write($"{newBox.Index}\n");
                    count++;
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void drawBattlefield(int Lines, int Columns, Grid Battlefield, Team team = (Team)0)
        {
            //CheckGrid(Battlefield);

            int gridId = 0;
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox currentgrid = Battlefield.grids[gridId];

                    if (currentgrid.ocupied)
                    {
                        if(currentgrid.team == (Team)1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"[XX]\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if(currentgrid.team == (Team)2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"[XX]\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        //Console.Write($"[{currentgrid.Index.ToString("00")}]\t");
                        Console.Write($"[{currentgrid.xIndex},{currentgrid.yIndex}]\t");
                    }
                    gridId++;
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

        void CheckGrid(Grid Grid)
        {
            Grid.grids.Count();
            foreach (GridBox grid in Grid.grids)
            {
                Console.WriteLine(grid.xIndex + "\t" + grid.yIndex + "\t" + grid.ocupied);
            }
        }

    }
}
