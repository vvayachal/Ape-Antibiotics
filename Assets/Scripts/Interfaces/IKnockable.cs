using UnityEngine;

/// <summary>
/// Interface <c>IKnockable</c> acts as the main entry point for anything that can be knocked back.
/// </summary>
public interface IKnockable
{
    /// <summary>
    /// Method <c>Knockback</c> will be in charge of all the logic necessary for each object to be knocked back.
    /// </summary>
    void KnockBack(Vector3 knockbackOrigin);

    /// <summary>
    /// Method <c>Recover</c> will be in charge of resetting the object to its original state to be knockable again.
    /// </summary>
    void Recover();
}
