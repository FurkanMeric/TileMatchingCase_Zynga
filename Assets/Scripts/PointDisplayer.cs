using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PointDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro pointDisplay;
    void Start()
    {
        GameManager.OnPointChanged += UpdatePoint;
    }

    private void UpdatePoint(int point)
    {
        pointDisplay.text = point + " pts";
    }
}
