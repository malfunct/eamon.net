using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EamonNet
{
    class Program
    {
        class Character
        {
            /// <summary>
            /// NAME$
            /// </summary>
            public string Name; //NAME$

            /// <summary>
            /// HD
            /// </summary>
            public int Hardiness; //HD

            /// <summary>
            /// AG
            /// </summary>
            public int Agility; //AG

            /// <summary>
            /// CH
            /// </summary>
            public int Charisma; //CH

            /// <summary>
            /// SA%() 4
            /// </summary>
            public int[] SpellAbility; //SA%() 4 Values

            public int[] CurrentSpellAbility;

            /// <summary>
            /// WA%() 5
            /// </summary>
            public int[] WeaponAbility; //WA%() 5 values

            /// <summary>
            /// AE
            /// </summary>
            public int ArmorExpertise; //AE

            /// <summary>
            /// SEX$
            /// </summary>
            public string Sex; //SEX$

            /// <summary>
            /// GOLD
            /// </summary>
            public int Gold; //GOLD

            /// <summary>
            /// BANK
            /// </summary>
            public int Bank; //BANK

            /// <summary>
            /// AC
            /// </summary>
            public int Armor; //AC

            /// <summary>
            /// WNAME$() 4
            /// </summary>
            public string[] WeaponName; //WNAME$() 4 values

            /// <summary>
            /// WTYPE$() 4
            /// </summary>
            public int[] WeaponType; //WTYPE$() 4 values

            /// <summary>
            /// WODDS%() 4
            /// </summary>
            public int[] WeaponOdds; //WODDS%() 4 values

            /// <summary>
            /// WDICE%() 4
            /// </summary>
            public int[] WeaponDice; //WDICE%() 4 values

            /// <summary>
            /// WSIDES%() 4
            /// </summary>
            public int[] WeaponSides; //WSIDES%() 4 values

            /// <summary>
            /// if the first character in a name is \n the character
            /// is adventuring and not available for use
            /// </summary>
            public bool Adventuring;

            public int CarriedWeight;

            public int ArmorWorn;

            public int ShieldWorn;
            
            public int EffectOfArmorOnOdds;

            public int Speed;

            public int LightSource;
        }

        class AdventureInfo
        {
            public string Name;

            public string Path;

            public int Version;

            public float MinorVersion;

            public int NumberOfRooms;

            public int NumberOfArtifacts;

            public int NumberOfEffects;

            public int NumberOfMonsters;

            public Room[] Rooms;

            public Artifact[] Artifacts;

            public int numberOfPlayerWeapons;

            public int numberOfPlayerArmor;

            public Monster[] Monsters;

            public string[] Effects;

            public int CurrentRoom;

            public int roomExited;

            public int roomToEnter;

            public bool enemyPresent;

            public bool exitAdventure;

            public int friendlyHardiness;

            public int enemyHardiness;

            public int friendlyDamageTaken;

            public int enemyDamageTaken;

            public bool invokeAttack;
        }

        class Artifact
        {
            public string Name;

            public string LongDescription;

            public int[] Data;
        }

        class Monster
        {
            public string Name;

            public string LongDescription;

            public bool seen = false;

            public int[] Data;

            public int index;
        }

        class Room
        {
            public string Name;

            public string Description;

            public int[] Directions;

            public int NaturalLight;

            public bool RoomVisted;
        }

        private DateTime _currentDate = DateTime.Now;

        private int _numberOfCharacters;
        private Character[] _characters;
        private int _theCharacter = -1;

        private AdventureInfo _currentAdventure;

        private const int CharacterRecordLength = 256;
        private const int RoomDescriptionRecordLength = 256;
        private const int ArtifactRecordLength = 128;
        private const int MonsterRecordLength = 128;
        private const int LR = 64;
        private const int LN = 64;

        const string Characterfile = "CHARACTERS";

        private readonly string[][] _snappy =
        {
            new[] {"HAVING BETTER THINGS TO","DO THAN WIPING THE NOSES OF GREENHORNS"},
            new[] {"YOUNG GREENHORNS ALWAYS","NEEDING SOMEONE TO LEAD THEM BY THE HAND"},
            new[] {"THE THINGS HE HAS TO GO","THROUGH FOR SOME STUPID UPSTARTS."},
            new[] {"HE SHOULD HAVE LISTENED","TO HIS MOTHER AND BEEN A BRAIN SURGEON."},
            new[] {"GOOD FER NOTHIN\' YOUNG","ADVENTURERS SHOULD BE TAKEN OUT AN HUNG."},
            new[] {"STRANGE SAXON NAMES YE ","CAN NEVER PRONOUNCE LET ALONE SPELL."},
            new[] {"HE SHOULD HAVE LISTENED","TO HIS MOTHER AND GONE INTO TAXIDERMY."},
            new[] {"HAVING TO COMPUTERIZE  ","THIS DAMN BOOK BUSINESS SOMETIME."},
            new[] {"HAVING SEEN UGLY FACES ","BEFORE BUT NEVER ANYTHING LIKE THIS."},
            new[] {"KNOW-IT-ALL ATTITUDES  ","OF THE NEW ADVENTURERS WILL DO THEM IN."},
            new[] {"IF HE GETS THE CHANCE  ","HE'S GOING TO GET OUTTA THIS RAT RACE."},
            new[] {"SOMEDAY LEAVING THIS   ","PLACE AN' RUNNIN OFF WITH THE BAR WENCH."},
            new[] {"EVERYBODY ALWAYS BEIN\'","IN A HURRY AND NEVER HAVE ANYTHIN TO DO."},
            new[] {"YOUNG ADVENTURERS NEVER","USIN\' THEIR RIGHTFUL NAME ANYWAYS."}
        };

        private Random _r;

        private string[] directions = { "NORTH", "SOUTH", "EAST", "WEST", "UP", "DOWN" };
        private string[] commands = { "GET", "DROP", "LOOK", "EXAMINE", "ATTACK", "FLEE", "GIVE", "INVENTORY", "BLAST", "HEAL", "POWER", "SPEED", "SMILE", "SAY", "READ", "READY", "SAVE", "LIGHT", "OPEN", "PUT", "DRINK", "FREE", "REQUEST", "WEAR", "REMOVE", "USE" };

        static void Main()
        {
            Program p = new Program {_r = new Random()};
            p.FrontDesk();
            
            p.MainHall();

            p.SaveCharacters();
        }

        private void FrontDesk()
        {
            this.ReadCharacters();

            Console.WriteLine("     EAMON ADVENTURER'S GUILD");
            Console.WriteLine("     7625 HAWKHAVEN DR.");
            Console.WriteLine("     CLEMMONS, NC 27012");
            Console.WriteLine("     (919)766-7490\r\n\r\n");

            int snappyNumber = this._r.Next(this._snappy.Length);
            string c1 = _snappy[snappyNumber][0];
            string c2 = _snappy[snappyNumber][1];

            Console.WriteLine("   YOU ARE IN THE OUTER CHAMBER OF THE");
            Console.WriteLine("HALL OF THE GUILD OF FREE ADVENTURERS.");
            Console.WriteLine("MANY MEN AND WOMEN ARE GUZZLING BEER");
            Console.WriteLine("AND THERE IS LOUD SINGING AND LAUGHTER.\r\n\r\n");
            Console.WriteLine("   ON THE NORTH SIDE OF THE CHAMBER IS");
            Console.WriteLine("A CUBBYHOLE WITH A DESK.  OVER THE DESK");
            Console.WriteLine("IS A SIGN WHICH SAYS \'REGISTER HERE");
            Console.WriteLine("OR ELSE!\'\r\n\r\n\r\n\r\n");

            Console.WriteLine("DO YOU GO OVER TO THE DESK OR JOIN THE");
            Console.WriteLine("   MEN DRINKING THE BEER?\r\n");

            string answer;

            do
            {
                Console.WriteLine("  (ENTER 'D' FOR DESK OR 'M' FOR MEN)  ");
                answer = Input().ToLowerInvariant();
            } while (answer != "d" && answer != "m");

            if (answer.StartsWith("m"))
            {
                Console.WriteLine("\r\n\r\n   AS YOU GO OVER TO THE MEN, YOU FEEL");
                Console.WriteLine("\r\nA SWORD BEING THRUST THROUGH YOUR BACK");
                Console.WriteLine("\r\nAND YOU HEAR SOMEONE SAY, 'YOU REALLY");
                Console.WriteLine("\r\nMUST LEARN TO FOLLOW DIRECTIONS!\'");
                Environment.Exit(0);
            }

            Console.WriteLine("\r\n\r\n   YOU ARE GREETED THERE BY A BURLY");
            Console.WriteLine("\r\nIRISHMAN WHO LOOKS AT YOU WITH A SCOWL");
            Console.WriteLine("\r\nAND ASKS YOU, \'WHAT'S YOUR NAME?\'\r\n");
            Console.WriteLine("YOU GIVE HIM YOUR NAME (TYPE IT IN NOW)        ");

            string name = Input();

            while (name.Length > 20)
            {
                Console.WriteLine("\r\n   HE SCOWLS AT YOU AND SAYS, \'YER");
                Console.WriteLine("\r\nNAME\'S TOO LONG FER ME BOOK.  I CANNA");
                Console.WriteLine("\r\nUSE MORE THAN TWENTY LETTERS.\r\n");
                name = Input();
            }

            int tries = 1;
            while (string.IsNullOrWhiteSpace(name) || !char.IsLetter(name.ElementAt(0)))
            {
                tries = ProcessCharacterDoesNotGiveName(tries, out name);
            }

            Console.WriteLine("  HE STARTS LOOKING THROUGH HIS BOOK,");
            Console.WriteLine("\r\nMUTTERING ABOUT {0}", c1);
            Console.WriteLine("\r\n{0}\r\n", c2);

            do
            {
                for (int i = 0; i < this._numberOfCharacters; i++)
                {
                    if (name.ToLowerInvariant() == this._characters[i].Name.ToLowerInvariant() && !this._characters[i].Adventuring)
                    {
                        this._theCharacter = i;
                        
                        Console.WriteLine("\r\n   HE LOOKS UP AND SAYS, \'AH, HERE YE");
                        Console.WriteLine("\r\nBE!  WELL, GO AND HAVE FUN IN THE HALL!\'\r\n");
                    }
                    else if(name.ToLowerInvariant() == this._characters[i].Name.ToLowerInvariant())
                    {
                        //TODO: implement restore saved game
                        //if player chooses to restore saved game
                        //start the adventure with this save
                    }
                }

                if (this._theCharacter < 0)
                {
                    Console.WriteLine("   HE EVENTUALLY LOOKS AT YOU AND SAYS,");
                    Console.WriteLine("\r\n\'YER NAME\'S NA IN HERE. HAVE YE GIVEN");
                    Console.WriteLine("\r\nIT TO ME ARIGHT?\'");
                    Console.WriteLine("HOW DO YOU ANSWER (ENTER 'Y' OR 'N')");

                    do
                    {
                        answer = Input().ToLowerInvariant();
                    } while (answer != "y" && answer != "n");

                    if (answer == "y")
                    {
                        Console.WriteLine("\r\n   HE HITS HIS FOREHEAD AND SAYS, \'AH,");
                        Console.WriteLine("\r\nYE MUST BE NEW HERE!  WELL, WAIT JUST");
                        Console.WriteLine("\r\nA MINUTE AND I'LL BRING SOMEONE OUT TO");
                        Console.WriteLine("\r\nTAKE CARE OF YE.\'\r\n");

                        Character newChar = this.CreateNewCharacter(name);
                        bool overwrite = false;
                        
                        //insert character into next available space
                        for (int i = 0; i < this._numberOfCharacters; i++)
                        {
                            if (this._characters[i].Adventuring)
                            {
                                this._theCharacter = i;
                                this._characters[i] = newChar;
                                overwrite = true;
                            }
                        }

                        //No empty space was found so appent to end of array
                        if (!overwrite)
                        {
                            this._theCharacter =
                                this._numberOfCharacters; //set current character to last character in characters
                            this._characters[this._numberOfCharacters] =
                                this.CreateNewCharacter(name); //create new character and put it in the array
                            _numberOfCharacters++; //we now have one additional character in the array
                        }
                    }
                    else
                    {
                        tries = ProcessCharacterDoesNotGiveName(tries, out name);
                    }
                }
            } while (this._theCharacter < 0);
        }

        private Character CreateNewCharacter(string name)
        {
            Character newChar = new Character();

            Console.WriteLine("\r\nTHE IRISHMAN SAYS, \'FIRST I MUST KNOW");
            Console.WriteLine("WHETHER YE BE MALE OR FEMALE. WHICH ARE");

            do
            {
                Console.WriteLine("YE(ENTER \'M\' FOR MALE, \'F\' FOR FEMALE)?");
                newChar.Sex = Input().ToLowerInvariant();
            } while (newChar.Sex!= "m" && newChar.Sex != "f");

            Console.WriteLine("\r\nTHE IRISHMAN WALKS AWAY AND IN WALKS A");
            Console.WriteLine("TALL MAN OF POSSIBLY ELVISH DESCENT.");
            Console.WriteLine("\r\nHE STUDIES YOU FOR A MOMENT AND SAYS,");
            Console.WriteLine("\'HERE IS A BOOKLET OF INSTRUCTION FOR");
            Console.WriteLine("YOU TO READ, AND YOUR PRIME ATTRIBUTES");
            Console.WriteLine("ARE--");

            string answer;

            bool suicide = false;
            do
            {
                newChar.Hardiness = RollDice(3, 8);
                newChar.Agility = RollDice(3, 8);
                newChar.Charisma = RollDice(3, 8);

                Console.WriteLine($"\r\n   HARDINESS--{newChar.Hardiness}   AGILITY--{newChar.Agility}   CHARISMA--{newChar.Charisma}");

                if (newChar.Hardiness + newChar.Agility + newChar.Charisma < 39)
                {
                    Console.WriteLine("\r\nYOUR CHARACTER IS SUCH A POOR EXCUSE");
                    Console.WriteLine("\r\nFOR AN ADVENTURER THAT WE WILL ALLOW");
                    Console.WriteLine("\r\nYOU TO COMMIT SUICIDE. ENTER \'Y\' TO");
                    Console.WriteLine("COMMIT SUICIDE, ELSE \'N\'");

                    do
                    {
                        answer = Input().ToLowerInvariant();
                    } while (answer != "y" && answer != "n");

                    if (answer == "y")
                    {
                        Console.WriteLine("WE RESSURRECT YOU AGAIN AND YOUR PRIME");
                        Console.WriteLine("ATTRIBUTES ARE --");
                        suicide = true;
                    }
                }
            } while (suicide);

            Console.WriteLine("(ENTER \'R\' TO READ INSTRUCTIONS, \'T\' TO    GIVE THEM BACK)");

            do
            {
                answer = Input().ToLowerInvariant();
            } while (answer != "t" && answer != "r");

            if (answer == "r")
            {
                Console.WriteLine("YOU READ THE INSTRUCTIONS AND THEY SAY--");
                Console.WriteLine("  INFORMATION ABOUT THE WORLD OF EAMON  "); //Inverse Text?
                Console.WriteLine("\r\nYOU WILL HAVE TO BUY A WEAPON. YOUR");
                Console.WriteLine("CHANCE TO HIT WITH IT WILL BE THE");
                Console.WriteLine("WEAPON COMPLEXITY, PLUS YOUR ABILITY");
                Console.WriteLine("IN THAT CLASS, PLUS TWICE YOUR AGILITY.\r\n");
                Console.WriteLine("THE FIVE CLASSES OF WEAPONS (AND YOUR");
                Console.WriteLine("CURRENT ABILITIES WITH EACH) ARE--");
                Console.WriteLine("   CLUB/MACE       20%");
                Console.WriteLine("   SPEAR           10%");
                Console.WriteLine("   AXE              5%");
                Console.WriteLine("   SWORD            0%");
                Console.WriteLine("   BOW            -10%\r\n");
                Console.WriteLine("EVERY TIME YOU SCORE A HIT IN BATTLE,");
                Console.WriteLine("YOUR ABILITY IN THE WEAPON CLASS MAY GO");
                Console.WriteLine("UP BY 2%, IF A RANDOM NUMBER FROM 1-100");
                Console.WriteLine("IS LESS THAN YOUR CHANCE TO MISS!\r\n");
                Console.WriteLine("(HIT ANY KEY TO CONTINUE)");
                Console.ReadKey(true);
                Console.WriteLine("\r\nTHERE ARE FOUR ARMOR TYPES, AND YOU MAY");
                Console.WriteLine("ALSO CARRY A SHIELD IF YOU DO NOT USE");
                Console.WriteLine("A TWO-HANDED WEAPON.  THESE PROTECTIONS");
                Console.WriteLine("WILL ABSORB HITS PLACED UPON YOU(ALMOST");
                Console.WriteLine("ALWAYS!) BUT THEY LOWER YOUR CHANCE TO");
                Console.WriteLine("HIT. THE PROTECTIONS ARE--\r\n");
                Console.WriteLine("ARMOR          HITS PROTECT  ODDS ADJUST");
                Console.WriteLine(" NONE             0                - 0%");
                Console.WriteLine(" LEATHER          1                -10%");
                Console.WriteLine(" CHAIN            2                -20%");
                Console.WriteLine(" PLATE            5                -60%");
                Console.WriteLine(" SHIELD           1                - 5%\r\n\r\r\n");
                Console.WriteLine("YOU WILL DEVELOP AN ARMOUR EXPERTISE,");
                Console.WriteLine("WHICH WILL GO UP WHEN YOU HIT A BLOW");
                Console.WriteLine("WEARING ARMOUR AND YOUR EXPERTISE IS");
                Console.WriteLine("LESS THAN THE ARMOUR YOU ARE WEARING.");
                Console.WriteLine("(HIT ANY KEY TO CONTINUE)   ");
                Console.ReadKey(true);
                Console.WriteLine("YOU CAN CARRY WEIGHTS UP TO TEN TIMES");
                Console.WriteLine($"YOUR HARDINESS, OR {newChar.Hardiness * 10} GRONDS.");
                Console.WriteLine("(A MEASURE OF WEIGHT, ONE GROND=10 DOS)\r\n");
                Console.WriteLine("ADDITIONALLY, YOUR HARDINESS TELLS HOW");
                Console.WriteLine("MANY POINTS OF DAMAGE YOU CAN SURVIVE.");
                Console.WriteLine($"THEREFORE, YOU CAN BE HIT WITH {newChar.Hardiness}");
                Console.WriteLine("1-POINT BLOWS BEFORE YOU DIE.\r\n");
                Console.WriteLine("HOWEVER, YOU WILL NOT BE TOLD HOW MANY");
                Console.WriteLine("BLOWS YOU HAVE TAKEN.  YOU WILL BE");
                Console.WriteLine("MERELY TOLD THINGS SUCH AS\r\n");
                Console.WriteLine("   \'WOW!  THAT ONE HURT!\'");
                Console.WriteLine("OR \'YOU DON\'T FEEL VERY WELL\'\r\n");
                Console.WriteLine("(HIT ANY KEY TO CONTINUE)   ");
                Console.ReadKey(true);
                Console.WriteLine($"YOUR CHARISMA ({newChar.Charisma}) AFFECTS HOW CITIZENS");
                Console.WriteLine("OF EAMON REACT TO YOU.  YOU AFFECT A");
                Console.WriteLine("MONSTER\'S FRIENDLINESS RATING BY YOUR");
                Console.WriteLine("CHARISMA LESS TEN, DIFFERENCE TIMES TWO");
                Console.WriteLine($"({(newChar.Charisma - 10) * 2}%)\r\n");
                Console.WriteLine("YOU START OFF WITH 200 GOLD PIECES,");
                Console.WriteLine("WHICH YOU WILL WANT TO SPEND ON SUPPLIES");
                Console.WriteLine("FOR YOUR FIRST ADVENTURE.  YOU WILL GET");
                Console.WriteLine("A LOWER PRICE FOR ITEMS IF YOUR CHARISMAIS HIGH.\r\n");
                Console.WriteLine("AFTER YOU BEGIN TO ACCUMULATE WEALTH,");
                Console.WriteLine("YOU MAY WANT TO PUT SOME OF YOUR MONEY");
                Console.WriteLine("INTO THE BANK, WHERE IT CANNOT BE");
                Console.WriteLine("STOLEN.  HOWEVER, IT IS A GOOD IDEA TO");
                Console.WriteLine("CARRY SOME GOLD WITH YOU FOR USE IN");
                Console.WriteLine("BARGAINING AND RANSOM SITUATIONS.\r\n");
                Console.WriteLine("(HIT ANY KEY TO CONTINUE)  ");
                Console.ReadKey(true);
                Console.WriteLine("YOU MAY ALSO HIRE A WIZARD TO TEACH YOU");
                Console.WriteLine("SOME MAGIC SPELLS. THERE ARE FOUR");
                Console.WriteLine("SPELLS YOU MAY LEARN.\r\n");
                Console.WriteLine("BLAST--HURT YOUR ENEMIES FROM A DISTANCE");
                Console.WriteLine("BLAST--HURT YOUR ENEMIES FROM A DISTANCE");
                Console.WriteLine("HEAL --REMOVE DAMAGE FROM YOUR BODY.");
                Console.WriteLine("SPEED--DOUBLE YOUR DEXTERITY FOR A TIME");
                Console.WriteLine("POWER--DOES SOMETHING WEIRD.  THE EXACT");
                Console.WriteLine("       EFFECT IS UNPREDICTABLE.\r\n\r\n");
                Console.WriteLine("OTHER TYPES OF MAGIC MAY WORK IN VARIOUS");
                Console.WriteLine("ADVENTURES, AND ITEMS MAY HAVE SPECIAL");
                Console.WriteLine("PROPERTIES.  HOWEVER, THESE WILL NOT");
                Console.WriteLine("WORK IN OTHER ADVENTURES THAN WHERE THEY");
                Console.WriteLine("WERE FOUND.  THUS, IT IS BEST (AND YOU");
                Console.WriteLine("HAVE NO CHOICE BUT TO) SELL ALL ITEMS");
                Console.WriteLine("FOUND IN ADVENTURES, EXCEPT FOR WEAPONS");
                Console.WriteLine("AND ARMOUR.\r\n");
                Console.WriteLine("(HIT ANY KEY TO CONTINUE)   ");
                Console.ReadKey(true);
                Console.WriteLine("\r\nTHE MAN BEHIND THE DESK TAKES BACK THE");
                Console.WriteLine("INSTRUCTIONS AND SAYS, \'IT IS NOW TIME");
                Console.WriteLine("FOR YOU TO START YOUR LIFE.\'  HE MAKES");
                Console.WriteLine("AN ODD SIGN WITH HIS HAND AND SAYS,");
                Console.WriteLine("\'LIVE LONG AND PROSPER.\'\r\n");
                Console.WriteLine("YOU NOW WANDER INTO THE MAIN HALL.");

                newChar.Name = name;
                newChar.SpellAbility = new[] {0, 0, 0, 0};
                newChar.WeaponAbility = new[] {5, -10, 20, 10, 0};
                newChar.ArmorExpertise = 0;
                newChar.Gold = 200;
                newChar.Bank = 0;
                newChar.Armor = 0;
                newChar.WeaponName = new[] {"none", "none", "none", "none"};
                newChar.WeaponType = new[] {0, 0, 0, 0};
                newChar.WeaponOdds = new[] {0, 0, 0, 0};
                newChar.WeaponDice = new[] {0, 0, 0, 0};
                newChar.WeaponSides = new[] {0, 0, 0, 0};
            }

            return newChar;
        }

        private static int ProcessCharacterDoesNotGiveName(int tries, out string name)
        {
            switch (tries)
            {
                case 0:
                case 1:
                    Console.WriteLine(" HE PULLS OUT A SWORD AND BEGINS TO");
                    Console.WriteLine("SHARPEN IT, SAYING \'YE\'D BEST BE GIVIN\'");
                    Console.WriteLine("ME YER NAME LADDIE, IF YE KNOW WOTS");
                    Console.WriteLine("GOOD FER YE!!!");
                    break;
                case 2:
                    Console.WriteLine("I\'VE \'BOUT HAD ME FILL O\' YER SICK");
                    Console.WriteLine("SENSA \'UMOR!!");
                    Console.WriteLine("NOW GIMME YER NAME!!");
                    break;
                case 3:
                    Console.WriteLine("THE MAN CUTS ONE OF YOUR FINGERS OFF!!");
                    Console.WriteLine("HE THEN EATS IT!!!!");
                    Console.WriteLine("THEN HE SAYS \'ARE YE READY T\' TALK NOW?");
                    break;
                default:
                    if (tries > 12)
                    {
                        Console.WriteLine("THE MAN STARTS SLOWLY, \'WELL YE BE ");
                        Console.WriteLine("OUTTA FINGERS!\'");
                        Console.WriteLine("THE MAN THEN SPINS AROUND AND RUNS YOU");
                        Console.WriteLine("THROUGH WITH A SPEED YOU HAVE NEVER");
                        Console.WriteLine("SEEN BEFORE! (AND NEVER WILL AGAIN.)");
                        Environment.Exit(0);
                    }

                    Console.WriteLine("THE MAN CUTS OFF ANOTHER FINGER!!!");
                    Console.WriteLine("HE EATS THIS ONE TOO!!");
                    break;
            }

            tries++;
            Console.WriteLine("YOU GIVE HIM YOUR NAME (TYPE IT IN NOW)        ");
            name = Input();
            return tries;
        }

        private void MainHall()
        {
            bool exit = false;

            Console.WriteLine("     EAMON ADVENTURER'S GUILD");
            Console.WriteLine("     7625 HAWKHAVEN DR.");
            Console.WriteLine("     CLEMMONS, NC 27012");

            do
            {
                switch (this.GetMainHallMenuChoice())
                {
                    case 1:
                        MainHallAdventure();
                        break;
                    case 2:
                        MainHallShopping();
                        break;
                    case 3:
                        MainHallWizard();
                        break;
                    case 4:
                        MainHallBank();
                        break;
                    case 5:
                        MainHallExamineAbility();
                        break;
                    case 6:
                        Console.WriteLine("AS YOU LEAVE THE HALL, THE IRISHMAN");
                        Console.WriteLine("COMES UP TO YOU, SLAPS YOU ON THE BACK");
                        Console.WriteLine("AND SAYS, \'Y\'ALL COME BACK REAL SOON,");
                        Console.WriteLine("YA HEAH?\'");
                        exit = true;
                        break;
                }
            } while (!exit);
        }

        private void MainHallAdventure()
        {
            AdventureInfo[] adventures = GetAdventureList();
            int adventureNumber = -1;

            do
            {
                int offset = 0;
                do
                {
                    int max = 6;
                    if (offset + 6 >= adventures.Length)
                    {
                        max = adventures.Length - offset;
                    }
                    
                    for (int i = 0; i < max; i++)
                    {
                        Console.WriteLine($"<{i}> {adventures[offset + i].Name}");
                    }
                    Console.WriteLine($"<{max}> LIST MORE ADVENTURES");
                    Console.WriteLine($"(Enter 0 to {max} to choose.)");

                    string answer;
                    int choice;
                    do
                    {
                        answer = Input();
                    } while (!int.TryParse(answer, out choice) || choice < 0 || choice > max);

                    if (choice != max)
                    {
                        adventureNumber = offset + choice;
                    }

                    offset = offset + max;
                } while (adventureNumber < 0 && offset < adventures.Length);
            } while (adventureNumber < 0);

            this._currentAdventure = adventures[adventureNumber];

            this.GoOnAdventure();
        }

        private void GoOnAdventure()
        {
            this.ReadDataFiles();
            _currentAdventure.roomToEnter = 1;

            do
            {
                ProcessEnterRoom();
                ProcessCommand();
            } while (!this._currentAdventure.exitAdventure);

            //do exit adventure checks
        }

        private void ProcessCommand()
        {
            Console.WriteLine("YOUR COMMAND? ");
            string answer;
            do
            {
                answer = Input().Trim();
            } while (string.IsNullOrWhiteSpace(answer));

            int firstSpace = answer.IndexOf(' ');

            string verb = "";
            string obj = "";

            if(firstSpace < 0)
            {
                verb = answer.ToUpper();
            }
            else
            {
                verb = answer.Substring(0, firstSpace).Trim().ToUpper();
                obj = answer.Substring(firstSpace).Trim().ToUpper();
            }

            if(this.directions.Contains(verb))
            {
                this.MovePlayer(verb);
            }
            else if(this.commands.Contains(verb))
            {
                switch(verb)
                {
                    case "GET":
                    case "PICK":
                        GetItem(obj);
                        break;
                    case "DROP":
                        DropItem(obj);
                        break;
                    case "ATTACK":
                        
                }

                CheckForMonsterAttack();
            }
            else
            {
                Console.WriteLine("I ONLY UNDERSTAND THESE COMMANDS--");
                for(int i = 0; i<this.directions.Length; i++)
                {
                    Console.WriteLine($"  {this.directions[i]}");
                }
                for(int i=0; i<this.commands.Length; i++)
                {
                    Console.WriteLine($"  {this.commands[i]}");
                }
            }
        }

        private void CheckForMonsterAttack()
        {
            if(!_currentAdventure.enemyPresent || !_currentAdventure.invokeAttack)
            {
                return;
            }

            for(int i=1; i<_currentAdventure.NumberOfMonsters; i++)
            {
                Monster currentMonster = _currentAdventure.Monsters[i];

                if(currentMonster.Data[5] == _currentAdventure.CurrentRoom)
                {
                    bool friendly = currentMonster.Data[14] == 1;

                    float monsterRating;

                    if(friendly)
                    {
                        monsterRating = ((float)_currentAdventure.friendlyDamageTaken / (float)_currentAdventure.friendlyHardiness) - ((float)_currentAdventure.enemyDamageTaken / (float)_currentAdventure.enemyDamageTaken) / 5 + _r.Next(41) - 20;

                        if (currentMonster.Data[4] < monsterRating)
                        {
                            //flee
                            Console.WriteLine($"\r\n{ currentMonster.Name} FLEES OUT AN EXIT.\r\n");
                            int room = PickRandomExit(CurrentRoom());

                            currentMonster.Data[5] = room;
                            _currentAdventure.friendlyHardiness -= currentMonster.Data[1];
                            _currentAdventure.friendlyHardiness -= currentMonster.Data[13];
                        }
                        else
                        {
                            //enemy monsters attack
                            for(int j = 1; j < _currentAdventure.NumberOfMonsters; j++)
                            {
                                Monster possibleEnemy = _currentAdventure.Monsters[j];
                                if(possibleEnemy.Data[5] == _currentAdventure.CurrentRoom && possibleEnemy.Data[14] == 0)
                                {
                                    this.attack(currentMonster, possibleEnemy);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        monsterRating = ((float)_currentAdventure.enemyDamageTaken / (float)_currentAdventure.enemyHardiness) - ((float)_currentAdventure.friendlyDamageTaken / (float)_currentAdventure.friendlyDamageTaken) / 5 + _r.Next(41) - 20;

                        if (currentMonster.Data[4] < monsterRating)
                        {
                            Console.WriteLine($"\r\n{ currentMonster.Name} FLEES OUT AN EXIT.\r\n");
                            int room = PickRandomExit(CurrentRoom());

                            currentMonster.Data[5] = room;
                            _currentAdventure.enemyHardiness -= currentMonster.Data[1];
                            _currentAdventure.enemyHardiness -= currentMonster.Data[13];
                        }
                        else
                        {
                            if (_currentAdventure.friendlyHardiness == _currentAdventure.Monsters[0].Data[1])
                            {
                                this.attack(currentMonster, _currentAdventure.Monsters[0]);
                            }
                            else
                            {
                                for (int j = 1; j < _currentAdventure.NumberOfMonsters; j++)
                                {
                                    Monster possibleEnemy = _currentAdventure.Monsters[j];
                                    if (possibleEnemy.Data[5] == _currentAdventure.CurrentRoom && possibleEnemy.Data[14] == 1 && _r.Next(4) < 1)
                                    {
                                        this.attack(currentMonster, possibleEnemy);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (_currentAdventure.enemyDamageTaken < _currentAdventure.enemyHardiness)
                {
                    _currentAdventure.enemyPresent = false;
                    break;
                }
            }
        }

        private void attack(Monster attacker, Monster defender)
        {
            if(attacker.Data[9] == -1)
            {
                return;
            }

            int damage;
            float skill;
            int a;

            Console.WriteLine($"{attacker.Name} ATTACKS {defender.Name}");
            int roll = _r.Next(1, 101);
            if(roll < 5 || roll < attacker.Data[10])
            {
                // hit
                if(_r.Next(1, 101) > attacker.Data[10])
                {
                    attacker.Data[10] += 2;
                }
                damage = attacker.Data[11];
                skill = attacker.Data[12];
                a = 1;

                if(roll > 5)
                {
                    Console.WriteLine(" -- A HIT!");
                }
                else
                {
                    Console.WriteLine(" -- A CRITICAL HIT!");
                    int criticalRoll = _r.Next(1, 101);

                    if(criticalRoll < 51)
                    {
                        a = 0;
                    }
                    else if( criticalRoll < 86)
                    {
                        skill *= (float)1.5;
                    }
                    else if( criticalRoll < 96)
                    {
                        damage *= 2;
                    }
                    else if ( criticalRoll < 100)
                    {
                        damage *= 3;
                    }
                    else
                    {
                        this.KillMonster(defender, 0);
                        return;
                    }
                }
            }
            else
            {
                // miss
                if (roll < 97)
                {
                    Console.WriteLine(" -- A MISS.");
                    return;
                }
                // fumble
                Console.WriteLine(" -- A FUMBLE!");
                int fumbleRoll = _r.Next(1, 101);
                if (fumbleRoll < (35 + 40 * (attacker.Data[9] == 0 ? 1 : 0)))
                {
                    Console.WriteLine(" -- FUMBLE RECOVERED.");
                    return;
                }

                if (fumbleRoll < 76)
                {
                    Console.WriteLine(" -- WEAPON DROPPED!");
                    _currentAdventure.Artifacts[attacker.Data[9]].Data[4] = _currentAdventure.CurrentRoom;
                    attacker.Data[9] = -1;
                    return;
                }

                if (fumbleRoll < 95)
                {
                    Console.WriteLine(" -- WEAPON BROKEN!");
                    _currentAdventure.Artifacts[attacker.Data[9]].Data[4] = 0;
                    attacker.Data[9] = -1;

                    if (_r.Next(11) > 9)
                    {
                        return;
                    }

                    Console.WriteLine(" -- BROKEN WEAPON HURTS USER!");
                }

                if(fumbleRoll != 100)
                {
                    damage = attacker.Data[11];
                    skill = attacker.Data[12];
                    a = 1;
                    defender = attacker;
                }
                else
                {
                    damage = attacker.Data[11] * 2;
                    skill = attacker.Data[12];
                    a = 0;
                    defender = attacker;
                }
            }

            int d2 = 0;

            for(int i = 0; i < damage; i++)
            {
                d2 += (int)(skill * _r.NextDouble() + 1);
            }

            d2 -= a * defender.Data[8];

            if(d2 < 1)
            {
                Console.WriteLine("  BLOW BOUNCES OFF ARMOR\r\n");
                return;
            }

            defender.Data[13] += d2;
            if(defender.Data[13] > defender.Data[1])
            {
                this.KillMonster(defender, d2);
                return;
            }

            if(defender.Data[14] == 0) //enemy monster
            {
                _currentAdventure.enemyDamageTaken += d2;
            }
            else
            {
                _currentAdventure.friendlyDamageTaken += d2;
            }

            switch(defender.Data[13] * 5 / defender.Data[1] + 1)
            {
                case 1:
                    Console.WriteLine($"\r\n{defender.Name} TAKES DAMAGE BUT");
                    Console.WriteLine("  IS STILL IN GOOD SHAPE.");
                    break;
                case 2:
                    Console.WriteLine($"\r\n{defender.Name} IS HURTING.");
                    break;
                case 3:
                    Console.WriteLine($"\r\n{defender.Name} IS IN PAIN.");
                    break;
                case 4:
                    Console.WriteLine($"\r\n{defender.Name} IS VERY BADLY INJURED.");
                    break;
                case 5:
                    Console.WriteLine($"\r\n{defender.Name} IS AT DEATH'S DOOR,");
                    Console.WriteLine("   KNOCKING LOUDLY.");
                    break;
            }
        }

        private void KillMonster(Monster defender, int damage)
        {
            Console.WriteLine($"{defender.Name} IS DEAD!");
            if(defender.Data[14] == 0) //enemy
            {
                _currentAdventure.enemyDamageTaken = _currentAdventure.enemyDamageTaken + defender.Data[1] - defender.Data[13] + damage;
            }
            else
            {
                _currentAdventure.friendlyDamageTaken = _currentAdventure.friendlyDamageTaken + defender.Data[1] - defender.Data[13] + damage;
            }

            defender.Data[5] = 0;
            _currentAdventure.Artifacts[_currentAdventure.NumberOfArtifacts - 1 - _currentAdventure.NumberOfMonsters + defender.index].Data[4] = _currentAdventure.CurrentRoom;
            if(defender.Data[9] > 0)
            {
                _currentAdventure.Artifacts[defender.Data[9]].Data[4] = _currentAdventure.CurrentRoom;
            }

            if(_currentAdventure.enemyDamageTaken >= _currentAdventure.enemyHardiness)
            {
                _currentAdventure.enemyPresent = false;
            }

            if(defender.index == 0)
            {
                // player death (GOTO 23010)
            }

            if (defender.Name == "PIRATE")
            {
                // print pirate special message
                _currentAdventure.Artifacts[10].Data[8] = 6;
                CurrentRoom().RoomVisted = false;
            }

            if(defender.Name == "MIMIC")
            {
                Console.WriteLine("\r\nAS THE MIMIC DIES, IT ROLLS OVER AND");
                Console.WriteLine("YOU FIND A RING UNDERNEATH IT.");
                _currentAdventure.Artifacts[6].Data[4] = _currentAdventure.CurrentRoom;
            }
        }

        private int PickRandomExit(Room room)
        {
            int exit = -1;
            int count = 0;
            int index = 0;
            int max = _r.Next(1,room.Directions.Length);
            while (count < max)
            {
                if (room.Directions[index] > 0) //valid room
                {
                    exit = room.Directions[index];
                    count++;
                }

                index++;
                index = index % room.Directions.Length;
            }

            return exit;
        }

        private void GetItem(string obj)
        {
            if(obj == "TORCH")
            {
                Console.WriteLine("\r\nALL TORCHES ARE BOLTED TO THE WALL AND");
                Console.WriteLine("AND CANNOT BE REMOVED.\r\n");
                return;
            }

            if(obj == "ALL")
            {
                for(int i = 1; i < _currentAdventure.NumberOfArtifacts; i++)
                {
                    Artifact currentArtifact = _currentAdventure.Artifacts[i];

                    if(currentArtifact.Data[4] == _currentAdventure.CurrentRoom)
                    {
                        if((_characters[_theCharacter].CarriedWeight + currentArtifact.Data[3]) < (10 * _currentAdventure.Monsters[0].Data[1]))
                        {
                            Console.WriteLine($"{currentArtifact.Name} TAKEN.");
                            currentArtifact.Data[4] = -1;
                            _characters[_theCharacter].CarriedWeight += currentArtifact.Data[3];
                        }
                    }
                }

                _currentAdventure.invokeAttack = true;
                return;
            }

            if(obj == "RAT")
            {
                for(int i = 14; i < 17; i++)
                {
                    Artifact currentArtifact = _currentAdventure.Artifacts[i];
                    if(currentArtifact.Data[4] == _currentAdventure.CurrentRoom)
                    {
                        obj = currentArtifact.Name.ToUpper();
                    }
                }
            }
            
            if(obj == "COINS" || obj == "GOLD")
            {
                obj = _currentAdventure.Artifacts[5].Name;
            }

            for(int i = 1; i < _currentAdventure.NumberOfArtifacts; i++)
            {
                Artifact currentArtifact = _currentAdventure.Artifacts[i];

                if (obj == currentArtifact.Name && currentArtifact.Data[4] == _currentAdventure.CurrentRoom)
                {
                    if ((_characters[_theCharacter].CarriedWeight + currentArtifact.Data[3]) < (10 * _currentAdventure.Monsters[0].Data[1]))
                    {
                        Console.WriteLine("\r\nGOT IT.");
                        currentArtifact.Data[4] = -1;
                        _characters[_theCharacter].CarriedWeight += currentArtifact.Data[3];

                        if (currentArtifact.Data[2] < 2 || _currentAdventure.Monsters[0].Data[9] != -1)
                        {
                            _currentAdventure.invokeAttack = true;
                        }
                        return;
                    }

                    Console.WriteLine("\r\nIT IS TOO HEAVY FOR YOU.\r\n");
                    return;
                }
            }

            Console.WriteLine($"\r\nI SEE NO {obj} HERE!\r\n");
        }

        private void DropItem(string obj)
        {
            throw new NotImplementedException();
        }

        private void MovePlayer(string verb)
        {
            if(this._currentAdventure.enemyPresent)
            {
                Console.WriteLine("YOU CAN'T TURN YOUR BACK HERE!");
                return;
            }

            int dirNumber = 0;
            for(int i=0;i<this.directions.Length;i++)
            {
                if(verb.Equals(this.directions[i]))
                {
                    dirNumber = i;
                    break;
                }
            }

            int roomToCheck = this._currentAdventure.Rooms[this._currentAdventure.CurrentRoom].Directions[dirNumber];

            if(roomToCheck > 500) // deal with door
            {
                Artifact door = this._currentAdventure.Artifacts[roomToCheck - 500];

                if(door.Data[8] > 0) //door hidden
                {
                    Console.WriteLine("\r\nYOU CAN'T GO THAT WAY!");
                    CurrentRoom().RoomVisted = false;
                    return;
                }

                if(door.Data[6] != 0) //door locked
                {
                    Console.WriteLine($"\r\nTHE {door.Name} BLOCKS THE WAY!");
                    return;
                }
                else
                {
                    roomToCheck = door.Data[5];
                }
            }

            if(roomToCheck == -99) //return to main hall
            {
                this._currentAdventure.exitAdventure = true;
                return;
            }

            if(roomToCheck <= 0 || roomToCheck >= this._currentAdventure.NumberOfRooms)
            {
                Console.WriteLine("\r\nYOU CAN'T GO THAT WAY!");
                CurrentRoom().RoomVisted = false;
                return;
            }

            this._currentAdventure.roomToEnter = roomToCheck;
        }

        private void ProcessEnterRoom()
        {
            this.GoToRoom();
            this.DecrementPlayerSpeedCounter();
            this.RegenerateSpellAbility();
            bool roomIsLit = this.CheckLighting();
            this.YouSee(roomIsLit);
            this.DisplayMonstersInRoom();
            this.DisplayArtifactsInRoom();
            CurrentRoom().RoomVisted = true;
        }

        private void DisplayArtifactsInRoom()
        {
            for(int i=0; i < this._currentAdventure.NumberOfArtifacts; i++)
            {
                if(this._currentAdventure.Artifacts[i].Data[4] == this._currentAdventure.CurrentRoom)
                {
                    Artifact artifactInRoom = this._currentAdventure.Artifacts[i];

                    if(CurrentRoom().RoomVisted)
                    {
                        Console.WriteLine($" - YOU SEE {artifactInRoom.Name}.");
                    }
                    else
                    {
                        PrintAsLines(artifactInRoom.LongDescription);
                    }
                }
            }

            for (int i = this._currentAdventure.NumberOfArtifacts; i < _currentAdventure.NumberOfArtifacts + _currentAdventure.numberOfPlayerWeapons; i++)
            {
                if(this._currentAdventure.Artifacts[i].Data[4] == this._currentAdventure.CurrentRoom)
                {
                    Console.WriteLine($"YOUR {this._currentAdventure.Artifacts[i].Name} IS HERE.");
                }
            }
        }

        private void GoToRoom()
        {
            _currentAdventure.roomExited = _currentAdventure.CurrentRoom;
            _currentAdventure.CurrentRoom = _currentAdventure.roomToEnter;

            if(_currentAdventure.roomExited != 0)
            {
                _currentAdventure.enemyHardiness = 0;
                _currentAdventure.friendlyHardiness = _currentAdventure.Monsters[0].Data[1];

                _currentAdventure.enemyDamageTaken = 0;
                _currentAdventure.friendlyDamageTaken = _currentAdventure.Monsters[0].Data[13];

                for (int i = 1; i < _currentAdventure.Monsters.Length; i++)
                {
                    Monster currentMonster = _currentAdventure.Monsters[i];

                    if(currentMonster.Data[5] == _currentAdventure.roomExited)
                    {
                        if(currentMonster.Data[14] != 0 || _r.Next(200) < currentMonster.Data[4])
                        {
                            currentMonster.Data[5] = _currentAdventure.CurrentRoom;
                        }
                    }
                    else if(currentMonster.Data[5] == _currentAdventure.CurrentRoom)
                    {
                        int FR = currentMonster.Data[3];
                        if(FR != 0 && FR != 100)
                        {
                            FR += (_characters[_theCharacter].Charisma - 10) / 2;
                        }

                        if (FR > 100 && _r.Next(10000) != 0)
                        {
                            currentMonster.Data[14] = 1;
                        }
                        else
                        {
                            currentMonster.Data[14] = 0;
                        }
                    }

                    if(currentMonster.Data[14] == 1) // friendly
                    {
                        _currentAdventure.friendlyDamageTaken += currentMonster.Data[13];
                        _currentAdventure.friendlyHardiness += currentMonster.Data[1];
                    }
                    else
                    {
                        _currentAdventure.enemyDamageTaken += currentMonster.Data[13];
                        _currentAdventure.enemyHardiness += currentMonster.Data[1];
                    }
                }

                if(_currentAdventure.enemyHardiness > 0)
                {
                    _currentAdventure.enemyPresent = true;
                }
            }
        }

        private void YouSee(bool roomIsLit)
        {
            if (roomIsLit)
            {
                this.DescribeRoom();
            }
            else
            {
                Console.WriteLine("IT'S TOO DARK TO SEE.");
            }
        }

        private void DisplayMonstersInRoom()
        {
            for (int i = 0; i < this._currentAdventure.NumberOfMonsters; i++)
            {
                if (this._currentAdventure.Monsters[i].Data[5] == this._currentAdventure.CurrentRoom)
                {
                    Monster monsterInRoom = this._currentAdventure.Monsters[i];

                    if (this._currentAdventure.Monsters[i].Data[15] == 0)
                    {
                        this._currentAdventure.Monsters[i].Data[15] = 1;
                        PrintAsLines(this._currentAdventure.Monsters[i].LongDescription);
                    }
                    else
                    {
                        Console.WriteLine($" - YOU SEE {monsterInRoom.Name} IS HERE.");
                    }
                }
            }
        }

        private void DescribeRoom()
        {
            if (!CurrentRoom().RoomVisted)
            {
                this.PrintAsLines(CurrentRoom().Description);
            }
            else
            {
                Console.WriteLine($"YOU ARE STANDING IN {this.CurrentRoom().Name}");
            }

        }

        private bool CheckLighting()
        {
            return CurrentRoom().NaturalLight > 0;

            bool playerHasLight = true;
            if (this.Player().LightSource > 0)
            {
                Artifact lightSource = _currentAdventure.Artifacts[this.Player().LightSource];
                lightSource.Data[5]--;

                if (lightSource.Data[5] == 0)
                {
                    Console.WriteLine($"YOUR {lightSource.Name} HAS GONE OUT!");
                    playerHasLight = false;
                    this.Player().LightSource = 0;
                }
                else if (lightSource.Data[5] < 10)
                {
                    Console.WriteLine($"YOUR {lightSource.Name} IS ALMOST OUT.");
                }
                else if (lightSource.Data[5] < 20)
                {
                    Console.WriteLine($"YOUR {lightSource.Name} GROWS DIM!");
                }

            }
            return playerHasLight;
        }

        private void RegenerateSpellAbility()
        {
            //power is never regenerated so only regenerate spells 0 to 3
            for (int i = 0; i < 3; i++)
            {
                if (this.Player().CurrentSpellAbility[i] < this.Player().SpellAbility[i])
                {
                    this.Player().CurrentSpellAbility[i] = (int)(this.Player().CurrentSpellAbility[i] * 1.1);

                    if (this.Player().CurrentSpellAbility[i] > this.Player().SpellAbility[i])
                    {
                        this.Player().CurrentSpellAbility[i] = this.Player().SpellAbility[i];
                    }
                }
            }

        }

        private void DecrementPlayerSpeedCounter()
        {
            if (this.Player().Speed > 0)
            {
                this.Player().Speed--;
                if (this.Player().Speed == 0)
                {
                    _currentAdventure.Monsters[0].Data[2] /= 2;
                    Console.WriteLine("YOUR SPEED SPELL HAS JUST EXPIRED");
                }
            }
        }

        private void ReadDataFiles()
        {
            this.ReadEamonName();
            this.ReadRooms();
            this.ReadEamonArtifacts();
            this.ReadEamonMonsters();
            this.ReadEffects();
            this.InitPlayerMonster();
        }

        private void InitPlayerMonster()
        {
            // add player monster
            Monster playerMonster = new Monster();
            playerMonster.index = 0;
            playerMonster.Data = new int[12];
            _currentAdventure.Monsters[0] = playerMonster;
            Character player = this.Player();

            //calculate effect of player armor
            int m = player.Armor / 2;
            if (m * 2 != player.Armor)
            {
                playerMonster.Data[8] = 1;
                playerMonster.Data[11] = 3;
                player.EffectOfArmorOnOdds = -5;
            }

            if (m > 0)
            {
                playerMonster.Data[8] += m;
                player.EffectOfArmorOnOdds -= m * 10;
            }

            if (m == 3)
            {
                playerMonster.Data[8] += 2;
                player.EffectOfArmorOnOdds -= 30;
            }

            // add armor artifact to artifacts
            player.ArmorWorn = _currentAdventure.NumberOfArtifacts + _currentAdventure.numberOfPlayerWeapons;
            _currentAdventure.numberOfPlayerArmor = 1;
            Artifact armorArtifact = new Artifact();
            _currentAdventure.Artifacts[player.ArmorWorn] = armorArtifact;

            string[] armorNames = {"", "LEATHER", "CHAIN MAIL", "PLATE ARMOR"};
            armorArtifact.Name = armorNames[m];

            armorArtifact.Data = new int[9];

            armorArtifact.Data[2] = 11;
            armorArtifact.Data[3] = m * 7;
            armorArtifact.Data[4] = -999;
            armorArtifact.Data[5] = m * 2;
            player.CarriedWeight += m * 7;
            
            // add shield to artifacts
            if (m * 2 != player.Armor)
            {
                _currentAdventure.numberOfPlayerArmor++;
                player.ShieldWorn = _currentAdventure.NumberOfArtifacts + _currentAdventure.numberOfPlayerWeapons + 1;
                Artifact shield = new Artifact();
                _currentAdventure.Artifacts[player.ShieldWorn] = shield;

                shield.Data = new int[9];

                shield.Name = "SHEILD";
                shield.Data[2] = 11;
                shield.Data[3] = 10;
                shield.Data[4] = -999;
                shield.Data[5] = 1;

                player.CarriedWeight += 10;
            }

            if (_currentAdventure.numberOfPlayerWeapons > 0)
            {
                ChangeWeapon(player, _currentAdventure.Artifacts[_currentAdventure.NumberOfArtifacts], _currentAdventure.NumberOfArtifacts);
            }

            _currentAdventure.Monsters[0].Data[1] = player.Hardiness;
        }

        private void ChangeWeapon(Character player, Artifact newWeapon, int artifactNumber)
        {
            Monster playerMonster = _currentAdventure.Monsters[0];
            playerMonster.Data[9] = artifactNumber;

            // calc odds to hit starting with armor encumberance
            int oddsToHit = player.EffectOfArmorOnOdds + player.ArmorExpertise;
            if (oddsToHit < 0)
            {
                oddsToHit = 0;
            }

            // add players weapon ability
            oddsToHit += player.WeaponAbility[newWeapon.Data[6] - 1];

            // add weapons complexity
            oddsToHit += newWeapon.Data[5];

            // add weapon type effect
            oddsToHit += newWeapon.Data[2] * 2;

            playerMonster.Data[10] = oddsToHit;
        }

        private void ReadEffects()
        {
            string path = Path.Combine(_currentAdventure.Path, "EAMON.DESC");
            string[] records = new string[_currentAdventure.NumberOfEffects];
            this.GetRecordsFromFile(path, RoomDescriptionRecordLength, _currentAdventure.NumberOfEffects, 201, records);

            _currentAdventure.Effects = new string[_currentAdventure.NumberOfEffects];
            for (int i = 0; i < _currentAdventure.NumberOfEffects; i++)
            {
                _currentAdventure.Effects[i] = this.TrimDescription(records[i]);
            }
        }

        private void ReadEamonMonsters()
        {
            string path = Path.Combine(_currentAdventure.Path, "EAMON.MONSTERS");
            string path2 = Path.Combine(_currentAdventure.Path, "EAMON.DESC");
            string[] records = new string[_currentAdventure.NumberOfMonsters];
            string[] descriptionRecords = new string[_currentAdventure.NumberOfMonsters];
            //monster array is 1 indexed in old code so allow a garbage record in to the 0 slot, it will be skipped
            this.GetRecordsFromFile(path, MonsterRecordLength, _currentAdventure.NumberOfMonsters, 1, records);
            this.GetRecordsFromFile(path2, RoomDescriptionRecordLength, _currentAdventure.NumberOfMonsters, 301, descriptionRecords);

            _currentAdventure.Monsters = new Monster[_currentAdventure.NumberOfMonsters + 1];
            for (int i = 0; i < _currentAdventure.NumberOfMonsters; i++)
            {
                string[] tokens = records[i].Split('\n');
                int currentToken = 0;

                _currentAdventure.Monsters[i] = new Monster();
                _currentAdventure.Monsters[i].index = i;
                
                _currentAdventure.Monsters[i].Name = tokens[currentToken++];
                _currentAdventure.Monsters[i].Data = new int[17];
                for (int j = 1; j <= 12; j++)
                {
                    _currentAdventure.Monsters[i].Data[j] = int.Parse(tokens[currentToken++]);
                }

                _currentAdventure.Monsters[i].LongDescription = this.TrimDescription(descriptionRecords[i]);
            }
        }

        private void ReadEamonArtifacts()
        {
            string path = Path.Combine(_currentAdventure.Path, "EAMON.ARTIFACTS");
            string path2 = Path.Combine(_currentAdventure.Path, "EAMON.DESC");
            string[] records = new string[_currentAdventure.NumberOfArtifacts];
            string[] descriptionRecords = new string[_currentAdventure.NumberOfArtifacts];
            this.GetRecordsFromFile(path, ArtifactRecordLength, _currentAdventure.NumberOfArtifacts, 1, records);
            this.GetRecordsFromFile(path2, RoomDescriptionRecordLength, _currentAdventure.NumberOfArtifacts, 101, descriptionRecords);

            _currentAdventure.Artifacts = new Artifact[_currentAdventure.NumberOfArtifacts + 6];
            for (int i = 0; i < _currentAdventure.NumberOfArtifacts; i++)
            {
                string[] tokens = records[i].Split('\n');
                int currentToken = 0;

                _currentAdventure.Artifacts[i] = new Artifact();
                _currentAdventure.Artifacts[i].Name = tokens[currentToken++];
                _currentAdventure.Artifacts[i].Data = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    _currentAdventure.Artifacts[i].Data[j] = int.Parse(tokens[currentToken++]);
                }

                _currentAdventure.Artifacts[i].LongDescription = this.TrimDescription(descriptionRecords[i]);
            }


            _currentAdventure.numberOfPlayerWeapons = 0;

            // add characters weapons into artifacts
            for (int i = 0; i < 4; i++)
            {
                Character player = this.Player();
                _currentAdventure.Artifacts[_currentAdventure.NumberOfArtifacts + i] = new Artifact();
                
                Artifact currentArtifact = _currentAdventure.Artifacts[_currentAdventure.NumberOfArtifacts + i];

                currentArtifact.Name = player.WeaponName[i];
                currentArtifact.Data = new int[9];
                currentArtifact.Data[6] = player.WeaponType[i];
                currentArtifact.Data[5] = player.WeaponOdds[i];
                currentArtifact.Data[7] = player.WeaponDice[i];
                currentArtifact.Data[8] = player.WeaponSides[i];

                if (!string.IsNullOrWhiteSpace(currentArtifact.Name) &&
                    !currentArtifact.Name.ToLowerInvariant().Equals("none"))
                {
                    _currentAdventure.numberOfPlayerWeapons++;

                    //A%(A,2) = 2 + (A%(A,7) * A%(A,8) > 20)
                    currentArtifact.Data[2] = (player.WeaponDice[i] * player.WeaponSides[i]) > 20 ? 3 : 2;

                    currentArtifact.Data[3] = 2;

                    currentArtifact.Data[4] = -1;

                    player.CarriedWeight += 2;
                }
            }

            bool renameHappened;

            do
            {
                renameHappened = false;
                for (int i = 0; i < _currentAdventure.NumberOfArtifacts; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_currentAdventure.Artifacts[i].Name.ToLowerInvariant()
                            .Equals(this.Player().WeaponName[j]))
                        {
                            _currentAdventure.Artifacts[i].Name = $"{_currentAdventure.Artifacts[i].Name}#";
                            renameHappened = true;
                            break;
                        }
                    }
                    if (renameHappened)
                    {
                        break;
                    }
                }
            } while (renameHappened);

        }

        private void ReadRooms()
        {
            //TODO: read all room info into room structures instead of having separate arrays
            string path = Path.Combine(_currentAdventure.Path, "EAMON.DESC");
            string[] record = new string[1];
            this.GetRecordsFromFile(path, RoomDescriptionRecordLength, 1, 0, record);

            string[] tokens = record[0].Split('\r');

            int currentToken = 0;

            _currentAdventure.NumberOfRooms = int.Parse(tokens[currentToken++]) + 1;
            _currentAdventure.NumberOfArtifacts = int.Parse(tokens[currentToken++]);
            _currentAdventure.NumberOfEffects = int.Parse(tokens[currentToken++]);
            _currentAdventure.NumberOfMonsters = int.Parse(tokens[currentToken]);

            _currentAdventure.Rooms = new Room[_currentAdventure.NumberOfRooms];
            string[] roomDescriptions = new string[_currentAdventure.NumberOfRooms];

            //because rooms are 1 indexed we are going to keep room 0 empty
            //so we will re-read the first record (which has junk as far as this part is concerned)
            //and record 0 will be skipped in the loop below
            this.GetRecordsFromFile(path, RoomDescriptionRecordLength, _currentAdventure.NumberOfRooms, 0, roomDescriptions);

            for (int i = 1; i < _currentAdventure.NumberOfRooms; i++)
            {
                _currentAdventure.Rooms[i] = new Room();
                _currentAdventure.Rooms[i].Description = TrimDescription(roomDescriptions[i]);

                // room not visited
                _currentAdventure.Rooms[i].RoomVisted = false;
            }

            // read rooms data
            path = Path.Combine(_currentAdventure.Path, "EAMON.ROOMS");

            record = new string[_currentAdventure.NumberOfRooms];

            //because rooms are 1 indexed we are going to keep room 0 empty
            //so we will re-read the first record (which has junk as far as this part is concerned)
            //and record 0 will be skipped in the loop below
            this.GetRecordsFromFile(path, LR, _currentAdventure.NumberOfRooms, 0, record);
            for (int i = 1; i < _currentAdventure.NumberOfRooms; i++)
            {
                tokens = record[i].Split('\r');
                currentToken = 0;
                _currentAdventure.Rooms[i].Directions = new int[6];
                for (int dir = 0; dir < 6; dir++)
                {
                    _currentAdventure.Rooms[i].Directions[dir] = int.Parse(tokens[currentToken++]);
                }

                _currentAdventure.Rooms[i].NaturalLight = int.Parse(tokens[currentToken]);
            }

            // read room names
            path = Path.Combine(_currentAdventure.Path, "EAMON.ROOM.NAME");
            record = new string[_currentAdventure.NumberOfRooms];

            //because rooms are 1 indexed we are going to keep room 0 empty
            //so we will re-read the first record (which has junk as far as this part is concerned)
            //and record 0 will be skipped in the loop below
            this.GetRecordsFromFile(path, LN, _currentAdventure.NumberOfRooms, 0, record);
            for (int i = 1; i < _currentAdventure.NumberOfRooms; i++)
            {
                int endIndex = record[i].IndexOf("\r", StringComparison.InvariantCulture);
                _currentAdventure.Rooms[i].Name = record[i].Substring(0, endIndex);
            }
        }

        private void ReadEamonName()
        {
            string path = Path.Combine(_currentAdventure.Path, "EAMON.NAME");
            using (StreamReader reader = new StreamReader(path))
            {
                string name = reader.ReadLine();
                string version = reader.ReadLine();
                string minorVersion = reader.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine($"{path} IS CORRUPT.");
                    Environment.Exit(1);
                }

                _currentAdventure.Name = name;

                if (!int.TryParse(version, out _currentAdventure.Version))
                {
                    Console.WriteLine($"{path} IS CORRUPT.");
                    Environment.Exit(1);
                }

                if (!float.TryParse(minorVersion, out _currentAdventure.MinorVersion))
                {
                    Console.WriteLine($"{path} IS CORRUPT.");
                    Environment.Exit(1);
                }
            }
        }

        private AdventureInfo[] GetAdventureList()
        {
            using (StreamReader reader = new StreamReader("Adventures.txt"))
            {
                int numberOfAdventures;
                if (!int.TryParse(reader.ReadLine(), out numberOfAdventures))
                {
                    Console.WriteLine("Adventures.txt is corrupt.");
                    Environment.Exit(1);
                }

                AdventureInfo[] adventures = new AdventureInfo[numberOfAdventures];
                for (int i = 0; i < numberOfAdventures; i++)
                {
                    adventures[i] = new AdventureInfo
                    {
                        Name = reader.ReadLine(),
                        Path = reader.ReadLine()
                    };
                }

                return adventures;
            }
        }

        private void MainHallExamineAbility()
        {
            Character examineCharacter = this.Player();
            string sexAttribute = examineCharacter.Sex.ToLowerInvariant() == "m" ? "MIGHTY " : "FAIR ";

            Console.WriteLine($"YOU ARE THE {sexAttribute}{examineCharacter.Name}");
            Console.WriteLine("YOUR ATTRIBUTES ARE:");
            Console.WriteLine($"  HD={examineCharacter.Hardiness}  AG={examineCharacter.Hardiness}  CH={examineCharacter.Charisma}\r\n");
            Console.WriteLine("YOU KNOW THE FOLLOWING SPELLS--");

            var spells = new[] {"BLAST", "HEAL", "SPEED", "POWER"};
            bool hasSpells = false;
            for (int i = 0; i < 4; i++)
            {
                if (examineCharacter.SpellAbility[i] > 0)
                {
                    hasSpells = true;
                    Console.Write($"{spells[i]}  ");
                }
            }
            if (!hasSpells)
            {
                Console.Write("   NO SPELLS");
            }

            Console.WriteLine("\r\nYOUR WEAPON ABILITIES ARE--");
            Console.WriteLine("  AXE   BOW   CLUB  SPEAR SWORD");
            Console.Write(" ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{examineCharacter.WeaponAbility[i].ToString().PadLeft(4)}% ");
            }
            Console.WriteLine("\r\n");

            int a2 = examineCharacter.Armor / 2;
            var armor = new[] {"SKIN", "LEATHER", "CHAIN", "PLATE"};
            Console.Write($"Armor: {armor[a2]}");
            if (examineCharacter.Armor > a2 * 2)
            {
                Console.Write(" AND SHIELD    ");
            }
            else
            {
                Console.Write("               ");
            }
            Console.WriteLine($"AE={examineCharacter.ArmorExpertise}%\r\n");
            Console.WriteLine($"\r\nGOLD IN HAND={examineCharacter.Gold}, BANK={examineCharacter.Bank}");
            Console.WriteLine("\r\nWEAPONS            CMPLX    DAM");

            bool hasWeapons = false;
            for (int i = 0; i < 4; i++)
            {
                if (examineCharacter.WeaponName[i].ToLowerInvariant() != "none")
                {
                    hasWeapons = true;
                    Console.Write($"{examineCharacter.WeaponName[i].PadRight(17)}{examineCharacter.WeaponOdds[i].ToString().PadLeft(4)}%");
                    Console.WriteLine($"{examineCharacter.WeaponDice[i].ToString().PadLeft(7)} D {examineCharacter.WeaponSides[i]}");
                }
            }

            if (!hasWeapons)
            {
                Console.WriteLine("        NO WEAPONS");
            }

            Console.WriteLine("(HIT ANY KEY TO CONTINUE)");
            Console.ReadKey(true);
        }

        private void MainHallBank()
        {
            Character bankingCharacter = this.Player();

            Console.WriteLine("YOU HAVE NO TROUBLE SPOTTING SHYLOCK");
            Console.WriteLine("MCFENNEY, THE LOCAL BANKER, DUE TO HIS");
            Console.WriteLine("LARGE BELLY.\r\n");
            Console.WriteLine("YOU ATTRACT HIS ATTENTION, AND HE COMES");
            Console.WriteLine("OVER TO YOU AND SAYS, \'WELL,");
            string gender = bankingCharacter.Sex.ToLowerInvariant() == "m" ? "boy" : "girl";
            Console.WriteLine($"{bankingCharacter.Name}, MY DEAR {gender},");
            Console.WriteLine("WHAT A PLEASURE TO SEE YOU! DO YOU WANT");
            Console.WriteLine("TO MAKE A DEPOSIT OR A WITHDRAWL?\'\r\n");
            Console.WriteLine("(ENTER \'D\' FOR DEPOSIT, \'W\' FOR WITHDRAWL)   ");
            string answer;
            do
            {
                answer = Input().ToLowerInvariant();
            } while (answer != "d" && answer != "w");

            if (answer == "w")
            {
                MainHallWithdrawBank(bankingCharacter);
            }
            else
            {
                MainHallDepositBank(bankingCharacter);
            }
        }

        private static void MainHallDepositBank(Character bankingCharacter)
        {
            Console.WriteLine("SHYLOCK GETS A WIDE GRIN ON HIS FACE");
            Console.WriteLine("AND SAYS, \'GOOD FOR YOU!  HOW MUCH DO");
            Console.WriteLine("YOU WANT TO DEPOSIT?\'");
            Console.WriteLine("(ENTER THE AMOUNT YOU WANT TO DEPOSIT)");

            int amount = GetBankingAmount();

            if (amount > bankingCharacter.Gold)
            {
                Console.WriteLine("THE BANKER WAS VERY PLEASED WHEN YOU");
                Console.WriteLine("TOLD HIM THE SUM, BUT WHEN HE DISCOVERED");
                Console.WriteLine("THAT YOU DIDN\'T HAVE THAT MUCH ON YOU,");
                Console.WriteLine("HE WALKED AWAY, SHOUTING ABOUT FOOLS");
                Console.WriteLine("WHO PLAY TRICKS ON A KINDLY BANKER.\r\n");
            }
            else
            {
                bankingCharacter.Bank += amount;
                bankingCharacter.Gold -= amount;
                Console.WriteLine("SHYLOCK TAKES YOUR MONEY, PUTS IT IN");
                Console.WriteLine("HIS BAG, LISTENS TO IT JINGLE, THEN");
                Console.WriteLine("THANKS YOU AND WALKS AWAY.\r\n");
            }
        }

        private static void MainHallWithdrawBank(Character bankingCharacter)
        {
            Console.WriteLine($"SHYLOCK SAYS, \'WELL, YOU HAVE {bankingCharacter.Bank}");
            Console.WriteLine("GOLD PIECES STORED WITH ME.  HOW MANY");
            Console.WriteLine("DO YOU WANT TO TAKE BACK?\'\r\n");
            Console.WriteLine("(ENTER THE NUMBER OF GOLD PIECES TO");
            Console.WriteLine("   WITHDRAW)  ");

            int amount = GetBankingAmount();

            if (amount > bankingCharacter.Bank)
            {
                Console.WriteLine("THE BANKER THROWS YOU A TERRIBLE GLANCE");
                Console.WriteLine("AND SAYS, \'THAT\'S MORE THAN YOU\'VE GOT!");
                Console.WriteLine("YOU KNOW I DON\'T MAKE LOANS TO YOUR");
                Console.WriteLine("KIND!\'  WITH THAT HE LOSES HIMSELF IN");
                Console.WriteLine("THE CROWD.\r\n");
            }
            else
            {
                bankingCharacter.Gold += amount;
                bankingCharacter.Bank -= amount;
                Console.WriteLine("\r\nTHE BANKER HANDS YOU YOUR GOLD AND");
                Console.WriteLine($"SAYS, 'THAT LEAVES YOU WITH {bankingCharacter.Bank}");
                Console.WriteLine("PIECES IN MY CARE.\'  HE SHAKES YOUR");
                Console.WriteLine("HAND AND WALKS AWAY.");
            }
        }

        private static int GetBankingAmount()
        {
            bool valid = true;
            int amount;
            do
            {
                var answer = Input();

                if (!int.TryParse(answer, out amount) || amount < 0)
                {
                    valid = false;
                    Console.WriteLine("THE BANKER SCOWLS AND SAYS,\'COME, COME,");
                    Console.WriteLine("YOU\'RE NOT MAKING SENSE!  TRY AGAIN.\'\r\n");
                }
            } while (!valid);

            return amount;
        }

        private void MainHallWizard()
        {
            Character learningChar = this.Player();

            Console.WriteLine("\r\nAFTER A FEW MINUTES DILIGENT SEARCHING,");
            Console.WriteLine("YOU FIND HOKAS TOKAS, THE OLD MAGE. HE");
            Console.WriteLine("LOOKS AT YOU AND SAYS, \'SO YOU WANT OLD");
            Console.WriteLine("TOKEY TO TEACH YOU SOME MAGIC, HEH HEH?");
            Console.WriteLine("WELL, IT\'LL COST YOU. TODAY MY FEES ARE:");
            int c2 = learningChar.Charisma + _r.Next(11) - 5;
            if (c2 == 0)
            {
                c2 = 1;
            }

            int rtio = 10 / c2;

            int blastPrice = Price(3000, rtio);
            int healPrice = Price(1000, rtio);
            int speedPrice = Price(5000, rtio);
            int powerPrice = Price(100, rtio);

            Console.WriteLine($"   BLAST  {blastPrice}GP");
            Console.WriteLine($"   HEAL   {healPrice}GP");
            Console.WriteLine($"   SPEED  {speedPrice}GP");
            Console.WriteLine($"   POWER  {powerPrice}GP\r\n");
            Console.WriteLine("WELL, WHICH WILL IT BE?'");
            Console.WriteLine("\r\n(ENTER THE KEY FOR YOUR SPELL:");
            Console.WriteLine("   N FOR NOTHING, OR B,H,S OR P)   ");

            string answer;
            do
            {
                answer = Input().ToLowerInvariant();
            } while (answer != "n" && answer != "b" && answer != "h" && answer != "s" && answer != "p");

            if (answer == "n")
            {
                Console.WriteLine("AS YOU LEAVE, YOU HEAR HOKAS MUTTERING");
                Console.WriteLine("ABOUT CHEAPSKATE ADVENTURERS ALWAYS");
                Console.WriteLine("WANTING SOMETHING FOR NOTHING.\r\n");
            }
            else
            {
                int price = 0;
                int sp = 0;
                switch (answer)
                {
                    case "b":
                        price = blastPrice;
                        sp = 0;
                        break;
                    case "h":
                        price = healPrice;
                        sp = 1;
                        break;
                    case "s":
                        price = speedPrice;
                        sp = 2;
                        break;
                    case "p":
                        price = powerPrice;
                        sp = 3;
                        break;
                }

                if (learningChar.Gold < price)
                {
                    Console.WriteLine("WHEN HOKAS SEES THAT YOU DON\'T HAVE");
                    Console.WriteLine("ENOUGH TO PAY HIM, HE STALKS TO THE");
                    Console.WriteLine("BAR, MUTTERING ABOUT YOUNGSTERS WHO");
                    Console.WriteLine("SHOULD BE TURNED INTO FROGS.\r\n");
                }
                else if(learningChar.SpellAbility[sp] > 0)
                {
                    Console.WriteLine("HOKAS SAYS, \'I OUGHT TO TAKE YOUR");
                    Console.WriteLine("GOLD ANYWAY, BUT HAVEN\'T YOU FORGOTTEN");
                    Console.WriteLine("SOMETHING?  I ALREADY TAUGHT YOU THAT");
                    Console.WriteLine("SPELL!\'\r\n");
                    Console.WriteLine("SHAKING HIS HEAD SADLY, HE RETURNS TO");
                    Console.WriteLine("BAR.\r\n");
                }
                else
                {
                    learningChar.Gold -= price;
                    learningChar.SpellAbility[sp] = _r.Next(50) + 26;
                    Console.WriteLine("HOKAS TEACHES YOU YOUR SPELL, TAKES");
                    Console.WriteLine("HIS FEE, AND RETURNS TO HIS STOOL ON");
                    Console.WriteLine("THE BAR.  AS YOU WALK AWAY YOU HEAR");
                    Console.WriteLine("HIM ORDER A DOUBLE DRAGON BLOMB.\r\n");
                }
            }
        }

        private void MainHallShopping()
        {
            Character shoppingChar = this.Player();

            //visit weapon shop
            Console.WriteLine("\r\nAS YOU ENTER THE WEAPON SHOP, MARCOS");
            Console.WriteLine("CAVIELLI (THE OWNER) COMES FROM OUT OF");
            Console.WriteLine("THE BACK ROOM AND SAYS, \'WELL, AS I");
            Console.WriteLine("LIVE AND BREATH, IF IT ISN\'T MY OLD PAL");
            Console.WriteLine($"{shoppingChar.Name}!  SO, YOU WANT TO");
            Console.WriteLine("BUY A WEAPON, SELL A WEAPON, OR GET");
            Console.WriteLine("SOME BETTER ARMOUR?");
            Console.WriteLine("\r\n(HIT THE KEY, B S OR A)   ");
            string answer;
            do
            {
                answer = Input().ToLowerInvariant();
            } while (answer != "b" && answer != "s" && answer != "a");

            int c2 = shoppingChar.Charisma + (this._r.Next(11) - 5);
            if (c2 == 0)
            {
                c2 = 1;
            }

            // ReSharper disable once PossibleLossOfFraction
            float rtio = 10 / c2;

            switch (answer)
            {
                case "b": //buy weapon
                    BuyWeaponInMainHall(shoppingChar, rtio);
                    break;
                case "s": //sell weapon
                    SellWeapoonMainHall(shoppingChar, rtio);
                    break;
                case "a": //armor
                    UpgradeArmorMainHall(shoppingChar, rtio);
                    break;
            }
        }

        private static void UpgradeArmorMainHall(Character shoppingChar, float rtio)
        {
            int a2 = shoppingChar.Armor / 2;
            int sh = shoppingChar.Armor - a2 * 2;
            int b1 = 0;
            switch (a2)
            {
                case 1:
                    b1 = 25;
                    break;
                case 2:
                    b1 = 60;
                    break;
                case 3:
                    b1 = 100;
                    break;
            }

            int tradeIn = (int) Math.Ceiling(b1 / rtio);

            Console.WriteLine("MARCOS TAKES YOU TO THE ARMOUR SECTION");
            Console.WriteLine("OF HIS SHOP AND SHOWS YOU SUITS OF");
            Console.WriteLine("LEATHER ARMOUR, CHAIN ARMOUR, AND PLATE.");

            if (tradeIn > b1 * 2)
            {
                tradeIn = b1 * 2;
            }

            int leatherPrice = Price(100, rtio);
            int chainPrice = Price(100, rtio);
            int platePrice = Price(100, rtio);

            Console.WriteLine("HE SAYS, \'I CAN PUT YOU IN ANY OF THESE");
            Console.WriteLine($"VERY CHEAPLY.  I NEED {leatherPrice} GOLD");
            Console.WriteLine($"PIECES FOR THE LEATHER, {chainPrice} FOR");
            Console.WriteLine($"THE CHAIN, AND {platePrice} FOR THE PLATE.\r\n");

            if (tradeIn > 0)
            {
                Console.WriteLine("ALSO, I CAN GIVE YOU A TRADE-IN ON YOUR");
                Console.WriteLine($"OLD ARMOUR OF {tradeIn} GOLD PIECES.\r\n");
                Console.WriteLine("WELL, WHAT WILL IT BE?\'");
                Console.WriteLine("\r\n(ENTER N FOR NOTHING OR L C OR P)  ");

                string answer;
                do
                {
                    answer = Input().ToLowerInvariant();
                } while (answer != "n" && answer != "l" && answer != "c" && answer != "p");

                int price = 0;
                int a = 0;
                switch (answer)
                {
                    case "l":
                        price = leatherPrice - tradeIn;
                        a = 2;
                        break;
                    case "c":
                        price = chainPrice - tradeIn;
                        a = 3;
                        break;
                    case "p":
                        price = platePrice - tradeIn;
                        a = 4;
                        break;
                }

                if (price > shoppingChar.Gold)
                {
                    Console.WriteLine("MARCOS FROWNS WHEN HE SEES THAT YOU DO");
                    Console.WriteLine("NOT HAVE ENOUGHT TO PAY FOR YOUR");
                    Console.WriteLine("ARMOUR AND SAYS, \'I DON\'T GIVE CREDIT!\'");
                }
                else
                {
                    Console.WriteLine("MARCOS TAKES YOUR OLD ARMOUR AND YOUR");
                    Console.WriteLine("GOLD AND HELPS YOU INTO YOUR NEW");
                    Console.WriteLine("ARMOUR.");

                    shoppingChar.Gold -= price;
                    a2 = a - 1;
                }

                if (sh < 1) //player doesn't have shield
                {
                    int shieldPrice = Price(50, rtio);
                    Console.WriteLine("MARCOS SMILES AND SAYS, \'NOW HOW ABOUT");
                    Console.WriteLine("A SHIELD?  I CAN LET YOU HAVE ONE FOR");
                    Console.WriteLine($"ONLY {shieldPrice} GOLD PIECES!\'");
                    Console.WriteLine("\r\n(ENTER Y OR N)");

                    do
                    {
                        answer = Input().ToLowerInvariant();
                    } while (answer != "y" && answer != "n");

                    if (answer == "y")
                    {
                        if (shoppingChar.Gold < shieldPrice)
                        {
                            Console.WriteLine("WHEN HE SEES THAT YOU DO NOT HAVE");
                            Console.WriteLine("ENOUGH GOLD TO BUY THE SHIELD, MARCOS");
                            Console.WriteLine("FROWNS AND SAYS, \'I\'M SORRY, BUT I");
                            Console.WriteLine("DON\'T GIVE CREDIT!\'\r\n");
                        }
                        else
                        {
                            shoppingChar.Gold -= shieldPrice;
                            sh = 1;
                            Console.WriteLine("MARCOS TAKES YOUR GOLD AND GIVES YOU");
                            Console.WriteLine("A SHIELD.\r\n");
                        }
                    }
                }
                shoppingChar.Armor = a2 * 2 + sh;
            }
        }

        private static void SellWeapoonMainHall(Character shoppingChar, float rtio)
        {
            Console.WriteLine("MARCOS ASKS YOU, \'IS THIS WEAPON YOU");
            Console.WriteLine("WANT TO SELL ME A STANDARD WEAPON LIKE");
            Console.WriteLine("I SELL?  (ENTER \'Y\' OR \'N\')  ");
            string yesno;
            do
            {
                yesno = Input().ToLowerInvariant();
            } while (yesno != "y" && yesno != "n");

            if (yesno == "n")
            {
                SellCustomWeaponMainHall(shoppingChar, rtio);
            }
            else
            {
                SellStandardWeaponMainHall(shoppingChar, rtio);
            }

            Console.WriteLine("MARCOS SMILES AND SAYS, \'COME BACK");
            Console.WriteLine("AGAIN SOON!\' AS HE SHOOS YOU OUT OF HIS");
            Console.WriteLine("SHOP.\r\n");
        }

        private static void SellStandardWeaponMainHall(Character shoppingChar, float rtio)
        {
            Console.WriteLine("\r\nMARCOS ASKS, \'WELL, WHAT WEAPON YOU");
            Console.WriteLine("WANTA RETURN?\r\n");

            string name;
            int basePrice, type, side, dice;
            SelectMainHallWeapon(out name, out basePrice, out type, out side, out dice);

            var weaponNum = GetCharacterWeaponIndexByName(shoppingChar, name);

            if (weaponNum < 0)
            {
                Console.WriteLine("MARCOS LAUGHS AND SAYS, \'YOU NEVER");
                Console.WriteLine("BOUGHT ONE FROM ME, REMEMBER?\'\r\n");
            }
            else
            {
                if (shoppingChar.WeaponOdds[weaponNum] > 5)
                {
                    basePrice = basePrice * 2;
                }
                else if (shoppingChar.WeaponOdds[weaponNum] < -5)
                {
                    basePrice = basePrice / 2;
                }

                int price = Price(basePrice, rtio);

                if (price > basePrice / 4)
                {
                    price = basePrice / 4;
                }

                Console.WriteLine("MARCOS EXAMINES YOUR WEAPONS AND SAYS,");
                Console.WriteLine("\'WELL, YOU\'VE BANGED IT UP A BIT, BUT");
                Console.WriteLine($"I CAN GIVE YOU {price} GOLD PIECES");
                Console.WriteLine("FOR IT, TAKE IT OR LEAVE IT.\'");
                Console.WriteLine("\r\n(HIT T OR L)  ");

                string answer;
                do
                {
                    answer = Input().ToLowerInvariant();
                } while (answer != "t" && answer != "l");

                if (answer == "t")
                {
                    MarcosTakesWeaponAndGivesGold(shoppingChar, price, weaponNum);
                }
            }
        }

        private static void SellCustomWeaponMainHall(Character shoppingChar, float rtio)
        {
            string answer;
            int price = (int)Math.Ceiling(50 / rtio);
            Console.WriteLine("MARCOS SAYS, \'THEN I CAN ONLY GIVE YOU");
            Console.WriteLine($"{price}  GOLD PIECES FOR IT, TAKE IT OR");
            Console.WriteLine("LEAVE IT!\'");
            Console.WriteLine("(ENTER T OR L)   ");

            do
            {
                answer = Input().ToLowerInvariant();
            } while (answer != "t" && answer != "l");

            if (answer == "t")
            {
                Console.WriteLine("MARCOS SAYS, \'OKAY, WHAT\'VE YOU GOT?");
                Console.WriteLine("(ENTER THE WEAPON NAME)");
                string name = Input().ToUpper();

                var weaponNum = GetCharacterWeaponIndexByName(shoppingChar, name);

                if (weaponNum < 0)
                {
                    Console.WriteLine("\r\nMARCOS FROWNS AT YOU AND SAYS, \'YOU");
                    Console.WriteLine("CAN\'T SELL A WEAPON YOU DON\'T OWN!\'\r\n");
                }
                else if (name == "NONE")
                {
                    Console.WriteLine("MARCOS FROWNS AND SAYS \'THEN WHY DO YOU WASTE MY TIME?\'");
                }
                else if (name.StartsWith("AXE") || name.StartsWith("BOW") || name.StartsWith("MACE") ||
                         name.StartsWith("SPEAR") || name.StartsWith("SWORD"))
                {
                    Console.WriteLine("MARCOS FROWNS AT YOU AND SAYS, \'THIS IS");
                    Console.WriteLine("A WEAPON LIKE I SELL, REMEMBER!\'");
                }
                else
                {
                    MarcosTakesWeaponAndGivesGold(shoppingChar, price, weaponNum);
                }
            }
        }

        private static void MarcosTakesWeaponAndGivesGold(Character shoppingChar, int price, int weaponNum)
        {
            Console.WriteLine("MARCOS GIVES YOU YOUR MONEY AND TAKES");
            Console.WriteLine("YOUR WEAPON.");

            shoppingChar.Gold += price;
            RemoveWeaponByname(shoppingChar, weaponNum);
        }

        private static void RemoveWeaponByname(Character shoppingChar, int weaponNum)
        {
            for (int i = weaponNum; i < 3; i++)
            {
                shoppingChar.WeaponName[i] = shoppingChar.WeaponName[i + 1];
                shoppingChar.WeaponType[i] = shoppingChar.WeaponType[i + 1];
                shoppingChar.WeaponOdds[i] = shoppingChar.WeaponOdds[i + 1];
                shoppingChar.WeaponDice[i] = shoppingChar.WeaponDice[i + 1];
                shoppingChar.WeaponSides[i] = shoppingChar.WeaponSides[i + 1];
            }
            shoppingChar.WeaponName[4] = "NONE";
            shoppingChar.WeaponType[4] = 0;
            shoppingChar.WeaponOdds[4] = 0;
            shoppingChar.WeaponDice[4] = 0;
            shoppingChar.WeaponSides[4] = 0;
        }

        private static int GetCharacterWeaponIndexByName(Character shoppingChar, string name)
        {
            int weaponNum = -1;
            for (int i = 0; i < 4; i++)
            {
                if (shoppingChar.WeaponName[i].ToUpper() == name)
                {
                    weaponNum = i;
                }
            }
            return weaponNum;
        }

        private static void BuyWeaponInMainHall(Character shoppingChar, float rtio)
        {
            if (shoppingChar.WeaponName[4].ToLowerInvariant() != "none") //already own 4 weapons
            {
                Console.WriteLine("MARCOS SMILES AT YOU AND SAYS, \'THATSA");
                Console.WriteLine("GOOD, BUT FIRST YOU GOTTA SELL ME A");
                Console.WriteLine("WEAPON. YOU KNOW THE LAW--NO MORE THAN");
                Console.WriteLine("FOUR WEAPONS PER PERSON!\'");
                Console.WriteLine("COME BACK WHEN YOU ARE READY TO SELL.\r\n");
            }
            else
            {
                Console.WriteLine("MARCOS SMILES AT YOU AND SAYS, \'GOOD.");
                Console.WriteLine("I GOTTA THE BEST. YOU WANTA AXE, BOW,");
                Console.WriteLine("MACE, SPEAR, OR SWORD?\r\n");

                string name;
                int basePrice, type, side, dice;
                SelectMainHallWeapon(out name, out basePrice, out type, out side, out dice);

                Console.WriteLine("MARCOS SAYS, \'WELL, I JUST HAPPEN TO");
                Console.WriteLine($"HAVE THREE {name}S IN, OF VARYING");
                Console.WriteLine("QUALITY. I\'VE GOT A VERY GOOD ONE FOR");

                int veryGoodPrice = Price(2 * basePrice, rtio);
                int fairPrice = Price(basePrice, rtio);
                int shabbyPrice = Price(basePrice / 2, rtio);

                Console.WriteLine($"{veryGoodPrice} GP, A FAIR ONE FOR {fairPrice} GP,");
                Console.WriteLine($"AND A KINDA SHABBY ONE FOR {shabbyPrice} GP");
                Console.WriteLine("WHICH DO YOU WANT?\'");
                Console.WriteLine("(ENTER G F, OR P)   ");

                string priceChoice;
                do
                {
                    priceChoice = Input().ToLowerInvariant();
                } while (priceChoice != "g" && priceChoice != "f" && priceChoice != "p");

                int chosenPrice = 0;
                int quality = 0;
                switch (priceChoice)
                {
                    case "g":
                        chosenPrice = veryGoodPrice;
                        quality = 1;
                        break;
                    case "f":
                        chosenPrice = fairPrice;
                        quality = 2;
                        break;
                    case "p":
                        chosenPrice = shabbyPrice;
                        quality = 3;
                        break;
                }

                if (chosenPrice > shoppingChar.Gold)
                {
                    Console.WriteLine("MARCOS SHAKES A FINGER AT YOU AND SAYS,");
                    Console.WriteLine("\'YOU SHOULDN\'T PLAY TRICKS ON AN OLD");
                    Console.WriteLine("FRIEND! COME BACK WHEN YOU GOTTA MORE");
                    Console.WriteLine("GOLD OR YOU WANT SOMETHING YOU CAN");
                    Console.WriteLine("AFFORD.\' HE THEN SHOOS YOU OUT THE");
                    Console.WriteLine("DOOR.\r\n");
                }
                else
                {
                    DeduplicateName(shoppingChar, ref name);

                    int emptyWeaponSlot = -1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (shoppingChar.WeaponName[i].ToLowerInvariant() == "none")
                        {
                            emptyWeaponSlot = i;
                            break;
                        }
                    }

                    shoppingChar.WeaponName[emptyWeaponSlot] = name;
                    shoppingChar.WeaponType[emptyWeaponSlot] = type;
                    shoppingChar.WeaponOdds[emptyWeaponSlot] = 10 * (2 - quality);
                    shoppingChar.WeaponDice[emptyWeaponSlot] = dice;
                    shoppingChar.WeaponSides[emptyWeaponSlot] = side;
                    shoppingChar.Gold = shoppingChar.Gold - chosenPrice;

                    Console.WriteLine("MARCOS HANDS YOU YOUR WEAPON AND TAKES");
                    Console.WriteLine("THE PRICE FROM YOU.\r\n");
                    Console.WriteLine("MARCOS SMILES AND SAYS, \'COME BACK");
                    Console.WriteLine("AGAIN SOON!\' AS HE SHOOS YOU OUT OF HIS");
                    Console.WriteLine("SHOP.\r\n");
                }
            }
        }

        private static void SelectMainHallWeapon(out string name, out int basePrice, out int type, out int side, out int dice)
        {
            name = "";

            Console.WriteLine("(ENTER, A B M SP OR SW)  ");

            string weapon;
            do
            {
                weapon = Input().ToLowerInvariant();
            } while (weapon != "a" && weapon != "b" && weapon != "m" && weapon != "sp" && weapon != "sw");
            basePrice = 0;
            type = 0;
            side = 0;
            switch (weapon)
            {
                case "a":
                    Console.WriteLine("AXE");
                    name = "AXE";
                    basePrice = 25;
                    type = 1;
                    side = 6;
                    break;
                case "b":
                    Console.WriteLine("BOW");
                    name = "AXE";
                    basePrice = 40;
                    type = 2;
                    side = 6;
                    break;
                case "m":
                    Console.WriteLine("MACE");
                    name = "MACE";
                    basePrice = 20;
                    type = 3;
                    side = 4;
                    break;
                case "sp":
                    Console.WriteLine("SPEAR");
                    name = "SPEAR";
                    basePrice = 25;
                    type = 4;
                    side = 4;
                    break;
                case "sw":
                    Console.WriteLine("SWORD");
                    name = "SWORD";
                    basePrice = 50;
                    type = 5;
                    side = 8;
                    break;
            }
            dice = 1;
        }

        private static void DeduplicateName(Character shoppingChar, ref string name)
        {
            bool found;
            do
            {
                found = false;
                for (int i = 0; i < 4; i++)
                {
                    if (shoppingChar.WeaponName[i] == name)
                    {
                        name = $"{name}#";
                        found = true;
                    }
                }
            } while (found);
        }

        private static int Price(int start, float rtio)
        {
            return (int)Math.Ceiling(start * rtio);
        }

        private int GetMainHallMenuChoice()
        {
            Console.WriteLine("\r\nAS YOU WANDER ABOUT THE HALL, YOU");
            Console.WriteLine("REALIZE YOU CAN DO ONE OF SIX THINGS--");
            Console.WriteLine("  1.  GO ON AN ADVENTURE.");
            Console.WriteLine("  2.  VISIT THE WEAPON SHOP FOR WEAPONS");
            Console.WriteLine("      AND/OR ARMOUR.");
            Console.WriteLine("  3.  HIRE A WIZARD TO TEACH YOU SOME");
            Console.WriteLine("      SPELLS.");
            Console.WriteLine("  4.  FIND THE BANKER TO DEPOSIT OR");
            Console.WriteLine("      WITHDRAW SOME GOLD.");
            Console.WriteLine("  5.  EXAMINE YOUR ABILITIES.");
            Console.WriteLine("  6.  TEMPORARILY LEAVE THE UNIVERSE.");
            Console.WriteLine("(ENTER YOUR CHOICE, 1-6)");

            string choice;
            int choiceValue;
            do
            {
                choice = Input();
            } while (!int.TryParse(choice, out choiceValue) || choiceValue < 1 || choiceValue > 6);

            return choiceValue;
        }

        private void ReadCharacters()
        {
            //get number of characters
            string[] numOfCharRecords = new string[1];
            GetRecordsFromFile(Characterfile, CharacterRecordLength, 1, 0, numOfCharRecords);

            this._numberOfCharacters = int.Parse(numOfCharRecords[0]);

            string[] characterStrings = new string[_numberOfCharacters];
            GetRecordsFromFile(Characterfile, CharacterRecordLength, _numberOfCharacters, 1, characterStrings);

            this._characters = new Character[_numberOfCharacters + 1]; //leave room for a new character to be created

            for (int i = 0; i < _numberOfCharacters; i++)
            {
                this._characters[i] = this.ParseCharacter(characterStrings[i]);
            }
        }

        private Character ParseCharacter(string characterString)
        {
            Character character = new Character();

            var currentToken = 0;
            var characterTokens = characterString.Split('\r');

            string adventuring = characterTokens[currentToken++];
            if (adventuring == "1")
            {
                character.Adventuring = true;
            }

            character.Name = characterTokens[currentToken++];
            character.Hardiness = int.Parse(characterTokens[currentToken++]);
            character.Agility = int.Parse(characterTokens[currentToken++]);
            character.Charisma = int.Parse(characterTokens[currentToken++]);

            character.SpellAbility = new int[4];
            character.CurrentSpellAbility = new int[4];
            for (int x = 0; x < 4; x++)
            {
                character.SpellAbility[x] = int.Parse(characterTokens[currentToken++]);
                character.CurrentSpellAbility[x] = character.SpellAbility[x];
            }

            character.WeaponAbility = new int[5];
            for (int x = 0; x < 5; x++)
            {
                character.WeaponAbility[x] = int.Parse(characterTokens[currentToken++]);
            }

            character.ArmorExpertise = int.Parse(characterTokens[currentToken++]);

            character.Sex = characterTokens[currentToken++];

            character.Gold = int.Parse(characterTokens[currentToken++]);

            character.Bank = int.Parse(characterTokens[currentToken++]);

            character.Armor = int.Parse(characterTokens[currentToken++]);

            character.WeaponName = new string[4];
            character.WeaponType = new int[4];
            character.WeaponOdds = new int[4];
            character.WeaponDice = new int[4];
            character.WeaponSides = new int[4];
            for (int x = 0; x < 4; x++)
            {
                //WNAME$(W),WTYPE%(W),WODDS%(W),WDICE%(W),WSIDES%(W)
                character.WeaponName[x] = characterTokens[currentToken++];
                if (string.IsNullOrEmpty(character.WeaponName[x]))
                {
                    character.WeaponName[x] = "NONE";
                }
                character.WeaponType[x] = int.Parse(characterTokens[currentToken++]);
                character.WeaponOdds[x] = int.Parse(characterTokens[currentToken++]);
                character.WeaponDice[x] = int.Parse(characterTokens[currentToken++]);
                character.WeaponSides[x] = int.Parse(characterTokens[currentToken++]);
            }

            return character;
        }

        private void SaveCharacters()
        {
            File.Delete($"{Characterfile}.BAK");
            File.Move(Characterfile, $"{Characterfile}.BAK");


            using (var writer = new StreamWriter(Characterfile))
            {
                writer.Write(this._numberOfCharacters.ToString().PadRight(CharacterRecordLength));

                for (int i = 0; i < this._numberOfCharacters; i++)
                {
                    string serializedCharacter = this.SerializeCharacter(this._characters[i]);
                    writer.Write(serializedCharacter);
                }
            }
        }

        private string SerializeCharacter(Character inChar)
        {
            StringBuilder builder = new StringBuilder();

            if (inChar.Adventuring)
            {
                Append("1",builder);
            }
            else
            {
                Append("0", builder);
            }

            Append(inChar.Name,builder);
            Append(inChar.Hardiness, builder);
            Append(inChar.Agility, builder);
            Append(inChar.Charisma, builder);

            for (int i = 0; i < 4; i++)
            {
                Append(inChar.SpellAbility[i], builder);
            }

            for (int i = 0; i < 5; i++)
            {
                Append(inChar.WeaponAbility[i], builder);
            }

            Append(inChar.ArmorExpertise, builder);
            Append(inChar.Sex, builder);
            Append(inChar.Gold, builder);
            Append(inChar.Bank, builder);
            Append(inChar.Armor, builder);

            for (int i = 0; i < 4; i++)
            {
                Append(inChar.WeaponName[i],builder);
                Append(inChar.WeaponType[i], builder);
                Append(inChar.WeaponOdds[i], builder);
                Append(inChar.WeaponDice[i], builder);
                Append(inChar.WeaponSides[i], builder);
            }

            return builder.ToString().PadRight(CharacterRecordLength,'\0');
        }

        private Character Player()
        {
            return _characters[_theCharacter];
        }

        private Room CurrentRoom()
        {
            return _currentAdventure.Rooms[_currentAdventure.CurrentRoom];
        }

        private int RollDice(int dice, int sides)
        {
            int result = 0;

            for (int i = 0; i < dice; i++)
            {
                result += _r.Next(sides) + 1; //rand.Next returns value >= 0 and < sides so to get 1 to sides you need to add one
            }

            return result;
        }

        private void GetRecordsFromFile(string path, int recordLength, int numberOfRecordsToRead, int recordOffset, string[] results)
        {
            using (var reader = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[recordLength];

                reader.Seek(recordOffset * recordLength, 0);

                for (int i = 0; i < numberOfRecordsToRead; i++)
                {
                    reader.Read(buffer, 0, recordLength);
                    results[i] = Encoding.ASCII.GetString(buffer).Replace("\0", "");
                }
            }
        }

        private void PrintAsLines(string stringToPrint)
        {
            int index = 0;
            while (index < stringToPrint.Length)
            {
                int lineLength = 40;

                if (index + lineLength > stringToPrint.Length)
                {
                    lineLength = stringToPrint.Length - index;
                }

                Console.WriteLine(stringToPrint.Substring(index, lineLength));
                index += lineLength;
            }
        }

        private static string Input()
        {
            return Console.ReadLine() ?? "";
        }

        private static void Append(object token, StringBuilder builder)
        {
            builder.Append(token);
            builder.Append("\r");
        }
        
        private string TrimDescription(string description)
        {
            int endIndex = description.IndexOf("\r", StringComparison.InvariantCulture);
            string trimmedDescription = description.Substring(0, endIndex);
            return trimmedDescription;
        }
    }
}
