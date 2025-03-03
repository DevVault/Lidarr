using System;
using System.Collections.Generic;
using FluentValidation.Results;
using NLog;

namespace NzbDrone.Core.Notifications.SendGrid
{
    public class SendGrid : NotificationBase<SendGridSettings>
    {
        private readonly ISendGridProxy _proxy;
        private readonly Logger _logger;

        public SendGrid(ISendGridProxy proxy, Logger logger)
        {
            _proxy = proxy;
            _logger = logger;
        }

        public override string Name => "SendGrid";
        public override string Link => "https://sendgrid.com/";

        public override void OnGrab(GrabMessage grabMessage)
        {
            _proxy.SendNotification(ALBUM_GRABBED_TITLE, grabMessage.Message, Settings);
        }

        public override void OnReleaseImport(AlbumDownloadMessage message)
        {
            _proxy.SendNotification(ALBUM_DOWNLOADED_TITLE, message.Message, Settings);
        }

        public override void OnArtistAdd(ArtistAddMessage message)
        {
            _proxy.SendNotification(ARTIST_ADDED_TITLE, message.Message, Settings);
        }

        public override void OnArtistDelete(ArtistDeleteMessage deleteMessage)
        {
            _proxy.SendNotification(ARTIST_DELETED_TITLE, deleteMessage.Message, Settings);
        }

        public override void OnAlbumDelete(AlbumDeleteMessage deleteMessage)
        {
            _proxy.SendNotification(ALBUM_DELETED_TITLE, deleteMessage.Message, Settings);
        }

        public override void OnHealthIssue(HealthCheck.HealthCheck healthCheck)
        {
            _proxy.SendNotification(HEALTH_ISSUE_TITLE, healthCheck.Message, Settings);
        }

        public override void OnHealthRestored(HealthCheck.HealthCheck previousCheck)
        {
            _proxy.SendNotification(HEALTH_RESTORED_TITLE, $"The following issue is now resolved: {previousCheck.Message}", Settings);
        }

        public override void OnDownloadFailure(DownloadFailedMessage message)
        {
            _proxy.SendNotification(DOWNLOAD_FAILURE_TITLE, message.Message, Settings);
        }

        public override void OnImportFailure(AlbumDownloadMessage message)
        {
            _proxy.SendNotification(IMPORT_FAILURE_TITLE, message.Message, Settings);
        }

        public override void OnApplicationUpdate(ApplicationUpdateMessage updateMessage)
        {
            _proxy.SendNotification(APPLICATION_UPDATE_TITLE, updateMessage.Message, Settings);
        }

        public override ValidationResult Test()
        {
            var failures = new List<ValidationFailure>();

            try
            {
                const string title = "Test Notification";
                const string body = "This is a test message from Lidarr";

                _proxy.SendNotification(title, body, Settings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to send test message");
                failures.Add(new ValidationFailure("", "Unable to send test message"));
            }

            return new ValidationResult(failures);
        }
    }
}
