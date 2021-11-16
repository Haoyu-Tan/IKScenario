using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabrikArmController : MonoBehaviour
{
    private GameObject root;
    [SerializeField]
    private GameObject endEffector;
    [SerializeField]
    private GameObject target;

    private int numItera = 10;

    private float eps = 0.001f;

    private float backFor = 1f;

    private float totalLen;


    // Start is called before the first frame update
    void Start()
    {
        root = this.gameObject;
        totalLen = 0f;

        
        GameObject currJoint = this.root;
        while (currJoint) {
            var currCmpt = currJoint.GetComponent<Joint_Fabrik>();
            totalLen += currCmpt.setLen();
            
            currJoint = currCmpt.child;
            
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        IK_Solver();
    }

    void IK_Solver() {

        //if it is not reachable
        //Debug.Log("total length: " + totalLen + ", target dist: " + (target.transform.position - this.root.transform.position).magnitude);
        if ((target.transform.position - this.root.transform.position).magnitude >= Mathf.Abs(totalLen))
        {
            var dir = (target.transform.position - root.transform.position).normalized;

            GameObject prevJoint = this.root;
            GameObject currJoint = this.root.GetComponent<Joint_Fabrik>().child;
            while (currJoint)
            {
                var currCmpt = currJoint.GetComponent<Joint_Fabrik>();
                currJoint.transform.position = prevJoint.transform.position + dir * currCmpt.length;


                prevJoint = currJoint;
                currJoint = currCmpt.child;

            }
        }
        else {
            for (int i = 0; i < numItera; i++) {

                GameObject currJoint = endEffector;
                GameObject prevJoint = null;
                while (currJoint.GetComponent<Joint_Fabrik>().parent) {
                    var currCmpt = currJoint.GetComponent<Joint_Fabrik>();
                    
                    if (!currCmpt.child)
                    {
                        currJoint.transform.position = target.transform.position;
                        //Debug.Log("I'm touching target!!!");
                    }
                    else {
                        var prevCmpt = prevJoint.GetComponent<Joint_Fabrik>();
                        Vector3 my_dir = (currJoint.transform.position - prevJoint.transform.position).normalized;
                        currJoint.transform.position = prevJoint.transform.position + my_dir * prevCmpt.length;
                    }
                    prevJoint = currJoint;
                    
                    currJoint = currCmpt.parent;
                }

                
                currJoint = root.GetComponent<Joint_Fabrik>().child;
                prevJoint = root;
                while (currJoint) {
                    var currCmpt = currJoint.GetComponent<Joint_Fabrik>();
                    Vector3 my_dir = (currJoint.transform.position - prevJoint.transform.position).normalized;
                    currJoint.transform.position = prevJoint.transform.position + my_dir * currCmpt.length;

                    prevJoint = currJoint;
                    currJoint = currCmpt.child;
                    
                }
                

                if ((target.transform.position - endEffector.transform.position).magnitude < eps) {
                    break;
                }

            }
        }


    }

}
