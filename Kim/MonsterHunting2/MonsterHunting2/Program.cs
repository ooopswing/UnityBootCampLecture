namespace MonsterHunting2
{
    public class Character
    {
        public string Name;
        public int Hp;
        public int Atk;

        public Character(string name, int hp, int atk)
        {
            Name = name;
            Hp = hp;
            Atk = atk;
        }

        public virtual void Attack(Character target)
        {
            Console.WriteLine($"{Name} 공격! > {target.Name} HP: {target.Hp - Atk}");
            target.Hp -= Atk;
            if (target.Hp < 0) target.Hp = 0;
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }
    }

    public class Player : Character
    {
        public int Exp = 0; // 경험치
        public int Potions = 3; // 포션 개수

        public Player(string name, int hp, int atk) : base(name, hp, atk)
        {
        }

        public void GainExp(int amount)
        {
            Exp += amount;
            Console.WriteLine($"{Name}이(가) 경험치 +{amount} 획득! (현재 EXP: {Exp})");
        }

        // 포션 사용 메서드
        public void UsePotion()
        {
            if (Potions > 0)
            {
                Hp += 30; // 포션 사용 시 HP 30 회복
                Potions--;
                Console.WriteLine($"{Name}이(가) 포션 사용! HP +30 회복! (현재 HP: {Hp}, 남은 포션: {Potions})");
            }
            else
            {
                Console.WriteLine($"{Name}은(는) 포션이 없습니다!");
            }
        }

        public class Monster : Character
        {
            private static Random random = new Random();
            public Monster(string name) : base(name, random.Next(20, 51), random.Next(2, 7))
            {
            }
        }

        internal class Program
        {
            static string RandomName()
            {
                string[] names = { "슬라임", "고블린", "늑대", "박쥐" };
                return names[new Random().Next(names.Length)];
            }

            static void Main(string[] args)
            {
                Console.WriteLine("***** 몬스터 사냥 2 *****");

                Console.Write("플레이어 이름을 입력하세요: "); // 사용자에게 이름 입력 받기
                string playerName = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(playerName)) playerName = "불한당"; // 기본 이름 설정

                Player player = new Player(playerName, 100, 20);

                while (!player.IsDead())
                {
                    Monster monster = new Monster(RandomName());
                    Console.WriteLine($"\n몬스터 등장! {monster.Name} (HP: {monster.Hp}, ATK: {monster.Atk})");
                    bool fled = false;

                    while (!monster.IsDead() && !player.IsDead())// 전투 반복
                    {
                        Console.WriteLine("\n행동 선택: 1.공격 2.포션 3.줄행랑");
                        string choice = Console.ReadLine()!.ToLower();

                        switch (choice)
                        {
                            case "1":
                                player.Attack(monster);
                                Console.WriteLine($"{monster.Name}의 HP: {monster.Hp}");
                                break;

                            case "2":
                                player.UsePotion();
                                break;

                            case "3":
                                Console.WriteLine($"{player.Name}이(가) 도망쳤습니다!");
                                fled = true; // 줄행랑 플래그 설정
                                break;

                            default:
                                Console.WriteLine("잘못된 선택입니다. 다시 선택하세요.");
                                continue;
                        }

                        if (!monster.IsDead())
                        {
                            monster.Attack(player);
                            Console.WriteLine($"{player.Name}의 HP: {player.Hp}");
                        }
                    }

                    if (fled) continue; // 줄행랑했으면 다음 몬스터로

                    if (monster.IsDead())
                    {
                        Console.WriteLine($"\n{monster.Name}을(를) 물리쳤습니다!");
                        player.GainExp(10);
                    }
                    else if (player.IsDead())
                    {
                        Console.WriteLine($"\n{player.Name}이(가) 쓰러졌습니다... 게임 종료!");
                        break;
                    }

                    Console.WriteLine("\n계속 싸우시겠습니까? (y/n): ");
                    string cont = Console.ReadLine()!.ToLower();
                    if (cont != "y")
                    {
                        Console.WriteLine("게임 종료!");
                        break;
                    }
                }
            }
        }
    }
}
