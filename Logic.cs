using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public class State
    {
        public const int Alive = 1;
        public const int Dead = 0;
    }

    public class Grid
    {
        int _gridHeight;
        int _gridWidth;

        public List<Generation> generations = new List<Generation>();

        public Grid(int height, int width)
        {
            _gridHeight = height;
            _gridWidth = width;
        }

        /// <summary>
        /// Initialize the grid with a random generation.
        /// </summary>
        public void initRandom()
        {
            Generation randomGeneration = new Generation(_gridWidth, _gridHeight);
            randomGeneration.GenerateRandomGeneration();
            generations.Add(randomGeneration);
        }
    }

    public class Cell
    {
        int _coordY;
        int _coordX;

        public int state = State.Dead; // default state of cell is dead
        public int aliveAdjacentCellsCount;

        public List<Cell> aliveAdjacentCells;
        public Cell(int coordX, int coordY)
        {
            this._coordX = coordX;
            this._coordY = coordY;
        }

        /// <summary>
        /// Checks if the given cell A is ajdacent to the current cell.
        /// </summary>
        /// <param name="A">Cell to be checked.</param>
        /// <returns>True if the given cell is adjacent, else false.</returns>
        private bool IsAdjacentCell(Cell A)
        {
            if ((this._coordX - 1 == A._coordX || this._coordX + 1 == A._coordX) && (this._coordY + 1 == A._coordY || this._coordY == A._coordY || this._coordY - 1 == A._coordY))
                return true;
            else if ((this._coordX == A._coordX) && (this._coordY + 1 == A._coordY || this._coordY - 1 == A._coordY))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if the cells in the given list are adjacent to the current one.
        /// </summary>
        /// <param name="cells">Cells to be checked</param>
        /// <returns>List of adjacent cells.</returns>
        private List<Cell> GetAdjacentCells(Cell[,] cells)
        {
            List<Cell> adjacentCells = new List<Cell>();
            foreach (Cell c in cells)
            {
                if (this.IsAdjacentCell(c))
                    adjacentCells.Add(c);
            }

            return adjacentCells;
        }

        /// <summary>
        /// Update the current cell's properties:
        /// aliveAdjacentCellsCount
        /// aliveAdjacentCells
        /// </summary>
        /// <param name="cells"></param>
        public void GetAliveAjacentCells(Cell[,] cells)
        {
            List<Cell> adjacentCells = GetAdjacentCells(cells);
            List<Cell> aliveAjacentCells = new List<Cell>();
            foreach (Cell A in adjacentCells)
            {
                if (A.state == State.Alive)
                    aliveAjacentCells.Add(A);
            }

            this.aliveAdjacentCellsCount = aliveAjacentCells.Count;
            this.aliveAdjacentCells = aliveAjacentCells;
        }
    }

    public class Generation
    {
        static int _gridHeight;
        static int _gridWidth;

        int _generationAge = 0;
        int _cellsAlive;

        public Cell[,] cells = new Cell[_gridHeight, _gridWidth];

        public int generationAge
        {
            get { return _generationAge; }
            set { _generationAge = value; }
        }

        public Generation(int width, int height)
        {
            _gridHeight = height;
            _gridWidth = width;
        }

        /// <summary>
        /// Check how many cells in the current generation are alive and update counter.
        /// </summary>
        /// <returns>Count of alive cells</returns>
        public int GetAliveCells()
        {
            int cellsAlive = 0;
            foreach (Cell C in cells)
            {
                if (C.state == State.Alive)
                    cellsAlive++;
            }

            return cellsAlive;
        }

        /// <summary>
        /// Generate a random generation of cells within in given grid's height and width.
        /// </summary>
        public void GenerateRandomGeneration()
        {
            var random = new Random();
            cells = new Cell[_gridHeight, _gridWidth];
            for (var y = 0; y < _gridHeight; y++)
            {
                for (var x = 0; x < _gridWidth; x++)
                {
                    cells[x, y] = new Cell(x, y);
                    // Equals the probability of being dead(0) or alive(1)
                    cells[x, y].state = random.Next(0, 2);
                }
            }
        }

        /// <summary>
        /// Draw the current generation to the console.
        /// Additional: age of current generation and count of alive cells.
        /// </summary>
        public void DrawGeneration()
        {
            Console.WriteLine("Generation age: {0}", _generationAge);
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    Console.Write("{0}", cells[y, x].state);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            _cellsAlive = GetAliveCells();
            Console.WriteLine("Cells alive: {0}", _cellsAlive);
        }

        /// <summary>
        /// Function to process the current generation.
        /// loops through all cells in the cells in the current generation, updates their state based on the following rules and populates
        /// the next generation with the updated cells.
        /// 
        /// Rules:
        /// 1. Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
        /// 2. Any live cell with two or three live neighbours lives on to the next generation.
        /// 3. Any live cell with more than three live neighbours dies, as if by overpopulation.
        /// 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        /// source: https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#Rules
        /// </summary>
        /// <returns>next generation</returns>
        public Generation ProcessGeneration()
        {
            Generation nextGeneration = new Generation(_gridHeight, _gridWidth);
            nextGeneration.generationAge = _generationAge + 1;
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    Cell currentCell = cells[x, y];
                    currentCell.GetAliveAjacentCells(cells);

                    Cell nextGenCell = new Cell(x, y);
                    nextGeneration.cells[x, y] = nextGenCell;
                    if (currentCell.state == State.Dead && currentCell.aliveAdjacentCellsCount == 3)
                        nextGenCell.state = State.Alive;
                    if (currentCell.state == State.Alive && (currentCell.aliveAdjacentCellsCount == 2 || currentCell.aliveAdjacentCellsCount == 3))
                        nextGenCell.state = State.Alive;
                }
            }

            return nextGeneration;
        }
    }
}