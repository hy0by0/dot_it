using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class PlayerController_2 : MonoBehaviour
{
    public EdgeLine edgeLine; //����Ȃ������B�����Ɠ�����Ȃ�
    public float speed = 8.0f; //���[�U�[�������Ă���Ԃ̈ړ����x
    public float speed_quick = 9.0f; //���[�U�[�������Ȃ��Ƃ��̈ړ����x
    public float jumpForce = 10.0f; //����Ȃ�����

    public float score = 0.0f; //score�l���p�ϐ�
    float axisX = 0.0f;
    float axisY = 0.0f;
    public string direction = "right"; //�ꉞ�����擾���邽�߂̕ϐ�
    public bool jumpFlag = false;
    float Scale_X; //�����ύX�p�̑傫���w�����̕ϐ�

    public GameObject lazerObj; //���[�U�[�I�u�W�F�N�g������
    Line lazer; //�����̃N���X�ϐ��ύX���Ă���
    Rigidbody2D rbody;

    public static string gameState = "playing";

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        lazer = lazerObj.GetComponent<Line>();
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

        axisX = Input.GetAxisRaw("Horizontal"); //���E���͂����m
        axisY = Input.GetAxisRaw("Vertical"); //�㉺���͂����m

        if (!lazer.backFlag && !lazer.moving)
        {
            if(lazer.getCount != 0)
            {
                score += lazer.tongueScore * (1.0f + 0.5f*(lazer.getCount - 1) ); //score���Z
                lazer.tongueScore = 0.0f;
                lazer.getCount = 0;
            }
        }



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


        if (edgeLine.tongueFlag || edgeLine.backFlag)
        {
            rbody.velocity = new Vector2(axisX * speed, axisY * speed); //�ړ��̔��f
        }
        else
        {
            rbody.velocity = new Vector2(axisX * speed_quick, axisY * speed_quick); //�ړ��̔��f
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

        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Lazer")
        {
            Debug.Log("<color=red>�ɂ��I</color>");
            Miss();
        }

        if (collider.gameObject.tag == "Item")
        {
            Debug.Log("���ڂނ���ނ���");
            Item ItemScript = collider.gameObject.GetComponent<Item>();
            score += ItemScript.scoreValue;
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


    void GameStop()
    {
        rbody.velocity = new Vector2(0, 0);
    }
}
