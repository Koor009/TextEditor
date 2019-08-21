using System.Data.Entity;

namespace TextsEditor.Models
{
    public class FileContext :DbContext
    {
        public DbSet<File> Files { get; set; }
    }
}