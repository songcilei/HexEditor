using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    
    public HexCoordinates coordinates;
    public Color color;

    [SerializeField]
    private HexCell[] neighbors;



    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction,HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;//将其邻居当相反接口设置为自身 因为临近相交是双向当
    }
}



public enum HexDirection
{
    NE,E,SE,SW,W,NW
}

 /// <summary>
 /// 求出相反当方向  这个是扩展方法
 /// </summary>
public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    //这里是为了检索相邻的上下两块的块
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
    
}