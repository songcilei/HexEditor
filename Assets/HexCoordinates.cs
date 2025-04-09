using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HexCoordinates
{

    [SerializeField]
    private int x, z;
    public int X {
        get
        {
            return x;
        } }
    public int Z {
        get
        {
            return z;
        }
    }
    

    public HexCoordinates(int x,int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x -z/2, z);//修复坐标
    }

    public int Y
    {
        get { return -X - Z; }
    }


    public override string ToString()
    {
        return "(" + X.ToString() + ","+Y.ToString()+ ","+ Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() +"\n"+Y.ToString()+ "\n" + Z.ToString();
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x; // 这里是因为在立方体坐标系中  y =  -x 即  y 是 x的镜像
        float offset = position.z / (HexMetrics.outerRadius * 3f);//因为只有当Z为0 时，才是正确坐标 所以每两行我们都要向左移动一个单位
        x -= offset;
        y -= offset;

        //又小数转为整数
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY +iZ !=0)
        {
            Debug.LogWarning("rounding error!");
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX>dY && dX>dZ)
            {
                iX = -iY - iZ;
            }
            else
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX,iZ);
    }
}
