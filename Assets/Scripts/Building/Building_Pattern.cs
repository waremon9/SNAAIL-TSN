using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Pattern : MonoBehaviour
{
    private int life = 5;
    public List<GameObject> gift = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;

        foreach(Transform cadeaux in child.transform)
        {
            gift.Add(cadeaux.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            life -= 1;
            Debug.Log(life);
            Destroy(collision.gameObject);
            Destroy(gift[Random.Range(0, gift.Count)]);
        }
    }
}
