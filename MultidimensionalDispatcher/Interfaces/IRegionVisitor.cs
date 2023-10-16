using MultidimensionalDispatcher.Regions;

namespace MultidimensionalDispatcher.Interfaces;

public interface IRegionVisitor<out T>
{
    T Visit(Premise premise);

    T Visit(Compartment compartment);

    T Visit(ConstructionRegion constructionRegion);

    T Visit(MountingRegion mountingRegion);
}