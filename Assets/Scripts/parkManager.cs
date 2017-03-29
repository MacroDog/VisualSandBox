using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class parkManager : MonoBehaviour {

    private static parkManager _parkManage;
    public static parkManager _ParkManage
    {
        get
        {
            if (_parkManage == null)
            {
                _parkManage = GameObject.FindObjectOfType<parkManager>();
                if (_parkManage == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "ParkManage";
                    temp.AddComponent<parkManager>();
                    _parkManage = temp.GetComponent<parkManager>();
                }
            }
            return _parkManage;
        }
        
       


    }
    [SerializeField]
    private GameObject[] parkings;
    [SerializeField]
    private GameObject[] prefabCar;
    // Use this for initialization
    void Awake()
    {
        VSSandGameManage._VSandGameManager.StatusUpdates += UpdataManager;
    }
    void Start () {
        
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //更新当前状态
    public void UpdataManager()
    {
        UpdataParking();
    }
    //生成汽车在占用停车位上
    public void UpdataParking()
    {
        List<int> ASLis = new List<int> (ExchangeData._ExChangeData.FreeParking);
        for (int i = 0; i < parkings.Length - 1; i++)
        {
            if (!ASLis.Contains(i))
            {
                //Debug.Log(i);
                CreatParking(parkings[i].transform);
            }
        }
    }
    //产生随机车辆在停车点
    private void CreatParking(Transform Pos)
    {
        int ramdonCar = Random.Range(0, prefabCar.Length-1);
        GameObject sas = GameObject.Instantiate(prefabCar[ramdonCar], Pos.position, prefabCar[ramdonCar].transform.rotation) as GameObject;
        sas.transform.parent = this.gameObject.transform;
        Destroy(sas.GetComponent<MeshCollider>());
    }
    public Transform GetParkPointTransform(int i)
    {
        if (i<= parkings.Length-1)
        {
            return parkings[i].transform;
        }
        else
        {
            return null;
        }
    }
}
