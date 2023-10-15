using MultidimensionalDispatcher.Interfaces;

namespace MultidimensionalDispatcher.ObjectStates;

public sealed class CreatedObjectState : ObjectState
{
    public override T Accept<T>(IObjectStateVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}