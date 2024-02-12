using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundPooling groundPooling;

    private void Start()
    {
        groundPooling = GameObject.FindObjectOfType<GroundPooling>();
    }

    private void OnTriggerExit(Collider other)
    {
        groundPooling.GetPooledObject();
        gameObject.SetActive(false);
    }
}
