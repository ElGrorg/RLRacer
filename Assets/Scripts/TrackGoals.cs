using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGoals : MonoBehaviour
{

    public class CarGoalEventArgs : EventArgs {
        public Transform carTransform;
        
        public CarGoalEventArgs(Transform carTransform) {
            this.carTransform = carTransform;
        }
    }

    public event EventHandler<CarGoalEventArgs> OnCarCorrectGoal;
    public event EventHandler<CarGoalEventArgs> OnCarWrongGoal;

    [SerializeField] private List<Transform> carTransformList;
    private List<GoalSingle> goalSingleList;
    private List<int> nextGoalSingleIndexList;
    private int lastGoalIndex;

    private void Awake() {
        Transform goalsTransform = transform.Find("Goals");

        goalSingleList = new List<GoalSingle>();
        foreach (Transform goalSingleTransform in goalsTransform) {
            GoalSingle goalSingle = goalSingleTransform.GetComponent<GoalSingle>();
            goalSingle.SetTrackGoals(this);
            goalSingleList.Add(goalSingle);
        }

        nextGoalSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList) {
            nextGoalSingleIndexList.Add(0);
        }
    }

    public void CarThroughGoal(GoalSingle goalSingle, Transform carTransform) {
        int nextGoalSingleIndex = nextGoalSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (goalSingleList.IndexOf(goalSingle) == nextGoalSingleIndex) {
            lastGoalIndex = nextGoalSingleIndex;
            nextGoalSingleIndexList[carTransformList.IndexOf(carTransform)]
            = (nextGoalSingleIndex + 1) % goalSingleList.Count;
            OnCarCorrectGoal?.Invoke(this, new CarGoalEventArgs(carTransform));
        } else {
            OnCarWrongGoal?.Invoke(this, new CarGoalEventArgs(carTransform));
        }
    }

    public GoalSingle GetNextGoal(Transform carTransform)
    {
        int carIndex = carTransformList.IndexOf(carTransform);
        int nextGoalSingleIndex = nextGoalSingleIndexList[carIndex];
        return goalSingleList[nextGoalSingleIndex];
    }

    public void ResetGoals(Transform carTransform)
    {
        int carIndex = carTransformList.IndexOf(carTransform);
        nextGoalSingleIndexList[carIndex] = 0;
    }
}
