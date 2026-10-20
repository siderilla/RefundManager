using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RefundManager.Services
{
    public class OutlayService : IOutlayService
    {
        private readonly RefundManagerContext _context;
        private readonly IRefundService _refundService;

        public OutlayService(RefundManagerContext context, IRefundService refundService)
        {
            _context = context;
            _refundService = refundService;
        }

        public async Task<List<OutlayOutputDto>> GetOutlaysByStatus(string[]? statuses, Guid hrEmployeeId)
        {
            var outlaysByStatuses = _context.Outlays
                .AsNoTracking()
                .Where(o => o.EmployeeId != hrEmployeeId)
                .AsQueryable();

            if (statuses != null && statuses.Any())
            {
                outlaysByStatuses = outlaysByStatuses.Where(o =>
                    (statuses.Contains("Pending") && o.Refund == null) ||
                    (statuses.Contains("Approved") && o.Refund != null && o.Refund.Approved == true) ||
                    (statuses.Contains("Rejected") && o.Refund != null && o.Refund.Approved == false)
                );
            }

            return await outlaysByStatuses
        .Select(o => new OutlayOutputDto
        {
            Id = o.Id,
            CategoryName = o.Category.Name,
            EmployeeFullName = o.Employee.Name + " " + o.Employee.Surname,
            Amount = o.Amount,
            Receipt = o.Receipt,
            Date = o.Date,
            Description = o.Description,
            Status = o.Refund == null ? "Pending" : (o.Refund.Approved ? "Approved" : "Rejected"),
            RefundedAmount = o.Refund != null ? o.Refund.Amount : null,
            MonthlyBudget = o.Category.MonthlyBudget,
            DailyBudget = o.Category.DailyBudget
        })
        .ToListAsync();
        }

        public async Task<List<OutlayOutputDto>> GetOutlaysAsync(Guid? employeeId = null, DateOnly? date = null, string? department = null)
        {
            var outlays = _context.Outlays.AsNoTracking();

            if (employeeId.HasValue)
                outlays = outlays.Where(o => o.EmployeeId == employeeId.Value);

            if (date.HasValue)
                outlays = outlays.Where(o => o.Date == date.Value);

            if (!string.IsNullOrEmpty(department))
                outlays = outlays.Where(o => o.Employee.Department == department);

            return await outlays
                .Select(eo => new OutlayOutputDto
                {
                    Id = eo.Id,
                    CategoryName = eo.Category.Name,
                    EmployeeFullName = eo.Employee.Name + " " + eo.Employee.Surname,
                    Amount = eo.Amount,
                    Receipt = eo.Receipt,
                    Date = eo.Date,
                    Description = eo.Description,
                    Status = eo.Refund == null ? "Pending" : (eo.Refund.Approved ? "Approved" : "Rejected"),
                    RefundedAmount = eo.Refund != null ? eo.Refund.Amount : null,
                    MonthlyBudget = eo.Category.MonthlyBudget,
                    DailyBudget = eo.Category.DailyBudget
                })
                .ToListAsync();
        }

        //public async Task<List<OutlayOutputDto>> GetAllEmployeeOutlays(Guid employeeId)
        //{
        //    var employeeOutalysList = await _context.Outlays
        //        .AsNoTracking()
        //        .Where(o => o.EmployeeId == employeeId)
        //        .Select(eo => new OutlayOutputDto
        //        {
        //            Id = eo.Id,
        //            CategoryName = eo.Category.Name,
        //            Amount = eo.Amount,
        //            Receipt = eo.Receipt,
        //            Date = eo.Date,
        //            Description = eo.Description,
        //            Status = eo.Refund == null ? "Pending" : (eo.Refund.Approved ? "Approved" : "Rejected"),
        //            RefundedAmount = eo.Refund != null ? eo.Refund.Amount : null,
        //            MonthlyBudget = eo.Category.MonthlyBudget,
        //            DailyBudget = eo.Category.DailyBudget
        //        })
        //        .ToListAsync();

        //    return employeeOutalysList;
        //}

        public async Task<List<CategoryBudgetSummaryDto>> GetBudgetSummaryAsync(Guid employeeId, int? month = null, int? year = null)
        {
            var targetMonth = month ?? DateTime.Today.Month;
            var targetYear = year ?? DateTime.Today.Year;

            var firstDay = new DateOnly(targetYear, targetMonth, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var todayDate = DateOnly.FromDateTime(DateTime.Today);

            var categories = await _context.Categories.ToListAsync();

            // Get the approved expenses for the chosen month
            var monthlyOutlays = await _context.Outlays
                .Where(o => o.EmployeeId == employeeId
                       && o.Date >= firstDay && o.Date <= lastDay
                       && o.Refund != null && o.Refund.Approved == true)
                .ToListAsync();

            return categories.Select(c => new CategoryBudgetSummaryDto
            {
                CategoryName = c.Name,
                DailyBudget = c.DailyBudget,
                MonthlyBudget = c.MonthlyBudget,
                SpentToday = monthlyOutlays.Where(o => o.CategoryId == c.Id && o.Date == todayDate).Sum(o => o.Amount),
                SpentMonth = monthlyOutlays.Where(o => o.CategoryId == c.Id).Sum(o => o.Amount)
            })
            .ToList();
        }

        public async Task<List<OutlaySummaryOutputDto>> GetCalendarSummaryAsync(Guid? employeeId, int month, int year, string? department)
        {
            var outlays = _context.Outlays.AsNoTracking();
            if (employeeId.HasValue)
            {
                outlays = outlays.Where(o => o.EmployeeId == employeeId.Value); // filter
            }

            // ADDED FILTER: when HR filters by department
            if (!string.IsNullOrEmpty(department))
            {
                outlays = outlays.Where(o => o.Employee.Department == department); // ok because Outlay includes the Employee entity
            }

            outlays = outlays.Where(o => o.Date.Month == month && o.Date.Year == year);

            var summaryOutlaysList = await outlays
                .GroupBy(o => o.Date) // group
                .Select(g => new OutlaySummaryOutputDto // map + sum
                {
                    Date = g.Key,
                    OutlayAmount = g.Sum(o => o.Amount),
                    RefundAmount = g.Sum(o => o.Refund != null && o.Refund.Approved ? o.Refund.Amount : 0), // first check the refund exists, then whether it's approved
                })
                .OrderBy(d => d.Date)
                .ToListAsync();

            return summaryOutlaysList;
        }


        public async Task<List<DaySummaryOutputDto>> GetDaySummaryAsync(DateOnly date, Guid? employeeId, string? department)
        {
            var outlays = _context.Outlays.AsNoTracking();
            if (employeeId.HasValue)
            {
                outlays = outlays.Where(o => o.EmployeeId == employeeId.Value);
            }

            if (!string.IsNullOrEmpty(department))
            {
                outlays = outlays.Where(o => o.Employee.Department == department);
            }

            outlays = outlays.Where(o => o.Date == date); // filter by single date

            var summaryOutlaysList = await outlays
                .GroupBy(o => o.Category.Name)
                .Select(g => new DaySummaryOutputDto
                {
                    CategoryName = g.Key,
                    OutlayAmount = g.Sum(o => o.Amount),
                    RefundAmount = g.Sum(o => o.Refund != null && o.Refund.Approved ? o.Refund.Amount : 0),
                })
                .OrderByDescending(d => d.OutlayAmount)
                .ToListAsync();

            return summaryOutlaysList;

        }




        public async Task<OutlayOutputDto> CreateOutlayFromEmployee(Guid employeeId, OutlayInputDto outlayInputDto)
        {
            var outlay = new Outlay
            {
                Id = Guid.NewGuid(),
                CategoryId = outlayInputDto.CategoryId,
                EmployeeId = employeeId,
                Amount = outlayInputDto.Amount,
                Receipt = outlayInputDto.Receipt,
                Date = outlayInputDto.Date,
                Description = outlayInputDto?.Description,
            };

            _context.Outlays.Add(outlay);
            await _context.SaveChangesAsync();
            await _refundService.ProcessRefundAsync(outlay.Id, true, true);

            // LAZY LOADING! need to reload this data, because right after saving it's null:
            var outlayData = await _context.Outlays
                .Include(o => o.Category)
                .Include(o => o.Employee)
                .Include(o => o.Refund)
                .FirstOrDefaultAsync(o => o.Id == outlay.Id);

            return new OutlayOutputDto
            {
                Id = outlayData.Id,
                CategoryName = outlayData.Category.Name,
                EmployeeFullName = outlayData.Employee.Name,
                Amount = outlayData.Amount,
                Receipt = outlayData.Receipt,
                Date = outlayData.Date,
                Description = outlayData.Description,
                Status = outlayData.Refund == null ? "Pending" : (outlayData.Refund.Approved ? "Approved" : "Rejected"),
                RefundedAmount = outlayData.Refund?.Amount,
                DailyBudget = outlayData.Category.DailyBudget,
                MonthlyBudget = outlayData.Category.MonthlyBudget

            };
        }


    }



}
