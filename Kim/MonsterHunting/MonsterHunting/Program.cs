namespace MonsterHunting
{
    //캐릭터 클래스
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

        //플레이어 클래스
        public class Player : Character
        {
            public int Exp = 0;

            public Player(string name, int hp, int atk) : base(name, hp, atk)
            {
            }

            public void GainExp(int amount)
            {
                Exp += amount;
            }
        }

        //몬스터 클래스
        public class Monster : Character
        {
            private static Random rand = new Random();

            public Monster(string name) : base(name, rand.Next(20, 51), rand.Next(2, 7))
            {
            }
        }
        //메인 프로그램
        internal class Program
        {
            static string RandomName()
            {
                string[] names = { "슬라임", "고블린", "늑대", "박쥐" };

                return names[new Random().Next(names.Length)];
            }

            static void Main(string[] args)
            {
                Console.WriteLine("***** 몬스터 사냥 *****");
                Player player = new Player("용사", 40, 8);

                while (true)
                {
                    //몬스터 등장
                    Monster monster = new Monster(RandomName());
                    Console.WriteLine($"\n몬스터 등장! {monster.Name} (HP: {monster.Hp}, ATK: {monster.Atk})");

                    //전투 반복
                    while (!monster.IsDead() && !player.IsDead())
                    {
                        //플레이어 공격
                        player.Attack(monster);

                        //몬스터 사망 확인
                        if (monster.IsDead())
                        {
                            Console.WriteLine($"{monster.Name} 처치! 경험치 +10");
                            player.GainExp(10);
                            Console.WriteLine($"현재 EXP: {player.Exp}");
                            break;
                        }

                        //몬스터 공격
                        monster.Attack(player);

                        //플레이어 사망 확인
                        if (player.IsDead())
                        {
                            Console.WriteLine($"{player.Name}이(가) 죽었습니다...");
                            Console.WriteLine("게임 종료");
                            return;
                        }
                    }

                    Console.WriteLine($"\n계속 싸우시겠습니까? (y/n):");
                    
                    string input = Console.ReadLine()!.ToLower();

                    if (input != "y")
                    {
                        Console.WriteLine("게임 종료");
                        break;
                    }
                }
            }
        }
    }
}
