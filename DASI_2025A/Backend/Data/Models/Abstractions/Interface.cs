namespace Backend
{
    public interface ISoftDelete
    {
        bool IsActive { get; set; }
    }
}
