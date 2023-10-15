using MultidimensionalDispatcher.Interfaces;
using MultidimensionalDispatcher.ObjectStates;
using MultidimensionalDispatcher.VersionStates;

namespace MultidimensionalDispatcher.Regions;

public sealed class Compartment : Region
{
    public Compartment(VersionState versionState, ObjectState objectState) : base(versionState, objectState)
    {
    }

    public override T Accept<T>(IRegionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}