using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMetrics
{
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public const float solidFactor = 0.75f;//混合区
    public const float blendFactor = 1f - solidFactor;

    public static Vector3[] corners =
    {
        
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius),

    };

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction] ;
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] ;
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;

    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;

    }

    //获取 三角形 内的桥  其实是为了避免染色污染 进行的三角形切分
    // public static Vector3 GetBridge(HexDirection direction)
    // {
    //     
    //     return (corners[(int)direction]+corners[(int)direction+1])* 0.5f * blendFactor;
    // }

    //该方法是 将上面的方法计算的双矩形合并成一个 
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction]+corners[(int)direction +1])*blendFactor;
    }
}


