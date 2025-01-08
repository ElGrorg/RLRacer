using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent {

    //Change for carController
    [SerializeField] private TrackGoals trackGoals;

    private CarController carController;

    private void Awake() {
        carController = GetComponent<CarController>();
    }

    private void Start() {
        trackGoals.OnCarCorrectGoal += TrackGoals_OnCarCorrectGoal;
        trackGoals.OnCarWrongGoal += TrackGoals_OnCarWrongGoal;
    }

    private void TrackGoals_OnCarWrongGoal(object sender, TrackGoals.CarGoalEventArgs e) {
        if (e.carTransform == transform) {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void TrackGoals_OnCarCorrectGoal(object sender, TrackGoals.CarGoalEventArgs e) {
        if (e.carTransform == transform) {
            AddReward(1f);
        }
    }

    public override void OnEpisodeBegin() {
        transform.rotation = Quaternion.identity;
        transform.localPosition = new Vector2(Random.Range(-6.21f, -5.13f), Random.Range(-6.38f, -4.26f));
        carController.ResetCar();
        trackGoals.ResetGoals(transform);
    }

    public override void CollectObservations(VectorSensor sensor) {
        Vector2 goalForward = trackGoals.GetNextGoal(transform).transform.forward;

        float directionDot = Vector2.Dot(transform.forward, goalForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0]) {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; break;
        }
        switch (actions.DiscreteActions[1]) {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
        Vector2 inputVector = new Vector2(forwardAmount, turnAmount);
        carController.SetInputVector(inputVector);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int forwardAction = 0;
        if (Input.GetKey("up")) forwardAction = 1;
        if (Input.GetKey("down")) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey("right")) turnAction = 1;
        if (Input.GetKey("left")) turnAction = 2;

        ActionSegment<int> DiscreteActions = actionsOut.DiscreteActions;
        DiscreteActions[1] = forwardAction;
        DiscreteActions[0] = turnAction;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall)) {
            AddReward(-1f);
            // EndEpisode();
        }
    }
}
