using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace othelloai
{
    class Program
    {
        static public int[,] board = new int[8, 8];
        static int myturn;
        static int currentTurn;

        static void Main(string[] args)
        {
            Random random = new Random();



            myturn = random.Next(1, 3);
            currentTurn = 1;
            ResetBoard();

            PrintBoard();
            CheckTurn();

        }
        
        static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine(GetPrintBoard());
            Console.WriteLine("●"+CountBoard()[0]);
            Console.WriteLine("○"+CountBoard()[1]);
            Console.WriteLine(myturn);

            if (currentTurn == 1)
            {
                Console.WriteLine("●");
            }
            else
            {
                Console.WriteLine("○");
            }
            
        }

        static public string GetPrintBoard()
        {
            string printText = "";

            for (int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    if (board[i,j] == 0)
                    {
                        printText += "□";
                    }
                    else if(board[i, j] == 1)
                    {
                        printText += "●";//white 1
                    }
                    else
                    {
                        printText += "○";//black 2
                    }
                }
                printText += "\n";
            }

            return printText;
        }

        static void ResetBoard()
        {

            Array.Clear(board,8,8);
            board[3, 3] = 1; board[3, 4] = 2;
            board[4, 3] = 2; board[4, 4] = 1;
            
        }

        static List<string> CanPut(int row, int col)
        {
            List<string> canplace = new List<string>();

            row--;
            col--;
            if (board[row, col] != 0)
            {
                return canplace;
            }

            for (int i = 1; i >= -1; i--)
            {
                for (int j = 1; j >= -1; j--)
                {
                    int reverse = 1;


                    if (j == 0 && i == 0)
                    {
                        continue;
                    }

                    if (row + i * reverse < 0 || row + i * reverse > 7 || col + j * reverse < 0 || col + j * reverse > 7)
                    {
                        continue;
                    }



                    while (true)
                    {
                        if (row + i * reverse < 0 || row + i * reverse > 7 || col + j * reverse < 0 || col + j * reverse > 7)
                        {
                            reverse = 0;
                            break;
                        }

                        if (board[row + i * reverse, col + j * reverse] != GetEnemy())
                        {
                            Console.WriteLine($"{row + i * reverse}  {col + j * reverse}");



                            if (board[row + i * reverse, col + j * reverse] == currentTurn)
                            {
                                reverse--;
                                break;
                            }
                            else
                            {
                                reverse = 0;
                                break;
                            }
                        }

                        reverse++;
                    }

                    if (reverse > 0)
                    {
                        canplace.Add($"{i}:{j}");
                    }


                }
            }

            
            return canplace;
        }



        static bool Put(int row, int col)
        {
            List<string> place = CanPut(row,col);

            if (place.Count == 0)
            {
                return false;
            }

            Console.WriteLine($"row : {row}  col : {col}");
            row--;
            col--;


            foreach(string p in place)
            {
                int i = int.Parse(p.Split(":")[0]);
                int j = int.Parse(p.Split(":")[1]);

                int reverse = 1;

                while (true)
                {
                    
                    if (row + i * reverse < 0 || row + i * reverse > 7 || col + j * reverse < 0 || col + j * reverse > 7)
                    {
                        reverse = 0;
                        break;
                    }

                    if (board[row + i * reverse, col + j * reverse] != GetEnemy())
                    {
                        Console.WriteLine($"{row + i * reverse}  {col + j * reverse}");



                        if (board[row + i * reverse, col + j * reverse] == currentTurn)
                        {
                            reverse--;
                            break;
                        }
                        else
                        {
                            reverse = 0;
                            break;
                        }
                    }

                    reverse++;
                }

                for (int k = reverse; k > 0; k--)//reverse만큼 지우기
                {
                    board[row + i * k, col + j * k] = currentTurn;
                }

            }


            board[row, col] = currentTurn;

            return true;
        }

        static int GetEnemy()
        {
            if (currentTurn == 2)
            {
                return 1;
            }

            return 2;
        }

        static void AiTurn()
        {
            PrintBoard();
            Console.WriteLine("인공지능 차례입니다.");

            for(int i=1; i<=8; i++)
            {
                for (int j = 1; j <=8; j++)
                {
                    if (Put(i, j))
                    {
                        ChangeTurn();
                    }
                }
            }

            ChangeTurn();
        }

        static void Myturn()
        {
            PrintBoard();

            

            Console.WriteLine("당신의 차례입니다.");

            while (true)
            {
                string inputValue = Console.ReadLine();

                PutInfo info = CheckInput(inputValue);

                if (info.isvalid)
                {
                    if (Put(info.row, info.col))
                    {
                        Console.WriteLine("test");
                        break;
                    }
                    
                    
                }
              
            }

            ChangeTurn();

            
        }

        class PutInfo
        {
            public bool isvalid;
            public int row;
            public int col;
        }

        static PutInfo CheckInput(string value)
        {
            PutInfo pi = new PutInfo();
            if (value.Length == 3)
            {
                Regex regex = new Regex(@"[1-8] [1-8]");
                if (regex.IsMatch(value))
                {
                    pi.isvalid = true;
                    pi.row = int.Parse(value.Split(' ')[0]);
                    pi.col = int.Parse(value.Split(' ')[1]);
                    return pi;
                }
                
            }
            
            pi.isvalid = false;
            return pi;

        }

        static void ChangeTurn()
        {
            currentTurn = GetEnemy();
            CheckTurn();
        }

        static void CheckTurn()
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (CanPut(i, j).Count > 0)
                    {
                        if (currentTurn == myturn)
                        {
                            Myturn();
                        }
                        else
                        {
                            AiTurn();
                        }
                    }
                }
            }

            ChangeTurn();

            
        }

        static int[] CountBoard()
        {
            int[] count = new int[2];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i,j] == 1)
                    {
                        count[0]++;//white
                    }
                    else if (board[i,j] == 2)
                    {
                        count[1]++;//black
                    }
                }
            }

            return count;
        }
    }
}
