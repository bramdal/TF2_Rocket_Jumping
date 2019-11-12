using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeLauncher : MonoBehaviour
{
    [Header("Weapon Stats")]
    //weapon stats
    public int clipSize = 8;
    int clipAmount = 8;
    public int totalAmmo;
    [Header("Projectile Variables")]
    //projectile variables
    public float projectileForce = 15f;
    public float projectileBuildUpRate = 1f;
    float projectileAppliedForce;
    bool buildingProjectileForce = false;
    public float buildForceLimitSeconds = 3f;
    float buildForceCounter;
    float buildForceCounterFinish;
    bool selfBlownMine = false; 
    [Header("Mines")]
    //managing mines
    public int allowedMines = 8;
    List<GameObject> activeMinesArray = new List<GameObject>();
    int currentActiveMineCount = 0; 
    //states
    bool canShoot = true;
    bool reloading = false;
    //public references
    public GameObject projectile;
    public Transform shootOrigin;
    public GameObject explosionFX;
    Animator anim;
    [Header("UI Referencess")]
    //UI
    public Text clipAmountText;
    public Text totalAmmoText;
    public Canvas minesUI;
    public Text activeMinesText;
    static int weaponIndex = 0;
    int requestedWeapon;
    private void Start() {
        projectileAppliedForce = projectileForce;
        anim = GetComponent<Animator>();
        clipAmount = clipSize;
        WeaponManager.ChangeWeapons(weaponIndex);
    }
    private void Update(){
        BlowMines();
        ShootMine();
        SwitchWeapon();   
    }
    void ShootMine(){
        if(clipAmount>0){
            if(buildingProjectileForce){
                buildForceCounter += Time.deltaTime;
                if(buildForceCounter <= buildForceCounterFinish){
                    projectileAppliedForce += projectileBuildUpRate * Time.deltaTime;
                }
                else{
                    Instantiate(explosionFX, shootOrigin.position, shootOrigin.rotation);
                    buildingProjectileForce = false;
                    clipAmount--;
                    clipAmountText.text = clipAmount.ToString();
                    projectileAppliedForce = projectileForce;
                    selfBlownMine = true;
                }
            }
            if(canShoot){
                if(Input.GetButtonDown("Fire1")){
                    buildingProjectileForce = true;
                    projectileAppliedForce = projectileForce;
                    buildForceCounter = Time.time;
                    buildForceCounterFinish = buildForceCounter + buildForceLimitSeconds;
                    selfBlownMine = false;
                    //clipAmount--;
                }
                else if(Input.GetButtonUp("Fire1") && !selfBlownMine){
                    anim.SetTrigger("Shoot");
                    canShoot = false;
                    buildingProjectileForce = false;
                }
            }
        }
        if(canShoot && clipAmount < clipSize){
            if(totalAmmo > 0 && !reloading){
                reloading = true;
                anim.SetTrigger("Reload");
            }
        }
    }
    void ShootMineAnimation(){
            //fire projectile
            GameObject currentProjectile = Instantiate(projectile, shootOrigin.position, shootOrigin.rotation);
            currentProjectile.GetComponent<StickyMine>().LaunchProjectile(projectileAppliedForce);      
            if(currentActiveMineCount == allowedMines - 1){
                //pop reference of first mine
                activeMinesArray[0].GetComponent<StickyMine>().Explode();
                activeMinesArray.RemoveAt(0);
                currentActiveMineCount--;
            }
            currentActiveMineCount++;
            activeMinesArray.Add(currentProjectile);
            projectileAppliedForce = projectileForce;
            clipAmount--;
            clipAmountText.text = clipAmount.ToString();
            activeMinesText.text = currentActiveMineCount.ToString();
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
    void BlowMines(){
        if(activeMinesArray.Count > 0){
            if(Input.GetButtonDown("Fire2")){
            int j = 0;
            for(int i = 0; i<activeMinesArray.Count; i++){
                if(activeMinesArray[i].GetComponent<StickyMine>().inSights == true){
                    activeMinesArray[i].GetComponent<StickyMine>().Explode();
                    activeMinesArray.RemoveAt(i--);
                    j++;
                }
            }
            currentActiveMineCount-=j;
            activeMinesText.text = currentActiveMineCount.ToString();
            }
        }
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
    private void OnEnable(){
        if(anim!=null)
            anim.SetTrigger("Swing In");
        clipAmountText.text = clipAmount.ToString();
        totalAmmoText.text = totalAmmo.ToString();

        minesUI.gameObject.SetActive(true);
        activeMinesText.text = currentActiveMineCount.ToString();
    }
    private void OnDisable(){
        if(minesUI!=null)
            minesUI.gameObject.SetActive(false);  
    }
}
