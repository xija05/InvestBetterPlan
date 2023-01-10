﻿using System;
using System.Collections.Generic;

namespace InvestBetterPlan_RestAPI.Models
{
    public partial class Fundingcomposition
    {
        public double Percentage { get; set; }
        public int Id { get; set; }
        public int Subcategoryid { get; set; }
        public int Fundingid { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public virtual Funding Funding { get; set; } = null!;
        public virtual Compositionsubcategory Subcategory { get; set; } = null!;
    }
}
