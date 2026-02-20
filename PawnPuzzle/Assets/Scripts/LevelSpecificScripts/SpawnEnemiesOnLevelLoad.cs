using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesOnLevelLoad : MonoBehaviour
{
    [SerializeField] GameObject enemiesPrefab;
    [SerializeField] Transform parentTransform;
    private void OnEnable()
    {
        Instantiate(enemiesPrefab, parentTransform);
    }
}
