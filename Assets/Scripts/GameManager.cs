using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject scoreText; // Textオブジェクト
    public float resultScore = 0; // スコア変数
    
    // Start is called before the first frame update
    void Start()
    {
        resultScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトからTextコンポーネントを取得
        Text score_text = scoreText.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text.text = "Score:" + resultScore;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController_2 playerCnt = playerObj.GetComponent<PlayerController_2>();
        if (playerCnt.score != 0)
        {
            resultScore += playerCnt.score;
            playerCnt.score = 0.0f;
        }

    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
