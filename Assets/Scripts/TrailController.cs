using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) {
            trailRenderer.enabled = true;
            //trailRenderer.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
           // mousePosition.z = -1;
            trailRenderer.transform.position = mousePosition;
        }
       // else {
            //trailRenderer.Clear();
            //trailRenderer.enabled = false;
        //}
    }
   
}
