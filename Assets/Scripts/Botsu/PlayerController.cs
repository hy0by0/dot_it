using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class PlayerController : MonoBehaviour
{
    public EdgeLine edgeLine;
    public float speed = 15.0f; //�ړ����x
    public float jumpForce = 10.0f; //����Ȃ�����
    public int satietyLevel = 100; //�����x
    public int foodCalorie = 10; //�H�ו��P������̖����x�񕜗�
    public float score = 0.0f; //score�l���p�ϐ�
    float axisX = 0.0f;
    float axisY = 0.0f;
    public string direction = "right"; //�ꉞ�����擾���邽�߂̕ϐ�
    public bool jumpFlag = false;
    float Scale_X; //�����ύX�p�̑傫���w�����̕ϐ�

    float countTime = 0.0f; //�����x���鎞�Ԍv���ϐ�
    public float intervalTime = 0.5f;//�����x���鎞�ԒP��(�������قǒZ���Ԋu��)
    public Transform satietyObj;
    public GameObject tongueObj; //��I�u�W�F�N�g������
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

        axisX = Input.GetAxisRaw("Horizontal"); //���E���͂����m
        axisY = Input.GetAxisRaw("Vertical"); //�㉺���͂����m

        if (!line.backFlag && !line.moving)
        {
            if(line.getCount != 0)
            {
                score += line.tongueScore * (1.0f + 0.5f*(line.getCount - 1) ); //score���Z
                satietyLevel += line.getCount * foodCalorie; //�����x��
                line.tongueScore = 0.0f;
                line.getCount = 0;
            }
        }

        satietyObj.localScale = new Vector3(1, (float)satietyLevel / 100, 1);


        //�����ύX
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

        //�����x�O���A�オ�L�тĂ��Ȃ��Ƃ�
        if (satietyLevel <= 0 && !line.backFlag && !line.moving && line.getCount == 0)
        {
            Debug.Log("�Q�[���I�[�o�[");
            gameState = "gameover";
            rbody.velocity = Vector3.zero;
            GameStop();
        }
        else if (satietyLevel > 0)
        {
            countTime += Time.deltaTime;
            if(countTime >= intervalTime)
            {
                satietyLevel -= 1; //�����x�����炷
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
            rbody.velocity = new Vector2(axisX * speed, axisY * speed); //�ړ��̔��f
        }
        

        //if (nowAnime != oldAnime) // �A�j���[�V�����̕ύX�𔽉f(�~�X�ƃN���A�ȊO)
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
            Debug.Log("<color=red>�S�[������</color>");
            Goal();
        }

        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("<color=red>�G�ɂ�������</color>");
            Miss();
        }

        if (collider.gameObject.tag == "Item")
        {
            Debug.Log("���ڂނ���ނ���");
            Item ItemScript = collider.gameObject.GetComponent<Item>();
            score += ItemScript.scoreValue;
            satietyLevel += foodCalorie; //�����x��
            Destroy(collider.gameObject);
        }
    }


    public void Goal()
    {
        //Debug.Log("<color=red>�܂��ŃS�[������</color>");
        gameState = "clearStage";
        GameStop();
    }


    public void Miss()
    {
        //Debug.Log("<color=red>�����Ă��������I</color>");
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
