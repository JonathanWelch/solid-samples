using System;
using System.Diagnostics;
using Quartz;
using _5._0_DependencyInversion.Common.EventLogger;

namespace _5._0_DependencyInversion.Common.Service
{
    public abstract class IwtJob : IJob
    {
        private bool jobHasFailed;

        protected IwtJob()
            : this(GetDefaultEventLogger())
        {
        }

        protected IwtJob(EventLogger.EventLogger eventLogger)
        {
            EventLogger = eventLogger;
            JobStarted += this.IwtJobStarted;
            JobFinished += this.IwtJobFinished;
            JobFailed += this.IwtJobFailed;
            this.jobHasFailed = false;
        }

        public delegate void JobStartedHandler();

        public delegate void JobFinishedHandler();

        public delegate void JobFailedHandler(string errorMessage);

        public event JobStartedHandler JobStarted;

        public event JobFinishedHandler JobFinished;

        public event JobFailedHandler JobFailed;

        protected static EventLogger.EventLogger EventLogger { get; set; }

        private string JobName
        {
            get { return this.GetType().Name; }
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.LogThatJobStarted();

                this.ExecuteJob(context);

                this.LogThatJobFinished();
            }
            catch (Exception ex)
            {
                OnFailure(ex.GetBaseException().Message);
                Logger.Error(ex);

                Debug.WriteLine(ex);

                throw new JobExecutionException(string.Format("An exception was thrown while executing job '{0}'", GetType().Name), ex);
            }
        }

        protected void OnFailure(string errorMessage)
        {
            if (this.JobFailed == null)
            {
                return;
            }

            this.JobFailed(errorMessage);
            this.jobHasFailed = true;
        }

        protected abstract void ExecuteJob(IJobExecutionContext context);

        private static EventLogger.EventLogger GetDefaultEventLogger()
        {
            var hostService = GetHostService();
            return new BufferedEventLogger(hostService);
        }

        private static string GetHostService()
        {
            var applicationDomainName = AppDomain.CurrentDomain.FriendlyName;
            return applicationDomainName.Replace(".exe", string.Empty);
        }

        private void LogThatJobStarted()
        {
            if (JobStarted == null)
            {
                return;
            }

            JobStarted();
        }

        private void LogThatJobFinished()
        {
            if (JobFinished == null || this.jobHasFailed)
            {
                return;
            }

            JobFinished();
        }

        private void IwtJobStarted()
        {
            var @event = new JobStartedEvent(JobName);

            EventLogger.LogEvent(@event);
        }

        private void IwtJobFailed(string errorMessage)
        {
            var @event = new JobFailedEvent(JobName, errorMessage);

            EventLogger.LogEvent(@event);
        }

        private void IwtJobFinished()
        {
            var @event = new JobFinishedEvent(JobName);

            EventLogger.LogEvent(@event);
        }
    }
}
