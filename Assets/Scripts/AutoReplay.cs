using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AutoReplay : MonoBehaviour
{
    float timer;
    public TextMeshProUGUI info;

    // Start is called before the first frame update
    void Start()
    {
        if (EnemyController.enemyKilled >= 4)
        {
            info.text = "Congratulations \n You Win!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 5)
        {
            Data.score = 0;
            EnemyController.enemyKilled = 0;
            SceneManager.LoadScene("Level1");
        }
    }
}
