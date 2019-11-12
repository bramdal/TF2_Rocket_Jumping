using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : MonoBehaviour
{
    [Header("Weapon Stats")]
    //weapon stats
    public int clipSize = 4;
    int clipAmount;
    public int totalAmmo = 20;
    [Header("Projectile Variables")]
    //projectile
    public GameObject projectile;
    public Transform shootOrigin;
    //states
    bool canShoot = true;
    bool reloading = false;
    Animator anim;
    [Header("UI Referencess")]
    //UI
    public Text clipAmountText;
    public Text totalAmmoText;
    static int weaponIndex = 1;
    int requestedWeapon;
    private void Start(){
        clipAmount = clipSize;
        anim = GetComponent<Animator>();
    }
    private void Update() {
        ShootRocket();
        SwitchWeapon();
    }
    void ShootRocket(){
        if(clipAmount>0){
            if(Input.GetButtonDown("Fire1") && canShoot){
                anim.SetTrigger("Shoot");
                canShoot = false;
            }
        }
        if(canShoot && clipAmount < clipSize){
            if(totalAmmo > 0 && !reloading){
                reloading = true;
                anim.SetTrigger("Reload");
            }
        }
    }
    void ShootRocketAnimation(){
        Instantiate(projectile, shootOrigin.position, shootOrigin.rotation);
        clipAmount--;
        clipAmountText.text = clipAmount.ToString();
    }
    void ReloadAmmo(){
        clipAmount++;
        totalAmmo--;
        reloading = false;
        clipAmountText.text = clipAmount.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }
    void UnlockAnimation(){
        canShoot = true;
        reloading = false;
    }    
    void SwitchWeapon(){
        float wheelMove = Input.mouseScrollDelta.y;
        if(wheelMove != 0){
            canShoot = false;
            anim.SetTrigger("Swing Out");
            if(wheelMove > 0){
                requestedWeapon = weaponIndex + 1;
            }
            else if(wheelMove < 0){
                requestedWeapon = weaponIndex - 1;
            }
        }
    }
    void SwingOut(){
        WeaponManager.ChangeWeapons(requestedWeapon);
    }
    private void OnEnable() {
        if(anim!=null)
            anim.SetTrigger("Swing In");
        clipAmountText.text = clipAmount.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }
}
