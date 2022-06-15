using AnL.Repository.Abstraction;
using AnL.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Dynamic;
using AnL.ViewModel;

namespace AnL.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Tan_DBContext _dbcontext;
        private readonly IUnitOfWork _UOW;

        public UnitOfWork(Tan_DBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        private ITimesheetDetail _TimesheetDetailRepository;
        private IUser _UserRepository;
        private IEmployeeDetails _EmployeeDetailRepository;
        private IProject _ProjectRepository;
        private IAudit _AuditRepository;

        public IAudit AuditRepository
        {
            get
            {
                if (_AuditRepository == null)
                {
                    _AuditRepository = new AuditRepository(_dbcontext, _UOW);
                }
                return _AuditRepository;
            }
        }
        public IProject ProjectRepository
        {
            get
            {
                if(_ProjectRepository==null)
                {
                    _ProjectRepository = new ProjectRepository(_dbcontext, _UOW);
                }
                return _ProjectRepository;
            }
        }

        public IEmployeeDetails EmployeeDetailsRepository
        {
            get
            {
                if (_EmployeeDetailRepository == null)
                {
                    _EmployeeDetailRepository = new EmployeeDetailsRepository(_dbcontext, _UOW);
                }

                return _EmployeeDetailRepository;
            }
        }
        public ITimesheetDetail TimesheetDetailRepository
        {
            get
            {
                if (_TimesheetDetailRepository == null)
                {
                    _TimesheetDetailRepository = new TimesheetDetailRepository(_dbcontext, _UOW);
                }

                return _TimesheetDetailRepository;
            }
        }
        public IUser UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(_dbcontext);
                }

                return _UserRepository;
            }
        }

        public List<SPViewModel> ExcecuteSP(string procedureName, Dictionary<string, string[]> parameters)
        {
            using (var command = this._dbcontext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = procedureName;
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = param.Key;
                    parameter.Value= param.Value[0];
                    //switch (param.Value[1])
                    //{
                    //    case "date":
                    //        {
                    //            parameter.DbType = DbType.Date;
                    //            break;
                    //        }
                    //    default:
                    //        {
                    //            parameter.DbType = DbType.String;
                    //            break;
                    //        }
                    //}
                    command.Parameters.Add(parameter);
                }
                this._dbcontext.Database.OpenConnection();
                var sqlDataReader = command.ExecuteReader();
                List<SPViewModel> items = new List<SPViewModel>();
                SPViewModel data = null;
                while (sqlDataReader.Read())
                {
                    data = new SPViewModel();
                    data.ProjectId = int.Parse(sqlDataReader["Project_ID"].ToString());
                    data.ProjectName = sqlDataReader["PROJECTNAME"].ToString();
                    data.EmployeeName = sqlDataReader["EMPLOYEENAME"].ToString();
                    data.EmployeeId = sqlDataReader["Employee_ID"].ToString();
                    data.Date = DateTime.Parse(sqlDataReader["DATE"].ToString());
                    data.NumberOfHours = double.Parse(sqlDataReader["NUMBEROFHOURS"].ToString());
                    items.Add(data);
                }
                return items;
            }
        }
        public void SaveChanges()
        {
            this._dbcontext.SaveChanges();
        }
    }
}
