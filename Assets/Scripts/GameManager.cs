using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Flag answer;

    public float flagSizeMod = 0.5f;

    public int score = 0;
    public int incorrectScore = 0;

    public Image flagImage;

    public TextMeshProUGUI scoreText, incorrectText;
    public Button choice1, choice2, choice3, choice4;
    private TextMeshProUGUI choiceText1, choiceText2, choiceText3, choiceText4;

    public GameObject gameOverPanel;

    public TextMeshProUGUI finalScoreText, finalIncorrectText;



    List<Flag> flags;
    public int totalFlags;

    public struct Flag
    {
        public Sprite sprite;
        public string name;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        finalScoreText = finalScoreText.GetComponent<TextMeshProUGUI>();
        finalIncorrectText = finalIncorrectText.GetComponent<TextMeshProUGUI>();

        flags = new List<Flag>();

        flagImage = flagImage.GetComponent<Image>();

        scoreText = scoreText.GetComponent<TextMeshProUGUI>();
        incorrectText = incorrectText.GetComponent<TextMeshProUGUI>();


        choice1 = choice1.GetComponent<Button>(); //Buttons
        choice2 = choice2.GetComponent<Button>();
        choice3 = choice3.GetComponent<Button>();
        choice4 = choice4.GetComponent<Button>();

        choiceText1 = choice1.gameObject.GetComponentInChildren<TextMeshProUGUI>(); //Text buttons
        choiceText2 = choice2.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        choiceText3 = choice3.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        choiceText4 = choice4.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        LoadRes();
        print(flags.Count);
        totalFlags = flags.Count;

        UpdateScore();

        NextFlag();
    }

    void NextFlag()
    {
        if(score < totalFlags)
        {
            int rng = Random.Range(0, flags.Count);

            flagImage.rectTransform.localScale = flags[rng].sprite.bounds.size * flagSizeMod; //change size
            flagImage.sprite = flags[rng].sprite; 
            print(flags[rng].name);

            answer = new Flag() { sprite = flags[rng].sprite, name = flags[rng].name };

            switch (flags.Count)
            {
                case 1:
                    choice1.interactable = true;
                    choice2.gameObject.SetActive(false);
                    choice3.gameObject.SetActive(false);
                    choice4.gameObject.SetActive(false);
                    break;
                case 2:
                    choice1.interactable = true;
                    choice2.interactable = true;
                    choice3.gameObject.SetActive(false);
                    choice4.gameObject.SetActive(false);
                    break;
                case 3:
                    choice1.interactable = true;
                    choice2.interactable = true;
                    choice3.interactable = true;
                    choice4.gameObject.SetActive(false);
                    break;
                default:
                    choice1.interactable = true;
                    choice2.interactable = true;
                    choice3.interactable = true;
                    choice4.interactable = true;
                    break;
            }

            UpdateButtons();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // visa overlay (med "score / totalFlags" och "incorrectScore")
        // stoppa input
        // fråga om spelaren vill gå tillbaka till "StartQuiz"
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
        finalIncorrectText.text = $"Incorrect Guesses: {incorrectScore}";
    }

    void UpdateButtons()
    {

        List<string> choices = new List<string>();

        int x = 0, y = 0;
        if (flags.Count == 1)
        {
            x = 0;
            y = 1;
        }
        else if (flags.Count == 2)
        {
            x = 1;
            y = 2;
        }
        else if (flags.Count == 3)
        {
            x = 2;
            y = 3;
        }
        else if (flags.Count >= 4)
        {
            x = 3;
            y = 4;
        }

        for (int i = 0; i < x; i++)
        {
            int rng = Random.Range(0, flags.Count);

            while (choices.Contains(flags[rng].name) || flags[rng].name == answer.name)
            {
                rng = Random.Range(0, flags.Count);
            }

            choices.Add(flags[rng].name);
        }

        choices.Add(answer.name);

        

        for (int i = 0; i < y; i++)
        {
            int n = Random.Range(0, choices.Count);
            switch (i)
            {
                case 0:
                    choiceText1.text = choices[n];
                    break;
                case 1:
                    choiceText2.text = choices[n];
                    break;
                case 2:
                    choiceText3.text = choices[n];
                    break;
                case 3:
                    choiceText4.text = choices[n];
                    break;
            }

            choices.RemoveAt(n);
        }
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {score} / {totalFlags}";
        incorrectText.text = $"Incorrect: {incorrectScore}";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    //on button clicks
    public void OnClick1()
    {
        if (choiceText1.text == answer.name)
        {
            CorrectAnswer();
        }
        else
        {
            choice1.interactable = false;
            incorrectScore += 1;
            UpdateScore();
        }
    }

    public void OnClick2()
    {
        if (choiceText2.text == answer.name)
        {
            CorrectAnswer();
        }
        else
        {
            choice2.interactable = false;
            incorrectScore += 1;
            UpdateScore();
        }
    }

    public void OnClick3()
    {
        if (choiceText3.text == answer.name)
        {
            CorrectAnswer();
        }
        else
        {
            choice3.interactable = false;
            incorrectScore += 1;
            UpdateScore();
        }
    }

    public void OnClick4()
    {
        if (choiceText4.text == answer.name)
        {
            CorrectAnswer();
        }
        else
        {
            choice4.interactable = false;
            incorrectScore += 1;
            UpdateScore();
        }
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("StartQuiz");
    }

    void CorrectAnswer()
    {
        flags.Remove(answer);

        score += 1;
        UpdateScore();
        
        NextFlag();
    }

    //Load resources
    void LoadRes()
    {
        Texture2D[] flagImages = Resources.LoadAll<Texture2D>("Flags");

        foreach (var image in flagImages)
        {
            Sprite flag = Sprite.Create(image, new Rect(0f, 0f, image.width, image.height), new Vector2(0.5f, 0.5f));
            flags.Add(new Flag() {sprite = flag, name = image.name});
        }
    }

}

