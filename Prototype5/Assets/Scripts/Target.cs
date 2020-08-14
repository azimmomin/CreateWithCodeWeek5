using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static Action<int> OnTargedClicked;
    public static Action OnBadTargetCollidedWithSensor;

    [SerializeField] private Vector2 targetForceRange = Vector2.zero;
    [SerializeField] private Vector3 torqueRanges = Vector3.zero;
    [SerializeField] private float positionRangeX = 4f;
    [SerializeField] private float yPosition = -2f;
    [SerializeField] private int pointValue = 5;
    [SerializeField] private ParticleSystem explosionParticle = null;

    private Rigidbody targetRigidbody;

    private void Start()
    {
        targetRigidbody = GetComponent<Rigidbody>();

        targetRigidbody.AddForce(
            GetRandomUpForce(),
            ForceMode.Impulse);

        targetRigidbody.AddTorque(
            GetRandomTorque(torqueRanges.x),
            GetRandomTorque(torqueRanges.y),
            GetRandomTorque(torqueRanges.z),
            ForceMode.Impulse);

        transform.position = GetRandomSpawnPosition();
    }

    private Vector3 GetRandomUpForce() =>
        Vector3.up * UnityEngine.Random.Range(targetForceRange.x, targetForceRange.y);

    private float GetRandomTorque(float torqueRange) =>
        UnityEngine.Random.Range(-torqueRange, torqueRange);

    private Vector2 GetRandomSpawnPosition() => new Vector2(
        UnityEngine.Random.Range(-positionRangeX, positionRangeX),
        yPosition);

    private void OnMouseDown()
    {
        if (GameManager.IsGameActive)
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            Destroy(gameObject);
            OnTargedClicked?.Invoke(pointValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor"))
        {
            Destroy(gameObject);

            if (gameObject.CompareTag("BadTarget"))
            {
                OnBadTargetCollidedWithSensor?.Invoke();
            }
        }
    }
}
