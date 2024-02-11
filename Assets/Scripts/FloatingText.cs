using UnityEngine;

public class FloatingText  : MonoBehaviour
{
    Transform mainCam;
    Transform unit;
    Transform worldSpaceCanvas;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceCanvas = GameObject.Find("Canvas1").transform;

        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
   void Update()
{
    if (unit != null && mainCam != null)
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position); //look at the camera
        transform.position = unit.position + offset;
    }
    else
    {
        Debug.Log("GameObject has been destroyed");
    }
}

}
