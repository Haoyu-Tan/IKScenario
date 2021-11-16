using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint_Advance: MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject child;

    [SerializeField]
    private Vector3 minAngle;
    [SerializeField]
    private Vector3 maxAngle;
    [SerializeField]
    private Vector3 angSpeed;

    [SerializeField]
    private Vector3 currentLocal;

    private float my_len;

    // Start is called before the first frame update
    void Start()
    {
        /**
        if (!this.parent) {
            this.child.GetComponent<Joint_Advance>().setLen();
        }

        //this.fk();
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IK_solver(GameObject target, GameObject endEffector){
        //end effector don't need this step
        if (this.child) {
            Vector3 startToTarget = target.transform.position - this.transform.position;
            Vector3 startToEndEffector = endEffector.transform.position - this.transform.position;

            startToTarget.Normalize();
            startToEndEffector.Normalize();

            //always return the smaller one
            Quaternion rotateInQ = Quaternion.FromToRotation(startToEndEffector, startToTarget);

           
            
            Quaternion limitQRot = this.addJointLimits(rotateInQ);

            //this.transform.localRotation = limitQRot;

            
            if (this.parent) {
                limitQRot = this.parent.transform.rotation * limitQRot;
            }

            this.transform.rotation = limitQRot;

            if (this.gameObject.name == "Joint0") {
                Debug.Log("rotation: " + currentLocal.x + " " + currentLocal.y
                    + " " + currentLocal.z);
            }

            this.child.GetComponent<Joint_Advance>().fk();
            /**
            //lerp
            else
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qRotate*this.transform.rotation, Time.deltaTime * rotSpeed);
            }
            */



        }

        //if not the root, perform ik
        if (this.parent) {
            var parCmpt = this.parent.GetComponent<Joint_Advance>();
            parCmpt.IK_solver(target, endEffector);
        }
    }

    public void setLen() {
        
        if (this.parent) {
            my_len = (this.transform.position - this.parent.transform.position).magnitude;
            //Debug.Log("I'm " + this.gameObject.name + ", set len, pos: " + this.transform.position);
        }
        if (this.child) {
            this.child.GetComponent<Joint_Advance>().setLen();            
        }
    }

    public void fk() {
        // set rotation

        if (this.parent) {
            //rotate parent's global rotation
            Vector3 my_dir = this.parent.transform.rotation * Vector3.back;
            my_dir.Normalize();

            //update position
            this.transform.position = this.parent.transform.position + my_dir * my_len;
            //Debug.Log("I'm " + this.gameObject.name + ", my position is " + this.transform.position);

            //update rotation
            this.transform.rotation = this.parent.transform.rotation * Quaternion.Euler(this.currentLocal);


        }
        else
        {
            this.transform.rotation = Quaternion.Euler(this.currentLocal);
        }

        if (this.child) {
            var childCmpt = this.child.GetComponent<Joint_Advance>();
            childCmpt.fk();
        }

    }

    Quaternion addJointLimits(Quaternion origQuaternion) {
        //convert to euler
        Vector3 deltaEuler = origQuaternion.eulerAngles;

        //euler Angle might return a value > 180, convert qToEuler to [-180, 180]
        if (deltaEuler.x > 180f) deltaEuler.x -= 360f;
        if (deltaEuler.y > 180f) deltaEuler.y -= 360f;
        if (deltaEuler.z > 180f) deltaEuler.z -= 360f;

        //clamp based on speed
        Vector3 maxRot = angSpeed * Time.deltaTime;
        deltaEuler.x = Mathf.Clamp(deltaEuler.x, (-1) * maxRot.x, maxRot.x);
        deltaEuler.y = Mathf.Clamp(deltaEuler.y, (-1) * maxRot.y, maxRot.y);
        deltaEuler.z = Mathf.Clamp(deltaEuler.z, (-1) * maxRot.z, maxRot.z);
        //Debug.Log("delta: " + deltaEuler.x + " " + deltaEuler.y + " " + deltaEuler.z);

        //Don't use this!!! two ways from Quaternion to Euler
        //get current local rotation
        //Vector3 currLocalRotation = this.transform.localRotation.eulerAngles;

        Vector3 currLocalRotation = currentLocal;
        
        //add change to it
        currLocalRotation += deltaEuler;

        //clamp between minAngle and maxAngle we allow to rotate
        currLocalRotation.x = Mathf.Clamp(currLocalRotation.x, minAngle.x, maxAngle.x);
        currLocalRotation.y = Mathf.Clamp(currLocalRotation.y, minAngle.y, maxAngle.y);
        currLocalRotation.z = Mathf.Clamp(currLocalRotation.z, minAngle.z, maxAngle.z);
        currentLocal = currLocalRotation;

        //Debug.Log(this.gameObject.name + " local: " + currLocalRotation.x + " " + currLocalRotation.y + " " + currLocalRotation.z);

        return Quaternion.Euler(currLocalRotation.x,currLocalRotation.y,currLocalRotation.z);

    }

    Quaternion myQuatCalculator(Vector3 startToEndEffector, Vector3 startToTarget) {
        Vector3 rot_axis = Vector3.Cross(startToEndEffector, startToTarget);

        float dotVec = Vector3.Dot(startToEndEffector, startToTarget);
        float w = (float)Mathf.Sqrt(Mathf.Pow(startToTarget.magnitude, 2) * Mathf.Pow(startToEndEffector.magnitude, 2));

        w += dotVec;

        Vector4 quatVec = new Vector4(rot_axis.x, rot_axis.y, rot_axis.z, w);
        quatVec.Normalize();

        Quaternion qRotate = new Quaternion(quatVec.x, quatVec.y, quatVec.z, quatVec.w);

        return qRotate;
    }

    //assume from and to are normalized
    Quaternion myQuatCalculatorV2(Vector3 from, Vector3 to) {
        Vector3 rotAxis = Vector3.Cross(from, to);
        rotAxis.Normalize();

        //calculate angle
        float mag = (float)Mathf.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
        float angle = (float)Mathf.Acos(Mathf.Clamp(Vector3.Dot(from, to), -1F, 1F));
        angle *= Mathf.Rad2Deg;

        float halfAngle = angle * Mathf.Deg2Rad * 0.5f;
        rotAxis *= Mathf.Sin(halfAngle);

        Quaternion q = new Quaternion(rotAxis.x, rotAxis.y, rotAxis.z, Mathf.Cos(halfAngle));
        return q;
    }


}
