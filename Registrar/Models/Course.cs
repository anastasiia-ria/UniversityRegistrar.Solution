using System.Collections.Generic;

namespace Registrar.Models
{
  public class Course
  {
    public Course()
    {
      this.JoinEntities = new HashSet<CourseStudent>();
    }

    public int CourseId { get; set; }

    // public string FullName => string.Format("{0} {1}", Name, Number) { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public virtual ICollection<CourseStudent> JoinEntities { get; set; }
  }
}