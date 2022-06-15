namespace AnL.Constants
{
    public static class MessageConstants
    {
        public static readonly string TimesheetAdditionSuccess = "TimeSheet records added successfully";
        public static readonly string TimesheetAdditionFailed = "TimeSheet records addition failed";
        public static readonly string TimesheetAlreadyExists = "Timesheet Records for the project and activity already exists!!";
        public static readonly string TimesheetDeletionSuccess = "TimeSheet records deleted successfully";
        public static readonly string TimesheetDeletionFailed = "TimeSheet records deletion failed";
        public static readonly string TimesheetModificationSuccess = "TimeSheet records modified successfully";
        public static readonly string TimesheetModificationFailed = "TimeSheet records modification Failed";
        public static readonly string TimesheetListingSuccess = "TimeSheet records fetched successfully";
        public static readonly string TimesheetListingFailed = "TimeSheet records fetch failed";
        public static readonly string ReviewTimesheetListingSuccess = "Review TimeSheet records fetched successfully";
        public static readonly string ReviewTimesheetListingFailed = "Review TimeSheet records fetch failed";
        public static readonly string TimesheetListingNoRecords = "Employee has no timesheet records during the selected week!!";
        
        public static readonly string GenerateOTPFailed = "OTP Generation failed";
        public static readonly string GenerateOTPSuccess = "OTP Generated successfully";
        public static readonly string InvalidEmailAddress = "Invalid Email Address";
        public static readonly string InvalidOTP = "Invalid OTP";
        public static readonly string OTPTimedOUT = "OTP Expired";
        public static readonly string LoginSuccess = "Login Successful";
        public static readonly string UserNotActive = "User has been deactivated";

        public static readonly string TimesheetSubmissionSuccess = "Timesheet Submitted successfully";
        public static readonly string TimesheetSubmissionSuccessMailFailure = "Timesheet Submitted successfully but email generation failed!!";
        public static readonly string TimesheetSubmissionFailed = "Timesheet Submission Failed";
        public static readonly string SupervisorListingSuccess = "Supervisor records fetched successfully";
        public static readonly string SupervisorListingFailed = "Supervisor records fetch failed";
        public static readonly string SupervisorApproved = "Supervisor approved the timesheet";
        public static readonly string SupervisorRejected = "Supervisor rejected the timesheet";
        public static readonly string SupervisorActionFailed = "Supervisor approval/rejection Failed";

        public static readonly string ProjectAllocationSuccess = "Resourcs Allocated to the project successfully";
        public static readonly string ProjectAdditionSuccess = "Porject Added successfully";
        public static readonly string ActivityAdditionSuccess = "Activity Added successfully";

        public static readonly string ProjectDeletionSuccess = "Project deleted successfully";
        public static readonly string ProjectDeletionFailed = "Project deletion failed as timesheet record exist against Project";

        public static readonly string ActivityDeletionSuccess = "Activity deleted successfully";
        public static readonly string ActivityDeletionFailed = "Activity deletion failed as timesheet record exist against Activity";


        public static readonly string EditProjectSuccess = "Project Updated successfully";
        public static readonly string EditProjectFailed = "Project Updated failed.";



        public static readonly string EmployeeFetchSuccess = "Employee records fetched successfully";
        public static readonly string EmployeeFetchFailed = "Employee records fetch failed";
    }
}


