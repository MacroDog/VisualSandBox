using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//netManager
public class NetManager : MonoBehaviour
{
    public enum ClientLinkState
    {
        DisConnet,
        Connet,
    }
    private static NetManager _netManager;
    public static  NetManager _NetManager
    {
        get
        {
            if (_netManager==null)
            {
                _netManager = GameObject.FindObjectOfType<NetManager>();
                if (_netManager==null)
                {
                    GameObject NM = new GameObject();
                    NM.name = "NetManager";
                    NM.AddComponent<NetManager>();
                    _netManager = NM.GetComponent<NetManager>();
                }
            }
            return _netManager;
        }
    }
    const int myProt = 7788;
    private IPAddress MyIp;
    
    private Thread getmessage;
    private Socket serverSocket;
    private XmlDocument MyData;
    private  byte[] buffer;
    private byte[] ReMessage=new byte[1024];
    private Thread listenClinkLink;
    private Thread AcceptClinkData;
    private Socket clientSocket;

    public ClientLinkState clientLinkState { get; set; }

   // private bool recevicemess=false;
    private bool needUpdateData=false;//用来标识是否有新的数据
    private static  bool canUpdateData = true;
    void Awake()
    {
        VSSandGameManage._VSandGameManager.StatusUpdates += ClsoeRecoverMessage;
        clientLinkState = ClientLinkState.DisConnet;
    }
    void Start()
    {
       // ReMessage=null;
    }
    void Update()
    {
        if (needUpdateData == true&&canUpdateData==true)
        {
            extractData(deserializationData(ReMessage));
            needUpdateData = false;
        }
      
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void loadMyXMLData()
    {
        //FileStream fs = File.OpenRead(Application.dataPath + "/VisualSandboxServerData.xml");//文件流读取发送的xml文件
        //byte[] senddata = new byte[1024];
        //fs.Read(senddata, 0, Convert.ToInt32(fs.Length));//存入所发送的byte数组中
        //buffer = senddata;
        //Debug.Log(System.Text.Encoding.UTF8.GetString(senddata));

        using (MemoryStream ms = new MemoryStream())
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + "/VisualSandboxServerData.xml");
            doc.Save(ms);
            //temp = ms.Read(senddata, 0, Convert.ToInt32(fs.Length))
            buffer = ms.ToArray();
        }
        //buffer = new byte[1024];
        //Stream  MStream =new MemoryStream();//声明出一块地址
        //MyData = new XmlDocument();
        //MyData.Load(Application.dataPath + "/VisualSandboxData.xml");
        //try
        //{
        //    XmlSerializer ser = new XmlSerializer(typeof(XmlDocument));
        //    ser.Serialize(MStream, MyData);//序列化XML文档
        //}
        //catch (XmlException e)
        //{

