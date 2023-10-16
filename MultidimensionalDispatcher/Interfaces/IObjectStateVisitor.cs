using MultidimensionalDispatcher.ObjectStates;

namespace MultidimensionalDispatcher.Interfaces;

public interface IObjectStateVisitor<out T>
{
    T Visit(CreatedObjectState createdObjectState);
    T Visit(DraftObjectState draftObjectState);
}