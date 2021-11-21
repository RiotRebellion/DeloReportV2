using Delo.DAL.Entities;
using Delo.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DeloReportV2.Services
{
    public class RepositoryManagerService
    {
        private readonly IRepository<Person> _PersonRepository;
        private readonly IRepository<Department> _DepartmentRepository;

        public IEnumerable<Person> Persons => _PersonRepository.Items;
        public IEnumerable<Department> Departments => _DepartmentRepository.Items;

        public RepositoryManagerService(IRepository<Person> PersonRepository, IRepository<Department> DepartmentRepository)
        {
            _PersonRepository = PersonRepository;
            _DepartmentRepository = DepartmentRepository;
        }
    }
}
