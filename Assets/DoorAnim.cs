using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DoorAnim : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField]
    private bool rotateSelf = true;
    [SerializeField]
    private GameObject doorObject;
    [SerializeField]
    private Vector3 rotationAxis = Vector3.up;
    [SerializeField]
    private float rotationAngle = -90f;
    [SerializeField]
    private float rotationSpeed = 1.5f;


    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isOpen = false;
    private Transform targetTransform;
    private bool isRotating = false;


    private void Awake()
    {
        targetTransform = (rotateSelf || doorObject == null) ? transform : doorObject.transform;

        initialRotation = targetTransform.rotation;
    }

    public void ToggleDoor()
    {

        if (isRotating) return;

        StopAllCoroutines();
        StartCoroutine(RotateDoor(!isOpen));
    }

    private IEnumerator RotateDoor(bool opening)
    {
        isRotating = true;

        targetRotation = opening ? initialRotation * Quaternion.Euler(rotationAxis * rotationAngle) : initialRotation;

        Quaternion startRotation = targetTransform.rotation;

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration) 
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            targetTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        } 

        targetTransform.rotation = targetRotation;
        isOpen = opening;
        isRotating = false;
    }

    
}
