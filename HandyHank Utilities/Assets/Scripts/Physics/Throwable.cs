using UnityEngine;
using UnityEngine.Events;

public class Throwable : MonoBehaviour
{
    public UnityEvent OnGroundHit;

    public GameObject Hitmarker;

    public Transform TransformBody;

    public int MaxBounces = 3;

    public float DestroyDelay = 0.5f;

    public float Gravity { get; set; } = -150;

    [HideInInspector]
    public bool IsGrounded;

    [HideInInspector]
    public Vector2 Direction;

    [HideInInspector]
    public float Height;

    private float initialHeight;

    private int bounces;

    private void Update()
    {
        UpdatePosition();
        CheckGroundHit();
    }

    // Todo: This is working rather well but could use some additional input
    public void Initialize(Vector2 src, Vector2 dst, float? height = null, float? length = null)
    {
        var len = Vector2.Distance(src, dst);
        var scale = 8.823f;

        length ??= 2.0f;
        height ??= scale * len / length;

        length *= scale;

        var dir = (dst - src).normalized;

        this.IsGrounded = false;

        this.Direction     = (Vector2) (dir * length);
        this.Height        = (float) height;
        this.initialHeight = (float) height;
    }

    private void UpdatePosition()
    {
        if (!this.IsGrounded)
        {
            this.Height += this.Gravity * Time.deltaTime;
            this.TransformBody.position += new Vector3(0, this.Height, 0) * Time.deltaTime;
        }

        this.transform.position += this.Direction.ToVector3() * Time.deltaTime;
    }

    private void CheckGroundHit()
    {
        if (this.TransformBody.position.y < this.transform.position.y && !this.IsGrounded)
        {
            this.TransformBody.position = this.transform.position;
            this.IsGrounded = true;
            this.GroundHit();
        }
    }

    private void GroundHit()
    {
        this.OnGroundHit.Invoke();
    }

    public void Stick()
    {
        this.Direction = Vector2.zero;
    }

    public void InstantiateHitmarker()
    {
        Instantiate(this.Hitmarker, transform.position, Quaternion.identity);
    }

    public void Bounce(float divisionFactor)
    {
        this.bounces++;

        if (this.bounces >= this.MaxBounces)
        {
            DestroyObject();
            return;
        }

        Initialize(transform.position, this.Direction, this.initialHeight / divisionFactor);
    }

    public void SlowDownGroundVelocity(float divisionFactor)
    {
        this.Direction /= divisionFactor;
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject, this.DestroyDelay);
    }
}
