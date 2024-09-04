using UserSupport.Enums;

namespace UserSupport
{
    public static class Extensions
    {
        public static int GetCapacity(this Seniority seniority)
        {
            switch (seniority)
            {
                case Seniority.Jonior:
                    return 4;
                case Seniority.MidLevel:
                    return 6;
                case Seniority.Senior:
                    return 8;
                case Seniority.Lead:
                    return 5;
                default: return 0;
            }
        }
    }
}