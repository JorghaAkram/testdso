using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prove.Data.Configuration;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Prove.Data.Configuration.ConnectionConfiguration;

namespace Prove.Data.Services.CorePTK
{
    public class EmployeeService : BaseService<Employee>, IEmployee
    {
        private IConfiguration _configuration;
        public EmployeeService(CorePTKContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                var appSettings = _configuration.Get<AppSettings>();
                return new SqlConnection(appSettings.ConnectionStrings[GeneralConstant.IsProduction ? "ProdConnectionMode" : "DevConnectionMode"] + appSettings.DataBase[DbConstant.CorePTKDb.ToString()]);
            }
        }

        private string DataBase = GeneralConstant.IsProduction ? "CorePTKNewDb" : "CorePTKDb";

        private async Task<EmployeePosition> FindCompanyPosition(Employee emp, int Part)
        {
            //if the position of the employee is same as the param
            //ex: i want to find division of current employee then i pass 3 as the part parameter
            if (emp.EmployeePosition.EmployeePositionAbbrv.Id == Part)
                return emp.EmployeePosition;


            //if the position of the employee more than the part then find until true
            if (emp.EmployeePosition.EmployeePositionAbbrv.Id > Part)
            {
                EmployeePositionStructs leader = await _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .SingleOrDefaultAsync(b => b.EmployeePosition == emp.EmployeePosition);
                while (leader.Leader.EmployeePositionAbbrv.Id != Part)
                {
                    leader = await _context
                        .Set<EmployeePositionStructs>().AsNoTracking()
                        .Include(b => b.EmployeePosition)
                        .Include(b => b.Leader)
                        .ThenInclude(b => b.EmployeePositionAbbrv)
                        .FirstOrDefaultAsync(b => b.EmployeePosition == leader.Leader);
                }
                Employee result = await _context.Set<Employee>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .SingleOrDefaultAsync(b => b.EmployeePosition == leader.Leader);
                return result.EmployeePosition;
            }

            //if until here the employee have higher position ex: VP cant find his division
            //division not divisions
            return null;
        }

        //public async Task<EmployeePosition> GetDivision(int employeeId)
        //{
        //    Employee emp = await _context
        //        .Set<Employee>().AsNoTracking()
        //         .Include(b => b.EmployeePosition)
        //            .ThenInclude(b => b.EmployeePositionAbbrv)
        //            .SingleOrDefaultAsync(b => b.Id == employeeId);

        //    if (emp == null)
        //        throw new DataNotFoundException("Employee Not Found!");
        //    return await FindCompanyPosition(emp, 3);
        //}

        //public async Task<EmployeePosition> GetDirectorate(int employeeId)
        //{
        //    Employee emp = await _context
        //        .Set<Employee>().AsNoTracking()
        //         .Include(b => b.EmployeePosition)
        //            .ThenInclude(b => b.EmployeePositionAbbrv)
        //            .SingleOrDefaultAsync(b => b.Id == employeeId);

        //    if (emp == null)
        //        throw new DataNotFoundException("Employee Not Found!");
        //    return await FindCompanyPosition(emp, 2);
        //}

