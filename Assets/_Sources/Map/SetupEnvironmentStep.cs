public class SetupEnvironmentStep : IGenerationStep
{
    private readonly EnvironmentPresenter _presenter;
    public SetupEnvironmentStep(EnvironmentPresenter presenter) => _presenter = presenter;

    public void Execute(GenerationContext context) => _presenter.Setup(context.Level.EnvironmentSetType);
}