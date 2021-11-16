using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;
    /**
    [SerializeField]
    private bool isParent;
    */
    [SerializeField]
    private GameObject child;
    /**
    [SerializeField]
    private GameObject endEffector;
    */

    private float rotSpeed = 10.0f;
    [SerializeField]
    private Vector3 minAngle;
    [SerializeField]
    private Vector3 maxAngle;
    [SerializeField]
    private Vector3 angSpeed;

    private Vector3 currentLocal;

    public bool isLimited;
    // Start is called before the first frame update
    void Start()
    {
        currentLocal = Vector3.zero;
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


            //my quaternion calculator v1
            Quaternion qRotate = myQuatCalculator(startToEndEffector, startToTarget);


            //this.transform.rotation = qRotate * this.transform.rotation;

            Quaternion qRotate2 = myQuatCalculatorV2(startToEndEffector, startToTarget);

            //always return the smaller one
            Quaternion rotateInQ = Quaternion.FromToRotation(startToEndEffector, startToTarget);

            /**
            Debug.Log("my quaternion v1: " + qRotate.x + " " + qRotate.y + " " + qRotate.z + " " + qRotate.w + "\n"
                + "my quaternion v2: " + qRotate2.x + " " + qRotate2.y + " " + qRotate2.z + " " + qRotate2.w + "\n"
                + " Unity's quaternion: " + rotateInQ.x + " " + rotateInQ.y + " " +  rotateInQ.z + " " +  rotateInQ.w);
            */
            //add joint limits
            if (isLimited)
            {


                Quaternion limitQRot = this.addJointLimits(rotateInQ);

                //Vector3 diff = startToTarget - startToEndEffector;
                //Quaternion limitQRot = addJointLimits2(diff);


                if (this.parent)
                {
                    limitQRot = this.parent.transform.rotation * limitQRot;
                }

                this.transform.rotation = limitQRot;

            }

            
            //lerp
            else
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qRotate*this.transform.rotation, Time.deltaTime * rotSpeed);
            }
            

            
              
           
            



        }

        //if not the root, perform ik
        if (this.parent) {
            var parCmpt = this.parent.GetComponent<Joint>();
            parCmpt.IK_solver(target, endEffector);
        }
    }

   Quaternion addJointLimits2(Vector3 deltaEuler)
    {
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

        //get current local rotation
        //Vector3 currLocalRotation = this.transform.localRotation.eulerAngles;

        //if (currLocalRotation.x > 180f) currLocalRotation.x -= 360f;
        //if (currLocalRotation.y > 180f) currLocalRotation.y -= 360f;
        //if (currLocalRotation.z > 180f) currLocalRotation.z -= 360f;
        Vector3 currLocalRotation = currentLocal;

        //add change to it
        currLocalRotation += deltaEuler;

        //clamp between minAngle and maxAngle we allow to rotate
        currLocalRotation.x = Mathf.Clamp(currLocalRotation.x, minAngle.x, maxAngle.x);
        currLocalRotation.y = Mathf.Clamp(currLocalRotation.y, minAngle.y, maxAngle.y);
        currLocalRotation.z = Mathf.Clamp(currLocalRotation.z, minAngle.z, maxAngle.z);
        currentLocal = currLocalRotation;
        /**
        if (this.gameObject.name == "Joint1")
        {
            Debug.Log(this.gameObject.name + " local: " + currLocalRotation.x + " " + currLocalRotation.y + " " + currLocalRotation.z);
        }
        */
        return Quaternion.Euler(currLocalRotation.x, currLocalRotation.y, currLocalRotation.z);
    }

    Quaternion addJointLimits(Quaternion origQuaternion) {
        //convert to euler
        Vector3 deltaEuler = origQuaternion.eulerAngles;

       

        //if (deltaEuler.x <= Mathf.Epsilon) deltaEuler.x = 0f;
        //f (deltaEuler.y <= Mathf.Epsilon) deltaEuler.y = 0f;
        //if (deltaEuler.z <= Mathf.Epsilon) deltaEuler.z = 0f;

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

        //get current local rotation
        //Vector3 currLocalRotation = this.transform.localRotation.eulerAngles;

        //if (currLocalRotation.x > 180f) currLocalRotation.x -= 360f;
        //if (currLocalRotation.y > 180f) currLocalRotation.y -= 360f;
        //if (currLocalRotation.z > 180f) currLocalRotation.z -= 360f;
        Vector3 currLocalRotation = currentLocal;
        
        //add change to it
        currLocalRotation += deltaEuler;

        //clamp between minAngle and maxAngle we allow to rotate
        currLocalRotation.x = Mathf.Clamp(currLocalRotation.x, minAngle.x, maxAngle.x);
        currLocalRotation.y = Mathf.Clamp(currLocalRotation.y, minAngle.y, maxAngle.y);
        currLocalRotation.z = Mathf.Clamp(currLocalRotation.z, minAngle.z, maxAngle.z);
        currentLocal = currLocalRotation;
        /**
        if (this.gameObject.name == "Joint1")
        {
            Debug.Log(this.gameObject.name + " local: " + currLocalRotation.x + " " + currLocalRotation.y + " " + currLocalRotation.z);
        }
        */
        return Quaternion.Euler(currLocalRotation.x,currLocalRotation.y,currLocalRotation.z);

    }

    Quaternion myQuatCalculator(Vector3 startToEndEffector, Vector3 startToTarget) {
        Vector3 rot_axis = Vector3.Cross(startToEndEffector, startToTarget);

        float dotVec = Vector3.Dot(startToEndEffector, startToTarget);
        float w = (float)Mathf.Sqrt(Mathf.Pow(startToTarget.magnitude, 2) * Mathf.Pow(startToEndEffector.magnitude, 2));

        w += dotVec;

        Vector4 quatVec = new Vector4(rot_axis.x, rot_axis.y, rot_axis.z, w);
        quatVec.Normalize();

        /**
        Debug.Log("quaternion normalize: " + quatVec.x + ", " + quatVec.y + ", " + quatVec.z + ", " + 
            quatVec.w);
        */

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
