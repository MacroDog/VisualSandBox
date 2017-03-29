using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;
using System.IO;

//沙盘可变数据
public class ExchangeData : MonoBehaviour
{

    private static ExchangeData _exchangeData;
    public static ExchangeData _ExChangeData
    {
        get
        {
            if (_exchangeData == null)
            {
                _exchangeData = GameObject.FindObjectOfType<ExchangeData>();
                if (_exchangeData == null)
                {
                    GameObject ss = new GameObject();
                    ss.name = "ExchangeData";
                    ss.AddComponent<ExchangeData>();
                    _exchangeData = ss.GetComponent<ExchangeData>();
                }
            }
            return _exchangeData;
        }
    }
    public int trafficsStream { get; set; }//车流量 单位  辆/分钟
    public int sandboxGlobalTime { get; set; }//沙盘时间
    public int sandboxRedLightTime { get; set; }
    public int sandboxGreenLightTime { get; set; }
    public int sandboxParkId { get; set; }
    public int[] FreeParking { get; set; }
    public List<string> ExceptionInformation { get; set; }//异常通知
    //[SerializeField]
    //private int nullCarPackNumber;
    [SerializeField]
    private List<int> Carport;
    void Awake()
    {
        trafficsStream = Random.Range(5, 15);
        FreeParking = RamdonCarport();
        ExceptionInformation = new List<string>();
        sandboxRedLightTime = 10;
        sandboxGreenLightTime = 10;
        //sandboxParkId = FreeParking[Random.Range(0, FreeParking.Length - 1)];
        //Debug.Log(sandboxParkId);


    }
    void Start()
    {
        LoadDataToXMLFile();
        InvokeRepeating("RamdonChangeTrafficAndSend", 0, 0.5f);

    }
    //改变沙盘时间

    public void SetSandboxGlobalTime(int i)
    {
        if (i == 1 || i == 0)
        {
            sandboxGlobalTime = i;
        }
        else
        {
            ExceptionInformation.Add("非法输入灯光控制 ");
        }
    }
    public void SetSandboxRedLightTime(int time)
    {
        if (time>=0&&time <=120)
        {
            sandboxRedLightTime = time;
        }
        else
        {
            ExceptionInformation.Add("红灯时间设置有误");
        }
    }
    public void SetSandboxGreenLightTime(int time)
    {
        if (time >= 0 && time <= 120)
        {
            sandboxGreenLightTime = time;
        }
        else
        {
            ExceptionInformation.Add("绿灯时间设置有误");
        }
        
    }
    public void SetsandboxParkId(int id)
    {
        bool bie = false;
        for (int i = 0; i < FreeParking.Length; i++)
        {
            if (FreeParking[i]==id)
            {
                bie = true;
                sandboxParkId = id;
                break;
            }
        }
        if (bie==false)
        {
            ExceptionInformation.Add("停车位设置有误");
        }
       
         
    }
    //生成随机空闲停车位
    private int[] RamdonCarport()
    {
        int NullCarportCount = Random.Range(2, 9);
        int[] nullCarport=new int[NullCarportCount];
        for (int i = 0; i < NullCarportCount; i++)
        {
            int d = Random.Range(1, Carport .Count-1);
            nullCarport[i] = Carport[d];
            Carport.Remove(Carport[d]);
            
        }
        return nullCarport;
    }

    //空闲停车位
    private string intArrayToString(int[] array)
    {
        string text=null;
        for (int i = 0; i < array.Length-1; i++)
        {
            text += array[i].ToString() + " ";
        }
        return text;
    }

    //将数据装载入xml文件
    private void LoadDataToXMLFile()
    {
        //ExchangeData._ExChangeData.ExceptionInformation.Add("无文件输入");
        XmlDocument doc = new XmlDocument();
        XmlNode  node = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlNode root = doc.CreateElement("VisualSandboxServerData");
        doc.AppendChild(root);

        XmlNode carNumber = doc.CreateNode(XmlNodeType.Element, "SandboxTrafficFlow", "");
        carNumber.InnerText = trafficsStream.ToString();
        root.AppendChild(carNumber);
        XmlNode nullCarPort = doc.CreateNode(XmlNodeType.Element, "SandboxFreeParking", "");
        nullCarPort.InnerText= intArrayToString(FreeParking);
        root.AppendChild(nullCarPort);
        doc.Save(Application.dataPath + "/VisualSandboxServerData.xml");
        NetManager._NetManager.initializeMyServer();
    }
    public void AddExcrption(string str)
    {
        ExceptionInformation.Add(str);
    }
    public void ClearException()
    {
        ExceptionInformation.Clear();
    }
    private void RamdonChangeTrafficAndSend()
    {
        if (NetManager._NetManager.clientLinkState==NetManager.ClientLinkState.Connet)
        {
            trafficsStream = Random.Range(5, 15);
            byte[] temp;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    XmlDocument doc = new XmlDocument();
            //    XmlNode node = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //    XmlNode root = doc.CreateElement("VisualSandboxServerData");
            //    doc.AppendChild(root);

            //    XmlNode carNumber = doc.CreateNode(XmlNodeType.Element, "SandboxTrafficFlow", "");
            //    carNumber.InnerText = trafficsStream.ToString();
            //    doc.Save(ms);
            //    temp = ms.ToArray();
            //    Debug.Log(System.Text.Encoding.UTF8.GetString(temp));
            //    
            //    temp = ms.Read(senddata, 0, Convert.ToInt32(fs.Length))
            //}


             temp = System.Text.Encoding.Default.GetBytes(trafficsStream.ToString());
            NetManager._NetManager.SendMessageToClient(temp);

        }
        
    }














}
