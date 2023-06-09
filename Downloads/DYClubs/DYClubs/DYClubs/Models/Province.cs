﻿using System;
using System.Collections.Generic;

namespace DYClubs.Models
{
    public partial class Province
    {
        public Province()
        {
            NameAddress = new HashSet<NameAddress>();
        }

        public string ProvinceCode { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string SalesTaxCode { get; set; }
        public double SalesTax { get; set; }
        public bool IncludesFederalTax { get; set; }
        public string FirstPostalLetter { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
        public virtual ICollection<NameAddress> NameAddress { get; set; }
    }
}
