using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{

    public PlayerStats PScore;
    public TextMeshProUGUI ScorePoints;


void Start()
    {
        
    }

void Update()
    {
     // Load the score, defaulting to 0 if "PlayerScore" doesn't exist
        int loadedScore = PlayerPrefs.GetInt("PScore", 0);
        ScorePoints.text = loadedScore.ToString();

    }

}
