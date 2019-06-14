using Flow;

namespace Dekuple.Utility
{
    public static class LoggingUtility
    {
        public static bool EarlyOut(this ILogger logger, ref bool field, string message)
        {
            if (field)
            {
                logger.Verbose(15, message);
                return true;
            }
            field = true;
            return false;
        }
    }
}