        //    Debug.Log(e.Message);
        //}
        //MStream.Flush();
        //MStream.Write(buffer, 0, buffer.Length);
        //MStream.Close();
        //Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));





    }
    /// <summary>
    /// 监听客户端连入 并发送初始数据
    /// </summary>
    void listenClientConnet()
    {
        while (true)
        {
            
            clientSocket = serverSocket.Accept();
            ExchangeData._ExChangeData.AddExcrption("已经有连入");
            clientSocket.Send(buffer);
            clientLinkState = ClientLinkState.Connet;
            AcceptClinkData.Start();
        }
    }
    /// <summary>
    /// 接收客户端消息
    /// </summary>
    void RecoverMessage()
    {
        while (true)
        {
            Debug.Log("Begin recover");
            clientSocket.Receive(ReMessage);
            if (ReMessage!=null)
            {
                needUpdateData = true;
            }
           // ReMessage = null;
        }
    }
     /// <summary>
     /// 初始化服务器
     /// </summary>
    public  void initializeMyServer()
    {
        Debug.Log("InitalizeServer");
        loadMyXMLData();
        IPAddress[] myIPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        foreach (IPAddress ip in myIPAddress)
        {
            //筛选出IPV4地址
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                MyIp = ip;
                Debug.Log(ip.ToString());
            }
        }
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(MyIp, myProt));  //绑定IP地址：端口               
        serverSocket.Listen(1);
        listenClinkLink = new Thread(listenClientConnet);
        AcceptClinkData = new Thread(RecoverMessage);
        listenClinkLink.Start();
       
        Debug.Log("ServerStart");
    }
    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <param name="data"></param>
   static XmlDocument deserializationData(byte[] data)
    {
        string s = System.Text.Encoding.UTF8.GetString(data);
        //使用字符串流反序列化xml文件
        Debug.Log(s);
        try
        {
            StringReader SR = new StringReader(s);
            XmlSerializer ser = new XmlSerializer(typeof(XmlDocument));
            XmlDocument accept = (XmlDocument)ser.Deserialize(SR);
            SR.Close();
            return accept;
        }
        catch 
        {

            ExchangeData._ExChangeData.AddExcrption("非法文件");
            return null;
        }
       

        // accept.Save(@"./VisualSandboxData.xml");
        
    }
    /// <summary>
    /// 解析xml获取信息并更新到exchangeData
    /// </summary>
    /// <param name="xmldoc"></param>
    static void extractData(XmlDocument xmldoc)
    {
        ExchangeData._ExChangeData.ClearException();
        canUpdateData = false;
        bool ReadFile=true;
        Debug.Log("开始解析数据");
        try
        {
            XmlNodeList topNode = xmldoc.DocumentElement.ChildNodes;
        }
        catch 
        {

           // ExchangeData._ExChangeData.AddExcrption("非法文件");
            ReadFile = false;
        }
        
        if (ReadFile==true )
        {
            XmlNode root = xmldoc.FirstChild;
            try
            {
                XmlNode SandboxGlobalTime = root.SelectSingleNode("SandboxGlobalTime");
                ExchangeData._ExChangeData.SetSandboxGlobalTime(int.Parse(SandboxGlobalTime.InnerText));
            }
            catch
            {

                ExchangeData._ExChangeData.AddExcrption("未读取到时间");
            }
            try
            {
                XmlNode SandboxGreenLightTime = root.SelectSingleNode("SandboxGreenLightTime");
                ExchangeData._ExChangeData.SetSandboxGreenLightTime(int.Parse(SandboxGreenLightTime.InnerText));
            }
            catch
            {
                ExchangeData._ExChangeData.AddExcrption("未读取到绿灯时间");

            }
            try
            {
                XmlNode SandboxRedLightTime = root.SelectSingleNode("SandboxRedLightTime");
                ExchangeData._ExChangeData.SetSandboxRedLightTime(int.Parse(SandboxRedLightTime.InnerText));
            }
            catch
            {
                ExchangeData._ExChangeData.AddExcrption("未读取到红灯时间");
            }
            try
            {
                XmlNode SandboxParkId = root.SelectSingleNode("SandboxParkId");
                ExchangeData._ExChangeData.SetsandboxParkId(int.Parse(SandboxParkId.InnerText));
                //Debug.Log(ExchangeData._ExChangeData.sandboxParkId);
                DataUIManage._DataUIManage.UpdataData();
            }
            catch
            {
                ExchangeData._ExChangeData.AddExcrption("未读取到停车点");

            }
        }
        else
        {
            ExchangeData._ExChangeData.AddExcrption("无效文件");
        }
        canUpdateData = true;




    }
    void ClsoeRecoverMessage()
    {
        AcceptClinkData.Abort();
    }
    public  void RestartServer()
    {
        //listenClinkLink.Abort();
        //AcceptClinkData.Abort();
        //serverSocket.Close();
        //if (clientSocket!=null)
        //{
        //    clientSocket.Close();
        //    clientSocket = null;
        //}

        //serverSocket = null;
        CloseServer();
        initializeMyServer();
    }
    void OnApplicationQuit()
    {
        CloseServer();
        Debug.Log("关闭程序");
    }
    void CloseServer()
    {
        try
        {
            listenClinkLink.Abort();
            AcceptClinkData.Abort();
            serverSocket.Close();
            clientSocket.Close();
            clientSocket = null;
            serverSocket = null;

        }
        catch 
        {
            
            
        }
    }
    public void SendMessageToClient( byte [] Message)
    {
        if (clientSocket!=null)
        {
            clientSocket.Send(Message);
            Debug.Log(System.Text.Encoding.UTF8.GetString(Message));
        }
    }
}

