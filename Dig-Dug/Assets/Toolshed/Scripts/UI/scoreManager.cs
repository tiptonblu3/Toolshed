using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{

    public PlayerStats PScore;
    public TextMeshProUGUI ScorePoints;

    public void ScoreInfo()
    {
        GameObject PlayerObj = GameObject.FindWithTag("Player");
        PScore = PlayerObj.GetComponent<PlayerStats>();
    }

    public void UpdateScoreText()
    {
    }

void Start()
    {
        ScoreInfo();
         // Load the score, defaulting to 0 if "PlayerScore" doesn't exist
        int loadedScore = PlayerPrefs.GetInt("PScore", 0);
        PScore.Score = loadedScore;
    }

void Update()
    {
        UpdateScoreText();
                ScorePoints.text = PScore.Score.ToString();

    }

}
