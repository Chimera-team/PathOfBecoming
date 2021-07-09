﻿using UnityEngine;

public class Fairy : MonoBehaviour
{
    Rigidbody2D rb;
    RelativeJoint2D joint;

    Engine engine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<RelativeJoint2D>();
        engine = transform.parent.GetComponent<Engine>();
    }

    public void Connect_Fairy(Rigidbody2D anchor)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        joint.connectedBody = anchor;
    }

    public void Load_State(FairyData data)
    {
        transform.position = data.checkPoint.Convert_to_UnityVector();
        if (data.connected)
            engine.Connect_Fairy_to_Player();
    }

    public FairyData Save_State()
    {
        return new FairyData(new Vector3Serial(transform.position));
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x > 0 && transform.rotation.eulerAngles.y == 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (rb.velocity.x < 0 && transform.rotation.eulerAngles.y == 180)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
