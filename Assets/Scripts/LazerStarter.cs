using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerStarter : MonoBehaviour
{
    [Header("LazermotherPrefab‚ğ‚±‚±‚É“ü‚ê‚é")]
    [SerializeField] private GameObject lazerMother;
    private Vector2 direction_First = new Vector2(1, 1); //Å‰‚Ì•ûŒü
    const float rotate_Speed = 0.02f;  //‰ñ“]‘¬“x
    private GameObject newLazer;
    private Lazer lazer;

    public void Start()
    {
        newLazer = Instantiate(lazerMother, this.gameObject.transform.position, Quaternion.identity);//lazerMother‚ğ¶¬
        lazer = newLazer.GetComponent<Lazer>();
        lazer.creat(newLazer.transform.position, direction_First, 0); //‚±‚±‚ÅÅ‰‚Ì‚O”½Ë–ÚƒŒ[ƒU[‚ğ¶¬
    }

    public void Update()
    {
        // ¶‰ñ“]
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, rotate_Speed) * lazer.getDirection());
        }
        // ‰E‰ñ“]
        if (Input.GetKey(KeyCode.RightArrow))
        {
            lazer.move(lazer.getOrigin(), Quaternion.Euler(0f, 0f, -rotate_Speed) * lazer.getDirection());
        }
    }

}


