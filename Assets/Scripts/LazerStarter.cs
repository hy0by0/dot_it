using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerStarter : MonoBehaviour
{
    [SerializeField] private GameObject lazerMother;
    private Vector2 INITIAL_DIRECTION = new Vector2(0.3f, 1); //最初の方向
    const float ROTATE_SPEED = 0.02f;  //回転速度
    private GameObject newLazer;
    private Lazer lazer;

    public void Start()
    {
        newLazer = Instantiate(lazerMother, this.gameObject.transform.position, Quaternion.identity);
        lazer = newLazer.GetComponent<Lazer>();
        //lazer.creat(newLazer.transform.position, INITIAL_DIRECTION, 0); //レーザー本体を作る処理つくる
    }

    public void Update()
    {
        // 左回転
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, ROTATE_SPEED) * lazer.getDirection());s//処理作ってから
        }
        // 右回転
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, -ROTATE_SPEED) * lazer.getDirection());//処理作ってから
        }
    }
}

