namespace UserSupport.Common.Model
{
    public class Team
    {
        public string Name { get; set; } = null!;

        public int QueueSize { get { return (int)(Capacity * 1.5); } }
        public int Capacity
        {
            get
            {

                var capacity = 0;
                foreach (var teamMember in TeamMembers)
                {
                    switch (teamMember.Seniority)
                    {
                        case Enums.Seniority.Jonior:
                            capacity += (int)Math.Floor(10 * 0.4);
                            break;
                        case Enums.Seniority.MidLevel:
                            capacity += (int)Math.Floor(10 * 0.6);
                            break;
                        case Enums.Seniority.Senior:
                            capacity += (int)Math.Floor(10 * 0.8);
                            break;
                        case Enums.Seniority.Lead:
                            capacity += (int)Math.Floor(10 * 0.5);
                            break;
                    }
                }
                return capacity;
            }
        }
        public List<TeamMember> TeamMembers { get; set; } = [];
        public TimeOnly ShiftFrom { get; set; }
        public TimeOnly ShiftTo { get; set; }
    }
}