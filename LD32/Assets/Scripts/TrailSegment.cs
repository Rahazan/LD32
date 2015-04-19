using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailSegment : MonoBehaviour
{

    private Mesh mesh;
    private Material mat;
    public Shader shader;
    EdgeCollider2D col;

    private Vector3[] _vertices;

    public bool done = false;

    void Awake()
    {
        mesh = new Mesh();
        mat = new Material(shader);
        mat.color = new Color(0, 0, 0, 0.8f);
    }


    public void Complete()
    {
        CreateCollider();

        GetComponent<MeshFilter>().mesh = mesh;

        done = true;

    }

    public void CreateCollider()
    {
        col = gameObject.GetComponent<EdgeCollider2D>();


        //Quad indices:
        //0 is left start
        //1 is right start
        //2 is left end
        //3 is right end
        List<Vector2> leftEdge = new List<Vector2>();
        List<Vector2> rightEdge = new List<Vector2>();

        for (int i = 0; i < _vertices.Length; i++)
        {
            if (i % 2 == 0)
                leftEdge.Add(_vertices[i]);
            else
                rightEdge.Add(_vertices[i]);
        }

        int pointCount = leftEdge.Count + rightEdge.Count + 1;
        Vector2[] colPoints = new Vector2[pointCount];

        //Close the start of the segment
        colPoints[0] = rightEdge[0];
        for (int i = 0; i < leftEdge.Count; i++)
        {
            colPoints[i + 1] = leftEdge[i];
        }

        //Looping around the right side
        rightEdge.Reverse();

        for (int i = 0; i < rightEdge.Count; i++)
        {
            colPoints[i + 1 + leftEdge.Count] = rightEdge[i];
        }

        col.points = colPoints;

    }

    void Update()
    {
        if (!done)
        {
            Draw();
        }
    }

    void Draw()
    {
        Graphics.DrawMesh(mesh, transform.localToWorldMatrix, mat, 0);
    }

    public void AddLine(Vector3[] quad, bool tmp)
    {
        int vl = mesh.vertices.Length;

        Vector3[] vs = mesh.vertices;
        if (!tmp || vl == 0) vs = resizeVertices(vs, 4);
        else vl -= 4;

        vs[vl] = quad[0];
        vs[vl + 1] = quad[1];
        vs[vl + 2] = quad[2];
        vs[vl + 3] = quad[3];

        int tl = mesh.triangles.Length;

        int[] ts = mesh.triangles;
        if (!tmp || tl == 0) ts = resizeTriangles(ts, 6);
        else tl -= 6;
        ts[tl] = vl;
        ts[tl + 1] = vl + 1;
        ts[tl + 2] = vl + 2;
        ts[tl + 3] = vl + 1;
        ts[tl + 4] = vl + 3;
        ts[tl + 5] = vl + 2;

        mesh.vertices = vs;
        _vertices = vs;
        mesh.triangles = ts;
        mesh.RecalculateBounds();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

}
