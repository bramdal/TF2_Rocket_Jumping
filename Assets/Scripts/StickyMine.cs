using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyMine : MonoBehaviour
{
    //projectile stats
    [HideInInspector]public float projectileForce;
    [Header("Damage Stats")]
    //damage stats
    public int damage = 20;
    public float explosionForce;
    public float blastRadius;
    //states
    [HideInInspector]public bool isFlying;
    [HideInInspector]public bool inSights;
    bool leftSight = false;
    [Header("Public References")]
    //public references
    public Material inSightsMaterial;
    Material regularMaterial;
    public GameObject explosionFX;
    Rigidbody rb; 
    private void Start() {
        regularMaterial = GetComponent<MeshRenderer>().material;
        
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
    }
    public void LaunchProjectile(float projectileForceFromParent){
        projectileForce = projectileForceFromParent;
        isFlying = true;
    }
    private void Update(){
        Vector3 viewportCoordinate = Camera.main.WorldToViewportPoint(transform.position);
        if(viewportCoordinate.x>0.25f && viewportCoordinate.x < 0.75f && viewportCoordinate.z >0){
            inSights = true;
        }       
        else{
            inSights = false;
        }

        if(inSights && !leftSight){
            GetComponent<MeshRenderer>().material = inSightsMaterial;
            leftSight = true;
        }
        else if(!inSights && leftSight){
            GetComponent<MeshRenderer>().material = regularMaterial; 
            leftSight = false;
        }         
    }
    public void Explode(){
        Collider[] players = Physics.OverlapSphere(transform.position, blastRadius);
        for(int i = 0; i < players.Length; i++){
            if(players[i].tag == "Player"){
                PlayerMovement playerMovement = players[i].gameObject.GetComponent<PlayerMovement>();
                PlayerManager playerManager = players[i].gameObject.GetComponent<PlayerManager>();
                if(playerMovement != null && playerManager != null){
                    playerMovement.AddImpact(explosionForce, transform.position - players[i].transform.position);
                    playerManager.doDamage(damage);
                }
            }
        }
        Instantiate(explosionFX, transform.position, transform.rotation);
        //Destroy(thisExplosionFX, 2f);  <- asset already has self destruct code, no need to keep reference
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 8){
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            rb.useGravity = false;
            isFlying = false;
        }
    } 
}
