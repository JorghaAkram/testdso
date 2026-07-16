using Prove.Data.Dao.CorePTK;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK.Interface
{
    public interface IEmployeePosition : IBaseCrud<EmployeePosition>
    {
        public Task<EmployeePosition> GetFunctionByEmp(int empId);
        public Task<EmployeePosition> GetFunctionDivisionId(int postId);
        public Task<IEnumerable<EmployeePosition>> GetFunctionDivision();
        public Task<IEnumerable<EmployeePosition>> GetFunction();
        /// <summary>
        /// for get the employee division
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="isNormalizedName"></param>
        /// <returns></returns>
        public Task<EmployeePosition> GetEmployeeDivision(int empId, bool isNormalizedName = true);
        /// <summary>
        /// for get the employee division
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="isNormalizedName"></param>
        /// <returns></returns>
        public Task<EmployeePosition> GetEmployeeDivision(Employee emp, bool isNormalizedName = true);
        public Task<EmployeePosition> GetPositionDivision(EmployeePosition position, bool isNormalizedName = true);
        public IEnumerable<EmployeePosition> GetDivisions(int leaderId, bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetDivisions(bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetRegionDivisions(bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetMarketingManagers(int regionId, bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetDirectorates(int bodId, bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetDirectorates(bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetGeneralManagers(bool isNormalizedName = true, bool withInclude = false);
        public Task<EmployeePosition> GetEmployeeDirectorates(int empId, bool isNormalizedName = true);
        public Task<EmployeePosition> GetEmployeeDirectorates(Employee emp, bool isNormalizedName = true);
        public Task<EmployeePosition> GetPositionDirectorates(EmployeePosition position, bool isNormalizedName = true);
        public Task<EmployeePosition> GetEmployeeBOD(int empId, bool isNormalizedName = true);
        public Task<EmployeePosition> GetEmployeeBOD(Employee emp, bool isNormalizedName = true);
        public Task<EmployeePosition> GetPositionBOD(EmployeePosition position, bool isNormalizedName = true);
        public IEnumerable<EmployeePosition> GetBODs(bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetLehers(bool withInclude = false);
        public Task<EmployeePosition> GetEmployeeStructureOnCompany(int empId);
        public Task<EmployeePosition> GetEmployeeStructureOnCompany(Employee emp);
        public Task<EmployeePosition> GetPositionStructureOnCompany(int positionId);
        public Task<EmployeePosition> GetPositionStructureOnCompany(EmployeePosition position);
        public string NormalizeDivisionName(string param);
        public string NormalizeDirectorateName(string param);
        public string NormalizeBODName(string param);
        public string NormalizeEmployeePositionName(string param);
        public IEnumerable<EmployeePosition> GetAreaDivisions(bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetAreaDivisions(int PAreaSubId, bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetAreaDivisionsByPAreaId(int pAreaId, bool isNormalizedName = true, bool withInclude = false);
        public IEnumerable<EmployeePosition> GetAreaDivisionsByRegionId(int regionId, bool isNormalizedName = true, bool withInclude = false);
        public EmployeePosition GetLeaderPosition(int positionId, bool isNormalizedName = true, bool withInclude = false);
        public Task<IEnumerable<EmployeePosition>> GetLeaderPositions(EmployeePosition empPosition);
    }
}
