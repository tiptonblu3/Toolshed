using UnityEngine;
using TMPro;

public class HscoreManager : MonoBehaviour
{

    public TextMeshProUGUI ScorePoints;

   public int SaveScoreKey;
   public int HighScore;

    public void HScoreInfo()
    {
        if (PlayerPrefs.GetInt("Highscore", 0) <= PlayerPrefs.GetInt("PScore", 0))
        {
            HighScore = PlayerPrefs.GetInt("PScore", 0);

            PlayerPrefs.SetInt("Highscore", HighScore);
            PlayerPrefs.Save(); // Save the new high score to disk
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
