using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    Sprite icon;

    [SerializeField]
    string itemName;
    [SerializeField]
    [TextArea(4, 16)]
    string description;

    [SerializeField]
    float rawDamage = 10f;

    [SerializeField]
    bool isPickupOnCollision = true;

    public float SaveTime = 1f;
    float ElapsedTime;

    private void Start()
    {
        if (isPickupOnCollision)
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            ElapsedTime = SaveTime;
        }
    }


    private void OnTriggerStay(Collider collider)
    {
        if (isPickupOnCollision)
        {
            if (ElapsedTime%60 <= 0)
            {
                if (collider.tag == "Player")
                {
                    ElapsedTime = SaveTime;
                    collider.SendMessageUpwards("Hit", rawDamage, SendMessageOptions.DontRequireReceiver);
                }

            }
            else
            {
                ElapsedTime -= Time.deltaTime;
            }
        }

    }
}