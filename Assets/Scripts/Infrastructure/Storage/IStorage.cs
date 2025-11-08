namespace Infrastructure.Storage
{
    public interface IStorage
    {
        bool HasKey(string key);
        void SetValue(string key, string value);
        string GetValue(string key, string defaultValue);
    }
}