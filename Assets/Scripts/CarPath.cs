using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarPath : MonoBehaviour
{
    [SerializeField]
    private List<Transform> WayPoints;

    public int WayPointCount()
    {
        return WayPoints.Count;
    }
    /// <summary>
    /// 返回目标点的下一点的transform
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Transform NextWayPoint(int i)
    {
        Transform sk = null;
        if (i++ < WayPoints.Count - 1)
        {
            sk = WayPoints[i];

        }
        return sk;

    }
    /// <summary>
    /// 返回目标点的transform
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Transform NowWayPoint(int i)
    {
        Transform sk = null;
        if (i < WayPoints.Count - 1)
        {
            sk = sk = WayPoints[i];
        }
        return sk;
    }


    /// <summary>
    /// 添加移动路径
    /// </summary>
    /// <param name="waypoint"></param>
    public void AddWayPoint(Transform waypoint)
    {
        if (WayPoints.Contains(waypoint) ==false)
        {
            WayPoints.Add(waypoint);
            //waypoint.parent = this.transform;
            //waypoint.name = "waypoint (" + WayPoints.Count.ToString() + ")";
        }
    }

    public void AddWayPoints(Transform[] waypoints)
    {
        for (int i = 0; i < waypoints.Length-1; i++)
        {
            AddWayPoint(waypoints[i]);
        }
    }
    public Transform[] wayPointToArray()
    {
        //Debug.Log(WayPoints.Count);
        Transform[] Temp= WayPoints.ToArray();
        return Temp;
    }
}
