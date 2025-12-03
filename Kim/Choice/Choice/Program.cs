using System.Threading;

namespace Program
{
    class Program
    {
        enum ClassType
        {
            None,
            Knight,
            Mage,
            Rogue
        }

        // 몬스터 클래스 이넘을 만들어주세요(None, Slime, Orc, Skeleton)
        enum MonsterType
        {
            None,
            Slime,
            Orc,
            Skeleton
        }

        struct Player
        {
            public int hp;
            public int atk;
        }

        // 몬스터 구조체를 만들어주세요(hp, atk) 
        struct Monster
        {
            public int hp;
            public int atk;
        }

        static ClassType ClassChoice()
        {
            Console.WriteLine("직업을 선택하세요!");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 마법사");
            Console.WriteLine("[3] 도둑");

            ClassType choice = ClassType.None;
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    choice = ClassType.Knight;
                    break;
                case "2":
                    choice = ClassType.Mage;
                    break;
                case "3":
                    choice = ClassType.Rogue;
                    break;
            }

            return choice;
        }

        static void CreatePlayer(ClassType choice, out Player player)
        {
            // 기사(100/10), 마법사(50/15), 도둑(75/12)
            switch (choice)
            {
                case ClassType.Knight:
                    player.hp = 100;
                    player.atk = 10;
                    break;
                case ClassType.Mage:
                    player.hp = 50;
                    player.atk = 15;
                    break;
                case ClassType.Rogue:
                    player.hp = 75;
                    player.atk = 12;
                    break;
                default:
                    player.hp = 0;
                    player.atk = 0;
                    break;
            }
        }

        static void CreateRandomMonster(out Monster monster)
        {
            // 랜덤한 몬스터 생성
            Random rand = new Random();
            int monsterChoice = rand.Next(1, 4); // 1~3 사이의 랜덤 숫자 생성
            
            MonsterType type = (MonsterType)monsterChoice;
            
            switch (type)
            {
                case MonsterType.Slime:
                    monster.hp = 20;
                    monster.atk = 2;
                    Console.WriteLine("슬라임이 나타났다! HP: 20, ATK: 2");
                    break;
            
                case MonsterType.Orc:
                    monster.hp = 40;
                    monster.atk = 4;
                    Console.WriteLine("오크가 나타났다! HP: 40, ATK: 4");
                    break;
                
                case MonsterType.Skeleton:
                    monster.hp = 30;
                    monster.atk = 3;
                    Console.WriteLine("스켈레톤이 나타났다! HP: 30, ATK: 3");
                    break;
                
                default:
                    monster.hp = 0;
                    monster.atk = 0;
                    break;
            }            
        }

        static void Main(string[] args)
        {
            ClassType choice = ClassType.None;

            Player player;

            while (true)
            {
                choice = ClassChoice();

                if (choice != ClassType.None)
                {
                    CreatePlayer(choice, out player);

                    Console.WriteLine($"HP {player.hp}, ATK {player.atk}");

                    Monster monster;
                    CreateRandomMonster(out monster);
                }
            }
        }
    }
}