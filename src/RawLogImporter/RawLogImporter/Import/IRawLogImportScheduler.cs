namespace LogChugger.Import
{
    public interface IRawLogImportScheduler
    {
        /**
         * <summary>Starts collecting logs periodically.</summary>
         */
        void Start();

        /**
         * <summary>Stops periodic log collection.</summary>
         */
        void Stop();
    }
}
