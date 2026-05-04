using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunAttack : MonoBehaviour
{
    Transform cameraTransform;
    float range = 100f;

    [SerializeField]
    float rawDamage = 10f;

    [SerializeField]
    int ammo = 8;

    [SerializeField]
    int mag = 0;

    [SerializeField]
    int magAmmo = 8;

    [SerializeField]
    LayerMask layerMask;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magAmmoText;
   
    PlayerInput playerInput;
    InputAction attackAction;
    InputAction reloadAction;

    void Start()
    {
        this.enabled = false;
    }
    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        var map = playerInput.currentActionMap;
        attackAction = map.FindAction("Attack", true);
        reloadAction = map.FindAction("Reload", true);

        magAmmoText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Update()
    {
        if (!MenuControls.IsGamePaused && ammo > 0 && attackAction.triggered)
        {
            FireWeapon();
            ammo--;
            magAmmoText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

        }

        if (reloadAction.triggered && ammo != 8)
        {
            Reload();
        }

    }
    void FireWeapon()
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

    void Reload()
    {
        if (mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }

        magAmmoText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
}