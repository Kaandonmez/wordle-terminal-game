class Wordle
{
    public Dictionary<string, bool> wordList = new Dictionary<string, bool>();
    public Dictionary<string, string> dicti = new Dictionary<string, string>();
    public string[] testWordle = new string[1];
    public string[] mainDictionary =
    {
        //generate 30 random words
        "abandon",
        "ability",
        "able",
        "about",
        "above",
        "absent",
        "absorb",
        "abstract",
        "absurd",
        "abuse",
        "access",
        "accident",
        "account",
        "accuse",
        "achieve",
        "acid",
        "acoustic",
        "acquire",
        "across",
        "act",
        "action",
        "actor",
        "actress",
        "actual",
        "adapt",
        "add",
        "addict",
        "address",
        "adjust",
        "admit",
        "adult",
        "advance",
        "advice",
        "aerobic",
        "affair",
        "afford",
        "afraid",
    };
    public string[] unknownKeys = new string[1];

    public char[] alphabet = new char[26];

    public int lifes = 5;
    public char guessedChar;
    public bool contains = false;

    public bool isGuessedThisTurn = false;
    public bool isGameOver = false;

    public Wordle()
    {
        Console.SetWindowSize(40, 10);

        //set window size
    }

    public void createGameUi()
    {
        // create game ui
    }

    public void createDictionary()
    {
        //dictionary between testWordle and unknownKeys
        // for (int i = 0; i < testWordle.Length; i++)
        // {
        //     dicti.Add(testWordle[i], unknownKeys[i]);
        // }

        //add to dicti all words and number of letters '_'
        foreach (var item in testWordle)
        {
            string newWord = "";
            foreach (var letter in item)
            {
                newWord += '_';
            }
            dicti.Add(item, newWord);
        }
    }

    public void setWordList()
    {
        // set word list

        //create alpabet array and fill it with lowerkey letters
        for (int i = 0; i < alphabet.Length; i++)
        {
            alphabet[i] = (char)('a' + i);
        }

        //add to testWordle random words from mainDictionary
        for (int i = 0; i < testWordle.Length; i++)
        {
            //random number between 0 and 29
            int randomNumber = new Random().Next(0, mainDictionary.Length - 1);

            testWordle[i] = mainDictionary[randomNumber];
        }

        //fill unknownKeys array with "_" characters per against its testWordle length
        for (int i = 0; i < unknownKeys.Length; i++)
        {
            unknownKeys[i] = String.Concat(Enumerable.Repeat("_", testWordle[i].Length));
        }

        //create dictionary with words and set their values to false(is guessed yet)
        // foreach (var item in testWordle)
        // {
        //     wordList.Add(item, false);
        // }
        //createDictionary();
    }

    public void printAlphabet()
    {
        int i = 0;
        foreach (var item in alphabet)
        {
            if (i % 10 == 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
            Console.Write(item + " ");
            i++;
        }
    }

    public void showLives()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Lives: " + lifes);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public bool checkIfContainsInOthers()
    {
        contains = false;
        foreach (var item in testWordle)
        {
            if (item.Contains(guessedChar))
            {
                contains = true;
                break;
            }
        }

        return contains;
    }

    public void guess()
    {
        do
        {
            showLives();
            printAlphabet();
            informPlayer();

            Console.SetCursorPosition(5, Console.CursorTop + 3);

            Console.Write("Type your guess(letter): ");
            //read character
            guessedChar = Console.ReadKey().KeyChar;
            int i;
            isGuessedThisTurn = false;
            for (i = 0; i < testWordle.Length; i++)
            {
                string? item = testWordle[i];

                if (item.Contains(guessedChar))
                {
                    isGuessedThisTurn = true;
                    foreach (var item1 in item)
                    {
                        if (item1 == guessedChar)
                        {
                            item = testWordle[i];
                            //remove char from item1 position in unknownKeys
                            unknownKeys[Array.IndexOf(testWordle, item)] = unknownKeys[
                                Array.IndexOf(testWordle, item)
                            ]
                                .Remove(item.IndexOf(item1), 1)
                                .Insert(item.IndexOf(item1), guessedChar.ToString());

                            //remove same positions from testWordle
                            testWordle[Array.IndexOf(testWordle, item)] = testWordle[
                                Array.IndexOf(testWordle, item)
                            ]
                                .Remove(item.IndexOf(item1), 1)
                                .Insert(item.IndexOf(item1), "_");
                        }
                    }
                    //control if it is in alphabet array, remove character from alphabet array
                    if (Array.IndexOf(alphabet, guessedChar) != -1)
                    {
                        alphabet[Array.IndexOf(alphabet, guessedChar)] = '-';

                        if (checkIfContainsInOthers())
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (checkIfContainsInOthers())
                        {
                            continue;
                        }
                    }
                }
                else if (checkIfContainsInOthers())
                {
                    isGuessedThisTurn = true;
                    continue;
                }
                else
                {
                    break;
                }

                i = i + 1;
            }
            if (isGuessedThisTurn)
            {
                Console.SetCursorPosition(Console.CursorLeft + 3, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Great! Your guess was Right -> {0}!", guessedChar);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(500);
            }
            else
            {
                Console.SetCursorPosition(Console.CursorLeft + 3, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Oops you guessed Wrong -> {0}!", guessedChar);
                Console.ForegroundColor = ConsoleColor.White;
                lifes = lifes - 1;
                if (Array.IndexOf(alphabet, guessedChar) != -1)
                {
                    alphabet[Array.IndexOf(alphabet, guessedChar)] = '-';
                }
                Thread.Sleep(500);
            }

            Thread.Sleep(500);

            Console.Clear();

            foreach (var item in unknownKeys)
            {
                if (item.Contains('_') == false)
                {
                    isGameOver = true;
                }
            }
        } while (!isGameOver && lifes > 0);

        if (lifes > 0)
        {
            //Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i < 5; i++)
            {
                Console.Write("You won!");
                Thread.Sleep(400);
                Console.Clear();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            //Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 5; i++)
            {
                Console.Write("You lose!");
                Thread.Sleep(400);
                Console.Clear();
            }
        }
    }

    public void informPlayer()
    {
        Console.SetCursorPosition(30, 0);
        Console.ForegroundColor = ConsoleColor.Blue;

        foreach (var item in unknownKeys)
        {
            Console.Write(item);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
        }

        Console.ForegroundColor = ConsoleColor.White;
    }
}
