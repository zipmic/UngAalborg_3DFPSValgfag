using UnityEngine;

public class LiftObject : MonoBehaviour
{
    public enum HoldPositionMode { Center, HitPoint } // Enum til at vælge hold-position.
    public HoldPositionMode holdMode = HoldPositionMode.Center;

    public Camera playerCamera; // Kameraet, der bruges til FPS-sigtet.
    public float maxDistance = 5f; // Maksimal rækkevidde for Raycast.
    public float liftSpeed = 10f; // Hvor hurtigt objektet følger med.
    public float throwForce = 10f; // Styrke, hvormed objektet kastes.
    public Transform holdPosition; // Position, hvor objektet skal holdes foran spilleren.

    private GameObject liftedObject = null; // Det objekt, spilleren løfter.
    private Rigidbody liftedRigidbody = null; // Rigidbody for det løftede objekt.
    private CollisionDetectionMode originalCollisionMode; // Gemmer den originale Collision Mode.
    private Vector3 holdOffset; // Offset for objektet, hvis holdMode er HitPoint.

    private bool isHoldingObject = false; // Holder styr på, om der løftes et objekt.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (liftedObject == null)
            {
                TryLiftObject();
            }
            else
            {
                DropObject();
            }
        }

        if (isHoldingObject && Input.GetMouseButtonDown(0)) // Venstre klik for at kaste.
        {
            ThrowObject();
        }
    }

    void FixedUpdate()
    {
        if (isHoldingObject)
        {
            MoveObject();
        }
    }

    void TryLiftObject()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.CompareTag("ObjectToLift"))
            {
                liftedObject = hit.collider.gameObject;
                liftedRigidbody = liftedObject.GetComponent<Rigidbody>();

                if (liftedRigidbody != null)
                {
                    // Gem den oprindelige Collision Mode.
                    originalCollisionMode = liftedRigidbody.collisionDetectionMode;

                    // Skift til Continuous Collision.
                    liftedRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    liftedRigidbody.useGravity = false; // Deaktiver tyngdekraft.
                    liftedRigidbody.freezeRotation = true; // Forhindre utilsigtet rotation.
                    isHoldingObject = true; // Marker, at et objekt løftes.

                    // Sæt holdOffset baseret på holdMode.
                    holdOffset = holdMode == HoldPositionMode.HitPoint
                        ? hit.point - liftedObject.transform.position
                        : Vector3.zero;
                }
            }
        }
    }

    void MoveObject()
    {
        if (liftedObject != null)
        {
            Vector3 targetPosition = holdPosition.position - holdOffset;
            Vector3 newPosition = Vector3.Lerp(liftedObject.transform.position, targetPosition, Time.fixedDeltaTime * liftSpeed);
            liftedRigidbody.MovePosition(newPosition);
        }
    }

    void DropObject()
    {
        if (liftedRigidbody != null)
        {
            // Gendan den oprindelige Collision Mode.
            liftedRigidbody.collisionDetectionMode = originalCollisionMode;
            liftedRigidbody.useGravity = true; // Genaktiver tyngdekraft.
            liftedRigidbody.freezeRotation = false;
        }

        liftedObject = null;
        liftedRigidbody = null;
        isHoldingObject = false; // Marker, at objektet ikke længere løftes.
    }

    void ThrowObject()
    {
        if (liftedRigidbody != null)
        {
            // Kast objektet fremad.
            liftedRigidbody.velocity = playerCamera.transform.forward * throwForce;

            // Drop objektet efter kast.
            DropObject();
        }
    }
}
