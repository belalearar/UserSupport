using UserSupport.Common.Enums;

namespace UserSupport.Common.Model
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Seniority Seniority { get; set; }
        public int NumberOfUsers { get; set; }
        public int Capicity
        {
            get
            {
                return Seniority.GetCapacity();
            }
        }
    }
}