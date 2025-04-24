using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaleidocode.Gists.ChangeTracking.Models.Base;

public class AuditBaseModel
{
    /// <summary>
    /// The Id for each Audit entry, for lookup.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The Date of the entry addition.
    /// </summary>
    public DateTime EntryCreated { get; set; } = DateTime.Now;
}

