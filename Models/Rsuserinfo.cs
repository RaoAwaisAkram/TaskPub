using System;
using System.Collections.Generic;


namespace TaskApp.Models;

public partial class Rsuserinfo
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string Passwordhash { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Phonenumber { get; set; }

    public bool Isactive { get; set; }

    public DateTime Creadtets { get; set; }

    public DateTime? Modefied { get; set; }

   public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

}
