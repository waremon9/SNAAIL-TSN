using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Pattern : MonoBehaviour
{
    private int life = 5;
    [SerializeField]
    private List<GameObject> gifts = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("test");
            life -= 1;
            Destroy(other.gameObject);

            Destroy(gifts[Random.Range(0, gifts.Count)]);
        }
    }

}
