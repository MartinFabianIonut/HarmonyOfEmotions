using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HarmonyOfEmotions.ApiService.Data
{
    public class HarmonyOfEmotionsDbContext(DbContextOptions<HarmonyOfEmotionsDbContext> options) : IdentityDbContext(options)
	{
		public DbSet<UserTrackPreference> UserTrackPreferences { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserTrackPreference>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasIndex(e => new { e.UserId, e.TrackId }).IsUnique();
			});
		}
	}
}
