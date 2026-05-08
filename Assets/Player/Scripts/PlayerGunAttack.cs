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
    GameObject ammoUI;

    [SerializeField]
    GameObject magUI;

    [SerializeField]
    GameObject gunArms;

    [SerializeField]
    float reloadTime = 4f;

    [SerializeField]
    LayerMask layerMask;

    bool canShoot;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magAmmoText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip ReloadAnim;
   
    PlayerInput playerInput;
    InputAction attackAction;
    InputAction reloadAction;

    void Start()
    {
        this.enabled = false;
        ammoUI.SetActive(false);
        magUI.SetActive(false);
        gunArms.SetActive(false);
    }
    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        canShoot = true;
        gunArms.SetActive(true);
        ammoUI.SetActive(true);
        magUI.SetActive(true);
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

        if (reloadAction.triggered && ammo != 8 && mag !=0)
        {
            canShoot = false;
            Debug.Log("Reloading...");
            animation.Play(ReloadAnim.name);
            Invoke("Reload", reloadTime);
        }


    }
    void FireWeapon()
    {

        if (canShoot == true)
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

    void Reload()
    {
        canShoot = false;
        if (mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }

        magAmmoText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
        canShoot = true;
        Debug.Log("Reloaded!");
    }
    void MagUp(int magPick)
    {
        mag += magPick;
        magAmmoText.text = mag.ToString();
        Debug.Log("Picked up: " + mag.ToString());
    }
}