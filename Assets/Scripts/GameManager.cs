using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject letter;
    public GameObject cen;
    string wordToGuess = "";
    int lengthOfWordToGuess;
    char[] lettersToGuess;
    bool[] lettersGuessed;
    string[] wordsToGuess = { "car", "elephant", "autocar" };
    int nbAttempts, maxNbAttempts;
    int score = 0;

    void Start()
    {
        nbAttempts = 0;

        cen = GameObject.Find("centerOfScreen");

        InitGame();
        InitLetters();

        UpdateNbAttempts();
        UpdateScore();

    }

    void Update()
    {
        CheckKeyboard2();
    }

    void CheckKeyboard2()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            if (string.IsNullOrEmpty(Input.inputString))
                return;

            char letterPressed = Input.inputString.ToCharArray()[0];
            int letterPressedAsInt = System.Convert.ToInt32(letterPressed);

            if (letterPressedAsInt >= 97 && letterPressedAsInt <= 122)
            {
                nbAttempts++;
                UpdateNbAttempts();
                if (nbAttempts > maxNbAttempts)
                {
                    SceneManager.LoadScene("wordGameEnd");
                    return;
                }

                for (int i = 0; i < lengthOfWordToGuess; i++)
                {
                    if (!lettersGuessed[i])
                    {
                        letterPressed = System.Char.ToUpper(letterPressed);

                        if (lettersToGuess[i] == letterPressed)
                        {
                            lettersGuessed[i] = true;

                            GameObject.Find("letter" + (i + 1))
                                .GetComponent<TMPro.TextMeshProUGUI>()
                                .text = letterPressed.ToString();

                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            CheckIfWordWasFound();
                        }
                        
                    }
                }
            }
        }
    }

    void InitGame()
    {
        wordToGuess = PickAWordFromFile();

        lengthOfWordToGuess = wordToGuess.Length;

        wordToGuess = wordToGuess.ToUpper();

        maxNbAttempts = wordToGuess.Length * 2;

        lettersToGuess = new char[lengthOfWordToGuess];

        lettersGuessed = new bool[lengthOfWordToGuess];

        lettersToGuess = wordToGuess.ToCharArray();
    }

    void InitLetters()
    {
        int nbLetters = lengthOfWordToGuess;

        for (int i = 0; i < nbLetters; i++)
        {
            Vector3 newPosition;

            newPosition = new Vector3(
            cen.transform.position.x + ((i - nbLetters / 2.0f) * 100),
            cen.transform.position.y,
            cen.transform.position.z
            );

            GameObject l = (GameObject)Instantiate(
                letter,
                newPosition,
                Quaternion.identity
            );

            l.name = "letter" + (i + 1);

            l.transform.SetParent(
                GameObject.Find("Canvas").transform
            );
        }
    }

    void UpdateNbAttempts()
    {
        GameObject.Find("nbAttempts")
            .GetComponent<TextMeshProUGUI>()
            .text = nbAttempts + "/" + maxNbAttempts;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI")
            .GetComponent<TextMeshProUGUI>()
            .text = "Score:" + score;
    }

    void CheckIfWordWasFound()
    {
        bool condition = true;

        for (int i = 0; i < lengthOfWordToGuess; i++)
        {
            condition = condition && lettersGuessed[i];
        }

        if (condition)
        {
            PlayerPrefs.SetString("lastWordGuessed", wordToGuess);
            SceneManager.LoadScene("wordGameWin");
        }
    }

    string PickAWordFromFile()
    {
        TextAsset t1 = (TextAsset)Resources.Load("words", typeof(TextAsset));

        string s = t1.text;

        string[] words = s.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        int randomWord = Random.Range(0, words.Length);

        return words[randomWord];
    }


}
