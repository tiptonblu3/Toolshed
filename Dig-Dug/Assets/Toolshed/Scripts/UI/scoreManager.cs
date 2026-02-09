using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{

    public PlayerStats PlayerScore;
    public TextMeshProUGUI ScorePoints;

    public void ScoreInfo()
    {
        GameObject PlayerObj = GameObject.FindWithTag("Player");
        PlayerStats Stats = PlayerObj.GetComponent<PlayerStats>();
    }

    public void UpdateScoreText()
    {
        ScorePoints.text = PlayerScore.Score.ToString();
    }

void Start()
    {
        ScoreInfo();
    }

void Update()
    {
        UpdateScoreText();
    }

}
