using System.Collections.Generic;
using System.Threading.Tasks;

namespace TextsEditor.Models
{
    public interface IFileRepository
    {
        Task Add(string name, string text);
        Task<IEnumerable<File>> ShowFiles(string search);
        Task<File> GetFile(int id);
        Task Remove(int id);
        Task SaveChanges(int id, string name, string text);
    }
}