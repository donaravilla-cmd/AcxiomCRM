using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class Customer {
 public int Id {get;set;} [Required, StringLength(100)] public string CustomerName {get;set;}="";
 [Required, EmailAddress, StringLength(150)] public string Email {get;set;}="";
 [Required, RegularExpression(@"^[0-9+() -]{10,15}$",ErrorMessage="Enter a valid phone number."), StringLength(20)] public string Phone {get;set;}="";
 [StringLength(120)] public string? CompanyName {get;set;} [StringLength(250)] public string? Address {get;set;}
 [StringLength(60)] public string? City {get;set;} [StringLength(60)] public string? State {get;set;}
 public string Status {get;set;}="Active"; public string? OwnerId {get;set;} public DateTime CreatedDate {get;set;}=DateTime.UtcNow; public string? CreatedBy {get;set;}
}
public class Lead {
 public int Id {get;set;} [Required,StringLength(100)] public string LeadName {get;set;}=""; [EmailAddress] public string? Email {get;set;}
 [RegularExpression(@"^[0-9+() -]{10,15}$")] public string? Phone {get;set;} public string? CompanyName {get;set;}
 [Required] public string Source {get;set;}="Website"; [Required] public string Status {get;set;}="New";
 [Range(0,1000000000)] public decimal ExpectedValue {get;set;} public string? AssignedTo {get;set;} public DateTime CreatedDate {get;set;}=DateTime.UtcNow;
}
public class Opportunity {
 public int Id {get;set;} [Required,StringLength(120)] public string OpportunityName {get;set;}=""; public int? CustomerId {get;set;}
 [Range(typeof(decimal),"0.01","999999999")] public decimal Amount {get;set;} [Range(0,100)] public int Probability {get;set;}
 [Required] public string Stage {get;set;}="Qualification"; [DataType(DataType.Date)] public DateTime ExpectedCloseDate {get;set;}=DateTime.Today.AddDays(30);
 public string Status {get;set;}="Open"; public string? AssignedTo {get;set;} public decimal WeightedValue => Amount*Probability/100m;
}
public class FollowUp {
 public int Id {get;set;} public int? CustomerId {get;set;} public int? LeadId {get;set;} [Required,DataType(DataType.Date)] public DateTime FollowUpDate {get;set;}=DateTime.Today.AddDays(1);
 [Required] public string FollowUpType {get;set;}="Call"; [StringLength(500)] public string? Remarks {get;set;} public string Status {get;set;}="Planned"; public string? AssignedTo {get;set;}
}
public class Activity { public int Id {get;set;} public string ActivityType {get;set;}="Call"; [Required] public string Subject {get;set;}=""; public string? Description {get;set;} public DateTime ActivityDate {get;set;}=DateTime.Now; public int? CustomerId {get;set;} public int? LeadId {get;set;} public string Status {get;set;}="Open"; public string? AssignedTo {get;set;} }
public class AuditLog { public int Id {get;set;} public string? UserId {get;set;} public string Action {get;set;}=""; public string EntityName {get;set;}=""; public string? RecordId {get;set;} public string? OldValue {get;set;} public string? NewValue {get;set;} public DateTime CreatedDate {get;set;}=DateTime.UtcNow; public string? IpAddress {get;set;} }
