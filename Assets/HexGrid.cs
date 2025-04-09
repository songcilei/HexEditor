using System;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    private HexCell[] cells;

    public Color defaultColor = Color.white;
    // public Color touchedColor = Color.magenta;
    
    public Text cellLabelPrefab;
    public Canvas gridCanvas;
    public HexMesh hexMesh;

    private void Awake()
    {
        cells = new HexCell[height * width];
        gridCanvas = this.GetComponentInChildren<Canvas>();

        for (int z = 0,i =0 ; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x,z,i++);
            }            
        }

    }


    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    private void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     HandleInput();
        // }
    }



    public void ColorCell(Vector3 position,Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
        Debug.Log("touched at :"+coordinates.ToString());
    }

    /// <summary>
    /// 这里当x z 分别指的是行列 当index  和 hexblock当坐标编号没关系
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="i"></param>
    void CreateCell(int x,int z,int i)
    {
        Vector3 position;
        // position.x = x * 10f;
        // position.y = 0f;
        // position.z = z * 10f;
        position.x = (x+0.5f*z - z/2) * HexMetrics.innerRadius * 2f;//水平移动  这里的+0.5*z-z/2 是为了偏移第二行 这是一种差值取整
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius * 1.5f;//垂直移动

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform,false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x,z);
        cell.color = defaultColor;//增加颜色
        //-------------------------------------------------Neighbors
        if (x>0)//这里x是 x index  不是 x 坐标
        {
            cell.SetNeighbor(HexDirection.W,cells[i-1]);
        }

        if (z>0)
        {
            if ((z&1)==0)//这里Z&1 是位运算符 用于求奇数列
            {
                cell.SetNeighbor(HexDirection.SE,cells[i-width]);
                if (x>0)
                {
                    cell.SetNeighbor(HexDirection.SW,cells[i-width-1]);
                }
            }
            else //这里是求偶数
            {
                cell.SetNeighbor(HexDirection.SW,cells[i-width]);
                if (x < width -1)
                {
                    cell.SetNeighbor(HexDirection.SE,cells[i-width+1]);
                }
            }
        }

        //-------------------------------------------------UI Start
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        label.text = cell.coordinates.ToStringOnSeparateLines();
        //------------------------------------------------- UI End



    }
}