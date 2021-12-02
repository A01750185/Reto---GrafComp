using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesapareceCoche : MonoBehaviour
{
    private Vector3 posicion;
    // Start is called before the first frame update
    void Start()
    {
        posicion = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position == posicion){
            gameObject.transform.parent.gameObject.SetActive(true);
        }       
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Barrera")){
            gameObject.transform.parent.gameObject.SetActive(false); 
        }
    }
    
}
