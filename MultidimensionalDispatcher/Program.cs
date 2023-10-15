using MultidimensionalDispatcher.Dispatchers;
using MultidimensionalDispatcher.ObjectStates;
using MultidimensionalDispatcher.Regions;
using MultidimensionalDispatcher.VersionStates;

namespace MultidimensionalDispatcher;

file static class Program
{
    public static void Main()
    {
        VersionState versionState = new AnnulVersionState();

        ObjectState objectState = new CreatedObjectState();
        
        Region region = new Premise(versionState, objectState);
        
        var regionDispatcher = new Dispatcher<string>(Do)
            .Premise.CreatedObjectState.WithAnnulVersionState(Do)
            .ConstructionRegion.DraftObjectState.WithActiveVersionState(Do);

        var objectStateDispatcher = region.Accept(regionDispatcher);

        var versionStateDispatcher = region.ObjectState.Accept(objectStateDispatcher);

        var result = region.VersionState.Accept(versionStateDispatcher);

        Console.WriteLine(result);
    }
    
    private static string Do(Region region, ObjectState objectState, VersionState versionState)
    {
        return "Обобщённый случай";
    }
    
    private static string Do(Premise region, CreatedObjectState objectState, AnnulVersionState versionState)
    {
        return "Тип региона: помещение; Статус объекта: создан; Статус версии: аннулирован;";
    }
    
    private static string Do(ConstructionRegion region, DraftObjectState objectState, ActiveVersionState versionState)
    {
        return "Тип региона: строительный регион; Статус объекта: черновик; Статус версии: действующая;";
    }
}