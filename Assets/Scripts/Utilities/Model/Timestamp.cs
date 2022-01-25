using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestamp: MonoBehaviour
{
    [SerializeField] private int spawnIndex = 0;
    [SerializeField] private int inputIndex = 0;
    [SerializeField] private int barIndex = 0;

    public int GetSpawnIndex() { return this.spawnIndex; }
    public void SetSpawnIndex(int spawnIndex) { this.spawnIndex = spawnIndex; }
    public int GetInputIndex() { return this.inputIndex; }
    public void SetInputIndex(int inputIndex) { this.inputIndex = inputIndex; }
    public int GetBarIndex() { return this.barIndex; }
    public void SetBarIndex(int barIndex) { this.barIndex = barIndex; }
}
