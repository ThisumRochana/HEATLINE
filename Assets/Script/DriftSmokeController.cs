using UnityEngine;

public class DriftSmokeController : MonoBehaviour
{
    public ParticleSystem leftSmoke;
    public ParticleSystem rightSmoke;

    public float driftThreshold = 0.5f;
    public float minSpeed = 5f;

    CarControl car;

    void Start()
    {
        car = GetComponent<CarControl>();
    }

    void Update()
    {
        /*if (car == null) return;
        if (leftSmoke == null || rightSmoke == null) return;

        float speed = car.GetSpeed();
        float steer = Mathf.Abs(car.GetSteerInput());

        bool isDrifting = steer > driftThreshold && speed > minSpeed;

        if (isDrifting)
        {
            if (!leftSmoke.isPlaying) leftSmoke.Play();
            if (!rightSmoke.isPlaying) rightSmoke.Play();
        }
        else
        {
            if (leftSmoke.isPlaying) leftSmoke.Stop();
            if (rightSmoke.isPlaying) rightSmoke.Stop();
        }*/
    }

    void OnDisable()
    {
        if (leftSmoke) leftSmoke.Stop();
        if (rightSmoke) rightSmoke.Stop();
    }
}
