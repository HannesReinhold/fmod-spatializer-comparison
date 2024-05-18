using UnityEngine;

public class ixRobotArmController : MonoBehaviour
{
    public enum State
    {
        Sleeping,
        Idle,
        Seeking,
        Holding
    }

    public int id;
    public Transform IKEnd;
    public Transform IKTarget; // IK solver positions IKEnd at IKTarget.
    public Transform TargetObject;
    public Transform GroundTarget;
    public Transform IdleTarget;
    public float timeOut;

    public GameObject HeldObject;
    [SerializeField] private AudioSource audioSource;

    public State state = State.Idle;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    private void Update()
    {
        if (timeOut > 0.0f)
        {
            timeOut -= Time.deltaTime;
            if (timeOut <= 0.0f)
                state = State.Idle;
        }

        if (audioSource == null) return;

        // Play audio when in Holding state
        if (state == State.Holding && !audioSource.isPlaying)
            audioSource.Play();
        else if
            (state != State.Holding &&
             audioSource.isPlaying) audioSource.Stop(); // Stop audio when not in Holding state
    }

    public void UpdateIKTarget(float speedRotation, float positionSmoothTime, float maxSpeed, float deltaTime)
    {
        IKTarget.rotation =
            Quaternion.RotateTowards(IKTarget.rotation, TargetObject.rotation, speedRotation * deltaTime);
        IKTarget.position = Vector3.SmoothDamp(IKTarget.position, TargetObject.position, ref velocity,
            positionSmoothTime, maxSpeed, deltaTime);
    }

    public void OnGrab(GameObject collidingObject)
    {
        if (state == State.Sleeping)
            return;

        ixRobotMasterController.Get().OnGrab(this, collidingObject);
    }

    public void OnRelease(GameObject collidingObject)
    {
//		HeldObject = null;
//		timeOut = 1.0f;
    }
}