using Prove.Data.Dao.CorePTK;
using Prove.Data.Services.CorePTK.Models;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK.Interface
{
    public interface IEmployee : IBaseCrud<Employee>
    {
        //public Task<List<BirthDayEmployeeModel>> GetEmployeesOnBrithDay();
        public Task<List<Employee>> GetEmployeesAtDivison(int divisionId);
        public Task<List<Employee>> GetEmployeesAtDirectorate(int directoareId);
        public Task<List<Employee>> GetEmployeesAtBOD(int bodId);
        public Task<IEnumerable<Employee>> GetEmployeesAtRegion(int regionId);
        public Task<IEnumerable<Employee>> GetEmployeesAtRegion(Region region);
        public Task<IEnumerable<Employee>> GetEmployeesAtPArea(int PAreaId);
        public Task<IEnumerable<Employee>> GetEmployeesAtPArea(int PAreaId, int regionId);
        public Task<IEnumerable<Employee>> GetEmployeesAtPArea(PArea PArea);
        public Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId);
        public Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId, int PAreaId, int regionId);
        public Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId, int PAreaId);
        public Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(PAreaSub PAreaSub);
        //public Task<EmployeePosition> GetDivision(int employeeId);
        //public Task<EmployeePosition> GetDirectorate(int employeeId);
        public Task<Employee> GetBoss(Employee emp);
        public Task<Employee> GetBoss(int employeeId);
        public Task<Employee> GetEmployeeByEmployeePositionId(int employeePositionId);
        public Task<IEnumerable<Employee>> GetBosses(int empId);

        public Task<List<Employee>> GetBossList(int empId);
        public Task<IEnumerable<Employee>> GetBosses(Employee emp);
        //Task<PAreaSub> GetRegion(int employeeId);

        //public Task<List<EmployeesStructOnCompanyModel>> GetAllEmployeesStructOnCompany();

        public Task<IEnumerable<Employee>> GetAllActiveEmployeesAsync();
        //IEnumerable<Employee> GetDivisionByEmpId(int empId);

        //public Task<IEnumerable<EmployeesStructOnCompanyModel>> GetDivisionByEmpId(int empId);


    }
}
