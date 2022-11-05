// Maak new game

yahtzee.Game game = new yahtzee.Game();

// Start de game

game.Start();

namespace yahtzee
{
    class Game
    {
        public static Random random = new();
        public void Start ()
        {
            bool loop = true;
            while(loop)
            {
                Console.WriteLine("1. Yahtzee spelregels weergeven");
                Console.WriteLine("2. Yahtzee spel spelen");
                Console.WriteLine("3. Stoppen");
                Console.Write("Maak uw keuze: ");
                if(int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch(choice)
                    {
                        case 1:
                            loop = DisplayRules();
                            Console.Clear();
                            break;
                        case 2:
                            loop = Play();
                            Console.Clear();
                            break;
                        case 3:
                            loop = false;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Dat is geen geldige keuze!");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Dat is geen geldige keuze!");
                }
            }
        }

        public static bool Play ()
        {
            Console.Write("Geef de naam van de speler: ");
            string? playername = Console.ReadLine();
            while(true)
            {
                Console.Clear();
                Console.Write("Druk op \'S\' om het spel te starten: ");
                if(Console.ReadKey().KeyChar == 's')
                {
                    break;
                }
            }
            int throws = 0;
            int[] hold = new int[5];
            int[] result = new int[5];
            KeyValuePair<string, int> score = GetCategory(result);
            while(throws < 3)
            {
                if(throws != 0)
                {
                    ShowResults(result, throws, score);
                    Console.Write("Wilt u nog een keer werpen (J/N): ");
                    while(true)
                    {
                        if(char.TryParse(Console.ReadLine(), out char choice) && (choice == 'J' || choice == 'N'))
                        {
                            if(choice == 'J')
                            {
                                throws++;
                                Console.Clear();
                                hold = Hold(result);
                                result = Throw(result, hold);
                                score = GetCategory(result);
                                ShowResults(result, throws, score);
                                break;
                            }
                            else if(choice == 'N')
                            {
                                goto a;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            score = GetCategory(result);
                            ShowResults(result, throws, score);
                            Console.WriteLine("Dat is geen goede input!");
                            Console.Write("Wilt u nog een keer werpen (J/N): ");
                        }
                    }
                }
                Console.Clear();
                if(throws == 0)
                {
                    throws++;
                    result = Throw(result, hold);
                    score = GetCategory(result);
                    ShowResults(result, throws, score);
                }
            }
        a:
            End(playername, throws, result, score);
            return true;
        }

        // Print de regels.

        public static bool DisplayRules ()
        {
            Console.Clear();
            Console.WriteLine("[1].\n\tEen speler mag maximaal drie keer met de dobbelstenen werpen.\n\tBij een goed resultaat mag de speler ook minder keren gooien. \n");
            Console.WriteLine("[2].\n\tBij de eerste en tweede worp mag de speler dobbelstenen ‘On hold’ zetten.\n\tDeze dobbelestenen worden bij de volgende worp niet meegenomen en behouden hun waarde.\n\tx = niet in de on-hold, h = on-hold.\n\tVoorbeeld van een geldige invoer: x, x, h, h, x\n");
            Console.WriteLine("[3].\n\tNa elke worp wordt er weergegeven welke vorm van yahtzee er is gegooid en \n\twordt het aantal bijhorende punten weergegeven. \n");
            Console.WriteLine("Druk een toets in om verder te gaan.");
            Console.ReadKey();
            return true;
        }

        // Converteert de input naar binary om te zien of het opnieuw gerold word

        public static int[] Hold ( int[] previous )
        {
            Console.Clear();

            int[] result = new int[5] { 0, 0, 0, 0, 0 };
            while(true)
            {
                Console.WriteLine("Uw vorige worp was: \n");
                Console.WriteLine(string.Join("\t", previous) + "\n\n");
                Console.WriteLine("Geef weer welke stenen je in de ‘on hold’ wilt plaatsen, h = on hold, x = opnieuw:");
                Console.WriteLine("Voorbeeld van een geldige invoer: h, x, h, x, h");
                Console.Write("Geef een geldige invoer:");
                var remove = new string[] { " ", "," };
                string input = Console.ReadLine();
                foreach(var c in remove)
                {
                    input = input.Replace(c, string.Empty);
                }
                for(int i = 0; i < 5; i++)
                {
                    
                    if(input.Length != 5)
                    {
                        Console.Clear();
                        Console.WriteLine("Dat is geen geldige input!");
                        break;
                    }
                    if(input[i] == 'x')
                    {
                        result[i] = 0;

                    }
                    else if(input[i] == 'h')
                    {
                        result[i] = 1;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Dat is geen geldige input!");
                        break;
                    }
                    if(i == 4)
                    {
                        return result;
                    }
                }

            }
        }

        // Haal de yahtzee categorie en de score op.

        public static KeyValuePair<string, int> GetCategory ( int[] result )
        {
            string category = "";
            int score = 0;
            List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();
            int[] result2 = result.OrderBy(numb => numb).ToArray();

            // Check if Yahtzee:
            if(result2[0] == result2[1] && result2[0] == result2[2] && result2[0] == result2[3] && result2[0] == result2[4])
            {
                score = 50;
                category = "Yahtzee!";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }

            // Check if Vier gelijke:
            if((result2[0] == result2[1] && result2[0] == result2[2] && result2[0] == result2[3]) ||
                     (result2[4] == result2[3] && result2[4] == result2[2] && result2[4] == result2[1])
                    )
            {
                score = result.Sum();
                category = "Vier Gelijke";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }
            // Check if Drie gelijke:
            if((result2[0] == result2[1] && result2[0] == result2[2]) ||
                     (result2[4] == result2[2] && result2[4] == result2[3]) ||
                     (result2[1] == result2[2] && result2[1] == result2[3])
                )
            {
                score = result.Sum();
                category = "Drie Gelijke";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }

            // Check if Full House
            if((result2[0] == result2[1] && result2[2] == result2[3] && result2[2] == result2[4]) ||
                     (result2[0] == result2[1] && result2[0] == result2[2] && result2[3] == result2[4])
                )
            {
                score = 25;
                category = "Full House";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }

            // Check if Grote straat:
            if((result2[0] == result2[1] - 1 && result2[0] == result2[2] - 2 && result2[0] == result2[3] - 3 && result2[0] == result2[4] - 4))
            {
                score = 40;
                category = "Grote straat";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }

            // Check if Kleine straat
            if((result2[0] == result2[1] - 1 && result2[0] == result2[2] - 2 && result2[0] == result2[3] - 3) ||
               (result2[1] == result2[2] - 1 && result2[1] == result2[3] - 2 && result2[1] == result2[4] - 3)
              )
            {
                score = 30;
                category = "Kleine straat";
                scores.Add(new KeyValuePair<string, int>(category, score));
            }

            // Kans:
            score = 0;
            for(int i = 0; i < result2.Length; i++)
            {
                score += result2[i];
                category = "Kans";

            }
            scores.Add(new KeyValuePair<string, int>(category, score));

            // Kijk welke de hoogste score heeft:
            var Max = scores.MaxBy(kvp => kvp.Value);
            return Max;
        }

        // Gooi de dobbelstenen.

        public static int[] Throw ( int[] result, int[] hold )
        {
            for(int i = 0; i < 5; i++)
            {
                if(hold[i] == 0)
                {
                    result[i] = random.Next(1, 7);
                }
            }
            return result;
        }

        // Display het einde

        public static void End ( string playername, int throws, int[] result, KeyValuePair<string, int> score )
        {
            Console.Clear();
            Console.WriteLine("Het resultaat van: " + playername);
            Console.WriteLine("Aantaal worpen: " + throws);
            Console.WriteLine("Resultaat:\t" + String.Join("\t", result));
            Console.WriteLine("Yahtzee categorie: " + score.Key);
            Console.WriteLine("Aantal punten: " + score.Value + "\n\n");
            Console.WriteLine("Druk een toets in om verder te gaan.");
            Console.ReadKey();
        }

        // Laat de resultaten van de worp zien.

        public static void ShowResults ( int[] result, int throws, KeyValuePair<string, int> score )
        {
            Console.Clear();
            if(throws == 1)
            {
                Console.WriteLine("Aantal ogen van de eerste worp: \n" + string.Join('\t', result) + "\n");
                Console.WriteLine("Resultaat van de eerste worp:");


            }
            else if(throws == 2)
            {
                Console.WriteLine("Aantal ogen van de tweede worp: \n" + string.Join('\t', result) + "\n");
                Console.WriteLine("Resultaat van de tweede worp:");

            }
            else
            {
                Console.WriteLine("Aantal ogen van de derde worp: \n" + string.Join('\t', result) + "\n");
                Console.WriteLine("Resultaat van de derde worp:");

            }
            Console.WriteLine("Yahtzee categorie = " + score.Key);
            Console.WriteLine("Aantal punten: " + score.Value);
        }
    }
}