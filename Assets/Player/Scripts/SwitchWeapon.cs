using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public enum Weapon
    {
        Gun,
        Bow
    };

    public enum ContainerName
    {
        Gun,
        Bow
    };

    [SerializeField]
    private Weapon weapon = Weapon.Bow;

    Dictionary<Weapon, ContainerName> containerNameByWeapon = new Dictionary<Weapon, ContainerName>();
    Dictionary<ContainerName, Transform> containerByName = new Dictionary<ContainerName, Transform>();

    void Start()
    {
        InitialiseContainers();
        WeaponSwitch(weapon);
    }

    void InitialiseContainers()
    {
        containerNameByWeapon.Add(Weapon.Gun, ContainerName.Gun);
        containerNameByWeapon.Add(Weapon.Bow, ContainerName.Bow);

        containerByName.Add(ContainerName.Gun, gameObject.transform.Find(ContainerName.Gun.ToString()));
        containerByName.Add(ContainerName.Bow, gameObject.transform.Find(ContainerName.Bow.ToString()));
    }

    void DisableAllContainers()
    {
        foreach (KeyValuePair<ContainerName, Transform> container in containerByName)
        {
            container.Value.gameObject.SetActive(false);
        }
    }

    void ActivateWeapon(Weapon weapon)
    {
        ContainerName container;
        if (containerNameByWeapon.TryGetValue(weapon, out container))
        {
            if (containerByName.TryGetValue(container, out Transform transform))
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    void WeaponSwitch(Weapon weapon)
    {
        DisableAllContainers();
        ActivateWeapon(weapon);
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public void SetWeapon(Weapon weapon)
    {
        if (this.weapon == weapon)
        {
            return;
        }

        this.weapon = weapon;

        WeaponSwitch(weapon);
    }
}