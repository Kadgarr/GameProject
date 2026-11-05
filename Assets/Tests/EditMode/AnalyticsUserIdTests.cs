using DarknessInside.Core.Save;
using NUnit.Framework;

namespace DarknessInside.Tests
{
    /// <summary>ID игрока для аналитики создаётся один раз и сохраняется между запусками.</summary>
    public class AnalyticsUserIdTests
    {
        private sealed class MemoryStorage : ISaveStorage
        {
            private SaveData _data;

            public bool TryLoad(out SaveData data)
            {
                data = _data;
                return data != null;
            }

            public void Save(SaveData data) => _data = data;

            public void Delete() => _data = null;
        }

        [Test]
        public void UserId_IsCreatedOnce_AndSurvivesRestart()
        {
            var storage = new MemoryStorage();
            string first = new SaveService(storage).GetOrCreateAnalyticsUserId();

            Assert.That(first, Is.Not.Empty);
            Assert.That(System.Guid.TryParse(first, out _), Is.True);
            Assert.That(new SaveService(storage).GetOrCreateAnalyticsUserId(), Is.EqualTo(first));
        }
    }
}
