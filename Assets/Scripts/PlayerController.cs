using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class PlayerController : MonoBehaviour
{
    public EdgeLine edgeLine; //いらないかも。ずっと動けるなら
    public float speed = 8.0f; //レーザーを撃っている間の移動速度
    public float speed_quick = 9.0f; //レーザーを撃たないときの移動速度
    public float jumpForce = 10.0f; //いらないかも

    public float score = 0.0f; //score獲得用変数
    float axisX = 0.0f;
    float axisY = 0.0f;
    public string direction = "right"; //一応向き取得するための変数
    public bool jumpFlag = false;
    float Scale_X; //向き変更用の大きさＸ方向の変数

    public LaserStarter lazerStarter;//LaserStarterを入れる
    Rigidbody2D rbody;

    public static string gameState = "playing";

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        //laser = laserObj.GetComponent<Line>();
        //animator = GetComponent<Animator>();
        //nowAnime = stopAnime;
        //oldAnime = stopAnime;
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

        if (Input.GetMouseButtonDown(0))
        {
            lazerStarter.Click();

        }
        else if(Input.GetMouseButtonUp(0))
        {
            lazerStarter.Stop();
        }




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


        if (edgeLine.tongueFlag || edgeLine.backFlag)
        {
            rbody.velocity = new Vector2(axisX * speed, axisY * speed); //移動の反映
        }
        else
        {
            rbody.velocity = new Vector2(axisX * speed_quick, axisY * speed_quick); //移動の反映
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

        //if (collider.gameObject.tag == "Damage" || collider.gameObject.tag == "Laser")
        //{
        //    Debug.Log("<color=red>痛い！</color>");
        //    Miss();
        //}

        if (collider.gameObject.tag == "Item")
        {
            Debug.Log("直接むしゃむしゃ");
            Item ItemScript = collider.gameObject.GetComponent<Item>();
            score += ItemScript.scoreValue;
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


    void GameStop()
    {
        rbody.velocity = new Vector2(0, 0);
    }
}
