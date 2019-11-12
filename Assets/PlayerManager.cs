using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    PlayerHealth playerHealth= new PlayerHealth();
    public Text healthText;
    public Slider healthSlider;

    public CinemachineVirtualCamera spectatorCam;
    public GameObject playerPrefab;
    GameObject player;
    public Transform spawnPoint;

    private void Start() {
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.GetHealth();
        healthText.text = (playerHealth.GetHealth()).ToString();
    }
    public void doDamage(int damage){
        playerHealth.AddDamage(damage);
        healthSlider.value = playerHealth.GetHealth();
        healthText.text = (playerHealth.GetHealth()).ToString();
        if(playerHealth.dead){
            Destroy(gameObject);
        }
    }

    public void Heal(int health){
        playerHealth.AddHealth(health);
        healthSlider.value = playerHealth.GetHealth();
        healthText.text = (playerHealth.GetHealth()).ToString();
    }
}
