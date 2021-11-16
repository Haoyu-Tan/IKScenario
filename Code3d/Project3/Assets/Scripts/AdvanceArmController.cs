using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceArmController: MonoBehaviour
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

        var rootCmpt = root.GetComponent<Joint_Advance>();
        rootCmpt.setLen();

        rootCmpt.fk();
    }

    // Update is called once per frame
    void Update()
    {
        var endCmpt = endEffector.GetComponent<Joint_Advance>();
        endCmpt.IK_solver(target, endEffector);
    }
}
