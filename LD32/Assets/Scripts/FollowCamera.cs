using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    private Rigidbody2D targetRigidbody;

    public float snappiness = 0.2f;

    private Vector3 targetPos;
    
    private Quaternion targetRot;



    void Awake()
    {
        if (!target) target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        if (!target) target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();

    }


    void OnEnable()
    {

        GetComponent<Rigidbody>().velocity = targetRigidbody.velocity*0.3f;
    }

    void LateUpdate()
    {



        var speed = targetRigidbody.velocity.magnitude;
        var desiredOffset = new Vector3(targetRigidbody.velocity.x *0.3f + 6f, targetRigidbody.velocity.y * 0.3f, -20 + speed / -12f);
        targetPos = target.position + desiredOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, snappiness* Time.deltaTime);


    }
}
