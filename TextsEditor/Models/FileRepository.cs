using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TextsEditor.Models
{

    /// <summary>
    /// class FileRepository
    /// describes the interaction with the database and operations
    /// </summary>
    public class FileRepository : IFileRepository
    {

        /// <summary>
        /// method Add
        /// add new element in database
        /// </summary>

        public async Task Add(string name, string text)
        {
            try
            {
                using (FileContext db = new FileContext())
                {
                    File file = new File() { Name = name, Text = text };
                    db.Files.Add(file);
                    await db.SaveChangesAsync();
                }
            }
            catch (InvalidOperationException e)
            {

                throw e;
            }

        }

        /// <summary>
        /// method ShowFiles
        /// get searc name
        /// </summary>
        /// <returns>
        /// Files
        /// </returns>
        public async Task<IEnumerable<File>> ShowFiles(string search)
        {
            try
            {
                using (FileContext db = new FileContext())
                {

                    if (!string.IsNullOrEmpty(search))
                    {
                        return await db.Files.Where(n => n.Name.IndexOf(search) > -1).ToListAsync();
                    }
                    else
                    {
                        return await db.Files.ToListAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// method GetFile
        /// get file by id
        /// </summary>
        /// <returns>
        /// File
        /// </returns>
        public async Task<File> GetFile(int id)
        {
            try
            {
                using (FileContext db = new FileContext())
                {
                    return await db.Files.Where(n => n.Id == id).FirstAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        /// <summary>
        /// method SaveChanges
        /// change file by id
        /// </summary>
        public async Task SaveChanges(int id, string name, string text)
        {
            try
            {
                using (FileContext db = new FileContext())
                {
                    File file = await db.Files.Where(n => n.Id == id).FirstAsync();
                   
                    file.Name = name;
                    file.Text = text;                    
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// method Remove
        /// remove by id
        /// </summary>
        public async Task Remove(int id)
        {
            try
            {
                using (FileContext db = new FileContext())
                {
                    db.Files.Remove(db.Files.Where(n => n.Id == id).First());
                    await db.SaveChangesAsync();
                }
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

        }

    }
}