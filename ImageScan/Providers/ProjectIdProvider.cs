namespace ImageScan.Providers
{
    public class ProjectIdProvider : IProjectIdProvider
    {
        private long _projectId = 0;

        public ProjectIdProvider()
        {
        }

        public ProjectIdProvider(long projectId)
        {
            _projectId = projectId;
        }

        public long GetProjectId()
        {
            if (_projectId != 0)
            {
                return _projectId;
            }

            var env = Environment.GetEnvironmentVariable("PID");

            if (env is null)
            {
                throw new ArgumentNullException(nameof(_projectId));
            }

            _projectId = long.Parse(env);

            return _projectId;
        }
    }
}
