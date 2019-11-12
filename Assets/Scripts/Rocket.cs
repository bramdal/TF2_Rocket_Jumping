using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Projectile Variables")]
    //projectile variables
    public float velocity;
    public float liveTime;
    [Header("Damage Stats")]
    //damagestats
    public int damage;
    public float explosionForce;
    public float blastRadius;
    [Header("Explosion FX")]
    //public references
    public GameObject explosionFX;

    Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocity;

        Invoke("Explode", liveTime);
    }
    private void Update() {
        //transform.position += -transform.right * Time.deltaTime * velocity;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 8){
            Explode();
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
        //Destroy(thisExplosionFX, 2f);  <- asset already has self destruct code
        Destroy(gameObject);
    }
}
