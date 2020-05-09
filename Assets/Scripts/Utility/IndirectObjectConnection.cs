using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndirectObjectConnection : MonoBehaviour
{
    [SerializeField]
    GameObject parentObject;
    Vector3 offsetPos;
    Vector3 offsetRot;
    Vector3 startRot;
    [SerializeField]
    bool m_FollowPosition;
    [SerializeField]
    bool m_FollowRotation;
    private void Start()
    {
        if(m_FollowPosition)
            offsetPos = transform.position - parentObject.transform.position;
        if(m_FollowRotation)
            offsetRot = transform.eulerAngles - parentObject.transform.eulerAngles;
    }
    void Update()
    {
        if(m_FollowPosition)
            transform.position = parentObject.transform.position + offsetPos;
        if(m_FollowRotation)
            transform.rotation = Quaternion.Euler(parentObject.transform.eulerAngles + offsetRot);
    }
}
