using UnityEngine;

[ExecuteInEditMode]
public class Sun : LightBase
{
    public float MinimumLight = 0.0f;
    public float MaximumLight = 1.0f;

    public Transform FollowTarget;
    public Vector3 offset = new Vector3(0, 0, 40);

    protected override void Awake()
    {
        if (FollowTarget == null)
            FollowTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        SetColorInternally = false;

        base.Awake();
    }

    public override void TimeUpdated(GameTime time)
    {
        var timeClamped = time.TimeClamped;

        if (!this.Light2D) 
            return;

        this.Light2D.transform.rotation = Quaternion.Euler((timeClamped * 360f) - 35, 170, 0);

        this.Light2D.intensity = Tween.EaseInOutSine(this.MinimumLight, this.MaximumLight, timeClamped * 2);

        this.UpdateColor(timeClamped);

        if (this.FollowTarget)
        {
            var position = this.FollowTarget.position;
            this.transform.position = new Vector3(position.x + this.offset.x, position.y + this.offset.y, this.offset.z);
        }
    }

   
}
