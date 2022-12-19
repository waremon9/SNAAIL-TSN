using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Pattern : MonoBehaviour
{
    private int life = 5;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(life);
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
        }
    }
}
