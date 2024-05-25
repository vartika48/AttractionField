using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject followObject;
    [SerializeField] bool isBG;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate() 
    {
        if(!isBG)
        transform.position = followObject.transform.position + new Vector3 (0, 0, -100);
        else
        transform.position = followObject.transform.position + new Vector3 (0, 0, -10);
    }
}
