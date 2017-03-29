using UnityEngine;
using System.Collections;

public class PlayerCarControl : MonoBehaviour {
    [SerializeField]
    private CarPath myWay;
    [SerializeField]
    private Transform[] parkpoint;
    private float speed = 15;
    public carState _carSate { get; private set; }
    private int pointId=0;
    private Hashtable ht;
    private float offset=0.05f;
    private string thisItweenName;
    [SerializeField]
    private Transform[] WayPoints;
    [SerializeField]
    private GameObject[] carLamp;  
    private Transform ShootPoint;
    //public iTween MyIwteen;
    // Use this for initialization
    void Awake()
    {
        VSSandGameManage._VSandGameManager.StatusUpdates += StartRun;
    }
	void Start () {
        ShootPoint = this.transform.FindChild("shootpoint");
        //_carSate = carState.Stop;
        //ht.Add("movetopath", true);
        //ht.Add("orienttopath", true);
        //ht.Add("speed", speed);
        //ht.Add("easetype", iTween.EaseType.linear);
        //StartRun();
       // Invoke("StartRun", 2);


    }
	
	// Update is called once per frame
	void Update()
    {
       // Debug.Log(pointId);
       // Debug.Log(_carSate);
        if (_carSate==carState.Run)
        {
            
            float distence = Vector3.Distance(this.transform.position, WayPoints[pointId].position);
            //Debug.Log(distence);
            //Debug.Log(_carSate);
            if (distence <= offset)
            {
                
                nextPoint();
                if (pointId == 8)
                {
                    speed = 5;
                }

            }
        }
        RaycastHit hits;
        //Debug.DrawRay(ShootPoint.position, transform.forward*10);
        if (Physics.Raycast(ShootPoint.position, this.transform.forward, out hits, 3))
        {
            if (_carSate == carState.Run)
            {
                ChangeCarState(carState.Standby);
            }
            if (hits.collider.GetComponent<QiLouJia>())
            {
                if (hits.collider.GetComponent<QiLouJia>().MyState == QiLouJia.UndercartState.Stop)
                {
                    hits.collider.GetComponent<QiLouJia>().RiseMyState();
                    //speed = 8;
                }

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

    /// <summary>
    /// 改变移动速度
    /// </summary>
    /// <param name="Speed"></param>
    private void ChageCarSpeed(float Speed)
    {
        if (Speed>0&&Speed<=20)
        {
            speed = Speed;
        }
    }
    //更改状态
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
                    iTween.Resume(this.gameObject);
                    
                    //Debug.Log(this.GetComponent<iTween>()._name);
                    break;
                default:
                    break;
            }
        }
    }

    //停止
    public  void Stop()
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
        if (ww<= WayPoints.Length-1)
        {
            
            pointId++;
            iTween.MoveTo(this.gameObject, iTween.Hash("position", WayPoints[pointId].position,
                                            "movetopath", true,
                                            "orienttopath", true,
                                            "speed", speed,
                                            "easetype", iTween.EaseType.linear,
                                            "looktarget", WayPoints[pointId]));
        } 
    }
    public void StartRun()
    {
        Debug.Log(ExchangeData._ExChangeData.sandboxParkId);
        myWay.AddWayPoint(parkpoint[ExchangeData._ExChangeData.sandboxParkId]);
        myWay.AddWayPoint(parkManager._ParkManage.GetParkPointTransform(ExchangeData._ExChangeData.sandboxParkId));
        WayPoints = myWay.wayPointToArray();
       // Debug.Log(ExchangeData._ExChangeData.sandboxParkId - 1);
       
       
        iTween.MoveTo(this.gameObject, iTween.Hash("position", WayPoints[pointId].position,
                                         "movetopath", true,
                                         "orienttopath", true,
                                         "speed", speed,
                                         "easetype", iTween.EaseType.linear,
                                         "looktarget", WayPoints[pointId]));
        thisItweenName = GetComponent<iTween>().name;
        ChangeCarState(carState.Run);
        Debug.Log(WayPoints.Length - 1);
        if (ExchangeData._ExChangeData.sandboxGreenLightTime>22&& ExchangeData._ExChangeData.sandboxGreenLightTime<6)
        {
            for (int i = 0; i < carLamp.Length-1; i++)
            {
                carLamp[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < carLamp.Length - 1; i++)
            {
                carLamp[i].SetActive(false);
            }
        }
        

    }

}