        public async Task<Employee> GetBoss(Employee emp)
        {
            if (emp.EmployeePosition?.EmployeePositionAbbrv == null)
                throw new DataNotFoundException("Employee Position Not Found!");


            EmployeePositionStructs empStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .Include(b => b.Leader)
                .SingleOrDefaultAsync(b => b.EmployeePosition == emp.EmployeePosition);

            Employee result = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .FirstOrDefaultAsync(b => b.EmployeePosition == empStruct.Leader);

            //_context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<Employee> GetBoss(int employeeId)
        {
            Employee emp = await _context
                .Set<Employee>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .SingleOrDefaultAsync(b => b.Id == employeeId);

            if (emp == null)
                throw new DataNotFoundException("Employee Not Found!");
            return await GetBoss(emp);
        }

        public async Task<Employee> GetEmployeeByEmployeePositionId(int employeePositionId)
        {
            Employee emp = await _context.Set<Employee>().AsNoTracking()
                .SingleOrDefaultAsync(b => b.EmployeePosition.Id == employeePositionId && b.IsDeleted == PrideConstant.NO && b.IsActive == PrideConstant.YES);

            _context.Entry(emp).State = EntityState.Detached;
            return emp;
        }

        public async Task<IEnumerable<Employee>> GetBosses(int empId)
        {
            Employee emp = await _context
                .Set<Employee>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .SingleOrDefaultAsync(b => b.Id == empId);

            if (emp == null)
                throw new DataNotFoundException("Employee Not Found!");
            return await GetBosses(emp);
        }

        public async Task<List<Employee>> GetBossList(int empId)
        {
            Employee emp = await _context
                .Set<Employee>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .SingleOrDefaultAsync(b => b.Id == empId);

            if (emp == null)
                throw new DataNotFoundException("Employee Not Found!");
            return await GetBossList(emp);
        }

        public async Task<IEnumerable<Employee>> GetBosses(Employee emp)
        {
            if (emp.EmployeePosition?.EmployeePositionAbbrv == null)
                throw new DataNotFoundException("Employee Position Not Found!");

            Stack<Employee> result = new Stack<Employee>();
            EmployeePositionStructs tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .Include(b => b.Leader)
                .SingleOrDefaultAsync(b => b.EmployeePosition == emp.EmployeePosition);
            if (tempStruct == null)
                return new List<Employee>();
            Employee tempEmployee;
            while (tempStruct.EmployeePosition != tempStruct.Leader)
            {
                tempEmployee = await _context.Set<Employee>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .FirstOrDefaultAsync(b => b.EmployeePosition == tempStruct.Leader);

                if (tempEmployee != null)
                    result.Push(tempEmployee);

                tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                 .Include(b => b.Leader)
                 .SingleOrDefaultAsync(b => b.EmployeePosition == tempStruct.Leader);
            }
            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<List<Employee>> GetBossList(Employee emp)
        {
            if (emp.EmployeePosition?.EmployeePositionAbbrv == null)
                throw new DataNotFoundException("Employee Position Not Found!");

            Stack<Employee> result = new Stack<Employee>();
            EmployeePositionStructs tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .Include(b => b.Leader)
                .SingleOrDefaultAsync(b => b.EmployeePosition == emp.EmployeePosition);
            if (tempStruct == null)
                return new List<Employee>();
            Employee tempEmployee;
            while (tempStruct.EmployeePosition != tempStruct.Leader)
            {
                tempEmployee = await _context.Set<Employee>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .FirstOrDefaultAsync(b => b.EmployeePosition == tempStruct.Leader);
                if (tempEmployee == null)
                    break;
                result.Push(tempEmployee);
                tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                 .Include(b => b.Leader)
                 .SingleOrDefaultAsync(b => b.EmployeePosition == tempEmployee.EmployeePosition);
            }
            return result.ToList();
        }

        //public async Task<PAreaSub> GetRegion(int employeeId)
        //{
        //    Employee emp = await _context
        //        .Set<Employee>().AsNoTracking()
        //         .Include(b => b.EmployeePosition)
        //         .ThenInclude(b => b.PAreaSub)
        //         .ThenInclude(b => b.PArea)
        //         .ThenInclude(b => b.Region)
        //            .SingleOrDefaultAsync(b => b.Id == employeeId);

        //    if (emp == null)
        //        throw new DataNotFoundException("Employee Not Found!");

        //    return emp.EmployeePosition.PAreaSub;
        //}

        private async Task<List<Employee>> GetCompanyEmployeeFromStructure(int id, int abbrvId)
        {
            EmployeePosition leader = await _context.Set<EmployeePosition>().AsNoTracking()
                .Include(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.EmployeePositionAbbrv.Id == abbrvId && b.Id == id);

            List<Employee> result = new List<Employee>();
            if (leader != null)
            {
                result = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .Where(b => b.EmployeePosition == leader)
                .ToListAsync();

                //anak buah
                List<EmployeePosition> subordinates = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                    .Where(b => b.Leader == leader)
                    .Select(b => b.EmployeePosition)
                    .ToListAsync();

                while (subordinates.Count() > 0)
                {
                    result.AddRange(await _context.Set<Employee>().AsNoTracking()
                    .Where(b => subordinates.Any(c => c == b.EmployeePosition))
                    .ToListAsync());
                    subordinates = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                    .Where(b => subordinates.Any(c => c == b.Leader))
                    .Select(b => b.EmployeePosition)
                    .ToListAsync();
                }
                return result;
            }
            // tambahan dwi, buat dapetin asset management
            else if (leader == null && (Convert.ToInt32(id) == 25 || Convert.ToInt32(id) == 610))
            {
                List<int> list = new List<int>();
                List<int> tempEmpId = new List<int>();
                List<int> tempAstManId = new List<int>();
                List<int> tempAstManChildId = new List<int>();

                List<int> tempMgrId = new List<int> { Convert.ToInt32(id) };
                list.AddRange(tempMgrId);

                tempAstManId.AddRange(
                    await _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Include(b => b.Leader)
                    .Where(b => b.IsDeleted == PrideConstant.NO && b.Leader.Id == id)
                    .Select(b => b.EmployeePosition.Id)
                    .ToListAsync()
                );
                list.AddRange(tempAstManId);

                IEnumerable<int> AstManChile = await _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Include(b => b.Leader)
                    .Where(b => b.IsDeleted == PrideConstant.NO && tempAstManId.Any(c => c == b.Leader.Id))
                    .Select(b => b.EmployeePosition.Id)
                    .ToListAsync();
                list.AddRange(AstManChile);

                IEnumerable<int> EmployeeDivision = await _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Include(b => b.Leader)
                    .Where(b => b.IsDeleted == PrideConstant.NO && list.Any(d => d == b.EmployeePosition.Id))
                    .Select(b => b.EmployeePosition.Id)
                    .ToListAsync();
                tempEmpId.AddRange(EmployeeDivision);

                result.AddRange(
                    await _context.Set<Employee>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Where(b => b.IsDeleted == PrideConstant.NO &&
                        tempEmpId.Any(c => c == b.EmployeePosition.Id) &&
                        b.EmployeePosition.EmployeePositionAbbrv.Id < 7)
                    .ToListAsync()
                );
                return result;
            }
            else
            {
                return new List<Employee>();
            }
        }

        public async Task<List<Employee>> GetEmployeesAtDivison(int divisionId) => await GetCompanyEmployeeFromStructure(divisionId, 3);

        public async Task<List<Employee>> GetEmployeesAtDirectorate(int directorateId) => await GetCompanyEmployeeFromStructure(directorateId, 2);

        public async Task<List<Employee>> GetEmployeesAtBOD(int bodId) => await GetCompanyEmployeeFromStructure(bodId, 1);

        public async Task<IEnumerable<Employee>> GetEmployeesAtRegion(int regionId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtRegion(Region region)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Region == region)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPArea(int PAreaId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Id == PAreaId)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPArea(int PAreaId, int regionId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId && b.EmployeePosition.PAreaSub.PArea.Id == PAreaId)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPArea(PArea PArea)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea == PArea)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.Id == PAreaSubId)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(PAreaSub PAreaSub)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub == PAreaSub)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId, int PAreaId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Id == PAreaId && b.EmployeePosition.PAreaSub.Id == PAreaSubId)
                .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAtPAreaSubs(int PAreaSubId, int PAreaId, int regionId)
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId && b.EmployeePosition.PAreaSub.PArea.Id == PAreaId && b.EmployeePosition.PAreaSub.Id == PAreaSubId)
                .ToListAsync();

        //public async Task<List<BirthDayEmployeeModel>> GetEmployeesOnBrithDay()
        //{
        //    string query = $@"
        //    SELECT
	       //     EMP.Nip NIP,
	       //     EMP.Name NAME,
	       //     EMP.Birthdate BirthDT,
	       //     {DataBase}.dbo.GetEmployeeStructOnCompany(EMP.Id) Division
	       //     FROM
		      //      {DataBase}.dbo.Employees AS EMP
	       //     WHERE
	       //     EMP.EmployeePositionId != 639 AND
	       //     MONTH(EMP.Birthdate) = MONTH(GETDATE())
	       //     ORDER BY DAY(EMP.Birthdate) ASC
        //    ";

        //    List<BirthDayEmployeeModel> result = await _context.Query<BirthDayEmployeeModel>().FromSql(query).ToListAsync();

        //    return result;
        //}
        //public async Task<List<EmployeesStructOnCompanyModel>> GetAllEmployeesStructOnCompany()
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {
        //        dbConnection.Open();
        //        var conn = _configuration.Get<AppSettings>();
        //        var db = conn.ConnectionStrings[DbConstant.CorePTKDb.ToString()].Replace(";", "");
        //        string query = $@"
        //        SELECT
        //            em.Id,
        //            em.Name,
        //            em.Email,
        //            em.Nip,
        //            em.FamilyCardNo,
        //            em.IndetityCardNo,
        //            em.IndetityCardNo AS IdentityCardNo,
        //            em.IdentityCardAddress,
        //            em.CurrentAddress,
        //            em.CurrentAddress AS Address,
        //            em.Gender,
        //            FORMAT(em.Birthdate, 'dd MMM yyyy') as BirthDate,
        //            em.BirthPlace,
        //            em.Npwp,
        //            em.PhoneNo,
        //            em.PhoneNo AS HomePhone,
        //            em.PhoneNo AS CellPhone,
        //            em.ReligionId,
        //            em.MaritalStatusId,
        //            em.Status,
        //            em.EmployeePositionId,
        //            em.TmtJoin,
        //            FORMAT(em.TmtJoin, 'dd MMM yyyy') AS TmtPtk,
        //            em.TmtMppk,
        //            FORMAT(em.TmtOrganik, 'dd MMM yyyy') AS TmtOrganik,
        //            em.TmtPensiun,
        //            re.Name AS Religion,
        //            {DataBase}.dbo.GetEmployeeStructOnCompany(em.Id) AS Division,
        //            ed.Degree AS Education,
        //            ed.Prodi AS EducationVal,
        //            (
        //                SELECT  TOP 1 eph.IndividualPRL
        //                FROM {DataBase}.dbo.EmployeePositionHistories eph
        //                WHERE eph.IsDeleted = 'N' AND eph.EmployeeId = em.Id
        //                ORDER BY eph.Tmt DESC
        //            ) AS IndividualPRL
        //            FROM {DataBase}.dbo.Employees em
        //            LEFT JOIN {DataBase}.dbo.Religion re ON re.Id = em.ReligionId
        //            LEFT JOIN {DataBase}.dbo.EmployeePositions ep ON ep.Id = em.EmployeePositionId
        //            OUTER APPLY
        //                (
        //                    SELECT TOP 1 *
        //                     FROM {DataBase}.dbo.Education edu
        //                      WHERE edu.IsDeleted = 'N' AND edu.EmployeeId = em.Id
        //                    ORDER BY edu.EntryYear DESC
	       //             ) ed
        //            WHERE em.IsDeleted = 'N'
        //            AND em.IsActive = 'Y'
        //        ";

        //        var data = dbConnection.Query<EmployeesStructOnCompanyModel>(query);
        //        return data.ToList();
        //    }
        //}

        public async Task<IEnumerable<Employee>> GetAllActiveEmployeesAsync()
        => await _context
                .Set<Employee>().AsNoTracking()
                .Where(b => b.IsActive == PrideConstant.YES && b.IsDeleted == PrideConstant.NO)
                .ToListAsync();

        //public async Task<IEnumerable<EmployeesStructOnCompanyModel>> GetDivisionByEmpId(int empId)
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {
        //        dbConnection.Open();
        //        string query = $@"
        //        SELECT
	       //         EMP.Nip NIP,
	       //         EMP.Name NAME,
	       //         EMP.Birthdate BirthDT,
	       //         {DataBase}.dbo.GetEmployeeStructOnCompany(EMP.Id) Division
	       //         FROM
		      //          {DataBase}.dbo.Employees AS EMP
	       //         WHERE
        //            EMP.IsDeleted = 'N' AND
	       //         EMP.Id = " + "'" + empId + "'";

        //        var data = dbConnection.Query<EmployeesStructOnCompanyModel>(query);
        //        return data;
        //    }
        //}



    }
}
