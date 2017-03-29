using UnityEngine;
using System.Collections;

public enum carState
{
    Stop,
    Standby,
    Run,
    None

}

public class AICarControl : MonoBehaviour {

    [SerializeField]
    private CarPath myWay;
   
    private float speed = 15;
    public carState _carSate { get; private set; }
    private int pointId = 0;
   
    private float offset = 0.05f;
    private string thisItweenName;
    [SerializeField]
    private Transform[] WayPoints;
    //private Ray ClickRay;
    private Transform ShootPoint;
    void Awake()
    {
       
    }
	// Use this for initialization
	void Start () {
        this.ShootPoint = this.transform.FindChild("shootpoint");
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        if (_carSate == carState.Run)
        {

            float distence = Vector3.Distance(this.transform.position, WayPoints[pointId].position);
            //Debug.Log(distence);
            //Debug.Log(_carSate);
            if (distence <= offset)
            {

                nextPoint();
            }
           // Ray ClickRay = new Ray(ShootPoint.position, transform.forward);
           
            

        }
        RaycastHit hits;
        //Debug.DrawRay(ShootPoint.position, transform.forward*10);
        if (Physics.Raycast(ShootPoint.position, this.transform.forward , out hits, 3))
        {
            if (_carSate == carState.Run)
            {
                ChangeCarState(carState.Standby);
            }
        }
        else 
        {
            if (_carSate == carState.Standby)
            {
                ChangeCarState(carState.Run);
            }
        }

    }



    private void ChangeCarState(carState cs)
    {
        if (cs != _carSate)
        {
            switch (cs)
            {
                case carState.Stop:
                    _carSate = carState.Stop;
                    iTween.Stop(this.gameObject);

                    break;
                case carState.Standby:
                    iTween.Pause(this.gameObject);
                    _carSate = carState.Standby;
                    break;
                case carState.Run:
                    _carSate = carState.Run;
                    //iTween.Resume(thisItweenName);
                    iTween.MoveTo(this.gameObject, iTween.Hash("position", WayPoints[pointId].position,
                                        "movetopath", true,
                                        "orienttopath", true,
                                        "speed", speed,
                                        "easetype", iTween.EaseType.linear,
                                        "looktarget", WayPoints[pointId]));
                    //Debug.Log(this.GetComponent<iTween>()._name);
                    break;
                default:
                    break;
            }
        }
    }

    //停止
    public void Stop()
    {
        ChangeCarState(carState.Stop);


    }
    //暂停
    public void pause()
    {
        ChangeCarState(carState.Standby);
    }
    //运行
    public void Run()
    {
        ChangeCarState(carState.Run);

    }

    private void nextPoint()
    {
        //print(pointId);
        int ww = pointId + 1;
        if (ww <= WayPoints.Length - 1)
        {

            pointId++;
            iTween.MoveTo(this.gameObject, iTween.Hash("position", WayPoints[pointId].position,
                                            "movetopath", true,
                                            "orienttopath", true,
                                            "speed", speed,
                                            "easetype", iTween.EaseType.linear,
                                            "looktarget", WayPoints[pointId]));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void StartRun()
    {
       // Debug.Log(ExchangeData._ExChangeData.sandboxParkId);
        WayPoints = myWay.wayPointToArray();
        // Debug.Log(ExchangeData._ExChangeData.sandboxParkId - 1);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", WayPoints[pointId].position,
                                         "movetopath", true,
                                         "orienttopath", true,
                                         "speed", speed,
                                         "easetype", iTween.EaseType.linear,
                                         "looktarget", WayPoints[pointId]));

        thisItweenName = GetComponent<iTween>().id;
        ChangeCarState(carState.Run);
        //Debug.Log(WayPoints.Length - 1);
    }
    
    //设置移动路线
    public void SetMyWay(CarPath Path)
    {
        myWay = Path;
    }
}
