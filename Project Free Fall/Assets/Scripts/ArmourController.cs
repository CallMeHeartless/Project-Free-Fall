using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourController : MonoBehaviour {
    [SerializeField]
    private float selfDestructTimer = 5.0f;

    public void StartSelfDestructTimer() {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(selfDestructTimer);
        Destroy(gameObject);
    }
}
