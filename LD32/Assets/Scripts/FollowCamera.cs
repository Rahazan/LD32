﻿using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    private Rigidbody2D targetRigidbody;

    public float snappiness = 0.2f;

    private Vector3 targetPos;
    
    private Quaternion targetRot;



    // Use this for initialization
    void Start()
    {
        if (!target) target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();

    }

    void LateUpdate()
    {
        var speed = targetRigidbody.velocity.magnitude;
        var desiredOffset = new Vector3(targetRigidbody.velocity.x *0.3f, targetRigidbody.velocity.y * 0.3f, -10 + speed / -7f);
        targetPos = target.position + desiredOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, snappiness* Time.deltaTime);


    }
}
