using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Impl
{
    public class UserRepoImpl : IUserRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly CorePTKContext _coreContext;

        public UserRepoImpl(ProveExtContext proveContext, CorePTKContext coreContext)
        {
            _proveContext = proveContext;
            _coreContext = coreContext;
        }

        public async Task<bool> insertUser(UserPost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Data.Dao.Prove.User userData = new Data.Dao.Prove.User();

                    userData.CompanyCode = param.CompanyCode;
                    userData.CompanyName = param.CompanyName;
                    userData.Email = param.Email;
                    userData.IdamanId = param.IdamanId;
                    userData.Name = param.Name;
                    userData.OrganizationId = param.OrganizationId;
                    userData.OrganizationName = param.OrganizationName;
                    userData.OrganizationParentId = param.OrganizationParentId;
                    userData.OrganizationParentName = param.OrganizationParentName;
                    userData.PositionId = param.PositionId;
                    userData.PositionName = param.PositionName;
                    userData.PositionParentId = param.PositionParentId;
                    userData.PositionParentName = param.PositionParentName;
                    userData.Role = param.Role;
                    userData.UserId = param.UserId;
                    userData.CreatedBy = "SYSTEM";
                    userData.UpdatedBy = "SYSTEM";

                    await _proveContext.User.AddAsync(userData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Insert";
                    logActivity.Method = "insertUser";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _coreContext.ActivityLog.AddAsync(logActivity);
                    await _coreContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Insert";
                logActivity.Method = "insertUser";
                logActivity.Remark = "Prove BusinessLogic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                _coreContext.ActivityLog.Add(logActivity);
                _coreContext.SaveChanges();

                #endregion

                return false;
            }
        }

        public async Task<bool> updateUser(UserPost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Data.Dao.Prove.User userData = new Data.Dao.Prove.User();

                    userData = _proveContext.User.Where(a => a.UserId == param.UserId).FirstOrDefault();

                    userData.CompanyCode = param.CompanyCode;
                    userData.CompanyName = param.CompanyName;
                    userData.Email = param.Email;
                    userData.IdamanId = param.IdamanId;
                    userData.Name = param.Name;
                    userData.OrganizationId = param.OrganizationId;
                    userData.OrganizationName = param.OrganizationName;
                    userData.OrganizationParentId = param.OrganizationParentId;
                    userData.OrganizationParentName = param.OrganizationParentName;
                    userData.PositionId = param.PositionId;
                    userData.PositionName = param.PositionName;
                    userData.PositionParentId = param.PositionParentId;
                    userData.PositionParentName = param.PositionParentName;
                    userData.Role = param.Role;
                    userData.UserId = param.UserId;
                    userData.UpdatedAt = DateTime.Now;
                    userData.UpdatedBy = "SYSTEM";

                    _proveContext.User.Update(userData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Update";
                    logActivity.Method = "updateUser";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _coreContext.ActivityLog.AddAsync(logActivity);
                    await _coreContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Update";
                logActivity.Method = "updateUser";
                logActivity.Remark = "Prove BusinessLogic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                _coreContext.ActivityLog.Add(logActivity);
                _coreContext.SaveChanges();

                #endregion

                return false;
            }
        }

        public async Task<bool> deleteUser(string userId)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Data.Dao.Prove.User userData = new Data.Dao.Prove.User();

                    userData = _proveContext.User.Where(a => a.UserId == userId).FirstOrDefault();

                    userData.IsActive = GeneralConstant.NO;
                    userData.IsDeleted = GeneralConstant.YES;
                    userData.UpdatedAt = DateTime.Now;
                    userData.UpdatedBy = "SYSTEM";

                    _proveContext.User.Update(userData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteUser";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _coreContext.ActivityLog.AddAsync(logActivity);
                    await _coreContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Delete";
                logActivity.Method = "deleteUser";
                logActivity.Remark = "Prove BusinessLogic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                _coreContext.ActivityLog.Add(logActivity);
                _coreContext.SaveChanges();

                #endregion

                return false;
            }
        }
    }
}
