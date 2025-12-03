using System;
using System.Collections.Generic;
using System.Threading;

namespace SnackGame
{
    internal class Program
    {
        static int width = 60;
        static int height = 25;
        static List<(int X, int Y)> snake = new List<(int X, int Y)>();
        static List<(int X, int Y)> foods = new List<(int X, int Y)>();
        static List<(int X, int Y)> walls = new List<(int X, int Y)>();
        static Random rand = new Random();
        static int score = 0;
        static int dx = 1, dy = 0;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            while (true)
            {
                ShowTitleScreen();
                InitializeGame();
                GameLoop();
                ShowGameOverScreen();

                if (!WaitForRestartOrExit()) break;
            }
        }

        static void InitializeGame()
        {
            score = 0;
            dx = 1; dy = 0;
            InitializeSnake();
            InitializeFoods();
            InitializeWalls();
        }

        static void ShowTitleScreen()
        {
            Console.Clear();
            string title = "에덴동산";
            Console.SetCursorPosition(width / 2 - title.Length / 2, height / 2);
            Console.Write(title);

            // 3초 동안 동일 음 반복
            for (int i = 0; i < 3; i++)
            {
                Console.Beep(300, 1000); // 타이틀 사운드
            }
        }

        static void InitializeSnake()
        {
            snake.Clear();
            int startX = 20;
            int startY = 10;
            for (int i = 0; i < 10; i++)
                snake.Add((startX - i, startY));
        }

        static void InitializeFoods()
        {
            foods.Clear();
            int count = 10;
            while (foods.Count < count)
            {
                var newFood = (X: rand.Next(1, width - 1), Y: rand.Next(1, height - 1));
                if (!snake.Contains(newFood) && !foods.Contains(newFood))
                    foods.Add(newFood);
            }
        }

        static void InitializeWalls()
        {
            walls.Clear();
            for (int i = 0; i < 3; i++)
            {
                int baseX = rand.Next(5, width - 10);
                int baseY = rand.Next(2, height - 5);
                int pattern = rand.Next(3); // 0: 좌우 5, 1: 상하 5, 2: 십자
                switch (pattern)
                {
                    case 0:
                        for (int j = -2; j <= 2; j++)
                            walls.Add((baseX + j, baseY));
                        break;
                    case 1:
                        for (int j = -2; j <= 2; j++)
                            walls.Add((baseX, baseY + j));
                        break;
                    case 2:
                        for (int j = -2; j <= 2; j++)
                        {
                            walls.Add((baseX + j, baseY));
                            walls.Add((baseX, baseY + j));
                        }
                        break;
                }
            }
        }

        static void GameLoop()
        {
            while (foods.Count > 0)
            {
                HandleInput();
                MoveSnake();
                CheckApple();
                Render();
                Thread.Sleep(100);
            }
        }

        static void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: if (dy == 0) { dx = 0; dy = -1; } break;
                    case ConsoleKey.DownArrow: if (dy == 0) { dx = 0; dy = 1; } break;
                    case ConsoleKey.LeftArrow: if (dx == 0) { dx = -1; dy = 0; } break;
                    case ConsoleKey.RightArrow: if (dx == 0) { dx = 1; dy = 0; } break;
                }
            }
        }

        static void MoveSnake()
        {
            var head = snake[0];
            int newHeadX = head.X + dx;
            int newHeadY = head.Y + dy;

            // 벽 또는 화면 경계 체크
            if (walls.Contains((newHeadX, newHeadY)) || newHeadX <= 0 || newHeadX >= width - 1 || newHeadY <= 0 || newHeadY >= height - 1)
            {
                newHeadX = head.X;
                newHeadY = head.Y;
            }

            var newHead = (X: newHeadX, Y: newHeadY);
            snake.Insert(0, newHead);
            snake.RemoveAt(snake.Count - 1);

            // 이동 사운드 (작게)
            Console.Beep(200, 20);
        }

        static void CheckApple()
        {
            for (int i = foods.Count - 1; i >= 0; i--)
            {
                if (snake[0].X == foods[i].X && snake[0].Y == foods[i].Y)
                {
                    // 뱀 길이 증가
                    var last = snake[snake.Count - 1];
                    snake.Add((last.X, last.Y));
                    snake.Add((last.X, last.Y));

                    score += 10;

                    // 사과 먹는 소리
                    Console.Beep(800, 100);

                    // 사과 제거
                    foods.RemoveAt(i);
                }
            }
        }

        static void Render()
        {
            Console.Clear();

            // 스코어 출력
            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");

            // 벽 출력
            foreach (var w in walls)
            {
                Console.SetCursorPosition(w.X, w.Y);
                Console.Write("#");
            }

            // 사과 출력
            foreach (var food in foods)
            {
                Console.SetCursorPosition(food.X, food.Y);
                Console.Write("$");
            }

            // 뱀 출력
            for (int i = 0; i < snake.Count; i++)
            {
                Console.SetCursorPosition(snake[i].X, snake[i].Y);
                Console.Write(i == 0 ? "@" : "*");
            }

            Console.SetCursorPosition(width - 1, height - 1);
        }

        static void ShowGameOverScreen()
        {
            Console.Clear();
            string msg1 = "Game Over";
            string msg2 = $"Total Score: {score}";
            string msg3 = "Press SPACE to Restart or ESC to Exit";

            Console.SetCursorPosition(width / 2 - msg1.Length / 2, height / 2 - 2);
            Console.WriteLine(msg1);
            Console.SetCursorPosition(width / 2 - msg2.Length / 2, height / 2);
            Console.WriteLine(msg2);
            Console.SetCursorPosition(width / 2 - msg3.Length / 2, height / 2 + 2);
            Console.WriteLine(msg3);
        }

        static bool WaitForRestartOrExit()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Spacebar) return true;
                if (key == ConsoleKey.Escape) return false;
            }
        }
    }
}
