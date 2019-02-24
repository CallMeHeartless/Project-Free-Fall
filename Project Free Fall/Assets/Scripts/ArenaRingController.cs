using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRingController : MonoBehaviour
{
    [SerializeField]
    private float fallRate = 1.0f;
    private bool isDropping = false;

    // Start is called before the first frame update
    void Start(){

       

    }

    // Update is called once per frame
    void Update(){
        if (isDropping) {
            transform.Translate(Vector3.down * fallRate * Time.deltaTime);
        }
    }

    public void DropRing() {
        isDropping = true;
    }
}
