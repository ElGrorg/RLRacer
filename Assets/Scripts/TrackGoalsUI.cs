using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGoalsUI : MonoBehaviour
{
    [SerializeField] private TrackGoals trackGoals;

    private void Start() {
        trackGoals.OnCarCorrectGoal += TrackGoals_OnCarCorrectGoal;
        trackGoals.OnCarWrongGoal += TrackGoals_OnCarWrongGoal;

        Hide();
    }

    private void TrackGoals_OnCarWrongGoal(object sender, System.EventArgs e) {
        Show();
    }

    private void TrackGoals_OnCarCorrectGoal(object sender, System.EventArgs e) {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
