using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeMvcDemo
{
    public class T1ServiceModel
    {
        [Display(Name = "Id")]
        public Int32 Id { get; set; }

        [Display(Name = "ProgramID")]
        public string ProgramID { get; set; }

        [Display(Name = "ProgramName")]
        public string ProgramName { get; set; }

        [Display(Name = "Key")]
        public string Key { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
    }
}