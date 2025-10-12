using Microsoft.EntityFrameworkCore;

namespace Navtrack.Database.Model;

public class NavtrackDbContext(DbContextOptions<NavtrackDbContext> options) : BaseNavtrackDbContext(options);