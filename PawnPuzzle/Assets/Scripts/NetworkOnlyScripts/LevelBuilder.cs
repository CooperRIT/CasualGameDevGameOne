using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
struct Levels
{
    public int levelID;
    public GameObject levelPrefab;
}
public class LevelBuilder : NetworkBehaviour
{
    //Later
    //[SerializeField] List<Levels> levels = new List<Levels>();

    [SerializeField] GameObject levelObject;

    [ClientRpc]
    public void LoadLevelClientRpc(int levelIndex)
    {
        levelObject.transform.GetChild(levelIndex).gameObject.SetActive(true);
    }

    //Could combine these, not worth it
    [ClientRpc]
    public void UnloadPreviousLevelClientRpc(int previousLevelIndex)
    {
        levelObject.transform.GetChild(previousLevelIndex).gameObject.SetActive(false);
    }
}
