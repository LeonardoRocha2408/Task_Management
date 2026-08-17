using System;
using System.Collections.Generic;
using System.Text;

namespace Enums.ServicesEnums.ProjectAndTasks
{
    public enum TaskActionsResult
    {
        // Sucess
        Created,
        Updated,
        Deleted,

        // Validation
        InvalidTitle,
        TitleTooLong,
        UserNotFound,

        // Assigment
        TaskAlreadyAssigned,
        AssignedUserNotFound,
    }
}
