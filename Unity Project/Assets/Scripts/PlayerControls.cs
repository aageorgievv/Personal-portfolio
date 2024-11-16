using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    public Camera playerCamera;
    public aiControls aiControls;

    Vector3 moveDirection = Vector3.zero;


    private float rotationX = 0;
    private float rotationY = 0;
    private float rayCastDistance = 3f;

    public float movementSpeed = 5f;
    public float lookSpeed = 1f;
    public float lookXLimit = 5f;
    public float knockbackForce = 2f;


    public bool isMoving;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Look();
        PickUpItem();
        ResetScene();
    }

    private void Look()
    {
        if(Cursor.visible)
        {
            return;
        }

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Input.GetAxis("Mouse X") * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);
    }

    private void Move()
    {
        if(Cursor.visible)
        {
            return;
        }

        Vector3 moveDirection = Vector3.zero;

        if(Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if(Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if(Input.GetKey(KeyCode.D)) moveDirection += transform.right;
        if(Input.GetKey(KeyCode.A)) moveDirection -= transform.right;


        if(moveDirection.magnitude > 0.1f)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }

        Vector3 velocity = moveDirection.normalized * movementSpeed * Time.deltaTime;
        transform.position += velocity;
    }

    public void PickUpItem()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, rayCastDistance))
            {
                if(hit.transform.CompareTag("Apple"))
                {
                    StartCoroutine(PickUpAnimation(hit.transform));
                }

                if(hit.transform.CompareTag("Mushroom"))
                {
                    StartCoroutine(PickUpAnimation(hit.transform));
                }

                if(hit.transform.CompareTag("Anvil"))
                {
                    StartCoroutine(PickUpAnimation(hit.transform));
                    aiControls.IsFighting();
                }
            }
        }
    }

    private IEnumerator PickUpAnimation(Transform transform)
    {
        float duration = 0.5f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward;

        Collider[] colliders = transform.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }

        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Lerp scale and position over time
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);

            yield return null; // Wait for the next frame
        }

        Destroy(transform.gameObject);
    }

    public void ApplyKnockback(Vector3 direction)
    {
        Vector3 knockbackDirection = new Vector3(direction.x, direction.y / 2, -direction.y).normalized; // Remove Y component
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    private void ResetScene()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
