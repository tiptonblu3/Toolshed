using UnityEngine;
using TMPro;

public class HscoreManager : MonoBehaviour
{

    public TextMeshProUGUI ScorePoints;

   public int SaveScoreKey;
   public int HighScore;

    public void HScoreInfo()
    {
        int lastRoundScore = PlayerPrefs.GetInt("PScore", 0);
        int currentHighScore = PlayerPrefs.GetInt("Highscore", 0);
        if (lastRoundScore >= currentHighScore)
        {
            HighScore = lastRoundScore;
            PlayerPrefs.SetInt("Highscore", HighScore);
            PlayerPrefs.Save();
        }
        else
        {
            HighScore = currentHighScore;
        }
    }

    public void UpdateHScoreText()
    {
        ScorePoints.text = HighScore.ToString();   
         }

void Start()
    {
        HScoreInfo();
        PlayerPrefs.SetInt("PScore", 0);
                    // Highly recommended: Force a save to disk immediately
        PlayerPrefs.Save();
        UpdateHScoreText();
    }

void Update()
    {
        
    }

}
