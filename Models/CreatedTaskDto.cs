namespace TaskApp.Models;
using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
// Models/TaskCreateDto.cs
public class TaskCreateDto
{
    public string Title { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public bool? Isdone { get; set; }
    public bool? Notificationon { get; set; }
    public DateTime? Starttime { get; set; }
}
