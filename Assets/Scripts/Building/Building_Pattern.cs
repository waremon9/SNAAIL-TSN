using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Pattern : MonoBehaviour
{
    private int life = 5;
    public List<GameObject> gift = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            life -= 1;
            Destroy(collision.gameObject);
            Destroy(gift[Random.Range(0, gift.Count)]);
        }
    }
}
