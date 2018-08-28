namespace GettingThingsDone.Contracts.Model
{
    public abstract class Entity
    {
        // The Id will be set automatically by the Entity Framework.
        // That's why we need the "private set".
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Id { get; set; }
    }
}
