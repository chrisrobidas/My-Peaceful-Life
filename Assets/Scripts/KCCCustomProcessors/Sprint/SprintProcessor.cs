using UnityEngine;
using Fusion.Addons.KCC;

/// <summary>
/// Sprint processor - multiplying kinematic speed based on Sprint property.
/// </summary>
public sealed class SprintProcessor : KCCProcessor, ISetKinematicSpeed
{
    // PRIVATE MEMBERS

    [SerializeField]
    private float _kinematicSpeedMultiplier = 1.5f;

    // KCCProcessor INTERFACE

    public override float GetPriority(KCC kcc) => _kinematicSpeedMultiplier;

    // ISetKinematicSpeed INTERFACE

    public void Execute(ISetKinematicSpeed stage, KCC kcc, KCCData data)
    {
        // Apply the multiplier only if the Sprint property is set.
        if (data.IsSprinting == true)
        {
            data.KinematicSpeed *= _kinematicSpeedMultiplier;

            // Suppress other sprint processors with lower priority.
            kcc.SuppressProcessors<SprintProcessor>();
        }
    }
}
