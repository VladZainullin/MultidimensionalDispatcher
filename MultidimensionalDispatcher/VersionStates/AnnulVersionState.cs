using MultidimensionalDispatcher.Interfaces;

namespace MultidimensionalDispatcher.VersionStates;

public sealed class AnnulVersionState : VersionState
{
    public override T Accept<T>(IRegionVersionStateVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}