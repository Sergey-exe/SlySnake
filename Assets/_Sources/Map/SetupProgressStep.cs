using _Sources.Map;

public class SetupProgressStep : IGenerationStep
{
    private readonly MapsProgressCollection _progressCollection;
    public SetupProgressStep(MapsProgressCollection progressCollection) => _progressCollection = progressCollection;

    public void Execute(GenerationContext context)
    {
        foreach (var map in context.Maps)
        {
            _progressCollection.AddHandler(new MapProgressHandler(map));
        }
    }
}