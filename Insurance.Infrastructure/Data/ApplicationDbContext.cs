using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Agents> Agents { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerAddress> CustomerAddresses { get; set; }

        public DbSet<CustomerNominee> CustomerNominees { get; set; }

        public DbSet<CustomerKyc> CustomerKycs { get; set; }

        public DbSet<HealthProfile> HealthProfiles { get; set; }

        public DbSet<InsuranceProduct> InsuranceProducts { get; set; }

        public DbSet<ProductBenefit> ProductBenefits { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<UnderwritingCase> UnderwritingCases { get; set; }

        public DbSet<Policy> Policies { get; set; }

        public DbSet<PolicyMember> PolicyMembers { get; set; }

        public DbSet<PremiumSchedule> PremiumSchedules { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<ClaimInvestigation> ClaimInvestigations { get; set; }

        public DbSet<ClaimSettlement> ClaimSettlements { get; set; }

        public DbSet<Commission> Commissions { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SupportTicket> SupportTickets { get; set; }

        public DbSet<AgentCommission> AgentCommissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal Precision

            modelBuilder.Entity<ProductBenefit>()
                .Property(x => x.BaseRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductBenefit>()
                .Property(x => x.Minimum)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductBenefit>()
                .Property(x => x.Maximum)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Quote>()
                .Property(x => x.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(x => x.SumInsured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(x => x.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Claim>()
                .Property(x => x.ClaimAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ClaimSettlement>()
                .Property(x => x.ApprovedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Commission>()
                .Property(x => x.CommissionAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CustomerNominee>()
                .Property(x => x.SharePercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<HealthProfile>()
                .Property(x => x.BMI)
                .HasPrecision(5, 2);

            // Disable Cascade Delete Globally

            foreach (var foreignKey in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}