using UnityEngine;
using System.Collections.Generic;


public class DrawTrail : MonoBehaviour {

	public GameObject trailPrefab;
    public float minimumSectionLength = 1f;
	

	public float lineSize = 4f;
    private TrailSegment currentSegment = null;

    private Vector3 lastPoint = Vector3.zero;
    private Vector3[] lastQuad = null;

	void Start () {
		
        //AddLine(ml, MakeQuad(new Vector3(5, 0, 0), new Vector3(0, 20, 0), 3), false);
	}

	void Update() {

        bool keepLastPoint = false;
        Vector3 currentPoint = GetNewPoint();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (currentSegment)
                currentSegment.Complete();
            currentSegment = null;
            lastPoint = Vector3.zero;
            lastQuad = null;
        }

        if (currentPoint == Vector3.zero)
        {
            return;
        }




        if (lastPoint != Vector3.zero) {

            if (currentSegment == null) //Create first part of new segment
            {
                lastQuad = MakeQuad(lastPoint, currentPoint, lineSize);
                currentSegment = CreateNewSegment(lastQuad);
            }

            else if (Vector3.Distance(currentPoint, lastPoint) > minimumSectionLength)  //Continue segment
            {
                lastQuad = MakeQuadWithPrevious(lastPoint, currentPoint, lastQuad, lineSize);
                currentSegment.AddLine(lastQuad, false);

            }
            else //Don't draw section yet, not big enough section
            {
                keepLastPoint = true;
            }
        }

        if (!keepLastPoint)
        {
            lastPoint = currentPoint;
        }


        

	}

    Vector3[] MakeQuad(Vector3 s, Vector3 e, float w)
    {
        w = w / 2;
        Vector3[] q = new Vector3[4];

        Vector3 n = Vector3.Cross(s, e);
        Vector3 l = Vector3.Cross(n, e - s);
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


    TrailSegment CreateNewSegment(Vector3[] quad)
    {
        var go = GameObject.Instantiate(trailPrefab);
        var ts = go.GetComponent<TrailSegment>();
        ts.AddLine(quad, false);
        return ts;
    }
	
	Vector3 GetNewPoint() {
        return CameraControl.instance.lastRayIntersect;
	}

}







