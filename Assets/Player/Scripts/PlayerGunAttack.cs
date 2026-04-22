using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunAttack : MonoBehaviour
{
    Transform cameraTransform;
    float range = 100f;

    [SerializeField]
    float rawDamage = 10f;

    [SerializeField]
    LayerMask layerMask;
   
    PlayerInput playerInput;
    InputAction attackAction;

    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        var map = playerInput.currentActionMap;
        attackAction = map.FindAction("Attack", true);
    }

    void Update()
    {
        if (!MenuControls.IsGamePaused)
        {
            FireWeapon();
        }

    }
    void FireWeapon()
    {
        if (attackAction.triggered)
        {

            cameraTransform = Camera.main.transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit raycastHit;

            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * range, Color.blue, 1f);

            if (Physics.Raycast(ray, out raycastHit, range, layerMask))
            {
                if (raycastHit.transform != null)
                {
                    raycastHit.collider.SendMessageUpwards("Hit", rawDamage, SendMessageOptions.DontRequireReceiver);

                }
            }
            else
            {
                Debug.Log("NO RAYCAST");
            }
        }
    }
}