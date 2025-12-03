using System;
using System.Collections.Generic;
using System.Threading;

namespace SideScrollingShooter_Final
{
    class Program
    {
        static int playerX, playerY;
        static int playerLife = 3;
        static int score = 0;

        static List<Bullet> playerBullets = new List<Bullet>();
        static List<Bullet> enemyBullets = new List<Bullet>();
        static List<Enemy> enemies = new List<Enemy>();

        static Random rnd = new Random();

        static int fireCooldown = 0;
        const int fireDelay = 3;

        static int enemySpawnTimer = 0;
        const int enemySpawnInterval = 30;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 30);
            Console.SetBufferSize(120, 30);

            while (true)
            {
                ShowTitleScreen();
                Initialize();
                GameLoop();
                ShowGameOverScreen();

                if (!WaitForRestartOrExit()) break;
            }
        }

        static void Initialize()
        {
            playerX = 5;
            playerY = Console.WindowHeight / 2;
            playerLife = 3;
            score = 0;
            playerBullets.Clear();
            enemyBullets.Clear();
            enemies.Clear();
            fireCooldown = 0;
            enemySpawnTimer = 0;
        }

        static void ShowTitleScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);
            Console.Write("운석들의 공격 2");

            // 같은 음 반복
            for (int i = 0; i < 6; i++)
            {
                Console.Beep(800, 300);
                Thread.Sleep(200);
            }
        }

        static void GameLoop()
        {
            while (playerLife > 0)
            {
                fireCooldown++;
                enemySpawnTimer++;

                HandleInput();
                UpdatePlayerBullets();
                SpawnEnemies();
                UpdateEnemies();
                UpdateEnemyBullets();
                CollisionCheck();
                Render();

                Thread.Sleep(25);
            }
        }

        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.W && playerY > 1) playerY--;
                if (key == ConsoleKey.S && playerY < Console.WindowHeight - 2) playerY++;
                if (key == ConsoleKey.A && playerX > 1) playerX--;
                if (key == ConsoleKey.D && playerX < Console.WindowWidth - 2) playerX++;

                if (key == ConsoleKey.Spacebar && fireCooldown > fireDelay)
                {
                    playerBullets.Add(new Bullet(playerX + 1, playerY, true));
                    Console.Beep(850, 30);
                    fireCooldown = 0;
                }

                // 게임 루프 내부에서 Esc 누르면 바로 종료
                if (key == ConsoleKey.Escape)
                    Environment.Exit(0);
            }
        }

        static void UpdatePlayerBullets()
        {
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                playerBullets[i].X++;
                if (playerBullets[i].X >= Console.WindowWidth)
                    playerBullets.RemoveAt(i);
            }
        }

        static void SpawnEnemies()
        {
            if (enemySpawnTimer >= enemySpawnInterval)
            {
                int count = rnd.Next(1, 4);
                for (int i = 0; i < count; i++)
                {
                    int y = rnd.Next(1, Console.WindowHeight - 2);
                    int speed = rnd.Next(1, 3);
                    enemies.Add(new Enemy(Console.WindowWidth - 4, y, speed));
                }
                enemySpawnTimer = 0;
            }
        }

        static void UpdateEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var e = enemies[i];
                e.Move();

                // 적 총알 발사 (적 이동 방향 기준)
                if (rnd.Next(0, 100) < 5)
                {
                    enemyBullets.Add(new Bullet(e.X - 1, e.Y, false));
                    Console.Beep(600, 20);
                }

                // 화면 끝에 도달 시 제거
                if (e.X <= 1)
                    enemies.RemoveAt(i);
            }
        }

        static void UpdateEnemyBullets()
        {
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].X--;
                if (enemyBullets[i].X < 0)
                    enemyBullets.RemoveAt(i);
            }
        }

        static void CollisionCheck()
        {
            // 플레이어 총알 ↔ 적 총알
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                bool destroyed = false;

                for (int j = enemyBullets.Count - 1; j >= 0; j--)
                {
                    if (playerBullets[i].X == enemyBullets[j].X &&
                        playerBullets[i].Y == enemyBullets[j].Y)
                    {
                        playerBullets.RemoveAt(i);
                        enemyBullets.RemoveAt(j);
                        Console.Beep(1200, 40); // 폭발음
                        destroyed = true;
                        break;
                    }
                }
                if (destroyed) continue;

                // 플레이어 총알 ↔ 적
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (playerBullets[i].X == enemies[j].X && playerBullets[i].Y == enemies[j].Y)
                    {
                        playerBullets.RemoveAt(i);
                        enemies.RemoveAt(j);
                        score += 10;
                        Console.Beep(1000, 60);
                        destroyed = true;
                        break;
                    }
                }
            }

            // 적 ↔ 플레이어
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].X == playerX && enemies[i].Y == playerY)
                {
                    enemies.RemoveAt(i);
                    playerLife--;
                    Console.Beep(350, 150);
                }
            }

            // 적 총알 ↔ 플레이어
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                if (enemyBullets[i].X == playerX && enemyBullets[i].Y == playerY)
                {
                    enemyBullets.RemoveAt(i);
                    playerLife--;
                    Console.Beep(350, 150);
                }
            }
        }

        static void Render()
        {
            Console.Clear();

            Console.SetCursorPosition(1, 1);
            Console.Write($"SCORE:{score}");

            Console.SetCursorPosition(Console.WindowWidth - 15, 1);
            Console.Write($"LIFE:{playerLife}");

            Console.SetCursorPosition(playerX, playerY);
            Console.Write(">");

            foreach (var b in playerBullets) { Console.SetCursorPosition(b.X, b.Y); Console.Write("-"); }
            foreach (var b in enemyBullets) { Console.SetCursorPosition(b.X, b.Y); Console.Write("-"); }
            foreach (var e in enemies) { Console.SetCursorPosition(e.X, e.Y); Console.Write("@"); }
        }

        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 2);
            Console.WriteLine("GAME OVER");
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2 - 1);
            Console.WriteLine($"Final Score: {score}");
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 17, Console.WindowHeight / 2);
            Console.WriteLine("Press SPACE to Restart or ESC to Exit");
        }

        static bool WaitForRestartOrExit()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Spacebar) return true;
                if (key == ConsoleKey.Escape) return false;
                // 다른 키는 무시
            }
        }
    }

    class Bullet
    {
        public int X, Y;
        public bool FromPlayer;
        public Bullet(int x, int y, bool fromPlayer)
        {
            X = x;
            Y = y;
            FromPlayer = fromPlayer;
        }
    }

    class Enemy
    {
        public int X, Y;
        int delay, timer;

        public Enemy(int x, int y, int spd)
        {
            X = x;
            Y = y;
            delay = spd;
        }

        public void Move()
        {
            timer++;
            if (timer < delay) return;
            timer = 0;

            X--;
            if (X < 0) X = 0; // 화면 끝 이하 방지, 제거는 UpdateEnemies에서 처리
        }
    }
}
