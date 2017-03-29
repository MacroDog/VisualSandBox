using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DataUIManage : MonoBehaviour {
    private static DataUIManage _dataUIManage;
    public static DataUIManage _DataUIManage
    {
        get
        {
            if (_dataUIManage==null)
            {
                _dataUIManage = GameObject.FindObjectOfType<DataUIManage>();
                if (_dataUIManage==null )
                {
                    GameObject temp = new GameObject();
                    temp.name = "DataUIManage";
                    temp.AddComponent<DataUIManage>();
                    _dataUIManage = temp.GetComponent<DataUIManage>();
                }
            }
            return _dataUIManage;
        }
    }
    [SerializeField]
    private Text trafficsStream;
    [SerializeField]
    private Text sandboxGlobalTime;
    [SerializeField]
    private Text sandboxRedLightTime;
    [SerializeField]
    private Text sandboxGreenLightTime;
    [SerializeField]
    private Text sandboxParkId;
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Text exceptiontext;
    //[SerializeField]
    // private UserMenu usermenu;


    void Start()
    {
        InvokeRepeating("UpdataData", 0.5f,0.5f);
    }
    public void UpdataData()
    {
        trafficsStream.text = ExchangeData._ExChangeData.trafficsStream.ToString();
        if (ExchangeData._ExChangeData.sandboxGlobalTime==1)
        {
            sandboxGlobalTime.text = "开启";
        }
        else
        {
            sandboxGlobalTime.text = "关闭";
        }
        
        sandboxRedLightTime.text = ExchangeData._ExChangeData.sandboxRedLightTime.ToString();
        sandboxGreenLightTime.text = ExchangeData._ExChangeData.sandboxGreenLightTime.ToString();
        sandboxParkId.text = ExchangeData._ExChangeData.sandboxParkId.ToString();
        UpdataExceptInfomation();


    }
    private void UpdataExceptInfomation()
    {
        exceptiontext.text = " ";
        //if (ExchangeData._ExChangeData.ExceptionInformation.Count == 0)
        //{
        //    StartButton.gameObject.SetActive(true);
        //}
        //else
        //{
            foreach (string item in ExchangeData._ExChangeData.ExceptionInformation)
            {
                exceptiontext.text += item + '\n';
            }

           // StartButton.gameObject.SetActive(false);
        //}
    }
    public void Resume()
    {
        
    }
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.sceneCount);
    }
    public void StartRun()
    {
        this.gameObject.SetActive(false);
        //usermenu.gameObject.SetActive(true);
        //usermenu.ChangeCanUse();
    }
    public void ReStartServer()
    {
        NetManager._NetManager.RestartServer();
        Debug.Log("ReStart");
        //StartButton.gameObject.SetActive(false);
    }
}
