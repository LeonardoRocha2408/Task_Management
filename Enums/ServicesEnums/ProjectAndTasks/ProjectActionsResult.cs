using System;
using System.Collections.Generic;
using System.Text;

namespace Enums.ServicesEnums.ProjectAndTasks
{
    public enum ProjectActionsResult
    {
        Created,
        Updated,
        Deleted,

        InvalidTitle,
        TitleTooLong,
        UserNotFound
    }
}
