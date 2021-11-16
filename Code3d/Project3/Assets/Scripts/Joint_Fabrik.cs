using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint_Fabrik : MonoBehaviour
{
   
    public GameObject parent;
    
    public GameObject child;

    public float length;

    private Transform armCube;

    // Start is called before the first frame update
    void Start()
    {
        /**
        length = 0f;
        if (this.parent) {
            this.length = (this.transform.position - this.parent.transform.position).magnitude;
        }
        */
        
        if (this.transform.childCount > 0)
            armCube = this.gameObject.transform.GetChild(0);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if (this.child)
            armCube.LookAt(child.transform);
        */
        if (this.child)
            this.transform.LookAt(this.child.transform);
    }

    public float setLen() {
        this.length = 0f;
        if (this.parent) {
            this.length = (this.transform.position - this.parent.transform.position).magnitude;
        }
        return this.length;
    }
}
