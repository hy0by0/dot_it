using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using static TreeEditor.TreeGroup;

public class Line : MonoBehaviour
{
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    public Transform startPos;
    public GameObject endPosObj;
    public Transform endPos;
    EdgeLine edgeLine;

    public float tongueSpeed = 1.0f;
    public float maxMagnitude = 2.0f;
    public float canMagnitude;

    public float tongueScore = 0.0f;
    public int getCount = 0;

    //Vector3[] positions;
    private Vector3 currentDirection; //���݂̐L�тĂ���������ێ����邽�߂̃x�N�g���ϐ�
    private List<Vector3> linePoints = new List<Vector3>(); //���_���W�̊Ǘ����郊�X�g�B�n�_�E�I�_�E�܂�Ȃ������_���W�����X�g�ŊǗ�


    public bool moving = false; //�L�т邩�ǂ����̃t���O
    public bool backFlag = false; //�k�ނ��ǂ����̃t���O


    void Start()
    {
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
        endPos = endPosObj.GetComponent<Transform>();
        edgeLine = endPosObj.GetComponent<EdgeLine>();

        // ������
        linePoints.Add(startPos.position);
        linePoints.Add(endPos.position);

        currentDirection = (endPos.position - startPos.position).normalized;
        UpdateLine();
        canMagnitude = maxMagnitude;


        // �_�̐����w�肷��
        //lineRenderer.positionCount = positions.Length;

        // ���������ꏊ���w�肷��
        //lineRenderer.SetPositions(positions);

    }

    // Update is called once per frame
    void Update()
    {
        //�N���b�N���̃}�E�X�ւ̕����x�N�g���@currentVector���X�V�����āI�I�I�I
        if (Input.GetMouseButton(0))
        {
            moving = true;
            currentDirection = endPos.up;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            moving = false;
            backFlag = true;
        }

        if (moving) //�L�тĂ����Ƃ�
        {
            int layerMask = 1 << LayerMask.NameToLayer("Wall");
            RaycastHit2D hit = Physics2D.Raycast(endPos.position, currentDirection, 0.5f, layerMask);
            Debug.DrawRay(endPos.position, currentDirection * 0.5f, Color.green);
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);
                linePoints.Insert(linePoints.Count - 1, endPos.position);
                Vector2 normal = hit.normal;
                currentDirection = Vector2.Reflect(currentDirection, normal).normalized;
                endPos.up = currentDirection;
                //�����̍ő�l�̎c�蒷���������Ŕ��f
                if (linePoints.Count >= 3)
                {
                    canMagnitude -= (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                }

            }

            Vector3 newPos = currentDirection * tongueSpeed * Time.deltaTime;
            endPos.position += new Vector3(newPos.x,newPos.y, 0);

            //���������̏���
            Vector3 limitPos = endPos.position - linePoints[linePoints.Count - 2];
            if (limitPos.magnitude > canMagnitude)
            {
                limitPos *= canMagnitude / limitPos.magnitude;
                endPos.position = limitPos + linePoints[linePoints.Count - 2];
            }

            linePoints[linePoints.Count - 1] = endPos.position;
            UpdateLine();
        }
        else if (backFlag) //�k��ōs���Ƃ�
        {
            //�P�O�̒��_or�N�_�֖߂��Ă����悤�ɂ���i���̂܂܍ēx�L�т邱�Ƃ��ł���悤��

            float distance = Vector2.Distance(endPos.position, linePoints[linePoints.Count - 2]);
            if (distance < 0.5f)
            {
                endPos.position = linePoints[linePoints.Count - 2];              
                if (linePoints.Count > 2) //���˂��P�ł��c���Ă���Ƃ�
                {
                    endPos.up = (endPos.position - linePoints[linePoints.Count - 3]).normalized;
                    canMagnitude += (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
                else
                {
                    Debug.Log("�߂����I");
                    backFlag = false;
                    edgeLine.backFlag = false;
                }
            }
            else if(distance >= 20.0f)
            {
                Debug.Log("�ُ�l�܂ŗ���Ă��܂�");
                endPos.position = linePoints[linePoints.Count - 2];
                if (linePoints.Count > 2) //���˂��P�ł��c���Ă���Ƃ�
                {
                    endPos.up = (endPos.position - linePoints[linePoints.Count - 3]).normalized;
                    canMagnitude += (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
                else
                {
                    Debug.Log("�ً}�Ŗ߂�܂�");
                    backFlag = false;
                    edgeLine.backFlag = false;
                }
            }
        }
        else //�N�_�ɖ߂����Ƃ��A�L�΂��Ă��Ȃ��Ƃ�
        {
            //endPos.position = startPos.position;
            currentDirection = (endPos.position - startPos.position).normalized;
            canMagnitude = maxMagnitude;

            //�Ƃ΂��Ė߂����ꍇ
            if (linePoints.Count > 2)
            {
                Debug.Log("�Ƃ΂��Ė߂��Ă܂�");
                for (int i = linePoints.Count; i > 2; i--)
                {
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
            }
            
        }

        linePoints[0] = startPos.position;
        linePoints[linePoints.Count - 1] = endPos.position;
        UpdateLine();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            Debug.Log("�~�X");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Debug.Log("���܂�����");
            //float score += collision.gameObject.Item().score;//�A�C�e���̃X�R�A���󂯎��悤�ɂ���
            Item ItemScript = collision.gameObject.GetComponent<Item>();
            getCount += 1;
            tongueScore += ItemScript.scoreValue;
            Destroy(collision.gameObject);
        }
    }

    //�����ŁALinRenderer��edgeCollider�̕ω��𔽉f�����Ă���
    void UpdateLine()
    {
        //���̒��_�̐��𔽉f������
        lineRenderer.positionCount = linePoints.Count; 
        //ToArray���\�b�h�ŁA���X�g����z��֕ϊ���������ň����Ƃ��ė^���Ă���B������LineRender�̒��_�̍��W�𔽉f�����Ă���
        lineRenderer.SetPositions(linePoints.ToArray()); 

        // EdgeCollider �̂��߂� Vector2 �ɕϊ�
        List<Vector2> edgePoints = new List<Vector2>();
        foreach (var point in linePoints)
        {
            edgePoints.Add(new Vector2(point.x, point.y));
        }
        //������edgeCollider�̒��_�̍��W�𔽉f�����Ă���
        edgeCollider.SetPoints(edgePoints);

    }


}
