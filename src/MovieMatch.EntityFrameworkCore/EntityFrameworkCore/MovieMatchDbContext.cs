using Microsoft.EntityFrameworkCore;
using MovieMatch.UserConnections;
using MovieMatch.Movies;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using MovieMatch.Posts;
using MovieMatch.People;
using MovieMatch.Genres;
using Volo.Abp;
using MovieMatch.Messages;

namespace MovieMatch.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]

[ConnectionStringName("Default")]
public class MovieMatchDbContext :
    AbpDbContext<MovieMatchDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{

    public DbSet<UserConnection> Connections { get; set; }
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */
    public DbSet<Message> Messages { get; set; }
    public DbSet<WatchedBefore> MoviesWatchedBefore { get; set; }
    public DbSet<WatchLater> MoviesWatchLater { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<MovieDetail> MovieDetails { get; set; }


    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public MovieMatchDbContext(DbContextOptions<MovieMatchDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();
        builder.ConfigureCmsKit();
        /* Configure your own tables/entities inside here */


        builder.Entity<UserConnection>(
            b =>
            {
                b.ToTable("Connections");
                b.ConfigureByConvention();
                b.Property(x => x.FollowerId).IsRequired();
                b.Property(x => x.FollowingId).IsRequired();
                b.Property(x => x.IsFollowed).IsRequired();
                b.HasKey(x => new { x.FollowerId, x.FollowingId });

                b.HasOne<IdentityUser>().WithMany().HasForeignKey(c => c.FollowerId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(c => c.FollowingId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                //      b.HasIndex(x => new { x.FollowerId, x.FollowingId });
            });
        builder.Entity<Movie>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "Movies" + MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.HasKey(x => x.Id);


        });
        builder.Entity<WatchedBefore>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "MoviesWatchedBefore",
                MovieMatchConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.MovieId).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.HasKey(x => new { x.UserId, x.MovieId });
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(c => c.UserId).IsRequired();
            b.HasOne<Movie>().WithMany().HasForeignKey(c => c.MovieId).IsRequired();
            b.HasIndex(x => new { x.UserId, x.MovieId });
        });
        builder.Entity<WatchLater>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "MoviesWatchLater",
                MovieMatchConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.MovieId).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.HasKey(x => new { x.UserId, x.MovieId });
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(y => y.UserId).IsRequired();
            b.HasOne<Movie>().WithMany().HasForeignKey(y => y.MovieId).IsRequired();
            b.HasIndex(x => new { x.UserId, x.MovieId });
        });
        builder.Entity<Post>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "Posts",
                MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();
            //b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.MovieId).IsRequired();
            b.Property(x => x.Rate).IsRequired();
            b.Property(x => x.Comment).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId);
            b.HasOne<Movie>().WithMany().HasForeignKey(x => x.MovieId);
            b.HasIndex(x => x.Id);
        });
        builder.Entity<Person>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "People", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.BirthDay).IsRequired();
            b.Property(x => x.Biography).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.HasIndex(x => x.Id);

        });
        builder.Entity<Director>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "Directors", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.BirthDay).IsRequired();
            b.Property(x => x.Biography).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.HasIndex(x => x.Id);
        });
        builder.Entity<Genre>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "Genres", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.HasIndex(x => x.Id);

        });
        builder.Entity<Message>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "Messages",
                MovieMatchConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Id).IsRequired();
            b.Property(x => x.Text).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(d => d.UserId);
        });
        builder.Entity<MovieDetail>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "MovieDetails", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            b.HasIndex(x => x.Id);

            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Overview).IsRequired();
            b.Property(x => x.ReleaseDate).IsRequired();
            b.Property(x => x.VoteAverage).IsRequired();
            b.Property(x => x.ImdbId).IsRequired();
            b.Property(x => x.DirectorId).IsRequired();

            b.HasOne<Director>().WithMany().HasForeignKey(x => x.DirectorId).IsRequired();
            b.HasMany(x => x.Stars).WithOne().HasForeignKey(x => x.MovieDetailId).IsRequired();

        });
        builder.Entity<MovieGenre>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "MovieGenres", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();

            //define composite key
            b.HasKey(x => new { x.MovieDetailId, x.GenreId });

            //many-to-many configuration
            b.HasOne<MovieDetail>().WithMany(x=>x.Genres).HasForeignKey(x => x.MovieDetailId).IsRequired();
            b.HasOne<Genre>().WithMany().HasForeignKey(x => x.GenreId).IsRequired();
            b.HasIndex(x => new { x.MovieDetailId, x.GenreId });
        });
        builder.Entity<MoviePerson>(b =>
        {
            b.ToTable(MovieMatchConsts.DbTablePrefix + "MoviePeople", MovieMatchConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasKey(x => new { x.MovieDetailId, x.PersonId });

            b.HasOne<Person>().WithMany().HasForeignKey(x => x.PersonId).IsRequired();
            b.HasOne<MovieDetail>().WithMany(x=>x.Stars).HasForeignKey(x => x.MovieDetailId).IsRequired();

            b.HasIndex(x => new { x.MovieDetailId, x.PersonId });
        });

    }


}