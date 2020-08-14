using System;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public static Action<float> OnDifficultySet;

    [SerializeField] private float difficulty;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    private void SetDifficulty()
    {
        OnDifficultySet?.Invoke(difficulty);
    }
}
