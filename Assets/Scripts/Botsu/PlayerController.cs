using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class PlayerController : MonoBehaviour
{
    public EdgeLine edgeLine;
    public float speed = 15.0f; //移動速度
    public float jumpForce = 10.0f; //いらないかも
    public int satietyLevel = 100; //満腹度
    public int foodCalorie = 10; //食べ物１個あたりの満腹度回復量
    public float score = 0.0f; //score獲得用変数
    float axisX = 0.0f;
    float axisY = 0.0f;
    public string direction = "right"; //一応向き取得するための変数
    public bool jumpFlag = false;
    float Scale_X; //向き変更用の大きさＸ方向の変数

    float countTime = 0.0f; //満腹度減る時間計測変数
    public float intervalTime = 0.5f;//満腹度減る時間単位(小さいほど短い間隔で)
    public Transform satietyObj;
    public GameObject tongueObj; //舌オブジェクトを入れる
    Line line;
    Rigidbody2D rbody;

    public static string gameState = "playing";

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        line = tongueObj.GetComponent<Line>();
        //animator = GetComponent<Animator>();
        //nowAnime = stopAnime;
        //oldAnime = stopAnime;
        countTime = 0.0f;
        Scale_X = this.transform.localScale.x;
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        axisX = Input.GetAxisRaw("Horizontal"); //左右入力を感知
        axisY = Input.GetAxisRaw("Vertical"); //上下入力を感知

        if (!line.backFlag && !line.moving)
        {
            if(line.getCount != 0)
            {
                score += line.tongueScore * (1.0f + 0.5f*(line.getCount - 1) ); //score加算
                satietyLevel += line.getCount * foodCalorie; //満腹度回復
                line.tongueScore = 0.0f;
                line.getCount = 0;
            }
        }

        satietyObj.localScale = new Vector3(1, (float)satietyLevel / 100, 1);


        //向き変更
        //if (axisX > 0)
        //{
        //    this.transform.localScale = new Vector2(Scale_X, this.transform.localScale.y);
        //    direction = "right";
        //}
        //else if (axisX < 0)
        //
        //    this.transform.localScale = new Vector2(-Scale_X, this.transform.localScale.y);
        //    direction = "left";
        //}
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }

        //満腹度０かつ、舌が伸びていないとき
        if (satietyLevel <= 0 && !line.backFlag && !line.moving && line.getCount == 0)
        {
            Debug.Log("ゲームオーバー");
            gameState = "gameover";
            rbody.velocity = Vector3.zero;
            GameStop();
        }
        else if (satietyLevel > 0)
        {
            countTime += Time.deltaTime;
            if(countTime >= intervalTime)
            {
                satietyLevel -= 1; //満腹度を減らす
                countTime = 0.0f;

                if (satietyLevel < 0)
                {
                    satietyLevel = 0;
                }
            }
        }

        if (edgeLine.tongueFlag || edgeLine.backFlag)
        {
            rbody.velocity = Vector3.zero;
        }
        else
        {
            rbody.velocity = new Vector2(axisX * speed, axisY * speed); //移動の反映
        }
        

        //if (nowAnime != oldAnime) // アニメーションの変更を反映(ミスとクリア以外)
        //{
        //    oldAnime = nowAnime;
        //animator.Play(nowAnime);
        //}
    }


    public void Jump()
    {

            jumpFlag = true;

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Goal")
        {
            Debug.Log("<color=red>ゴールした</color>");
            Goal();
        }

        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("<color=red>敵にあたった</color>");
            Miss();
        }

        if (collider.gameObject.tag == "Item")
        {
            Debug.Log("直接むしゃむしゃ");
            Item ItemScript = collider.gameObject.GetComponent<Item>();
            score += ItemScript.scoreValue;
            satietyLevel += foodCalorie; //満腹度回復
            Destroy(collider.gameObject);
        }
    }


    public void Goal()
    {
        //Debug.Log("<color=red>まじでゴールした</color>");
        gameState = "clearStage";
        GameStop();
    }


    public void Miss()
    {
        //Debug.Log("<color=red>いってええええ！</color>");
        gameState = "miss";
        //animator.Play(missAnime);
        //transform.DOLocalMoveY(1, 1f);
        GameStop();
    }

    public void Eat()
    {
        //
    }


    void GameStop()
    {
        rbody.velocity = new Vector2(0, 0);
    }
}
