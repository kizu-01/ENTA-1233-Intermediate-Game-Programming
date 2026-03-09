using UnityEngine;

public interface ITargetProvider
{
    bool HasTarget {  get; }
    Transform GetTarget();
    Vector3 GetTargetPosition();
}
