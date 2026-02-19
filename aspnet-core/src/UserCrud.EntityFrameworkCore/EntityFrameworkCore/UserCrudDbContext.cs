using Abp.Zero.EntityFrameworkCore;
using UserCrud.Authorization.Roles;
using UserCrud.Authorization.Users;
using UserCrud.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using UserCrud.Patients;
using UserCrud.Docters;
using UserCrud.Beds;
using UserCrud.Rooms;
using UserCrud.PatientAdmission;
using UserCrud.PatientImages;

namespace UserCrud.EntityFrameworkCore;

public class UserCrudDbContext : AbpZeroDbContext<Tenant, Role, User, UserCrudDbContext>
{
    /* Define a DbSet for each entity of the application */

    public UserCrudDbContext(DbContextOptions<UserCrudDbContext> options)
        : base(options)
    {
    }
    public DbSet<patient> patients { get; set; }
    public DbSet<doctor> docters { get; set; }
    public DbSet<room> rooms { get; set; }
    public DbSet<bed> beds { get; set; }
    public DbSet<patientAdmission> patientAdmissions { get; set; }
    public DbSet<patienttImages> patienttImages { get; set; }
}
