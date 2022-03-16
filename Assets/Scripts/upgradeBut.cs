using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeBut : MonoBehaviour
{
    public Player player;
    public static upgradeBut Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Upgrade()
    {
        player.soldierLimit= player.soldierLimit + 10;
        // Size için Level koyup, her levelde feda edilecek asker sayýsýnýn artýrýlmasý saðlanabilir...!
        
        for (int i = 0; i < 5; i++)
        {
            player.fsoldierList.RemoveAt(i);
            player.fsoldierList[i].gameObject.SetActive(false);
            Destroy(player.fsoldierList[i].gameObject);
            // ObjectPool ile spawn yapýlmasý durumunda destroy yerine SetActive(false)...!
        }
        this.gameObject.SetActive(false);
    }
}
