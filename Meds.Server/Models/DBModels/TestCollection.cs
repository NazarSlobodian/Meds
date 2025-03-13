using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class TestCollection
{
    public int TestCollectionId { get; set; }

    public string CollectionName { get; set; } = null!;

    public virtual ICollection<TestType> TestTypes { get; set; } = new List<TestType>();
}
