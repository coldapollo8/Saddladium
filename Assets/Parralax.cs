using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{


    private float startPosition;
    private GameObject cam;
    private float length;
    [SerializeField] private float parralaxFactor;
     // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        cam = GameObject.Find("CM vcam1");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {   
        float temp  = (cam.transform.position.x * (1 - parralaxFactor));
        float distance = (cam.transform.position.x * parralaxFactor);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if(temp > startPosition + length){
            startPosition += length;
        }
          if(temp < startPosition - length){
            startPosition -= length;
        }
    }
}
