using NipSearch.Db;
using NipSearch.Entities;

namespace NipSearch
{
    public interface ISubjectsRepository
    {
        void SaveSubject(Subject subject);
    }

    public class SubjectsRepository : ISubjectsRepository
    {
        /// <summary>
        /// Zapis przedsiębiorcy do bazy
        /// </summary>
        /// <param name="subject"></param>
        public void SaveSubject(Subject subject)
        {
            using (NipDbContext context = new NipDbContext())
            {
                context.Subjects.Add(subject);
                context.SaveChanges();
            }
        }
    }
}
