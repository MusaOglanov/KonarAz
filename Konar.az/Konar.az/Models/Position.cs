namespace Konar.az.Models
{
	public class Position
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Employee> Employee { get; set; }
        public bool IsDeactive { get; set; }

    }
}
