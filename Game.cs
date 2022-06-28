using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe
{
    internal class Game
    {
        private char[] arr = new char[9];
        private char icon = ' ', player1Char = '-', player2Char = '+';
        private int turnCount = 0, minmaxCallCount = 0;
        Random random = new Random();

        internal void Start()
        {
            string message = "";
            

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = ' ';
            }
            
            while(true)
            {

                if (CheckWin(arr))
                {
                    if (turnCount % 2 == 0)
                    { 
                        message = $"player 2 has won!";
                        break;
                    }
                    message = $"player 1 has won!";
                    break;
                }
                if(turnCount == arr.Length)
                {
                    message = $"draw!";
                    break;
                }
                 
                if (turnCount % 2 == 0)
                {
                    icon = player1Char;
                    Console.WriteLine(AI(false) + 1);
                    //input((AI(false) + 1), icon);
                    //continue;
                }
                else
                {
                //    Console.WriteLine(AI(true)+1);
                    icon = player2Char;
                    Console.WriteLine(AI(true) + 1);
                    //    //input((AI(true) + 1), icon);
                }
                //icon = player2Char;

                Console.WriteLine(Print(arr));
                Console.WriteLine(message);
                Console.WriteLine($"turnCount = {turnCount}");
                Console.WriteLine("Enter a number:");

                if (!int.TryParse(Console.ReadLine(), out int key))
                {
                    message = "not a valid input";
                    continue;
                }
                input(key, icon);
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine(Print(arr));
            Console.WriteLine($"turnCount = {turnCount}");
            Console.WriteLine(message);


            void input(int key, char icon)
            {
                if (key < 1 || key > arr.Length)
                {
                    message = "not a valid number";
                    return;
                }
                if (arr[key - 1] != ' ')
                {
                    message = "not a valid position";
                    return;
                }
                arr[key - 1] = icon;
                message = "";
                turnCount++;
            }
        }

        private bool CheckWin(char[] arr)
        {
            return 
            ( 
                //horizontal
                Check(0, 1, 2) ||
                Check(3, 4, 5) ||
                Check(6, 7, 8) ||
                //vertical
                Check(0, 3, 6) ||
                Check(1, 4, 7) ||
                Check(2, 5, 8) ||
                // Diagonal
                Check(0, 4, 8) ||
                Check(2, 4, 6)
            );

            bool Check(int a, int b, int c) => (arr[a] != ' ' && arr[a] == arr[b] && arr[b] == arr[c]);
        }


        private int AI(bool minimize)
        {
            minmaxCallCount = 0;
            int bestIndex = 0;
            int[] positions = new int[arr.Length];
            int bestEval = 0;

            if (minimize)
            {
                Console.WriteLine("minimize");
                bestEval = 1000;
            }
            else
            {
                Console.WriteLine("maximize");
                bestEval = -1000;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == ' ')
                {
                    Console.WriteLine("original:");
                    Console.WriteLine(Print(arr));
                    char[] newboardCopy = (char[])arr.Clone();
                    newboardCopy[i] = icon;

                    positions[i] = MiniMax(newboardCopy, minimize);
                    Console.WriteLine("=============================");

                    Console.WriteLine($" position {i + 1}: {positions[i]}");

                    if (minimize && bestEval > positions[i])
                    {
                        bestEval = positions[i];
                    }
                    else if(!minimize && bestEval < positions[i])
                    {
                        bestEval = positions[i];
                    }
                    continue;
                }
                Console.WriteLine($" position {i + 1}: #");
            }

            //choose random index where eval == bestEval
            List<int> positionsBestEval = new List<int>();
            Console.WriteLine();
            Console.WriteLine("pick random out of these positions:");
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == bestEval && arr[i] == ' ')
                {
                    positionsBestEval.Add(i);
                    Console.Write($"{positionsBestEval.Last() + 1}, ");
                }
            }
            
            bestIndex = positionsBestEval[random.Next(0, positionsBestEval.Count())];

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"best eval: {bestEval}");
            Console.WriteLine($"use of recursion: {minmaxCallCount-1}");
            Console.WriteLine($"random optimal position: {bestIndex + 1}");
            return bestIndex;
        }


        private int MiniMax(char[] boardCopy, bool maximize)
        {
            minmaxCallCount++;

            Console.WriteLine(Print(boardCopy));

            if (CheckWin(boardCopy)&&maximize)
            {
                Console.WriteLine($"win for maximizing player: ({player2Char}):, return -1");
                return -1;
            }
            if (CheckWin(boardCopy) && !maximize)
            {
                Console.WriteLine($"win for minimizing player: ({player1Char}):, return 1");
                return 1;
            }
            bool emtyPlace = false;
            for (int i = 0; i < 9; i++)
            {
                if (boardCopy[i] == ' ')
                {
                    emtyPlace = true;
                    break;
                }
            }

            if (!emtyPlace) return 0;

            int endResult;

            //Console.WriteLine();
            //Console.WriteLine(Print(boardCopy));

            if (maximize)
            {
                endResult = -1000;
                for (int i = 0; i < 9; i++)
                {
                    if (boardCopy[i] == ' ')
                    {
                        
                        char[] newboardCopy = (char[])boardCopy.Clone();
                        newboardCopy[i] = player1Char;

                        Console.WriteLine("maximize");

                        int newResult = MiniMax(newboardCopy, false);

                        if (newResult > endResult)
                        {
                            endResult = newResult;
                        }
                    }
                }
                return endResult;
            }

            endResult = 1000;
            for (int i = 0; i < 9; i++)
            {
                if (boardCopy[i] == ' ')
                {
                        
                    char[] newboardCopy = (char[])boardCopy.Clone();
                    newboardCopy[i] = player2Char;

                    Console.WriteLine("minimize");

                    int newResult = MiniMax(newboardCopy, true);

                    if (newResult < endResult)
                    {
                        endResult = newResult;
                    }
                }
            }
            return endResult;
            
        }

       private string Print(char[] arr)
        {
            return 
            "  _________________ \n" +
            " |1    |2    |3    |\n" +
            $" |  {arr[0]}  |  {arr[1]}  |  {arr[2]}  |\n" +
            " |_____|_____|_____|\n" +
            " |4    |5    |6    |\n" +
            $" |  {arr[3]}  |  {arr[4]}  |  {arr[5]}  |\n" +
            " |_____|_____|_____|\n" +
            " |7    |8    |9    |\n" +
            $" |  {arr[6]}  |  {arr[7]}  |  {arr[8]}  |\n" +
            " |_____|_____|_____|";
        }
            



    }
}
