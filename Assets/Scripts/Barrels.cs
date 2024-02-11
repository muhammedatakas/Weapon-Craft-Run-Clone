using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Barrels : MonoBehaviour
{
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetComponentInChildren<TextMeshPro>().text=(float.Parse(GetComponentInChildren<TextMeshPro>().text)-BulletSettings.Instance.bulletDamage).ToString();
            
            if (float.Parse(GetComponentInChildren<TextMeshPro>().text) <= 0)
            {
                GameManager.Instance.IncreaseMoney(50);
                Destroy(gameObject);
            }
             collision.gameObject.SetActive(false);
        }
        
    }
}
