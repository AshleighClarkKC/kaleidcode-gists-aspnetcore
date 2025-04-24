namespace Kaleidocode.Gists.ChangeTracking.Models.Base;

public class BaseModel
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public Guid? CreatedBy { get; set; }

    public DateTime? Updated { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool Active { get; set; } = true;

    public bool Deleted { get; set; } = false;

    public Guid? DeletedBy { get; set; }

}

