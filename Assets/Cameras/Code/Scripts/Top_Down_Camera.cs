using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top_Down_Camera : MonoBehaviour {

    public Transform m_Target;
    public float m_Height = 10f;
    public float m_Distance = 20f;
    public float m_Angle = 45f;
    public float m_SmoothSpeed = 0.5f;
    private Vector3 refVelocity;
	// Use this for initialization
	void Start ()
    {
        HandleCamera();

	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleCamera();
	}

    protected virtual void HandleCamera()

    {
        if(!m_Target)
        {
            return;
        }
        //Build world Pos vector
        Vector3 worldPos = (Vector3.forward *  -m_Distance) + (Vector3.up * m_Height);

//      Debug.DrawLine(m_Target.position, worldPos, Color.red);

        // Build rotated Vector
        Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle , Vector3.up)* worldPos;

//      Debug.DrawLine(m_Target.position, rotatedVector, Color.green);

        //Move
        Vector3 flatTargetPosition = m_Target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPos = flatTargetPosition + rotatedVector;

//      Debug.DrawLine(m_Target.position, finalPos, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref refVelocity, m_SmoothSpeed);
        transform.LookAt(flatTargetPosition);

    }
/*      void OnDrawGizmos()
    {
        if(m_Target)
        {
            Gizmos.DrawLine(transform.position, m_Target.position);
            Gizmos.DrawSphere(m_Target.position, 1.5f);
        }
        Gizmos.DrawSphere(transform.position, 1.5f);
    } */
}
