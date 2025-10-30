using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EdgeLine : MonoBehaviour
{
    public bool tongueFlag = false; //舌が伸びていくかどうかのフラグ
    public bool backFlag = false; //舌が縮んでいくかどうかのフラグ
    public GameObject StartPos; //伸びはじめの起点の位置
    public float speed = 4.0f; //舌が伸びていくスピード（使わないかも？）
    public float backSpeed = 20.0f; //舌が戻るスピード（使わないかも？）
    public float arriveThreshold = 0.1f; // 誤差吸収用。元の位置との距離がこの値以下になれば、戻ったことにする。
    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        tongueFlag = false;
        backFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //キー入力による状態の反映
        if (Input.GetMouseButtonDown(0))
        {
            tongueFlag = true;
            backFlag = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            tongueFlag = false;
            backFlag = true;
        }

        if (tongueFlag) //舌が伸びていくとき
        {
            Vector2 vec = transform.up * speed;
            rbody.velocity = vec; //移動の反映
        }
        else if(backFlag) //舌が縮んでいくとき
        {
            //rbody.velocity = Vector2.zero;  // Rigidbodyの移動を止める（MoveTowardsはTransform操作）
            //transform.position = Vector2.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), 20 * Time.deltaTime);

            //１つ前の頂点or起点へ戻っていくようにする（そのまま再度伸びることもできるように）
            rbody.velocity = -transform.up * backSpeed;
            //Vector2 toStart = ((Vector2)StartPos.transform.position - rbody.position).normalized; //戻る方向ベクトルを計算
            //rbody.velocity = toStart * backSpeed;//元の位置へ戻らせる

            //元の位置との距離を見て、戻り切ったかどうかを見る。
            //float distance = Vector2.Distance(rbody.position, StartPos.transform.position);
            //if (distance < arriveThreshold)
            //{
                // 戻り完了
                //Debug.Log("帰宅完了");
            //    backFlag = false;
            //    transform.position = StartPos.transform.position;
            //    rbody.velocity = Vector2.zero;
            //}
            
        }
        else //舌が起点へ縮み切っているとき、伸ばしていないとき
        {
            this.transform.position = StartPos.transform.position;
            rbody.velocity = Vector2.zero;
            // マウスの方向へ向きを追随させる
            var pos = Camera.main.WorldToScreenPoint(this.transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
        }

        
    }
}
