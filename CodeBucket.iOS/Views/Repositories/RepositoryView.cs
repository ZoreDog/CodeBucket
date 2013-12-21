using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CodeFramework.iOS.ViewControllers;
using CodeFramework.iOS.Views;
using CodeBucket.Core.ViewModels.Repositories;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BitbucketSharp.Models;

namespace CodeBucket.iOS.Views.Repositories
{
    public class RepositoryView : ViewModelDrivenViewController
    {
		private readonly HeaderView _header = new HeaderView();

        public new RepositoryViewModel ViewModel
        {
            get { return (RepositoryViewModel)base.ViewModel; }
            protected set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowExtraMenu());
            NavigationItem.RightBarButtonItem.Enabled = false;

            ViewModel.Bind(x => x.Repository, x =>
            {
				ViewModel.ImageUrl = x.LargeLogo(64);
                NavigationItem.RightBarButtonItem.Enabled = true;
                Render(x);
            });
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Title = _header.Title = ViewModel.RepositoryName;
		}

        private void ShowExtraMenu()
        {
            var repoModel = ViewModel.Repository;
            if (repoModel == null || ViewModel.IsStarred == null || ViewModel.IsWatched == null)
                return;

            var sheet = MonoTouch.Utilities.GetSheet(repoModel.Name);
			var pinButton = sheet.AddButton(ViewModel.IsPinned ? "Unpin from Slideout Menu".t() : "Pin to Slideout Menu".t());
            var starButton = sheet.AddButton(ViewModel.IsStarred.Value ? "Unstar This Repo".t() : "Star This Repo".t());
            var watchButton = sheet.AddButton(ViewModel.IsWatched.Value ? "Unwatch This Repo".t() : "Watch This Repo".t());
            //var forkButton = sheet.AddButton("Fork Repository".t());
			var showButton = sheet.AddButton("Show in Bitbucket".t());
            var cancelButton = sheet.AddButton("Cancel".t());
            sheet.CancelButtonIndex = cancelButton;
            sheet.DismissWithClickedButtonIndex(cancelButton, true);
            sheet.Clicked += (s, e) => {
                // Pin to menu
                if (e.ButtonIndex == pinButton)
                {
                    ViewModel.PinCommand.Execute(null);
                }
                else if (e.ButtonIndex == starButton)
                {
                    ViewModel.ToggleStarCommand.Execute(null);
                }
                else if (e.ButtonIndex == watchButton)
                {
                    ViewModel.ToggleWatchCommand.Execute(null);
                }
                // Fork this repo
//                else if (e.ButtonIndex == forkButton)
//                {
//                    ForkRepository();
//                }
                // Show in Bitbucket
                else if (e.ButtonIndex == showButton)
                {
					ViewModel.GoToUrlCommand.Execute(null);
                }
            };

            sheet.ShowInView(this.View);
        }

//        private void ForkRepository()
//        {
//            var repoModel = Controller.Model.RepositoryModel;
//            var alert = new UIAlertView();
//            alert.Title = "Fork".t();
//            alert.Message = "What would you like to name your fork?".t();
//            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
//            var forkButton = alert.AddButton("Fork!".t());
//            var cancelButton = alert.AddButton("Cancel".t());
//            alert.CancelButtonIndex = cancelButton;
//            alert.DismissWithClickedButtonIndex(cancelButton, true);
//            alert.GetTextField(0).Text = repoModel.Name;
//            alert.Clicked += (object sender2, UIButtonEventArgs e2) => {
//                if (e2.ButtonIndex == forkButton)
//                {
//                    var text = alert.GetTextField(0).Text;
//                    this.DoWork("Forking...".t(), () => {
//                        //var fork = Application.Client.Users[model.Owner.Login].Repositories[model.Name].Fo(text);
//                        BeginInvokeOnMainThread(() => {
//                            //  NavigationController.PushViewController(new RepositoryInfoViewController(fork), true);
//                        });
//                    }, (ex) => {
//                        //We typically get a 'BAD REQUEST' but that usually means that a repo with that name already exists
//                        MonoTouch.Utilities.ShowAlert("Unable to fork".t(), "A repository by that name may already exist in your collection or an internal error has occured.".t());
//                    });
//                }
//            };
//
//            alert.Show();
//        }

