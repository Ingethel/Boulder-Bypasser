using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{

    public Text highscore_text, score_text, tempScore_text;
    public GameObject endGameScore;
    int highscore = 0, score = 0, tempScore = 0, acc = 0;
    float lastIncrease = 0;
    public float intervalBonus;

    GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += OnStart;
        manager.EndGameEvent += OnEnd;
    }

    public void OnStart()
    {
        highscore = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.HIGHSCORE));
        highscore_text.text = "Highscore: " + highscore.ToString();
        score = 0;
        score_text.text = "Score: " + score.ToString();
        tempScore = 0; acc = 0;
        manager.InGameEvent -= OnStart;
    }

    public void OnEnd()
    {
        Outline line = endGameScore.GetComponent<Outline>();
        Color cLine = line.effectColor;
        if (acc > 0)
            score += acc * tempScore;
        if (score > highscore) {
            PlayerPrefs.SetInt(Settings.getString(Settings.LABELS.HIGHSCORE), score);
            cLine.a = 0.5f;
        }
        else
        {
            cLine.a = 0;
        }
        line.effectColor = cLine;
        Text endGameText = endGameScore.GetComponent<Text>();
        endGameText.text = score.ToString();
    }

    public void ScoreIncrease(int increaseScore)
    {
        lastIncrease = Time.realtimeSinceStartup;
        if (acc == 0)
            StartCoroutine(BonusCountdown());
        acc++;
        tempScore += increaseScore;
        UpdateTempScoreText();
    }

    public void ResetBonus()
    {
        score += acc * tempScore;
        UpdateScoreText();
        acc = 0;
        tempScore = 0;
        UpdateTempScoreText();
    }

    IEnumerator BonusCountdown()
    {
        while (Time.realtimeSinceStartup - lastIncrease < intervalBonus)
        {
            yield return new WaitForSecondsRealtime(intervalBonus);
        }
        ResetBonus();
    }

    void UpdateHighScoreText()
    {
        highscore_text.text = "Highscore: " + highscore.ToString();
    }

    void UpdateScoreText()
    {
        score_text.text = "Score: " + score.ToString();
    }

    void UpdateTempScoreText()
    {
        if (tempScore == 0)
            tempScore_text.text = " ";
        else
            tempScore_text.text = "x" + acc.ToString() + " " + tempScore.ToString();
    }

}
