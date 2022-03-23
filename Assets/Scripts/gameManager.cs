using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public PlayerController pCont;
    public bool isWalking;
    public static gameManager instance;
    public GameObject soldierBag;
    public Player p1;
    public GameObject upgradeButton;
    public GameObject FriendlybulletParent;
    public ParticleSystem particle;
    public GameObject enemyBulletParent;
    public SphereCollider pCollider;
    public HealthBar healthBar;
  
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance==null || instance == this)
        {
            instance = this;
        } else
        {
            DestroyImmediate(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
