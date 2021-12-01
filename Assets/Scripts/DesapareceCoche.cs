using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesapareceCoche : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("El " + gameObject.name + " colicionó con el gamobject " + collision.gameObject.name);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
