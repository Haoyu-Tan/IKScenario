using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private bool mouseClick;
    public Vector3 screenSpace;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        mouseClick = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 0 left click, 1 right click, 2 middle click
        if (Input.GetMouseButtonDown(0)) {
            Ray cmRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (raySphereIntersect(cmRay))
            {
                //Debug.Log("hit!");
                mouseClick = true;
                screenSpace = Camera.main.WorldToScreenPoint(this.transform.position);
                offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            mouseClick = false;
        }

        if (mouseClick) {
            var currScrSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            var currPos = Camera.main.ScreenToWorldPoint(currScrSpace) + offset;

            this.transform.position = currPos;
        }
    }

    
    GameObject checkClickedObject(Vector3 mousePos) {
        GameObject target = null;

        Ray cmRay = Camera.main.ScreenPointToRay(mousePos);

        if (raySphereIntersect(cmRay)) {
            target = this.gameObject;
        }

        return target;
    }
    
    bool raySphereIntersect(Ray ray) {

        Vector3 diff = ray.origin - this.transform.position;
        var rend = this.GetComponent<Renderer>();
        float radius = rend.bounds.extents.magnitude;

        float a = ray.direction.sqrMagnitude;
        float b = 2 * Vector3.Dot(ray.direction, diff);
        float c = diff.sqrMagnitude - Mathf.Pow(radius, 2);

        float discr = b * b - 4 * a * c;

        if (discr < 0) return false;

        float t0 = (-b - Mathf.Sqrt(discr)) / (2 * a);

        if (t0 > 0) return true;

        return false;
    }
}
