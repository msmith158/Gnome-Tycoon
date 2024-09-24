using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void MoveObject(float transformX)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + transformX,
            gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