		public void Render(RepositoryDetailedModel model)
        {
            Title = model.Name;
            var root = new RootElement(Title) { UnevenRows = true };
			_header.Subtitle = "Updated ".t() + (model.UtcLastUpdated).ToDaysAgo();
			_header.ImageUri = ViewModel.ImageUrl;

            root.Add(new Section(_header));
            var sec1 = new Section();

            if (!string.IsNullOrEmpty(model.Description) && !string.IsNullOrWhiteSpace(model.Description))
            {
                var element = new MultilinedElement(model.Description)
                {
                    BackgroundColor = UIColor.White,
                    CaptionColor = Theme.CurrentTheme.MainTitleColor, 
                    ValueColor = Theme.CurrentTheme.MainTextColor
                };
                element.CaptionColor = element.ValueColor;
                element.CaptionFont = element.ValueFont;
                sec1.Add(element);
            }

            sec1.Add(new SplitElement(new SplitElement.Row {
				Text1 = model.IsPrivate ? "Private".t() : "Public".t(),
				Image1 = model.IsPrivate ? Images.Locked : Images.Unlocked,
                Text2 = model.Language,
                Image2 = Images.Language
            }));


            //Calculate the best representation of the size
            string size;
            if (model.Size / 1024f < 1)
                size = string.Format("{0:0.##}KB", model.Size);
            else if ((model.Size / 1024f / 1024f) < 1)
                size = string.Format("{0:0.##}MB", model.Size / 1024f);
            else
                size = string.Format("{0:0.##}GB", model.Size / 1024f / 1024f);
//
//            sec1.Add(new SplitElement(new SplitElement.Row {
//                Text1 = model.OpenIssues + (model.OpenIssues == 1 ? " Issue".t() : " Issues".t()),
//                Image1 = Images.Flag,
//				Text2 = model.ForkCount.ToString() + (model.ForkCount == 1 ? " Fork".t() : " Forks".t()),
//                Image2 = Images.Fork
//            }));

            sec1.Add(new SplitElement(new SplitElement.Row {
				Text1 = (model.UtcCreatedOn).ToString("MM/dd/yy"),
                Image1 = Images.Create,
                Text2 = size,
                Image2 = Images.Size
            }));

            var owner = new StyledStringElement("Owner".t(), model.Owner) { Image = Images.Person,  Accessory = UITableViewCellAccessory.DisclosureIndicator };
			owner.Tapped += () => ViewModel.GoToOwnerCommand.Execute(null);
            sec1.Add(owner);
//
//            if (model.Parent != null)
//            {
//				var parent = new StyledStringElement("Forked From".t(), model.FullName) { Image = Images.Fork,  Accessory = UITableViewCellAccessory.DisclosureIndicator };
//				parent.Tapped += () => ViewModel.GoToForkParentCommand.Execute(model.Parent);
//                sec1.Add(parent);
//            }

			var followers = new StyledStringElement("Followers".t(), "" + model.FollowersCount) { Image = Images.Star, Accessory = UITableViewCellAccessory.DisclosureIndicator };
			followers.Tapped += () => ViewModel.GoToStargazersCommand.Execute(null);
            sec1.Add(followers);

			var events = new StyledStringElement("Events".t(), () => ViewModel.GoToEventsCommand.Execute(null), Images.Event);
            var sec2 = new Section { events };

            if (model.HasIssues)
				sec2.Add(new StyledStringElement("Issues".t(), () => ViewModel.GoToIssuesCommand.Execute(null), Images.Flag));

            var sec3 = new Section
            {
				new StyledStringElement("Commits".t(), () => ViewModel.GoToCommitsCommand.Execute(null), Images.Commit),
//				new StyledStringElement("Pull Requests".t(), () => ViewModel.GoToPullRequestsCommand.Execute(null), Images.Hand),
				new StyledStringElement("Source".t(), () => ViewModel.GoToSourceCommand.Execute(null), Images.Script),
            };

            root.Add(new[] { sec1, sec2, sec3 });

            Root = root;
        }
    }
}