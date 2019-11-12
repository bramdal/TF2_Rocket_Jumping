using System;
using UnityEngine;

public static class WeaponManager
{
    public static GameObject[] weapons = new GameObject[2];
    static GameObject weaponHolder;
    
    public static int currentWeapon;

    static WeaponManager(){
        weaponHolder = GameObject.Find("Weapon Holder");
        weapons[0] = GameObject.Find("Grenade Launcher");
        weapons[1] = GameObject.Find("Rocket Launcher");
        weapons[1].SetActive(false);
        currentWeapon = 0;
        
    }

    public static void ChangeWeapons(int weaponIndex){
        if(weaponIndex == 2)
            weaponIndex = 0;
        if(weaponIndex == -1)
            weaponIndex = 1;        
        weapons[currentWeapon].SetActive(false);
        currentWeapon = weaponIndex;
        weapons[currentWeapon].SetActive(true);
    }

}
