using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public int Segments;
    public float XRadius;
    public float YRadius;

    float x;
    float y;
    float z = 0f;
    float angle = 10f;

    LineRenderer line;
    Transform ParentTransform;
    float Rot = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ParentTransform = this.transform.parent.GetComponent<Transform>();

        line = gameObject.GetComponent<LineRenderer>();
        Color color = Color.green;

        line.SetWidth(0.05f, 0.05f);

        line.SetVertexCount(Segments + 1);
        line.useWorldSpace = false;


        for(int i = 0; i < Segments + 1; i++)
        {
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * XRadius;
            y = Mathf.Sin(Mathf.Deg2Rad * angle) * XRadius;

            line.SetColors(Color.green, Color.green);

            line.SetPosition(i, new Vector3(x, y, z));
            angle += (360f / Segments);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rot = 2.0f;
        ParentTransform.Rotate(new Vector3(0, Rot, 0),Space.World);
    }
}
