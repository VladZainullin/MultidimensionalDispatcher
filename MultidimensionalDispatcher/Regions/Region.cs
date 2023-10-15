using MultidimensionalDispatcher.Interfaces;
using MultidimensionalDispatcher.ObjectStates;
using MultidimensionalDispatcher.VersionStates;

namespace MultidimensionalDispatcher.Regions;

public abstract class Region
{
    protected Region(VersionState versionState, ObjectState objectState)
    {
        VersionState = versionState;
        ObjectState = objectState;
    }

    public VersionState VersionState { get; }

    public ObjectState ObjectState { get; }

    public abstract T Accept<T>(IRegionVisitor<T> visitor);
}