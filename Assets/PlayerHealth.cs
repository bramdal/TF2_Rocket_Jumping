using UnityEngine;

public class PlayerHealth
{
    public int maxHealth = 150;
    int currentHealth;
    public bool dead;
    public PlayerHealth(){
        currentHealth = maxHealth;
        dead = false;
    }
    public void AddDamage(int damage){
        currentHealth -= damage;
        if(currentHealth<=0){
            currentHealth = 0;
            dead = true;
        }    
    }
    public void AddHealth(int health){
        currentHealth += health;
    }
    public int GetHealth(){
        return currentHealth;
    }
}
