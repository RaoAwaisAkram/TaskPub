using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add this using directive
using System.ComponentModel.DataAnnotations.Schema; // Add this using directive
using System.Text.Json.Serialization; // Add this using directive


namespace TaskApp.Models;

public partial class TaskModel
{
    public int Taskid { get; set; }

    [Required] // You should add validation for fields that are required from the client
    public string Title { get; set; } = null!;

    [Required]
    public string Subject { get; set; } = null!;

    public bool Isdone { get; set; }

    [JsonIgnore] // Use this to ignore the property during JSON serialization and deserialization
    public string Createdby { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public bool Notificationon { get; set; }

    public DateTime Starttime { get; set; }

    // [JsonIgnore] or [System.Text.Json.Serialization.JsonIgnore]
    // Use the attribute that fits your project's JSON serializer
    [JsonIgnore]
    public virtual Rsuserinfo CreatedbyNavigation { get; set; } = null!;
}