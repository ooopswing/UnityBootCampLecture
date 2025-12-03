using System.Threading;

namespace Program
{
    class Program
    {
        /*    enum ClassType
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
        */
        abstract class Animal
        {
            public string Name;
            
            public Animal()
            {

            }
            public Animal(string Name)
            {
                Console.WriteLine($"안녕하세요, 저는 {Name}입니다.");
            }
            public abstract void MakeSound();
        }


        class Dog : Animal
        {
            public Dog(string Name)
            {
                Console.WriteLine($"안녕하세요, 저는 {Name}입니다.");
            }
            
            public override void MakeSound()
            {
                Console.WriteLine("멍멍!");
            }
        }

        class Cat : Animal
        {
            public Cat(string Name)
            {
                Console.WriteLine($"안녕하세요, 저는 {Name}입니다.");
            }

            public override void MakeSound()
            {
                Console.WriteLine("야옹~");
            }
        }

        class Cow : Animal
        {
            public Cow(string Name)
            {
                Console.WriteLine($"안녕하세요, 저는 {Name}입니다.");
            }

            public override void MakeSound()
            {
                Console.WriteLine("음머~");
            }
        }
 /*       abstract class Player
        {
            public int hp;
            public int mp;

            public void Move()
            {
                Console.WriteLine("이동");
            }

            public virtual void Attack()
            {
                Console.WriteLine("공격!");
            }

            public abstract void UseSpecialSkill();
        }


        class Knight : Player
        {
            public override void UseSpecialSkill()
            {
                Console.WriteLine("기사 스페셜스킬");
            }

            public override void Attack()
            {
                base.Attack();
                Console.WriteLine("기사의 공격!!!");
            }
        }

        class Mage : Player
        {
            public override void UseSpecialSkill()
            {
                Console.WriteLine("법사 스페셜스킬");
            }

            public override void Attack()
            {
                Console.WriteLine("법사의 공격!!!");
            }
        }

        class Archer : Player
        {
            public override void UseSpecialSkill()
            {
                Console.WriteLine("아쳐 스페셜스킬");
            }

        public override void Attack()
        {
            Console.WriteLine("아쳐의 공격!!!");
        }
*/

    
        
        static void Main()
        {
            Console.WriteLine("동물을 선택해주세요. [1]강아지 [2]고양이 [3]소");

            while (true)
            {
                string input = Console.ReadLine();
                Animal animal = null;
                

                switch (input)
                {
                    case "1":
                        animal = new Dog("강아지");
                        break;
                    case "2":
                        animal = new Cat("고양이");
                        break;
                    case "3":
                        animal = new Cow("소");
                        break;
                }
                if(animal != null)
                {
                    animal.MakeSound();
                }
            }    
            
            Console.WriteLine("동물 선택 [1]강아지 [2]고양이 [3]소");
        }
    }
}