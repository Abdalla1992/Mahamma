using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Settings
{
    public class ActivitesSettings
    {
        public string CommentActivity { get; set; } = "Type Comment";
        public string SubTaskActivity { get; set; } = "Added Sub Task";
        public string TaskActivity { get; set; } = "Added Task";
        public string TaskSubmittedActivity { get; set; } = "Submitted The Task";
        public string TaskAcceptedActivity { get; set; } = "Accepted The Task";
        public string TaskRejectedActivity { get; set; } = "Rejected The Task";
        public string UploadActivity { get; set; } = "Upload File";
        public string UpdateActivity { get; set; } = "Updated Task";
        public string DeleteActivity { get; set; } = "Deleted task";
        public string ArchiveActivity { get; set; } = "Archived task";
        public string RemoveFileActivity { get; set; } = "Removed a File";
        public string AssignTaskMember { get; set; } = "Assign Task Member";
        public string UpdateTaskProgressPercentage { get; set; } = "Update task progress percentage";

        // Setting For Project
        public string UploadProjectFile { get; set; } = "Upload File";
        public string UpdateProject { get; set; } = "Update Project";
        public string DeleteProject { get; set; } = "Delete Project";
        public string ArchiveProject { get; set; } = "Archive Project";
        public string RemoveProjectFileActivity { get; set; } = "Removed a File";
        public string AddProject { get; set; } = "Added Project";
        public string DeleteProjectComment { get; set; } = "Delete Comment";
        public string AssignProjectMember { get; set; } = "Assign Project Member";
        public string UpdateProjectCharter { get; set; } = "Update Project Charter";
        public string AddProjectRiskPlan { get; set; } = "Add Project Risk Plan";
        public string UpdateProjectRiskPlan { get; set; } = "Update Project Risk Plan";
        public string DeleteProjectRiskPlan { get; set; } = "Delete Project Risk Plan";
        public string AddProjectCommunicationPlan { get; set; } = "Add Project Communication Plan";
        public string UpdateProjectCommunicationPlan { get; set; } = "Update Project Communication Plan";
        public string DeleteProjectCommunicationPlan { get; set; } = "Delete Project Communication Plan";

    }
}
