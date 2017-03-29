using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QiLouJia : MonoBehaviour {
    public  enum UndercartState
    {
        Rise,
        Down,
        Stop
    }
    public  UndercartState MyState { get; set; }
   
    [SerializeField]
    private GameObject Undercart;//起落架
    [SerializeField]
    private Text LED;
	// Use this for initialization
	void Start () {
        MyState = UndercartState.Stop;
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (MyState)
        {
            case UndercartState.Rise:
                if (Undercart.transform.rotation.x > -0.85)
                {
                    //Debug.Log(Undercart.transform.rotation.x);
                    Undercart.transform.Rotate(new Vector3(-5 * Time.deltaTime, 0, 0));
                }
                else
                {
                    this.gameObject.GetComponent<BoxCollider>().enabled=false;
                    MyState = UndercartState.Stop;
                   
                    
                }
                break;
            case UndercartState.Down:
                if (Undercart.transform.rotation.x < -0.71)
                {
                    Undercart.transform.Rotate(new Vector3(5 * Time.deltaTime, 0, 0));
                }
                else
                {
                    
                    MyState = UndercartState.Stop;
                }
                break;
            case UndercartState.Stop:
                break;
            default:
                break;
        }
    }
   
    public void RiseMyState()
    {
        MyState = UndercartState.Rise;
        LED.text = "停车位" + ExchangeData._ExChangeData.sandboxParkId.ToString();
    }
}
