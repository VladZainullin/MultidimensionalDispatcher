using MultidimensionalDispatcher.VersionStates;

namespace MultidimensionalDispatcher.Interfaces;

public interface IRegionVersionStateVisitor<out T>
{
    T Visit(AnnulVersionState versionState);

    T Visit(ActiveVersionState versionState);
}