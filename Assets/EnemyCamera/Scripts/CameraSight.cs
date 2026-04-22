using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraSight : MonoBehaviour
{

    [SerializeField]
    Transform playerTransform;
    Transform lensTransform;
    float maxDistanceToTarget = 6f;
    float distanceToTarget;

    void Start()
    {
        lensTransform = gameObject.transform.Find("Lens");
        RotateObject();
    }

    void LookAtTarget()
    {
        //this.transform.LookAt(playerTransform.position);
        Vector3 lookVector = playerTransform.position - transform.position;
        float AngleBetween = Vector3.Angle(transform.position, playerTransform.position);
        if (AngleBetween <= 65)
        {
            Quaternion rotation = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
        }
    }
    private void RotateObject()
    {
        //transform.rotation = Quaternion.Euler(new Vector3(0, 10, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
        Spot();

    }

    void Spot()
    {
        distanceToTarget = Vector3.Distance(playerTransform.position, lensTransform.position);
        

        if (distanceToTarget <= maxDistanceToTarget)
        {
            LookAtTarget();

        }
    }
}

