using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSingle : MonoBehaviour
{
    private TrackGoals trackGoals;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<CarController>(out CarController car)) {
            trackGoals.CarThroughGoal(this, other.transform);
        }
    }

    public void SetTrackGoals(TrackGoals trackGoals) {
        this.trackGoals = trackGoals;
    }

}
