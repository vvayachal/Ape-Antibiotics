using UnityEngine;

/// <summary>
/// Interface <c>IKnockable</c> acts as the main entry point for anything that can be knocked back.
/// </summary>
public interface IKnockable
{
    /// <summary>
    /// Method <c>Knockback</c> will be in charge of all the logic necessary for each object to be knocked back.
    /// </summary>
    /// <param name="baseKnockbackForce">The force of the attack.</param>
    /// <param name="knockbackOrigin">The position of the attack.</param>
    void KnockBack(Vector3 knockbackOrigin, float baseKnockbackForce);

    /// <summary>
    /// Method <c>Recover</c> will be in charge of resetting the object to its original state to be knockable again.
    /// </summary>
    void Recover();
}
