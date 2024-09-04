namespace UserSupport.Helpers
{
    public static class TimeHepler
    {
        public static bool DuringWorkingHours()
        {
            var currentTime = DateTime.UtcNow;
            if (currentTime.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
            {
                return false;
            }
            else if (currentTime.Hour > 9 && currentTime.Hour < 18)
            {
                return true;
            }
            return false;
        }

    }
}
