﻿using System;
using System.Linq;

namespace LiveTddTotalAmount
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public decimal EffectiveDays(Budget budget)
        {
            if (StartDate > budget.LastDay)
            {
                return 0;
            }
            if (EndDate < budget.FirstDay)
            {
                return 0;
            }
            var effectiveEndDate = EndDate > budget.LastDay
                ? budget.LastDay
                : EndDate;

            var effectiveStartDate = StartDate < budget.FirstDay
                ? budget.FirstDay
                : StartDate;

            var days = (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
            return days;
        }
    }

    public class Accouting
    {
        private readonly IRepository<Budget> _repository;

        public Accouting(IRepository<Budget> repository)
        {
            _repository = repository;
        }

        public decimal TotalAmount(DateTime startDate, DateTime endDate)
        {
            var period = new Period(startDate, endDate);
            var budgets = _repository.GetAll();
            if (budgets.Any())
            {
                return period.EffectiveDays(budgets[0]);
            }
            return 0;
        }
    }
}