using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float pickupDistance = 3f;
    public float holdDistance = 2f;

    private Camera playerCamera;

    private bool isPickedUp = false;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (!isPickedUp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickup();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Place();
            }
        }
    }

    private void TryPickup()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Pickup();
            }
        }
    }
    private void Pickup()
    {
        isPickedUp = true;

        transform.SetParent(playerCamera.transform);

        transform.localPosition = new Vector3(
            0f,
            0f,
            holdDistance
        );

        transform.localRotation = Quaternion.identity;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void Place()
    {
        isPickedUp = false;

        transform.SetParent(null);

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}