using UnityEngine;
using System.Collections;

public interface IThrowable {

    void Update(GameObject carrier);
    void GetThrown(Vector3 direction);

}