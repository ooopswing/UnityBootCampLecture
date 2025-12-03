using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleShooterChasingEnemy
{
    class Program
    {
        static int playerX, playerY;
        static int playerLife;
        static int score;

        static List<Bullet> playerBullets;
        static List<Enemy> enemies;

        const int fireDelay = 5;
        static int fireCooldown = 0;

        static Random rnd = new Random();
        static int enemySpawnTimer = 0;
        const int enemySpawnInterval = 50;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(60, 25);
            Console.SetBufferSize(60, 25);

            while (true)
            {
                ShowTitleScreen();
                InitializeGame();
                RunGameLoop();
                ShowGameOverScreen();
                if (!WaitForRestartOrExit()) break; // 재게임 여부 선택
            }
        }

        // ------------------------
        // 게임 초기화
        // ------------------------
        static void InitializeGame()
        {
            playerX = Console.WindowWidth / 2;
            playerY = Console.WindowHeight - 2;
            playerLife = 3;
            score = 0;

            playerBullets = new List<Bullet>();
            enemies = new List<Enemy>();
            fireCooldown = 0;
            enemySpawnTimer = 0;
        }

        // ------------------------
        // 게임 타이틀 화면
        // ------------------------
        static void ShowTitleScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2);
            Console.Write("운석들의 공격");

            for (int i = 0; i < 5; i++)
            {
                Console.Beep(600, 200);
                Thread.Sleep(300);
            }
        }

        // ------------------------
        // 메인 게임 루프
        // ------------------------
        static void RunGameLoop()
        {
            while (playerLife > 0)
            {
                fireCooldown++;
                enemySpawnTimer++;

                HandleInput();
                UpdatePlayerBullets();
                SpawnEnemies();
                UpdateEnemies();
                CheckCollisions();
                Render();

                Thread.Sleep(30);
            }
        }

        // ------------------------
        // 키 입력 처리
        // ------------------------
        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow && playerX > 1)
                    playerX--;
                else if (keyInfo.Key == ConsoleKey.RightArrow && playerX < Console.WindowWidth - 2)
                    playerX++;

                if (keyInfo.Key == ConsoleKey.Spacebar && fireCooldown > fireDelay)
                {
                    playerBullets.Add(new Bullet(playerX, playerY - 1));
                    fireCooldown = 0;
                    Console.Beep(800, 50);
                }

                if (keyInfo.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
            }
        }

        // ------------------------
        // 플레이어 총알 이동
        // ------------------------
        static void UpdatePlayerBullets()
        {
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                playerBullets[i].Y--;
                if (playerBullets[i].Y < 0)
                    playerBullets.RemoveAt(i);
            }
        }

        // ------------------------
        // 적 생성
        // ------------------------
        static void SpawnEnemies()
        {
            if (enemySpawnTimer >= enemySpawnInterval)
            {
                int spawnCount = rnd.Next(2, 6);
                for (int i = 0; i < spawnCount; i++)
                {
                    int enemyX = rnd.Next(1, Console.WindowWidth - 2);
                    int speed = rnd.Next(3, 9);
                    enemies.Add(new Enemy(enemyX, 0, speed));
                }
                enemySpawnTimer = 0;
            }
        }

        // ------------------------
        // 적 이동 (플레이어 X 추적)
        // ------------------------
        static void UpdateEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].MoveTowardsPlayer(playerX, Console.WindowWidth, Console.WindowHeight);

                if (enemies[i].Y >= Console.WindowHeight)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        // ------------------------
        // 충돌 처리
        // ------------------------
        static void CheckCollisions()
        {
            // 플레이어 총알과 적 충돌
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                Bullet b = playerBullets[i];
                bool hit = false;

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Enemy e = enemies[j];
                    if (b.X == e.X && b.Y == e.Y)
                    {
                        playerBullets.RemoveAt(i);
                        enemies.RemoveAt(j);
                        score += 10;
                        Console.Beep(1000, 80);
                        hit = true;
                        break;
                    }
                }
                if (hit) continue;
            }

            // 플레이어와 적 충돌
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy e = enemies[i];
                if (e.X == playerX && e.Y == playerY)
                {
                    playerLife--;
                    enemies.RemoveAt(i);
                    Console.Beep(400, 150);
                }
            }
        }

        // ------------------------
        // 화면 렌더링
        // ------------------------
        static void Render()
        {
            Console.Clear();

            Console.SetCursorPosition(1, 1);
            Console.Write($"Score: {score}");

            Console.SetCursorPosition(Console.WindowWidth - 15, 1);
            Console.Write($"Life: {playerLife}");

            Console.SetCursorPosition(playerX, playerY);
            Console.Write("^");

            foreach (var b in playerBullets)
            {
                Console.SetCursorPosition(b.X, b.Y);
                Console.Write("*");
            }

            foreach (var e in enemies)
            {
                Console.SetCursorPosition(e.X, e.Y);
                Console.Write("@");
            }
        }

        // ------------------------
        // 게임오버 화면
        // ------------------------
        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
            Console.WriteLine("GAME OVER");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2 + 1);
            Console.Write($"Final Score: {score}");
        }

        // ------------------------
        // 다시 플레이 / 종료 선택
        // ------------------------
        static bool WaitForRestartOrExit()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 + 3);
            Console.Write("SPACE: Restart  /  ESC: Exit");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Spacebar) return true;
                if (key == ConsoleKey.Escape) return false;
            }
        }
    }

    class Bullet
    {
        public int X, Y;
        public Bullet(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Enemy
    {
        public int X, Y;
        private int moveTimer = 0;
        private int moveDelay;

        public Enemy(int x, int y, int speed)
        {
            X = x;
            Y = y;
            moveDelay = speed;
        }

        public void MoveTowardsPlayer(int playerX, int maxWidth, int maxHeight)
        {
            moveTimer++;
            if (moveTimer < moveDelay) return;
            moveTimer = 0;

            Y++; // 아래로 이동

            // 플레이어 X 좌표를 향해 이동
            if (playerX > X) X++;
            else if (playerX < X) X--;

            if (X < 1) X = 1;
            if (X > maxWidth - 2) X = maxWidth - 2;
        }
    }
}
