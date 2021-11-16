using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject root;
    [SerializeField]
    private GameObject endEffector;
    [SerializeField]
    private GameObject target;
    void Start()
    {
        root = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var endCmpt = endEffector.GetComponent<Joint>();
        endCmpt.IK_solver(target,endEffector);
    }
}
