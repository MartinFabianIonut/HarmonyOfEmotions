﻿using System.ComponentModel.DataAnnotations;

namespace HarmonyOfEmotions.Domain.RecommenderSystem
{
	public class UserTrackPreference
	{
		[Key]
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? TrackId { get; set; }
		public bool IsLiked { get; set; }
	}
}
