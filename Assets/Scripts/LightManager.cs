using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour {

    [SerializeField]
    private Material waiter;
    [SerializeField]
    private Material black;
    private LightManager _lightManage;
    [SerializeField]
    private Light SunLight;
    public LightManager _LightManage
    {
        get
        {
            if (_lightManage==null)
            {
                _lightManage = GameObject.FindObjectOfType<LightManager>();
                if (_lightManage==null)
                {
                    GameObject asd = new GameObject();
                    asd.name = "LightManager";
                    asd.AddComponent<LightManager>();
                    _lightManage = asd.GetComponent<LightManager>();
                }
            }
            return _lightManage;
        }
    }
    [SerializeField]
    private GameObject[] streetLamp;
    
   void Awake()
    {
        VSSandGameManage._VSandGameManager.StatusUpdates += UpdateManager;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void UpdateManager()
    {
        Debug.Log("lightManagerUpdate");
        UpDateLightStata(ExchangeData._ExChangeData.sandboxGlobalTime);
    }
    public void UpDateLightStata(int time )
    {
        
        bool  StreetLampState = false;
        if (time==0)
        {
            RenderSettings.skybox = waiter;
            StreetLampState = false;
        }
        else
        {
            RenderSettings.skybox = black;
            StreetLampState = true;
            Destroy(SunLight);
        }
        foreach (var item in streetLamp)
        {
            item.SetActive(StreetLampState);
        }
    }
}
