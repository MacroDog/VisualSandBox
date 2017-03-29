using UnityEngine;
using System.Collections;



public class trafficLightaBox1 : MonoBehaviour {
    public enum trafficlightBoxStata
    {
        stop,
        run
    }
    private static TrafficLight tl;
	// Use this for initialization
	void Start () {
        tl = this.transform.parent.GetComponent<TrafficLight>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision col)
    {

        if (col.collider.tag =="Car")
        {

        }
       
    }
}
