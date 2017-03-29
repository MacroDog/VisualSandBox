using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TrafficLight : MonoBehaviour
{
    public int RedTime { get; set; }
    public int GreenTime { get; set ; }
    public int YellowTime { get; set; }
    public bool isRunning = false;
    public TrafficLightState lightState { get; set; }
    [SerializeField]
    private Light[] green;
    [SerializeField]
    private Light[] red;
    [SerializeField]
    private Light[] yellow;
    [SerializeField]
    private Text timeshower;//时间展示板
    [SerializeField]
    private GameObject trafficbox;//用于右侧阻挡车辆
    [SerializeField]
    private GameObject trafficbox1;//用于阻挡前方车辆
    private float timer = 0;
    public enum TrafficLightState
    {
        Green,
        Yellow,
        Red,
        other

    }


   // private TrafficLightState nextState;
    // Use this for initialization
    void Start()
    {
        VSSandGameManage._VSandGameManager.StatusUpdates += StartRunning;
        ChangeLightState(TrafficLightState.Yellow);
        YellowTime = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        switch (lightState)
        {
            case TrafficLightState.Green:
                timer-=Time.deltaTime;
                timeshower.text = timer.ToString("0");
                if (timer <= 0)
                {
                    ChangeLightState(TrafficLightState.Yellow);
                    //nextState = TrafficLightState.Red;
                   
                }
                break;
            case TrafficLightState.Yellow:
                timer -= Time.deltaTime;
                timeshower.text = timer.ToString("0");
                if (timer <= 0)
                {
                    ChangeLightState(TrafficLightState.Red);
                    
                }
                break;
            case TrafficLightState.Red:
                timer -= Time.deltaTime;
                timeshower.text = timer.ToString("0");
                if (timer <= 0)
                {
                    ChangeLightState(TrafficLightState.Green);
                    

                }
                break;
            case TrafficLightState.other:
                break;
            default:
                break;
        }

    }
    

    public void ChangeRedLightTiem(int a)
    {
        if (a <= 120 && a >= 1)
        {
            RedTime = a;
        }
    }
    public void ChangeGreenLightTime(int a)
    {
        if (a <= 120 && a >= 1)
        {
            GreenTime = a;
        }
    }
    private void ChangeLightState(TrafficLightState a)
    {
        if (lightState != a)
        {
            lightState = a;
        }
        Debug.Log(GreenTime+" "+ RedTime+" "+ YellowTime);
        switch (lightState)
        {
            case TrafficLightState.Green:
                timer = GreenTime;
                foreach (var item in green)
                {
                    item.GetComponent<Light>().enabled = true;
                }
                foreach (var item in yellow)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                foreach (var item in red)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                lightState = TrafficLightState.Green;
                trafficbox.SetActive(true);
                trafficbox1.SetActive(false);
                break;
            case TrafficLightState.Yellow:
                timer = YellowTime;
                
                foreach (var item in green)
                {
                    item.GetComponent<Light>().enabled = false ;
                }
                foreach (var item in yellow)
                {
                    item.GetComponent<Light>().enabled = true;
                }
                foreach (var item in red)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                lightState = TrafficLightState.Yellow;
                trafficbox.SetActive(false);
                trafficbox1.SetActive(true);
                break;
            case TrafficLightState.Red:
                timer = RedTime;
               
                foreach (var item in green)
                {
                    item.GetComponent<Light>().enabled = false ;
                }
                foreach (var item in yellow)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                foreach (var item in red)
                {
                    item.GetComponent<Light>().enabled = true;
                }
                lightState = TrafficLightState.Red;
                //trafficbox.SetActive(false);
                //trafficbox1.SetActive(true);
                break;
            case TrafficLightState.other:
                
                foreach (var item in green)
                {
                    item.GetComponent<Light>().enabled = false ;
                }
                foreach (var item in yellow)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                foreach (var item in red)
                {
                    item.GetComponent<Light>().enabled = false;
                }
                lightState = TrafficLightState.other;
                break;
           
        }
    }
    public void StartRunning()
    {
        ChangeRedLightTiem(ExchangeData._ExChangeData.sandboxRedLightTime);
        ChangeGreenLightTime(ExchangeData._ExChangeData.sandboxGreenLightTime);
        ChangeLightState(TrafficLightState.Red);
    }
}
