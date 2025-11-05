using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerStarter : MonoBehaviour
{
    [SerializeField] private GameObject lazerMother;
    private Vector2 direction_First = new Vector2(1, 1); //Å‰‚Ì•ûŒü

    public void Click()
    {
        GameObject newLazer = Instantiate(lazerMother, this.gameObject.transform.position, Quaternion.identity);
        newLazer.GetComponent<Lazer>().creat(newLazer.transform.position, direction_First, 0);
    }
}


