using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EdgeLine : MonoBehaviour
{
    public bool tongueFlag = false; //�オ�L�тĂ������ǂ����̃t���O
    public bool backFlag = false; //�オ�k��ł������ǂ����̃t���O
    public GameObject StartPos; //�L�т͂��߂̋N�_�̈ʒu
    public float speed = 4.0f; //�オ�L�тĂ����X�s�[�h�i�g��Ȃ������H�j
    public float backSpeed = 20.0f; //�オ�߂�X�s�[�h�i�g��Ȃ������H�j
    public float arriveThreshold = 0.1f; // �덷�z���p�B���̈ʒu�Ƃ̋��������̒l�ȉ��ɂȂ�΁A�߂������Ƃɂ���B
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
        //�L�[���͂ɂ���Ԃ̔��f
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

        if (tongueFlag) //�オ�L�тĂ����Ƃ�
        {
            Vector2 vec = transform.up * speed;
            rbody.velocity = vec; //�ړ��̔��f
        }
        else if(backFlag) //�オ�k��ł����Ƃ�
        {
            //rbody.velocity = Vector2.zero;  // Rigidbody�̈ړ����~�߂�iMoveTowards��Transform����j
            //transform.position = Vector2.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), 20 * Time.deltaTime);

            //�P�O�̒��_or�N�_�֖߂��Ă����悤�ɂ���i���̂܂܍ēx�L�т邱�Ƃ��ł���悤�Ɂj
            rbody.velocity = -transform.up * backSpeed;
            //Vector2 toStart = ((Vector2)StartPos.transform.position - rbody.position).normalized; //�߂�����x�N�g�����v�Z
            //rbody.velocity = toStart * backSpeed;//���̈ʒu�֖߂点��

            //���̈ʒu�Ƃ̋��������āA�߂�؂������ǂ���������B
            //float distance = Vector2.Distance(rbody.position, StartPos.transform.position);
            //if (distance < arriveThreshold)
            //{
                // �߂芮��
                //Debug.Log("�A���");
            //    backFlag = false;
            //    transform.position = StartPos.transform.position;
            //    rbody.velocity = Vector2.zero;
            //}
            
        }
        else //�オ�N�_�֏k�ݐ؂��Ă���Ƃ��A�L�΂��Ă��Ȃ��Ƃ�
        {
            this.transform.position = StartPos.transform.position;
            rbody.velocity = Vector2.zero;
            // �}�E�X�̕����֌�����ǐ�������
            var pos = Camera.main.WorldToScreenPoint(this.transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
        }

        
    }
}
