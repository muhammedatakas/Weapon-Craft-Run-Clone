using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panels : MonoBehaviour
{
    private float lastValue;

    void Start()
    {
        // Ensure that the array contains at least 3 elements
        TextMeshPro[] textMeshes = GetComponentsInChildren<TextMeshPro>();
        if (textMeshes.Length >= 3)
        {
            // Parse the last text value and store it
            lastValue = float.Parse(textMeshes[2].text);
        }
        else
        {
            Debug.LogError("There are not enough TextMeshProUGUI components in the children!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Increase the last value by the value of the second text component
            lastValue += float.Parse(GetComponentsInChildren<TextMeshPro>()[1].text);
            // Update the third text component with the new value
            GetComponentsInChildren<TextMeshPro>()[2].text = lastValue.ToString();

            // Change the color based on the value
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (lastValue > 0)
                {
                    renderer.material.color = Color.green;
                }
                else if (lastValue < 0)
                {
                    renderer.material.color = Color.red;
                }
                else
                {
                    renderer.material.color = Color.white;
                }
            }
            else
            {
                Debug.LogError("Renderer component not found!");
            }

            // Deactivate the collided bullet
            other.gameObject.SetActive(false);
        }
    }
}
