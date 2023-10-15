using MultidimensionalDispatcher.Interfaces;

namespace MultidimensionalDispatcher.VersionStates;

public abstract class VersionState
{
    public abstract T Accept<T>(IRegionVersionStateVisitor<T> visitor);
}