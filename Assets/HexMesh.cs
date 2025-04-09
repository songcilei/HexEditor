using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh hexMesh;
    public List<Vector3> vertices;
    private List<int> triangles;
    private List<Color> colors;
    private MeshCollider _meshCollider;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();

    }


    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.RecalculateNormals();
        _meshCollider.sharedMesh = hexMesh;
    }


    private void Triangulate(HexCell cell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)//这里是通过枚举进行for 循环
        {
            Triangulate(d,cell);
        }
    }

    public void Triangulate(HexDirection direction,HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);
        //中心区域
        AddTriangle(
            center,v1,v2
        );
        AddTriangleColor(cell.color);


        if (direction <= HexDirection.SE)
        {
            TriangulateConnection(direction,cell,v1,v2);

        }
/*
        
        //添加三角形之间的桥  其实就是为了避免 临近三角形 造成的互相污染染色  所以通过剖分一个长方形来隔离染色污染
        Vector3 bridge = HexMetrics.GetBridge(direction);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;
    
        //桥区域
        AddQuad(v1,v2,v3,v4);
        //混合相邻的桥的颜色--------------------------------- 这里是跨角混合
        HexCell prevNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
        HexCell neighbor = cell.GetNeighbor(direction)??cell;
        HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;
        
        Color bridgeColor = (cell.color + neighbor.color) * 0.5f;
        AddQuadColor(cell.color,bridgeColor);
        //--------------------------------------------
        // 补充区域
        AddTriangle(v1,center+HexMetrics.GetFirstCorner(direction),v3);
        AddTriangleColor(
            cell.color,
            (cell.color + prevNeighbor.color + neighbor.color)/3f,
            bridgeColor
            );

        AddTriangle(v2,v4,center + HexMetrics.GetSecondCorner(direction));
        AddTriangleColor(
            cell.color,
            bridgeColor,
            (cell.color + neighbor.color + nextNeighbor.color)/3f
        );
        
*/
    }

    void TriangulateConnection(HexDirection direction,HexCell cell,Vector3 v1,Vector3 v2)
    {
        HexCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null)
        {
            return;
        }

        Vector3 bridge = HexMetrics.GetBridge(direction);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;
        AddQuad(v1,v2,v3,v4);
        AddQuadColor(cell.color, neighbor.color);

        HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
        if (direction<= HexDirection.E && nextNeighbor != null)
        {
            AddTriangle(v2,v4,v2+HexMetrics.GetBridge(direction.Next()));
            AddTriangleColor(cell.color,neighbor.color,nextNeighbor.color);
        }
    }

    void AddTriangle(Vector3 v1,Vector3 v2,Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void AddTriangleColor(Color c1,Color c2,Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    void AddQuad(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex+2);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+2);
        triangles.Add(vertexIndex+3);
        
    }

    void AddQuadColor(Color c1,Color c2, Color c3,Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }

    void AddQuadColor(Color c1,Color c2)
    {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }

}