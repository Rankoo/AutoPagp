using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAlertDB.Models
{
    public class UserSubmodules : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid SubModuleId { get; set; }

        public bool IsEnabled { get; set; }


        public Users? User { get; set; }
        public SubModules? SubModule { get; set; }
    }
}
