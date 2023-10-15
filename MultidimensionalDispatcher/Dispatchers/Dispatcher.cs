using MultidimensionalDispatcher.Interfaces;
using MultidimensionalDispatcher.ObjectStates;
using MultidimensionalDispatcher.Regions;
using MultidimensionalDispatcher.VersionStates;

namespace MultidimensionalDispatcher.Dispatchers;

public sealed class Dispatcher<TResult> : IRegionVisitor<IObjectStateVisitor<IRegionVersionStateVisitor<TResult>>>
{
    public Dispatcher(Func<Region, ObjectState, VersionState, TResult> generalCase)
    {
        Premise = new Dispatcher<Premise, TResult>(this, generalCase);
        Compartment = new Dispatcher<Compartment, TResult>(this, generalCase);
        ConstructionRegion = new Dispatcher<ConstructionRegion, TResult>(this, generalCase);
        MountingRegion = new Dispatcher<MountingRegion, TResult>(this, generalCase);
    }

    public Dispatcher<Premise, TResult> Premise { get; }
    
    public Dispatcher<Compartment, TResult> Compartment { get; }
    
    public Dispatcher<ConstructionRegion, TResult> ConstructionRegion { get; }
    
    public Dispatcher<MountingRegion, TResult> MountingRegion { get; }
    public IObjectStateVisitor<IRegionVersionStateVisitor<TResult>> Visit(Premise premise)
    {
        return Premise.Take(premise);
    }

    public IObjectStateVisitor<IRegionVersionStateVisitor<TResult>> Visit(Compartment compartment)
    {
        return Compartment.Take(compartment);
    }

    public IObjectStateVisitor<IRegionVersionStateVisitor<TResult>> Visit(ConstructionRegion constructionRegion)
    {
        return ConstructionRegion.Take(constructionRegion);
    }

    public IObjectStateVisitor<IRegionVersionStateVisitor<TResult>> Visit(MountingRegion mountingRegion)
    {
        return MountingRegion.Take(mountingRegion);
    }
}

public sealed class Dispatcher<TRegion, TResult> : IObjectStateVisitor<IRegionVersionStateVisitor<TResult>>
    where TRegion : Region
{
    public Dispatcher(
        Dispatcher<TResult> dispatcher,
        Func<Region, ObjectState, VersionState, TResult> generalCase)
    {
        CreatedObjectState = new Dispatcher<TRegion, CreatedObjectState, TResult>(dispatcher, this, generalCase);
        DraftObjectState = new Dispatcher<TRegion, DraftObjectState, TResult>(dispatcher, this, generalCase);
    }
    
    public TRegion Target { get; private set; } = default!;

    public Dispatcher<TRegion, CreatedObjectState, TResult> CreatedObjectState { get; }
    
    public Dispatcher<TRegion, DraftObjectState, TResult> DraftObjectState { get; }
    
    public Dispatcher<TRegion, TResult> Take(TRegion region)
    {
        Target = region;
        return this;
    }

    public IRegionVersionStateVisitor<TResult> Visit(CreatedObjectState createdObjectState)
    {
        return CreatedObjectState.Take(createdObjectState);
    }

    public IRegionVersionStateVisitor<TResult> Visit(DraftObjectState draftObjectState)
    {
        return DraftObjectState.Take(draftObjectState);
    }
}

public sealed class Dispatcher<TRegion, TObjectState, TResult> : IRegionVersionStateVisitor<TResult>
    where TRegion : Region
    where TObjectState : ObjectState
{
    private readonly Dispatcher<TResult> _firstDispatcher;
    private readonly Dispatcher<TRegion, TResult> _secondDispatcher;
    
    private Func<TRegion, TObjectState, AnnulVersionState, TResult> _annulVersionStateCase;
    private Func<TRegion, TObjectState, ActiveVersionState, TResult> _activeVersionStateCase;

    public TObjectState Target { get; private set; } = default!;

    public Dispatcher(
        Dispatcher<TResult> firstDispatcher,
        Dispatcher<TRegion, TResult> secondDispatcher,
        Func<Region, ObjectState, VersionState, TResult> generalCase)
    {
        _firstDispatcher = firstDispatcher;
        _secondDispatcher = secondDispatcher;
        
        _annulVersionStateCase = generalCase;
        _activeVersionStateCase = generalCase;
    }

    public Dispatcher<TResult> WithAnnulVersionState(Func<TRegion, TObjectState, AnnulVersionState, TResult> function)
    {
        _annulVersionStateCase = function;
        return _firstDispatcher;
    }
    
    public Dispatcher<TResult> WithActiveVersionState(Func<TRegion, TObjectState, ActiveVersionState, TResult> function)
    {
        _activeVersionStateCase = function;
        return _firstDispatcher;
    }

    public Dispatcher<TRegion, TObjectState, TResult> Take(TObjectState objectState)
    {
        Target = objectState;
        return this;
    }

    public TResult Visit(AnnulVersionState versionState)
    {
        return _annulVersionStateCase(_secondDispatcher.Target, Target, versionState);
    }

    public TResult Visit(ActiveVersionState versionState)
    {
        return _activeVersionStateCase(_secondDispatcher.Target, Target, versionState);
    }
}