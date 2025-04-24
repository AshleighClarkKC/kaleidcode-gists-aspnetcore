namespace Kaleidocode.Gists.ChangeTracking.Models.Auditing;

using Kaleidocode.Gists.ChangeTracking.Models.Base;

public class AuditEntryModel : AuditBaseModel
{
    /// <summary>
    /// The ID of the value being affected.
    /// </summary>
    public Guid EntryId { get; set; }

    /// <summary>
    /// The identifier for the user who initiated the action. 
    /// </summary>
    public Guid? Actor { get; set; }

    /// <summary>
    /// The name of the Entity.
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// The property that has been changed.
    /// </summary>
    public string? AffectedProperty { get; set; } = string.Empty;

    /// <summary>
    /// Optional for <see cref="EntityState.Added" /> values.
    /// </summary>
    public string? OriginalValue { get; set; }

    /// <summary>
    /// Optional for <see cref="EntityState.Deleted" /> values.
    /// </summary>
    public string? NewValue { get; set; }

    /// <summary>
    /// Value representing the <see cref="EntityState"/>.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Check if the entry is value. 
    /// </summary>
    public bool? IsValueActive { get; set; }

    /// <summary>
    /// Check if the entry has been deleted.
    /// </summary>
    public bool? IsValueDeleted { get; set; }

    /// <summary>
    /// The date in which the entry has been created. 
    /// </summary>
    public DateTime? ValueCreated { get; set; }

    /// <summary>
    /// The date in which the entry has been modified.
    /// </summary>
    public DateTime? ValueUpdated { get; set; }
}
