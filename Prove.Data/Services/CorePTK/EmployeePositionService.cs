using Microsoft.EntityFrameworkCore;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK
{
    public class EmployeePositionService : BaseService<EmployeePosition>, IEmployeePosition
    {
        public EmployeePositionService(CorePTKContext context) : base(context)
        {

        }
        private EmployeePosition NormalizedEmployeePositionDivision(EmployeePosition b)
            => new EmployeePosition
            {
                CreatedAt = b.CreatedAt,
                CreatedBy = b.CreatedBy,
                ExtPhoneNo = b.ExtPhoneNo,
                Id = b.Id,
                IsActive = b.IsActive,
                IsDeleted = b.IsDeleted,
                Organization = b.Organization,
                EmployeePositionAbbrv = b.EmployeePositionAbbrv,
                PAreaSub = b.PAreaSub,
                PositionIdSap = b.PositionIdSap,
                PositionName = NormalizeDivisionName(b.PositionName),
                PrlHigher = b.PrlHigher,
                PrlLower = b.PrlLower,
                Status = b.Status,
                UpdatedAt = b.UpdatedAt,
                UpdatedBy = b.UpdatedBy
            };
        private EmployeePosition NormalizedEmployeePositionDirectorate(EmployeePosition b)
            => new EmployeePosition
            {
                CreatedAt = b.CreatedAt,
                CreatedBy = b.CreatedBy,
                ExtPhoneNo = b.ExtPhoneNo,
                Id = b.Id,
                IsActive = b.IsActive,
                IsDeleted = b.IsDeleted,
                Organization = b.Organization,
                EmployeePositionAbbrv = b.EmployeePositionAbbrv,
                PAreaSub = b.PAreaSub,
                PositionIdSap = b.PositionIdSap,
                PositionName = NormalizeDirectorateName(b.PositionName),
                PrlHigher = b.PrlHigher,
                PrlLower = b.PrlLower,
                Status = b.Status,
                UpdatedAt = b.UpdatedAt,
                UpdatedBy = b.UpdatedBy
            };
        private EmployeePosition NormalizedEmployeePositionBOD(EmployeePosition b)
            => new EmployeePosition
            {
                CreatedAt = b.CreatedAt,
                CreatedBy = b.CreatedBy,
                ExtPhoneNo = b.ExtPhoneNo,
                Id = b.Id,
                IsActive = b.IsActive,
                IsDeleted = b.IsDeleted,
                Organization = b.Organization,
                EmployeePositionAbbrv = b.EmployeePositionAbbrv,
                PAreaSub = b.PAreaSub,
                PositionIdSap = b.PositionIdSap,
                PositionName = NormalizeBODName(b.PositionName),
                PrlHigher = b.PrlHigher,
                PrlLower = b.PrlLower,
                Status = b.Status,
                UpdatedAt = b.UpdatedAt,
                UpdatedBy = b.UpdatedBy
            };


        public IEnumerable<EmployeePosition> GetMarketingManagers(int regionId, bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 3 && b.EmployeePosition.EmployeePositionAbbrv.Id == 3 && b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 3 && b.EmployeePosition.EmployeePositionAbbrv.Id == 3 && b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }

        public IEnumerable<EmployeePosition> GetDivisions(int leaderId, bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => b.Leader.Id == leaderId && (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => b.Leader.Id == leaderId && (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }

        public IEnumerable<EmployeePosition> GetDivisions(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);


        }

        public IEnumerable<EmployeePosition> GetDirectorates(int bodId, bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                               .Include(b => b.EmployeePosition)
                                .ThenInclude(b => b.PAreaSub)
                                .ThenInclude(b => b.PArea)
                                .ThenInclude(b => b.Region)
                               .Where(b => b.Leader.Id == bodId && b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                               .Include(b => b.EmployeePosition)
                               .Where(b => b.Leader.Id == bodId && b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            //if (withInclude)
            //    employeePositionStructs =
            //    _context
            //                   .Set<EmployeePositionStructs>()
            //                   .Include(b => b.EmployeePosition)
            //                   .Where(b => b.Leader.Id == bodId
            //                   && b.EmployeePosition.EmployeePositionAbbrv.Id != 1
            //                   && b.Leader.EmployeePositionAbbrv.Id == 1);

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDirectorate(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
            //return await _context
            //               .Set<EmployeePositionStructs>()
            //               .Where(b => b.Leader.Id == bodId && b.EmployeePosition.EmployeePositionAbbrv.Id != 1 && b.Leader.EmployeePositionAbbrv.Id == 1)
            //               .Select(b => b.EmployeePosition)
            //               .ToListAsync();
        }

        public IEnumerable<EmployeePosition> GetDirectorates(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                               .Include(b => b.EmployeePosition)
                                .ThenInclude(b => b.PAreaSub)
                                .ThenInclude(b => b.PArea)
                                .ThenInclude(b => b.Region)
                               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                               .Include(b => b.EmployeePosition)
                               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id == 1).AsEnumerable();
            //if (withInclude)
            //    employeePositionStructs =
            //    _context
            //                   .Set<EmployeePositionStructs>()
            //                   .Include(b => b.EmployeePosition)
            //                   .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id != 1
            //                   && b.Leader.EmployeePositionAbbrv.Id == 1);

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDirectorate(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
            //IQueryable<EmployeePositionStructs> employeePositionStructs = _context
            //               .Set<EmployeePositionStructs>()
            //               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id != 1
            //               && b.Leader.EmployeePositionAbbrv.Id == 1);
            //if (isNormalizedName)
            //    return await employeePositionStructs.Select(b => b.EmployeePosition)
            //                                        .Include(b => b.PAreaSub)
            //                                        .ThenInclude(b => b.PArea)
            //                                        .ThenInclude(b => b.Region)
            //                                        .ToListAsync();
            //else
            //    return await employeePositionStructs.Select(b => NormalizedEmployeePositionDirectorate(b.EmployeePosition))
            //                                        .Include(b => b.PAreaSub)
            //                                        .ThenInclude(b => b.PArea)
            //                                        .ThenInclude(b => b.Region).ToListAsync();
            //return await _context
            //               .Set<EmployeePositionStructs>()
            //               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id != 1 && b.Leader.EmployeePositionAbbrv.Id == 1)
            //               .Select(b => b.EmployeePosition)
            //               .ToListAsync();
        }

        public IEnumerable<EmployeePosition> GetGeneralManagers(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                               .Include(b => b.EmployeePosition)
                                .ThenInclude(b => b.PAreaSub)
                                .ThenInclude(b => b.PArea)
                                .ThenInclude(b => b.Region)
                               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                               .Include(b => b.EmployeePosition)
                               .Where(b => b.EmployeePosition.EmployeePositionAbbrv.Id == 2
                               && b.Leader.EmployeePositionAbbrv.Id == 1 && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDirectorate(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }

        public IEnumerable<EmployeePosition> GetBODs(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs = _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                               .Include(b => b.EmployeePosition)
                               .ThenInclude(b => b.PAreaSub)
                               .ThenInclude(b => b.PArea)
                               .ThenInclude(b => b.Region)
                               .Where(b => b.Leader.Id == 1 && b.EmployeePosition.EmployeePositionAbbrv.Id == 1).AsEnumerable();
            else
                employeePositionStructs = _context
                               .Set<EmployeePositionStructs>().AsNoTracking()
                               .Include(b => b.EmployeePosition)
                               .Where(b => b.Leader.Id == 1 && b.EmployeePosition.EmployeePositionAbbrv.Id == 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionBOD(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);

        }

        public IEnumerable<EmployeePosition> GetLehers(bool withInclude = false)
        {
            if (withInclude)
                return _context
                           .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                           .Include(b => b.EmployeePosition)
                           .ThenInclude(b => b.PAreaSub)
                           .ThenInclude(b => b.PArea)
                           .ThenInclude(b => b.Region)
                           .Where(b => b.EmployeePosition.Id != 1 && b.Leader.Id == 1 && b.EmployeePosition.EmployeePositionAbbrv.Id == 2).AsEnumerable().Select(b => b.EmployeePosition);
            else
                return _context
                           .Set<EmployeePositionStructs>().AsNoTracking()
                           .Include(b => b.EmployeePosition)
                           .Where(b => b.EmployeePosition.Id != 1 && b.Leader.Id == 1 && b.EmployeePosition.EmployeePositionAbbrv.Id == 2).AsEnumerable().Select(b => b.EmployeePosition);
            //return await _context
            //               .Set<EmployeePositionStructs>()
            //               .Where(b => b.EmployeePosition.Id != 1 && b.Leader.Id == 1 && b.EmployeePosition.EmployeePositionAbbrv.Id == 2)
            //               .Select(b => b.EmployeePosition)
            //               .ToListAsync();
        }

        //private bool GetEmployeeSectionInCompanyDivisionParam(int id, int compareId)
        //{
        //    if (compareId == 3)
        //        return (id > 3 && id > 4);
        //    return id > compareId;
        //}
        private async Task<EmployeePosition> GetPositionSectionInCompany(EmployeePosition position, int abbrvId)
        {

            EmployeePosition result = position;
            EmployeePosition leader = position;

            if (result.EmployeePositionAbbrv.Id == abbrvId)
                return result;

            do
            {
                var empPost = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                     .FirstOrDefaultAsync(b => b.EmployeePosition == leader);
                if (empPost == null)
                    return null;
                result = empPost.EmployeePosition;
                leader = empPost.Leader;
            } while (leader.EmployeePositionAbbrv.Id >= abbrvId && result.EmployeePositionAbbrv.Id != leader.EmployeePositionAbbrv.Id);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        private async Task<EmployeePosition> GetEmployeeSectionInCompany(Employee emp, int abbrvId)
        {

            EmployeePosition result = emp.EmployeePosition;
            EmployeePosition leader = emp.EmployeePosition;

            if (result.EmployeePositionAbbrv.Id == abbrvId)
                return result;

            do
            {
                var empPost = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                     .FirstOrDefaultAsync(b => b.EmployeePosition == leader);
                if (empPost == null)
                    return null;
                result = empPost.EmployeePosition;
                leader = empPost.Leader;
            } while (leader.EmployeePositionAbbrv.Id >= abbrvId && result.EmployeePositionAbbrv.Id != leader.EmployeePositionAbbrv.Id);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<EmployeePosition> GetEmployeeDivision(int empId, bool isNormalizedName = true)
        {
            Employee emp = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == empId);


            if (emp.EmployeePosition == null || emp.EmployeePosition.EmployeePositionAbbrv.Id < 3)
                return null;

            EmployeePosition result = await GetEmployeeSectionInCompany(emp, 3);

            if (isNormalizedName)
                result = NormalizedEmployeePositionDivision(result);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<EmployeePosition> GetEmployeeDivision(Employee emp, bool isNormalizedName = true)
        {

            if (emp.EmployeePosition == null || emp.EmployeePosition.EmployeePositionAbbrv.Id < 3)
                return null;

            EmployeePosition result = await GetEmployeeSectionInCompany(emp, 3);

            if (result == null)
                return null;
            if (isNormalizedName)
                result = NormalizedEmployeePositionDivision(result);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<EmployeePosition> GetEmployeeDirectorates(int empId, bool isNormalizedName = true)
        {
            Employee emp = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == empId);

            if (emp.EmployeePosition == null || emp.EmployeePosition.EmployeePositionAbbrv.Id < 2)
                return null;

            EmployeePosition result = await GetEmployeeSectionInCompany(emp, 2);

            if (isNormalizedName)
                result = NormalizedEmployeePositionDirectorate(result);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<EmployeePosition> GetEmployeeDirectorates(Employee emp, bool isNormalizedName = true)
        {

            if (emp.EmployeePosition == null || emp.EmployeePosition?.EmployeePositionAbbrv?.Id < 2)
                return null;

            EmployeePosition result = await GetEmployeeSectionInCompany(emp, 2);

            if (result == null)
                return null;
            if (isNormalizedName)
                result = NormalizedEmployeePositionDirectorate(result);

            ////_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<EmployeePosition> GetEmployeeBOD(int empId, bool isNormalizedName = true)
        {
            Employee emp = await _context.Set<Employee>()
                .Include(b => b.EmployeePosition)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == empId);


            if (emp.EmployeePosition == null)
                return null;

            //EmployeePosition result = emp.EmployeePosition;

            //do
            //{
            //    var empPost = await _context.Set<EmployeePositionStructs>()
            //        .Include(b => b.Leader)
            //        .ThenInclude(b => b.EmployeePositionAbbrv)
            //         .FirstOrDefaultAsync(b => b.EmployeePosition == result);
            //    //condition if the employee is leher
            //    //if (empPost.EmployeePosition.EmployeePositionAbbrv.Id != 1 && empPost.Leader.Id == 1)
            //    //    return null;
            //    result = empPost.Leader;
            //} while (result.EmployeePositionAbbrv.Id > 1);

            //return result;
            return await GetEmployeeSectionInCompany(emp, 1);
        }
        public async Task<EmployeePosition> GetEmployeeBOD(Employee emp, bool isNormalizedName = true)
        {
            if (emp.EmployeePosition == null)
                return null;
            return await GetEmployeeSectionInCompany(emp, 1);
        }
        public string PositionToFunction(string position = "")
        {
            string funct = position
                //.Replace("Direktur ", "")
                .Replace("VP ", "")
                .Replace("GM ", "")
                .Replace("Chief ", "")

                .Replace("Manager ", "")
                .Replace("Head of ", "")
                .Replace("Ast Manager ", "")
                .Replace("Sr. Manager ", "")
                .Replace("Sr. ", "")
                .Replace("Ast ", "")
                .ToString();
            return funct;
        }
        public string NormalizeDivisionName(string param) => param
                .Replace("Manager ", "")
                .Replace("Head of ", "")
                .Replace("Ast ", "")
                .Replace("Ast Manager ", "")
                .Replace("Sr. Manager ", "")
                .Replace("Sr. ", "");
        public string NormalizeDirectorateName(string param) => param
                .Replace("VP ", "")
                .Replace("GM ", "")
                .Replace("Chief ", "");
        public string NormalizeBODName(string param) => param
                .Replace("Direktur ", "");
        public string NormalizeEmployeePositionName(string param) => param
                .Replace("Manager ", "")
                .Replace("Head of ", "")
                .Replace("Ast Manager ", "")
                .Replace("Sr. Manager ", "")
                .Replace("Sr. ", "")
                .Replace("VP ", "")
                .Replace("GM ", "")
                .Replace("Chief ", "")
                .Replace("Direktur ", "");
        public async Task<EmployeePosition> GetFunctionByEmp(int empId)
        {
            // service ini untuk mendapatkan fungsi dari employe
            // property PositionName sebagai fungsi
            Employee emp = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == empId);


            if (emp.EmployeePosition == null)
                return null;
            int abbrvId = 3;
            if (emp.EmployeePosition.EmployeePositionAbbrv.Id < 3)
            {
                abbrvId = emp.EmployeePosition.EmployeePositionAbbrv.Id;
            }

            EmployeePosition result = emp.EmployeePosition;
            if (result.EmployeePositionAbbrv.Id != abbrvId)
            {
                do
                {
                    var empPost = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                        .Include(b => b.EmployeePosition)
                        .Include(b => b.Leader)
                        .ThenInclude(b => b.EmployeePositionAbbrv)
                         .FirstOrDefaultAsync(b => b.EmployeePosition == result);
                    if (empPost == null)
                        return null;
                    result = empPost.Leader;
                } while (result.EmployeePositionAbbrv.Id > abbrvId);
            }
            result.PositionName = PositionToFunction(result.PositionName);

            //_context.Entry(result).State = EntityState.Detached;
            return result;
        }
        public async Task<IEnumerable<EmployeePosition>> GetFunction()
        {
            // service ini untuk mendapatkan semua fungsi
            // property PositionName sebagai fungsi
            List<EmployeePosition> empPost = await _context
                .Set<EmployeePositionStructs>()
                .AsNoTracking()
                .Where(b => b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2 || b.Leader.EmployeePositionAbbrv.Id == 3)
                .Select(b => b.Leader)
                .Distinct()
                .ToListAsync();

            for (int i = 0; i < empPost.Count; i++)
            {
                empPost[i].PositionName = PositionToFunction(empPost[i].PositionName);
            }

            return empPost;
        }
        public async Task<EmployeePosition> GetEmployeeStructureOnCompany(int empId)
        {
            Employee emp = await _context.Set<Employee>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == empId);
            return await GetEmployeeStructureOnCompany(emp);
        }
        public async Task<IEnumerable<EmployeePosition>> GetFunctionDivision()
        {
            IEnumerable<EmployeePosition> emps = GetDivisions();
            List<EmployeePosition> employees = emps.ToList();
            for (int i = 0; i < employees.Count(); i++)
            {
                employees[i].PositionName = PositionToFunction(employees[i].PositionName);
            }
            return employees;
        }
        public async Task<EmployeePosition> GetFunctionDivisionId(int postId)
        {
            var empPost = await _context.Set<EmployeePositionStructs>()
                .AsNoTracking()
                        .Include(b => b.EmployeePosition)
                        .ThenInclude(b => b.PAreaSub)
                        .ThenInclude(b => b.PArea)
                        .ThenInclude(b => b.Region)
                        .Include(b => b.Leader)
                        .ThenInclude(b => b.EmployeePositionAbbrv)
                         .FirstOrDefaultAsync(b => b.EmployeePosition.Id == postId);
            empPost.EmployeePosition.PositionName = PositionToFunction(empPost.EmployeePosition.PositionName);

            ////_context.Entry(empPost).State = EntityState.Detached;
            return empPost.EmployeePosition;
        }
        public async Task<EmployeePosition> GetEmployeeStructureOnCompany(Employee emp)
        {
            if (emp?.EmployeePosition?.EmployeePositionAbbrv == null)
                return null;
            EmployeePositionStructs leader = await _context.Set<EmployeePositionStructs>()
                .AsNoTracking()
                .Include(b => b.Leader)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.EmployeePosition == emp.EmployeePosition);

            if (leader?.Leader == null)
                return null;

            if (emp.EmployeePosition.EmployeePositionAbbrv.Id == 3 || emp.EmployeePosition.EmployeePositionAbbrv.Id == 4 || (leader.Leader.EmployeePositionAbbrv.Id <= 7 && leader.Leader.EmployeePositionAbbrv.Id >= 3))
                return await GetEmployeeDivision(emp);
            if (emp.EmployeePosition.EmployeePositionAbbrv.Id == 2 || (leader.Leader.EmployeePositionAbbrv.Id < 3 && leader.Leader.EmployeePositionAbbrv.Id >= 2))
                return await GetEmployeeDirectorates(emp);
            if (leader.Leader.EmployeePositionAbbrv.Id == 1)
                return await GetEmployeeBOD(emp);
            return null;
        }
        public IEnumerable<EmployeePosition> GetAreaDivisions(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }
        public IEnumerable<EmployeePosition> GetAreaDivisions(int PAreaSubId, bool isNormalizedName = true, bool withInclude = false)
        {

            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.Id == PAreaSubId).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.Id == PAreaSubId).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }
        public IEnumerable<EmployeePosition> GetAreaDivisionsByPAreaId(int pAreaId, bool isNormalizedName = true, bool withInclude = false)
        {

            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 2 && b.Leader.PAreaSub.Id != 1 && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.PArea.Id == pAreaId).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 2 && b.Leader.PAreaSub.Id != 1 && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.PArea.Id == pAreaId).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }
        public IEnumerable<EmployeePosition> GetAreaDivisionsByRegionId(int regionId, bool isNormalizedName = true, bool withInclude = false)
        {

            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 2 && b.Leader.PAreaSub.Id != 1 && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => b.Leader.EmployeePositionAbbrv.Id == 2 && b.Leader.PAreaSub.Id != 1 && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1 && b.EmployeePosition.PAreaSub.PArea.Region.Id == regionId).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }
        public EmployeePosition GetLeaderPosition(int positionId, bool isNormalizedName = true, bool withInclude = false)
        {
            EmployeePositionStructs employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs = _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                    .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.Organization)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .FirstOrDefault(b => b.EmployeePosition.Id == positionId);
            else
                employeePositionStructs = _context
                    .Set<EmployeePositionStructs>().AsNoTracking()
                    .Include(b => b.EmployeePosition)
                    .Include(b => b.Leader)
                    .ThenInclude(b => b.EmployeePositionAbbrv)
                    .FirstOrDefault(b => b.EmployeePosition.Id == positionId);

            //_context.Entry(employeePositionStructs).State = EntityState.Detached;
            if (isNormalizedName)
            {
                return employeePositionStructs.Leader.EmployeePositionAbbrv.Id switch
                {
                    1 => NormalizedEmployeePositionBOD(employeePositionStructs.Leader),
                    2 => NormalizedEmployeePositionDirectorate(employeePositionStructs.Leader),
                    3 => NormalizedEmployeePositionDivision(employeePositionStructs.Leader),
                    _ => null,
                };
            }

            return employeePositionStructs.Leader;
        }
        public async Task<IEnumerable<EmployeePosition>> GetLeaderPositions(EmployeePosition empPosition)
        {
            if (empPosition.EmployeePositionAbbrv?.Id == null)
                throw new ArgumentException("Paramter must be at least a division");

            Stack<EmployeePosition> result = new Stack<EmployeePosition>();
            EmployeePositionStructs tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                .Include(b => b.EmployeePosition)
                .Include(b => b.Leader)
                .SingleOrDefaultAsync(b => b.EmployeePosition == empPosition);

            if (tempStruct == null)
                return new List<EmployeePosition>();

            while (tempStruct.EmployeePosition != tempStruct.Leader)
            {
                result.Push(tempStruct.Leader);

                tempStruct = await _context.Set<EmployeePositionStructs>().AsNoTracking()
                 .Include(b => b.EmployeePosition)
                 .Include(b => b.Leader)
                 .SingleOrDefaultAsync(b => b.EmployeePosition == tempStruct.Leader);
            }
            return result;
        }
        public IEnumerable<EmployeePosition> GetRegionDivisions(bool isNormalizedName = true, bool withInclude = false)
        {
            IEnumerable<EmployeePositionStructs> employeePositionStructs = null;

            if (withInclude)
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                      .ThenInclude(b => b.Organization)
                      .Include(b => b.EmployeePosition)
                    .ThenInclude(b => b.PAreaSub)
                    .ThenInclude(b => b.PArea)
                    .ThenInclude(b => b.Region)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();
            else
                employeePositionStructs =
                _context
                       .Set<EmployeePositionStructs>().AsNoTracking()
                      .Include(b => b.EmployeePosition)
                       .Where(b => (b.Leader.EmployeePositionAbbrv.Id == 1 || b.Leader.EmployeePositionAbbrv.Id == 2) && (b.EmployeePosition.EmployeePositionAbbrv.Id == 3 || b.EmployeePosition.EmployeePositionAbbrv.Id == 4) && b.EmployeePosition.PAreaSub.Id != 1).AsEnumerable();

            if (isNormalizedName)
                return employeePositionStructs.Select(b => NormalizedEmployeePositionDivision(b.EmployeePosition));
            else
                return employeePositionStructs.Select(b => b.EmployeePosition);
        }
        public async Task<EmployeePosition> GetPositionStructureOnCompany(int positionId)
        {
            EmployeePosition position = await _context.Set<EmployeePosition>().AsNoTracking()
                .Include(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.Id == positionId);
            return await GetPositionStructureOnCompany(position);
        }
        public async Task<EmployeePosition> GetPositionStructureOnCompany(EmployeePosition position)
        {
            if (position.EmployeePositionAbbrv == null)
                return null;
            EmployeePositionStructs leader = await _context.Set<EmployeePositionStructs>()
                .AsNoTracking()
                .Include(b => b.Leader)
                .ThenInclude(b => b.EmployeePositionAbbrv)
                .FirstOrDefaultAsync(b => b.EmployeePosition == position);

            if (leader?.Leader == null)
                return null;

            if (position.EmployeePositionAbbrv.Id == 3 || position.EmployeePositionAbbrv.Id == 4 || (leader.Leader.EmployeePositionAbbrv.Id <= 7 && leader.Leader.EmployeePositionAbbrv.Id >= 3))
                return await GetPositionDivision(position);
            if (position.EmployeePositionAbbrv.Id == 2 || (leader.Leader.EmployeePositionAbbrv.Id < 3 && leader.Leader.EmployeePositionAbbrv.Id >= 2))
                return await GetPositionDirectorates(position);
            if (leader.Leader.EmployeePositionAbbrv.Id == 1)
                return await GetPositionBOD(position);
            return null;
        }
        public async Task<EmployeePosition> GetPositionDivision(EmployeePosition position, bool isNormalizedName = true)
        {
            if (position == null || position.EmployeePositionAbbrv.Id < 3)
                return null;

            EmployeePosition result = await GetPositionSectionInCompany(position, 3);

            if (result == null)
                return null;
            if (isNormalizedName)
                result = NormalizedEmployeePositionDivision(result);

            return result;
        }
        public async Task<EmployeePosition> GetPositionDirectorates(EmployeePosition position, bool isNormalizedName = true)
        {
            if (position == null || position.EmployeePositionAbbrv.Id < 3)
                return null;

            EmployeePosition result = await GetPositionSectionInCompany(position, 2);

            if (result == null)
                return null;
            if (isNormalizedName)
                result = NormalizedEmployeePositionDirectorate(result);

            return result;
        }
        public async Task<EmployeePosition> GetPositionBOD(EmployeePosition position, bool isNormalizedName = true)
        {
            if (position == null)
                return null;
            return await GetPositionSectionInCompany(position, 1);
        }
    }
}
