using UnityEngine;
using TMPro;

public class HscoreManager : MonoBehaviour
{

    public PlayerStats PlayerStats;
    public TextMeshProUGUI ScorePoints;

    public int HighScore;

    public void HScoreInfo()
    {
        HighScore = PlayerStats.HighScore;
;
    }

    public void UpdateHScoreText()
    {
        ScorePoints.text = HighScore.ToString();
    }

void Start()
    {
        HScoreInfo();
    }

void Update()
    {
        UpdateHScoreText();
    }

}
