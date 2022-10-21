using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI txScore;
    public TextMeshProUGUI txHighscore;
    TextMeshProUGUI txSelamat;
    int highscore;

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("HS", 0);
        if(Data.score > highscore)
        {
            highscore = Data.score;
            PlayerPrefs.SetInt("HS",highscore);
        }else if (EnemyController.enemyKilled >= 4)
        {
            SceneManager.LoadScene("Congratulations");
        }
        txHighscore.text = "Highscore : " + highscore;
        txScore.text = "Score : " + Data.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Replay()
    {
        Data.score = 0;
        EnemyController.enemyKilled = 0;
        SceneManager.LoadScene("Level1");
    }
}
