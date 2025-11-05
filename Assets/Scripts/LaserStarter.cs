using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStarter : MonoBehaviour
{
    [Header("LazermotherPrefabをここに入れる")]
    [SerializeField] private GameObject laserMother;
    private Vector2 direction_First = new Vector2(1, 1); //最初の方向
    const float rotate_Speed = 0.02f;  //回転速度
    private GameObject newLaser;
    private Laser laser;

    float axisX_Lazer = 0.0f;

    public void Start()
    {
        axisX_Lazer = 0.0f;
    }

    public void Update()
    {
        if (newLaser == null)
        {
            return;
        }

        axisX_Lazer = Input.GetAxisRaw("Horizontal"); //左右入力を感知
        //newLaser.transform.position = this.gameObject.transform.position;
        Debug.Log(this.gameObject.transform.position);
        laser.move(this.gameObject.transform.position, Quaternion.Euler(0f, 0f, axisX_Lazer*rotate_Speed) * laser.getDirection());

    }

    public void Click()
    {
        newLaser = Instantiate(laserMother, this.gameObject.transform.position, Quaternion.identity);//lazerMotherを生成
        laser = newLaser.GetComponent<Laser>();
        laser.creat(newLaser.transform.position, direction_First, 0); //ここで最初の０反射目レーザーを生成
    }

    public void Stop()
    {
        Destroy(newLaser);
    }

}


