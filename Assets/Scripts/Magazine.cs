using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class Magazine : MonoBehaviour
{
    public int childIndexToActivate = 0;
    private bool isStarted = false;
    public float speed = 1f;
    private SplineAnimate splineAnimate;
    public bool isGunCollided = false;
    private TextMeshProUGUI text;

    void OnTriggerEnter(Collider collision)
    {
        // Check if the collided GameObject has the tag "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Activate the specified child
            if (childIndexToActivate < transform.childCount)
            {
                // Activate the specified child
                Transform childToActivate = transform.GetChild(childIndexToActivate);
                childToActivate.gameObject.SetActive(true);
                transform.Rotate(0, 0, 30);
                childIndexToActivate++;
            }
            // Increase the bullet count text
            GetComponentInChildren<TextMeshPro>().text = (float.Parse(GetComponentInChildren<TextMeshPro>().text) + 1).ToString();
        }
    }

    void Update()
    {
        // Check if the gun is collided or the activation of children is completed
        if (GameObject.Find("Gun").transform.position.z >= transform.position.z - 1)
        {
            isGunCollided = true;
        }
        if ((childIndexToActivate == 6 && !isStarted) || isGunCollided)
        {
            isStarted = true;
            splineAnimate = GetComponent<SplineAnimate>();
            splineAnimate.Play();
        }
    }
}
