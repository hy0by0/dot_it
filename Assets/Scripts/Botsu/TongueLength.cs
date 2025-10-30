using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueLength : MonoBehaviour
{
    GameObject playerObj;
    private float heightPos = 0.1f;
    bool tongueFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = transform.parent.gameObject;
        heightPos = 0.1f;
        tongueFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tongueFlag && transform.localScale.y <= 0.1f)
        {
            // ƒ}ƒEƒX‚Ì•ûŒü‚Ö’Ç‚³‚¹‚é
            var pos = Camera.main.WorldToScreenPoint(playerObj.transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            tongueFlag = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            tongueFlag = false;
        }

        if (tongueFlag)
        {
            if (transform.localScale.y <= 5.0f)
            {
                transform.localScale = new Vector3(1f, heightPos, 1f);
                heightPos += 0.05f;
            }
            
        }
        else
        {
            if (transform.localScale.y >= 0.1f)
            {
                transform.localScale = new Vector3(1f, heightPos, 1f);
                heightPos -= 0.5f;
            }
        }
    }
}
