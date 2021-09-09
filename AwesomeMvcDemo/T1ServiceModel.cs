using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeMvcDemo
{
    public class T1ServiceModel
    {
        [Display(Name = "Id")]
        public Int32? Id { get; set; }

        [Display(Name = "ProgramID")]
        public string ProgramID { get; set; }

        [Required]
        [Display(Name = "HostName")]
        public string HostName { get; set; }

        [Required]
        [Display(Name = "ProgramName")]
        public string ProgramName { get; set; }

        [Required]
        [Display(Name = "Key")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "IsActive")]
        public bool? IsActive { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    public class T1ServiceModelInput
    {
        [Required]
        [Display(Name = "HostName")]
        public string HostName { get; set; }

        [Required]
        [Display(Name = "ProgramName")]
        public string ProgramName { get; set; }

        [Required]
        [Display(Name = "Key")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "Value")]
        public string Value { get; set; }         
    }
}