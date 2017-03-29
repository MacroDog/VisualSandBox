using UnityEngine;
using System.Collections;

public delegate void SandBoxStatusUpdates();  
public class VSSandGameManage:MonoBehaviour  {
    public SandBoxStatusUpdates StatusUpdates;
    private static VSSandGameManage _vsSandGameManager;
    public static VSSandGameManage _VSandGameManager
    {
        get
        {
            if (_vsSandGameManager==null)
            {
                _vsSandGameManager = GameObject.FindObjectOfType<VSSandGameManage>();
                if (_vsSandGameManager==null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "SandBoxManager";
                    temp.AddComponent<VSSandGameManage>();
                    _vsSandGameManager = temp.GetComponent<VSSandGameManage>();
                }
            }
            return _vsSandGameManager;
        }
    }
   
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnStartButtonClick()
    {
        Invoke("startrun", 0);

    } 
    private void startrun()
    {
        StatusUpdates();
    }
}
