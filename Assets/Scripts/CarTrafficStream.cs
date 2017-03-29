using UnityEngine;
using System.Collections;

public class CarTrafficStream : MonoBehaviour {
    [SerializeField]
    private GameObject[] PrbCar;
    [SerializeField]
    private CarPath[] AICarPath;
    [SerializeField]
    private Transform rotetion;
    private bool isRun=false;
	// Use this for initialization
	void Start () {
        VSSandGameManage._VSandGameManager.StatusUpdates += starwork;

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void starwork()
    {

        Debug.Log("trafficStreamStart");
        isRun = true;
        StartCoroutine(CreateCarInvoke());
       
    }
    IEnumerator CreateCarInvoke()
    {
        while (isRun==true)
        {
            CreateNewCar();
            yield return new WaitForSeconds(60/ExchangeData._ExChangeData.trafficsStream);
        }
        
    }
    void CreateNewCar()
    {
        //Debug.Log("Creat");
        int a = Random.Range(0, PrbCar.Length );
        int b = Random.Range(0, AICarPath.Length );
        GameObject temp= GameObject.Instantiate(PrbCar[a], AICarPath[b].NowWayPoint(0).position, rotetion.rotation) as GameObject;
        temp.AddComponent<AICarControl>();
        temp.GetComponent<AICarControl>().SetMyWay(AICarPath[b]);
        temp.GetComponent<AICarControl>().StartRun();
        

    }
}
