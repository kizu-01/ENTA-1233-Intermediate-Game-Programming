using UnityEngine;

public interface IWeapon
{
    bool CanFire { get; }
    void Fire(Vector3 targetPosition);
    void Fire(Vector3 direction, bool useDirection);
}
