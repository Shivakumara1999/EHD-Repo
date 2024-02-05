using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EHD.BAL.Exceptions
{
    public class AllExceptions
    {
        //Employee
        public class EmployeeIdExistException : Exception
        {
            public EmployeeIdExistException() { }
            public EmployeeIdExistException(string message) : base(message) { }
        }
        public class EmployeeIdNotNullException : Exception
        {
            public EmployeeIdNotNullException() { }
            public EmployeeIdNotNullException(string message) : base(message) { }
        }
        public class EmployeeIdNotExistException : Exception
        {
            public EmployeeIdNotExistException() { }
            public EmployeeIdNotExistException(string message) : base(message) { }
        }
        public class InvalidGenderException : Exception
        {
            public InvalidGenderException() { }
            public InvalidGenderException(string message) : base(message) { }
        }
        public class EmployeeEmailExistException : Exception
        {
            public EmployeeEmailExistException() { }
            public EmployeeEmailExistException(string message) : base(message) { }
        }
        public class EmployeeContactExistException : Exception
        {
            public EmployeeContactExistException() { }
            public EmployeeContactExistException(string message) : base(message) { }
        }
        public class EmployeeEmailAlternateException : Exception
        {
            public EmployeeEmailAlternateException() { }
            public EmployeeEmailAlternateException(string message) : base(message) { }
        }

        //Department

        public class DepartmentIdNotExistException : Exception
        {
            public DepartmentIdNotExistException() { }
            public DepartmentIdNotExistException(string message) : base(message) { }
        }

        public class DepartmentNameExistException : Exception
        {
            public DepartmentNameExistException() { }
            public DepartmentNameExistException(string message) : base(message) { }
        }
        public class DepartmentException : Exception
        {
            public DepartmentException() { }
            public DepartmentException(string message) : base(message) { }
        }



        //Role

        public class RoleIdNotExistException : Exception
        {
            public RoleIdNotExistException() { }
            public RoleIdNotExistException(string message) : base(message) { }
        }

        public class RoleNameNotExistException : Exception
        {
            public RoleNameNotExistException() { }
            public RoleNameNotExistException(string message) : base(message) { }
        }

        //Issue

        public class IssueIdNotExistException : Exception
        {
            public IssueIdNotExistException() { }
            public IssueIdNotExistException(string message) : base(message) { }
        }

        public class IssueNameExistException : Exception
        {
            public IssueNameExistException() { }
            public IssueNameExistException(string message) : base(message) { }

        }

        public class IssueException : Exception
        {
            public IssueException() { }
            public IssueException(string message) : base(message) { }
        }

        //Login
        public class UserNotFound : Exception
        {
            public UserNotFound() { }
            public UserNotFound(string message) : base(message) { }

        }
        public class InvalidPassw : Exception
        {
            public InvalidPassw() { }
            public InvalidPassw(string message) : base(message) { }

        }
        public class Invalidotp : Exception
        {
            public Invalidotp() { }
            public Invalidotp(string message) : base(message) { }

        }


        public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(ExceptionContext context)
            {
                switch (context.Exception)
                {

                    //Employee
                    case EmployeeIdExistException:
                        context.Result = new BadRequestObjectResult("EmployeeId already Exists");
                        break;
                    case EmployeeIdNotNullException:
                        context.Result = new BadRequestObjectResult("EmployeeId cannot be null or empty");
                        break;
                    case EmployeeIdNotExistException:
                        context.Result = new BadRequestObjectResult("Employee with entered ID is not found");
                        break;

                    case InvalidGenderException:
                        context.Result = new BadRequestObjectResult("Gender Field allows only F and M");
                        break;
                    case EmployeeEmailExistException:
                        context.Result = new BadRequestObjectResult("Official mail already exists");
                        break;
                    case EmployeeContactExistException:
                        context.Result = new BadRequestObjectResult("Contact Number already exists");
                        break;
                    case EmployeeEmailAlternateException:
                        context.Result = new BadRequestObjectResult("Official Mail and Alternate Mail should not be same");
                        break;

                    //Department
                    case DepartmentIdNotExistException:
                        context.Result = new BadRequestObjectResult("No such Department with entered id");
                        break;
                    case DepartmentNameExistException:
                        context.Result = new BadRequestObjectResult("Department Name already exists");
                        break;
                    case DepartmentException:
                        context.Result = new BadRequestObjectResult("Select the Department");
                        break;

                    //Roles
                    case RoleIdNotExistException:
                        context.Result = new BadRequestObjectResult("No such Role with entered id");
                        break;
                    case RoleNameNotExistException:
                        context.Result = new BadRequestObjectResult("Role Name already exists");
                        break;

                    //Issue
                    case IssueIdNotExistException:
                        context.Result = new BadRequestObjectResult("No such Issue with entered id");
                        break;
                    case IssueNameExistException:
                        context.Result = new BadRequestObjectResult("Issue Name already exists");
                        break;

                    case IssueException:
                        context.Result = new BadRequestObjectResult("Select the IssueType");
                        break;
                    //LOGIN
                    case UserNotFound:
                        context.Result = new BadRequestObjectResult("You're not registered");
                        break;
                    case InvalidPassw:
                        context.Result = new BadRequestObjectResult("Oops! Wrong password");
                        break;
                    case Invalidotp:
                        context.Result = new BadRequestObjectResult("Please check for Correct OTP");
                        break;
                }
                context.ExceptionHandled = true;
            }
        }
    }
}
