using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class RoleVM
    {
        public string Id { get; set; }

        public string RoleName { get; set; }

        public string[] AddId { get; set; }

        public string[] DeleteId { get; set; }
    }
}
