namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class PathUtils
	{
		public static string GetAbsolutePath<T> (string relativePath)
		{
			var assembly = typeof(T).Assembly;
			var assemblyDir = Path.GetDirectoryName(assembly.Location);
			if (assemblyDir == null)
			{
				throw new Exception($"Cannot find directory for assembly: {assembly.FullName}");
			}
			Console.WriteLine(assemblyDir + "," + relativePath);
			if (assemblyDir.Contains("wwwroot"))
			{
				var path = Path.Combine(assemblyDir, relativePath);
				if (File.Exists(path))
				{
					return path;
				}
				else
				{
					throw new Exception($"Cannot find file: {path}");
				}
			}
			var root = Path.GetDirectoryName(assemblyDir);
			while (!root.EndsWith("HarmonyOfEmotions.ApiService"))
			{
				root = Path.GetDirectoryName(root);
			}
			root = Path.GetDirectoryName(root);
			return Path.Combine(root!, relativePath);
		}
	}
}
