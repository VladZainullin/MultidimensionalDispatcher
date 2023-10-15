using MultidimensionalDispatcher.Interfaces;

namespace MultidimensionalDispatcher.ObjectStates;

public abstract class ObjectState
{
    public abstract T Accept<T>(IObjectStateVisitor<T> visitor);
}