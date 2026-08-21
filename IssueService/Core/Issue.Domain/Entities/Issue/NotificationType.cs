using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  enum NotificationType
    {
        NewIssue,
        StatusChange,
        Scheduled,
        Comment,
        System
    }
}
