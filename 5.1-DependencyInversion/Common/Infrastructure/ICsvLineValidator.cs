namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public interface ICsvLineValidator
    {
        ICsvLine Validate(string[] line);
    }
}
