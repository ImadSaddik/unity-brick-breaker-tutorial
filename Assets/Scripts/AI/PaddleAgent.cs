using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Ball ball;
    public float speed = 30f;
    public float maxBounceAngle = 75f;

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        ResetAgent();
        ball.ResetBall();
    }

    public void ResetAgent()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = new Vector2(0f, transform.position.y);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(rigidbody.velocity.x);

        sensor.AddObservation(ball.transform.position);
        sensor.AddObservation(ball.rigidbody.velocity);

        sensor.AddObservation(ball.transform.position - transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontalInput = actions.ContinuousActions[0];
        direction = new Vector2(horizontalInput, 0f);

        rigidbody.velocity = direction.normalized * speed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball myBall = collision.gameObject.GetComponent<Ball>();

        if (myBall != null)
        {
            Vector2 paddlePosition = transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float maxOffset = collision.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, myBall.rigidbody.velocity);
            float bounceAngle = (offset / maxOffset) * maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBounceAngle, maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            myBall.rigidbody.velocity = rotation * Vector2.up * myBall.rigidbody.velocity.magnitude;
        }
    }
}
