﻿using System;
using System.Collections.Generic;

namespace ECommerce.CheckoutService.Model
{
    public class CheckoutSummary
    {
        public IList<CheckoutProduct> Products { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
