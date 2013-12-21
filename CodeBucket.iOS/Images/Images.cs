using MonoTouch.UIKit;

namespace CodeBucket.iOS
{
    public static class Images
    {
        public static UIImage Merge { get { return UIImage.FromBundle("/Images/merge"); } }
        public static UIImage Language { get { return UIImage.FromBundle("/Images/language"); } }
        public static UIImage Webpage { get { return UIImage.FromBundle("/Images/webpage"); } }
        public static UIImage Repo { get { return UIImage.FromBundle("/Images/repo"); } }
        public static UIImage Team { get { return UIImageHelper.FromFileAuto("Images/team"); } }
        public static UIImage Size { get { return UIImage.FromBundle("/Images/size"); } }
        public static UIImage Locked { get { return UIImage.FromBundle("/Images/locked"); } }
        public static UIImage Unlocked { get { return UIImage.FromBundle("/Images/unlocked"); } }
        public static UIImage Heart { get { return UIImage.FromBundle("/Images/heart"); } }
        public static UIImage Fork { get { return UIImage.FromBundle("/Images/fork"); } }
        public static UIImage Pencil { get { return UIImage.FromBundle("/Images/pencil"); } }
        public static UIImage Tag { get { return UIImage.FromBundle("/Images/tag"); } }
        public static UIImage Comments { get { return UIImage.FromBundle("/Images/comments"); } }
        public static UIImage BinClosed { get { return UIImage.FromBundle("/Images/bin_closed"); } }
        public static UIImage Milestone { get { return UIImage.FromBundle("/Images/milestone"); } }
        public static UIImage Script { get { return UIImage.FromBundle("/Images/script"); } }
        public static UIImage Commit { get { return UIImage.FromBundle("/Images/commit"); } }
        public static UIImage Following { get { return UIImage.FromBundle("/Images/following"); } }
        public static UIImage Eye { get { return UIImageHelper.FromFileAuto("Images/eye"); } }
        public static UIImage Hand { get { return UIImageHelper.FromFileAuto("Images/hand"); } }
        public static UIImage Folder { get { return UIImage.FromBundle("/Images/folder"); } }
        public static UIImage File { get { return UIImage.FromBundle("/Images/file"); } }
        public static UIImage Branch { get { return UIImage.FromBundle("/Images/branch"); } }
        public static UIImage Create { get { return UIImage.FromBundle("/Images/create"); } }
        public static UIImage Info { get { return UIImage.FromBundle("/Images/info"); } }
        public static UIImage Flag { get { return UIImage.FromBundle("/Images/flag"); } }
        public static UIImage User { get { return UIImage.FromBundle("/Images/user"); } }
        public static UIImage Explore { get { return UIImage.FromBundle("/Images/explore"); } }
        public static UIImage Group { get { return UIImage.FromBundle("/Images/group"); } }
        public static UIImage Event { get { return UIImage.FromBundle("/Images/events"); } }
        public static UIImage Person { get { return UIImage.FromBundle("/Images/person"); } }
        public static UIImage Cog { get { return UIImage.FromBundle("/Images/cog"); } }
        public static UIImage Star { get { return UIImage.FromBundle("/Images/star"); } }
        public static UIImage Star2 { get { return UIImage.FromBundle("/Images/star2"); } }
        public static UIImage News { get { return UIImage.FromBundle("/Images/news"); } }
        public static UIImage Public { get { return UIImage.FromBundle("/Images/public"); } }
        public static UIImage Notifications { get { return UIImage.FromBundle("/Images/notifications"); } }
        public static UIImage Priority { get { return UIImage.FromBundle("/Images/priority"); } }
        public static UIImage Anonymous { get { return UIImage.FromBundle("/Images/anonymous"); } }

        public static class Logos
        {
			public static UIImage Bitbucket { get { return UIImage.FromFile("Images/Logos/bitbucket.png"); } }
        }

        public static class Buttons
        {
            public static UIImage GreyButton { get { return UIImageHelper.FromFileAuto("Images/Buttons/grey_button"); } }
        }

//        public static class Notifications
//        {
//            public static UIImage Commit { get { return UIImageHelper.FromFileAuto("Images/Notifications/commit"); } }
//            public static UIImage PullRequest { get { return UIImageHelper.FromFileAuto("Images/Notifications/pull_request"); } }
//        }
    }
}

