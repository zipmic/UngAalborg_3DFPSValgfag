using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public enum MovementDirection
    {
        Up,
        Down,
        Forward,
        Backward,
        Right,
        Left
    }

    [Header("Movement Settings")]
    public MovementDirection direction = MovementDirection.Forward; // Retningen for bevægelsen
    public float speed = 1f; // Hastighed for bevægelsen
    public float distance = 5f; // Hvor langt objektet bevæger sig
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Bevægelsens kurve

    private Vector3 startPosition; // Objektets udgangspunkt
    private Vector3 targetPosition; // Målet for bevægelsen
    private float timer; // Tæller for bevægelsen

    void Start()
    {
        // Gem startpositionen og beregn målpositionen
        startPosition = transform.position;
        targetPosition = startPosition + GetLocalDirectionVector() * distance;
    }

    void Update()
    {
        // Opdater timeren med tid og hastighed
        timer += Time.deltaTime * speed;

        // Løb timeren mellem 0 og 1 for at bevæge frem og tilbage
        float curveValue = movementCurve.Evaluate(Mathf.PingPong(timer, 1f));

        // Bevæg objektet frem og tilbage mellem start- og målpositionen
        transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
    }

    Vector3 GetLocalDirectionVector()
    {
        // Returnér enhedsvektoren baseret på den valgte retning (lokal retning)
        switch (direction)
        {
            case MovementDirection.Up: return transform.up;
            case MovementDirection.Down: return -transform.up;
            case MovementDirection.Forward: return transform.forward;
            case MovementDirection.Backward: return -transform.forward;
            case MovementDirection.Right: return transform.right;
            case MovementDirection.Left: return -transform.right;
            default: return transform.forward;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Beregn målposition baseret på lokal retning og distance
        Vector3 directionVector = GetLocalDirectionVector();
        Vector3 target = transform.position + directionVector * distance;

        // Tegn en linje fra start- til målposition
        Gizmos.DrawLine(transform.position, target);

        // Tegn kugler ved start- og målpositionen
        Gizmos.DrawSphere(transform.position, 0.2f); // Startposition
        Gizmos.DrawSphere(target, 0.2f);             // Målposition
    }
}
