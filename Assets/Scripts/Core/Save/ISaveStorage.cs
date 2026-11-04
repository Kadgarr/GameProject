namespace DarknessInside.Core.Save
{
    /// <summary>
    /// Где физически лежит сохранение (Repository). SaveService не знает про файлы —
    /// в тестах подставляется хранилище в памяти.
    /// </summary>
    public interface ISaveStorage
    {
        bool TryLoad(out SaveData data);

        void Save(SaveData data);

        void Delete();
    }
}
