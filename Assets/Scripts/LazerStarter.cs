using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerStarter : MonoBehaviour
{
    [SerializeField] private GameObject lazerMother;
    private Vector2 INITIAL_DIRECTION = new Vector2(0.3f, 1); //ç≈èâÇÃï˚å¸
    const float ROTATE_SPEED = 0.02f;  //âÒì]ë¨ìx
    private GameObject newLazer;
    private Line lazer;

    public void Start()
    {
        newLazer = Instantiate(lazerMother, this.gameObject.transform.position, Quaternion.identity);
        lazer = newLazer.GetComponent<Lazer>();
        lazer.creat(newLazer.transform.position, INITIAL_DIRECTION, 0);
    }

    public void Update()
    {
        // ç∂âÒì]
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, ROTATE_SPEED) * lazer.getDirection());
        }
        // âEâÒì]
        if (Input.GetKey(KeyCode.RightArrow))
        {
            lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, -ROTATE_SPEED) * lazer.getDirection());
        }
    }
}

