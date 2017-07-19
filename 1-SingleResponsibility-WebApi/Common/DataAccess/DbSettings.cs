namespace _1_SingleResponsibility_WebApi.Common.DataAccess
{
    public static class DbSettings
    {
        // Palliative resolution to the timeout issues until we have the time to improve the reliability/fault-proof of the system.
        public static int CommandTimeout = 300;
        public static int Batching = 2000;
    }
}