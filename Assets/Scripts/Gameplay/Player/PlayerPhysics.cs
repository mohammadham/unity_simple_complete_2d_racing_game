using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplySlip(float slipCoefficient)
    {
        // Example of applying slip physics
        rb.drag = slipCoefficient;
    }
}
