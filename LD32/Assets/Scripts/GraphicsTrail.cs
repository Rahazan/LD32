using UnityEngine;
using System.Collections.Generic;

class Point {
	public Vector3 p;
	public Point next;
}

class TrailSegment
{
    
}


public class GraphicsTrail : MonoBehaviour {

	public Shader shader;

	private Mesh ml;
	private Material lmat;
	
	private Vector3 last = Vector3.zero;

	private float lineSize = 4f;
	
	
	private Point first;

	void Start () {
		
		ml = new Mesh();
		lmat = new Material(shader);
		lmat.color = new Color(0,0,0,0.8f);


        //AddLine(ml, MakeQuad(new Vector3(5, 0, 0), new Vector3(0, 20, 0), 3), false);
	}

    private Vector3[] prevQuad = null;

	void Update() {


        Vector3 current = GetNewPoint();
        Vector3[] quad = MakeQuad(last, current, lineSize);

        if (prevQuad != null)
        {
            AddLine(ml, MakeQuadWithPrevious(last, current, prevQuad, lineSize), false);
        }
        else
        {
            AddLine(ml, quad, false);
        }

        prevQuad = quad;
        last = current;


		Draw();

        if (Input.GetKeyDown(KeyCode.C))
        {
            ml = new Mesh();
            first = null;
        }

	}
	
	void Draw() {
		Graphics.DrawMesh(ml, transform.localToWorldMatrix, lmat, 0);
	}
	
	Vector3[] MakeQuad(Vector3 s, Vector3 e, float w) {
		w = w / 2;
		Vector3[] q = new Vector3[4];

		Vector3 n = Vector3.Cross(s, e);
		Vector3 l = Vector3.Cross(n, e-s);
		l.Normalize();


        q[0] = s + l * w; //From left
        q[1] = s + l * -w; //From right

        q[2] = (e + l * w); //To left
        q[3] = (e + l * -w); //To Right

		return q;
	}

    Vector3[] MakeQuadWithPrevious(Vector3 s, Vector3 e, Vector3[] previous, float w)
    {
        w = w / 2;
        Vector3[] q = new Vector3[4];

        Vector3 n = Vector3.Cross(s, e);
        Vector3 l = Vector3.Cross(n, e - s);
        l.Normalize();

        q[0] = previous[2];
        q[1] = previous[3];

        q[2] = (e + l * w); //To left
        q[3] = (e + l * -w); //To Right

        return q;
    }

	
	void AddLine(Mesh m, Vector3[] quad, bool tmp) {
			int vl = m.vertices.Length;
			
			Vector3[] vs = m.vertices;
			if(!tmp || vl == 0) vs = resizeVertices(vs, 4);
			else vl -= 4;
			
			vs[vl] = quad[0];
			vs[vl+1] = quad[1];
			vs[vl+2] = quad[2];
			vs[vl+3] = quad[3];
			
			int tl = m.triangles.Length;
			
			int[] ts = m.triangles;
			if(!tmp || tl == 0) ts = resizeTraingles(ts, 6);
			else tl -= 6;
			ts[tl] = vl;
			ts[tl+1] = vl+1;
			ts[tl+2] = vl+2;
			ts[tl+3] = vl+1;
			ts[tl+4] = vl+3;
			ts[tl+5] = vl+2;
			
			m.vertices = vs;
			m.triangles = ts;
			m.RecalculateBounds();
	}
	
	Vector3 GetNewPoint() {
        return CameraControl.instance.lastRayIntersect + Vector3.forward;
	}
	
	Vector3[] resizeVertices(Vector3[] ovs, int ns) {
		Vector3[] nvs = new Vector3[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
		return nvs;
	}
	
	int[] resizeTraingles(int[] ovs, int ns) {
		int[] nvs = new int[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
		return nvs;
	}

}







