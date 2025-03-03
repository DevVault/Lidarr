using System.Collections.Generic;
using FluentValidation.Results;
using NzbDrone.Common.Extensions;

namespace NzbDrone.Core.Notifications.Simplepush
{
    public class Simplepush : NotificationBase<SimplepushSettings>
    {
        private readonly ISimplepushProxy _proxy;

        public Simplepush(ISimplepushProxy proxy)
        {
            _proxy = proxy;
        }

        public override string Name => "Simplepush";
        public override string Link => "https://simplepush.io/";

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

            failures.AddIfNotNull(_proxy.Test(Settings));

            return new ValidationResult(failures);
        }
    }
}
