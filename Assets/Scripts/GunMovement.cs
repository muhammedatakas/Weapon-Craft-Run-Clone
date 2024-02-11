using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunMovement : MonoBehaviour
{
   public float moveSpeed = 0.1f; // Nesne hareket hızı
    public float maxX = 6f; // Sağa hareket sınırı
    public float minX = -4f; // Sola hareket sınırı

    private Vector3 touchStartPosition; // Dokunma başlangıç pozisyonu
    private Vector3 targetPosition; // Hedef pozisyon

    

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Dokunma başladığında
            if (touch.phase == TouchPhase.Began&&GameManager.isGameStarted)
            {
                touchStartPosition = touch.position;
                targetPosition = transform.position;
            }
            // Dokunma hareket ettiğinde
            else if (touch.phase == TouchPhase.Moved&&GameManager.isGameStarted)
            {
                // Dokunma hareket yönünü belirler
                float swipeDirection = touch.position.x - touchStartPosition.x;

                // Hedef pozisyonu güncelle (sınırlar içinde)
                targetPosition.x = Mathf.Clamp(targetPosition.x + swipeDirection * moveSpeed * Time.deltaTime, minX, maxX);

                // Nesneyi hedef pozisyona doğru hareket ettir
                transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
            }
        }
        if(GameManager.isGameStarted&&!GameManager.isLevelOver){transform.Translate(Vector3.up * Time.deltaTime * 5f);}
        
    }

    [System.Obsolete]
    void OnTriggerEnter(Collider collision)
    {
        TextMeshPro[] texts = collision.gameObject.GetComponentsInChildren<TextMeshPro>();
        string tempVariable = "";
        float tempValue = 0;

        if(collision.gameObject.CompareTag("BulletCollider")){
            if(collision.transform.GetChildCount()>=4){
                if(collision.gameObject.name=="Collider1"){
                    BulletSettings.Instance.gunYear+=3;
                }
                else if(collision.gameObject.name=="Collider2"){
                    BulletSettings.Instance.gunYear+=7;
                }
                else if(collision.gameObject.name=="Collider3"){
                    BulletSettings.Instance.gunYear+=12;
                }
            }
        }

        if (collision.gameObject.CompareTag("Panel"))
        {
        foreach (TextMeshPro text in texts)
            {
                if (text.name == "Variable")
                {
                    tempVariable = text.text.Trim();
                    Debug.Log(tempVariable);
                }
                else if (text.name == "Value")
                {
                    tempValue = float.Parse(text.text);
                }
            }

        if (tempVariable == "Fire Rate")
        {
            BulletSettings.Instance.IncreaseFireRate(tempValue);
            Debug.Log("Fire Rate Increased");
        }
        else if (tempVariable == "Bullet Range")
        {
            BulletSettings.Instance.IncreaseBulletRange(tempValue);
        }
        else if(tempVariable=="Year"){
            BulletSettings.Instance.IncreaseGunYear(tempValue);
        }
        }
        
        
        
        // Check if the collided GameObject has the tag "pANEL"
        if (collision.gameObject.CompareTag("Panel"))
        { 
             List<GameObject> taggedObjects = GameObject.FindGameObjectsWithTag("Panel").ToList();
            taggedObjects.Remove(collision.gameObject);

        // Iterate through each game object
        foreach (GameObject obj in taggedObjects)
        {
            if(Mathf.Approximately(collision.gameObject.transform.position.z, obj.transform.position.z))

            {
                obj.GetComponent<BoxCollider>().enabled = false;
                Destroy(collision.gameObject);
                break;
            }
            else Destroy(collision.gameObject);
        }
            
        }
        if(collision.gameObject.CompareTag("Obstacle")){
            Debug.Log("Obstacle");
            GameManager.Instance.LevelOver();
        }else if(collision.gameObject.CompareTag("Barrels")){
            Debug.Log("Barrels");
            GameManager.Instance.LevelOver();
        }
        
        
        
        
       
    }
    }


