using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletHolder : MonoBehaviour
{GameObject[] magazines;
int lastChildCount=1;
int count=0;
    // Start is called before the first frame update
    void Start()
{
    magazines = GameObject.FindGameObjectsWithTag("Magazine");
    if (magazines == null || magazines.Length == 0)
    {
        Debug.LogError("No objects found with tag 'Magazine'");
    }
}

    void Update(){
    magazines = GameObject.FindGameObjectsWithTag("Magazine");
    foreach (GameObject magazine in magazines)
    {
        if(magazine.transform.position.z>=transform.position.z){
            for (int i = lastChildCount; i < magazine.transform.childCount; i++)
            {
                Transform bulletTransform = magazine.transform.GetChild(i); // Çocuk nesnenin transformu
                if (bulletTransform.gameObject.activeSelf)
                {
                    transform.GetChild(count).gameObject.SetActive(true);
                    count++;
                }
            }
            
            Destroy(magazine);
            lastChildCount= magazine.transform.childCount;
        }
    }


}}
